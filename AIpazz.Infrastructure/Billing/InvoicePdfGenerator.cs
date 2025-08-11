using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Aipazz.Domian.Billing;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Common.Aipazz.Application.Common;

namespace AIpazz.Infrastructure.Billing;

public class InvoicePdfGenerator : IInvoicePdfGenerator
{
    private const string Font = Fonts.Arial;
    private readonly IUserContext _userContext;
    public InvoicePdfGenerator(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public Task<byte[]> GeneratePdfAsync(
        Invoice invoice,
        List<TimeEntry> timeEntries,
        List<ExpenseEntry> expenseEntries)
    {
        var stream = new MemoryStream();
        var currency = string.IsNullOrWhiteSpace(invoice.Currency) ? "Rs." : invoice.Currency;

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(28);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontFamily(Font).FontSize(11));

                /*──────────────── HEADER ────────────────*/
                page.Header().Row(row =>
                {
                    row.ConstantItem(310).Column(col =>
                    {
                        col.Item().Text($"Law Office of {_userContext.FullName}")
                                  .FontSize(20).Bold();
                        col.Item().Text(invoice.ClientAddress);

                        if (!string.IsNullOrWhiteSpace(invoice.Subject))
                            col.Item().PaddingTop(4)
                                      .Text(invoice.Subject).Italic();
                    });

                    row.RelativeItem().AlignRight().Column(col =>
                    {
                        col.Item().Text("INVOICE").FontSize(22).Bold();
                        col.Item().Text($"Invoice # {invoice.InvoiceNumber}");
                        col.Item().Text($"Date: {invoice.IssueDate:MM/dd/yyyy}");
                        col.Item().Text($"Due On: {invoice.DueDate:MM/dd/yyyy}");
                    });
                });

                /*──────────────── CONTENT ────────────────*/
                page.Content().PaddingVertical(10).Column(col =>
                {
                    /* TIME ENTRIES */
                    if (timeEntries.Any())
                    {
                        col.Item().PaddingTop(10).Text("Consultations").Bold();
                        col.Item().Element(ComposeTimeTable);
                    }

                    /* EXPENSE ENTRIES */
                    if (expenseEntries.Any())
                    {
                        col.Item().PaddingTop(10).Text("Expenses").Bold();
                        col.Item().Element(ComposeExpenseTable);
                    }

                    /* TOTALS */
                    col.Item().PaddingTop(12).AlignRight().Column(total =>
                    {
                        // Calculate discount amount
                        var discountAmount = invoice.DiscountType == "%"
                            ? (invoice.TotalAmount * invoice.DiscountValue) / (100 - invoice.DiscountValue)
                            : invoice.DiscountValue;

                        // Subtotal is Total + Discount
                        var subtotal = invoice.TotalAmount + discountAmount;

                        // Final paid & balance logic
                        var isPaid = invoice.Status?.Equals("Paid", StringComparison.OrdinalIgnoreCase) == true;
                        var paidAmount = isPaid ? invoice.TotalAmount : invoice.PaidAmount;
                        var balanceDue = isPaid ? 0 : invoice.TotalAmount - paidAmount;


                        // Subtotal
                        total.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Subtotal");
                            r.ConstantItem(90).AlignRight()
                                .Text($"{currency} {subtotal:0.00}");
                        });

                        // Discount
                        if (discountAmount > 0)
                        {
                            total.Item().Row(r =>
                            {
                                r.RelativeItem().Text($"Discount ({invoice.DiscountValue}{invoice.DiscountType})");
                                r.ConstantItem(90).AlignRight()
                                    .Text($"- {currency} {discountAmount:0.00}");
                            });
                        }

                        // Paid
                        total.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Paid");
                            r.ConstantItem(90).AlignRight()
                                .Text($"{currency} {paidAmount:0.00}");
                        });

                        // Balance
                        total.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Due Amount").Bold();
                            r.ConstantItem(90).AlignRight()
                                .Text($"{currency} {balanceDue:0.00}").Bold();
                        });

                    });


                    /* NOTES */
                    col.Item().PaddingTop(20).Text(invoice.FooterNotes);
                    col.Item().Text(invoice.PaymentProfileNotes);
                });

                /* FOOTER: page numbers */
                page.Footer().AlignCenter().Text(txt =>
                {
                    txt.CurrentPageNumber();
                    txt.Span(" / ");
                    txt.TotalPages();
                });


                /*──────────── helper tables ────────────*/
                void ComposeTimeTable(IContainer container)
                {
                    container.Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(3);
                            c.ConstantColumn(70); // date
                            c.ConstantColumn(50); // hrs
                            c.ConstantColumn(70); // rate
                            c.ConstantColumn(80); // total
                        });

                        StyleHeader(table, "Description", "Date", "Hrs", "Rate", "Total");

                        foreach (var e in timeEntries)
                        {
                            table.Cell().Text(e.Description ?? "—");
                            table.Cell().Text(e.Date.ToString("MM/dd/yyyy"));
                            table.Cell().AlignCenter().Text($"{e.Duration.TotalHours:0.##}");
                            table.Cell().AlignCenter().Text($"{currency} {e.RatePerHour:0.00}");
                            table.Cell().AlignCenter().Text($"{currency} {e.Amount:0.00}");
                        }
                    });
                }

                void ComposeExpenseTable(IContainer container)
                {
                    container.Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(3);
                            c.ConstantColumn(70); // date
                            c.ConstantColumn(50); // qty
                            c.ConstantColumn(70); // rate
                            c.ConstantColumn(80); // total
                        });

                        StyleHeader(table, "Description","Date", "Qty", "Rate", "Total");

                        foreach (var e in expenseEntries)
                        {
                            table.Cell().Text(e.Description ?? "—");
                            table.Cell().Text(e.Date.ToString("MM/dd/yyyy"));
                            table.Cell().AlignCenter().Text($"{e.Quantity}");
                            table.Cell().AlignCenter().Text($"{currency} {e.Rate:0.00}");
                            table.Cell().AlignCenter().Text($"{currency} {e.Amount:0.00}");
                        }
                    });
                }

                /* HEADER CELLS styling fix */
                static void StyleHeader(TableDescriptor table, params string[] headers)
                {
                    table.Header(header =>
                    {
                        foreach (var h in headers)
                        {
                            header.Cell()
                                  .Background("#D7E3F4")
                                  .PaddingVertical(2).PaddingHorizontal(4)
                                  .Text(t => t.Span(h).Bold());
                        }
                    });
                }
            });
        })
        .GeneratePdf(stream);

        return Task.FromResult(stream.ToArray());
    }
}
