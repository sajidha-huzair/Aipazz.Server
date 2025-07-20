using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Interfaces
{
    public interface IInvoiceBlobService
    {
        /// <summary>Uploads a PDF and returns its public (or SAS) URL.</summary>
        Task<string> SavePdfAsync(string userId, string invoiceId, byte[] pdfBytes);

        /// <summary>Deletes a previously‑uploaded PDF (if needed).</summary>
        Task<bool> DeletePdfAsync(string pdfUrl);
    }
}
