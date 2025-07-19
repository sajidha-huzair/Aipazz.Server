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
        public static string GenerateInvoiceLinkEmail(Invoice invoice, string token,string SenderEmail)
        {
            // You can customize this base URL to your frontend view page
            var link = $"https://witty-field-0e9483e0f.6.azurestaticapps.net/view-invoice?token={token}";

            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <h2>Hello,</h2>
                    <p>You have received a new invoice from <strong>Aipazz Legal</strong>.</p>

                    <strong>Client:</strong> {invoice.ClientName}<br/>
                    <strong>Matter:</strong> {string.Join(", ", invoice.MatterTitles)}<br/>
                    <strong>Total Amount:</strong> Rs. {invoice.TotalAmount:N2}</p>

                    <p>You can view and download your invoice securely by clicking the link below:</p>

                    <p><a href='{link}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>View Invoice</a></p>

                    <p>This link will expire in 1 hour. If you did not request this invoice, please ignore this email.</p>

                    <br/>
                    <p>Best regards,<br/>The Aipazz Team</p>
                </body>
                </html>";
        }
    }
}
