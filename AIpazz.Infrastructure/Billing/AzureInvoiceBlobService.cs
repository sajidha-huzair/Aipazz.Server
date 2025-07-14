using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Common;
using AIpazz.Infrastructure.Billing.Aipazz.Application.Common;

namespace AIpazz.Infrastructure.Billing
{
    public class AzureInvoiceBlobService : IInvoiceBlobService
    {
        private readonly BlobContainerClient _container;

        public AzureInvoiceBlobService(IOptions<InvoiceBlobOptions> opts)
        {
            var client = new BlobServiceClient(opts.Value.ConnectionString);
            _container = client.GetBlobContainerClient(opts.Value.ContainerName);
            _container.CreateIfNotExists(PublicAccessType.None); // Keep access controlled
        }

        public async Task<string> SavePdfAsync(string userId, string invoiceId, byte[] pdfBytes)
        {
            var blobName = $"{userId}/invoice_{invoiceId}.pdf";
            var blob = _container.GetBlobClient(blobName);

            var headers = new BlobHttpHeaders
            {
                ContentType = "application/pdf",
                ContentDisposition = $"inline; filename=\"invoice_{invoiceId}.pdf\""
            };

            using var stream = new MemoryStream(pdfBytes);
            await blob.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = headers
            });

            return blob.Uri.ToString();
        }

        public async Task<bool> DeletePdfAsync(string pdfUrl)
        {
            var uri = new Uri(pdfUrl);
            var blobName = string.Join('/', uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries).Skip(1));
            var blob = _container.GetBlobClient(blobName);
            var response = await blob.DeleteIfExistsAsync();
            return response.Value;
        }
    }
}
