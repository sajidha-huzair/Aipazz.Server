using Aipazz.Application.Common.Aipazz.Application.Common;
using Aipazz.Domian.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public static class EmailTemplateGenerator
    {
        public static string GenerateInvoiceLinkEmail(Invoice invoice, string token, IUserContext userContext)
        {
            string senderName = userContext.FullName;

            var link = $"http://localhost:5173/view-invoice?token={token}";

            return $@"
        <html>
        <body style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
            <h2 style='color: #2c3e50;'>Dear {invoice.ClientName},</h2>

            <p>You have received an invoice from <strong>{senderName}</strong>.</p>

            <table style='margin: 10px 0;'>
                <tr><td><strong>Client:</strong></td><td>{invoice.ClientName}</td></tr>
                <tr><td><strong>Matter:</strong></td><td>{string.Join(", ", invoice.MatterTitles)}</td></tr>
                <tr><td><strong>Total Amount:</strong></td><td>Rs. {invoice.TotalAmount:N2}</td></tr>
            </table>

            <p>Click the button below to view and download your invoice securely:</p>

            <p style='margin: 20px 0;'>
                <a href='{link}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                    View Invoice
                </a>
            </p>

            <p style='font-size: 0.9em; color: #888;'>
                This link will expire in 30 days. If you did not expect this invoice, you may safely ignore this message.
            </p>

            <br/>
            <p>
                Regards,<br/>
                <strong>{senderName}</strong><br/>
                <em>Law Office of {senderName}</em>
            </p>
        </body>
        </html>";
        }

    }
}
