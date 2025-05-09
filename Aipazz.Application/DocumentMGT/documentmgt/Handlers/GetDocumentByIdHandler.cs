using System.Xml.Linq;
using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Application.DocumentMGT.DTO;
using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Domian.DocumentMgt;
using DocumentFormat.OpenXml.Packaging;
using MediatR;
using OpenXmlPowerTools;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class GetDocumentByIdHandler : IRequestHandler<GetDocumentByIdQuery, DocumentHtmlResponse?>
    {
        private readonly IdocumentRepository _repository;

        public GetDocumentByIdHandler(IdocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<DocumentHtmlResponse?> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _repository.GetByIdAsync(request.DocumentId, request.UserId);
            if (document == null || string.IsNullOrWhiteSpace(document.HtmlUrl))
            {
                Console.WriteLine("Document not found or URL is null/empty.");
                return null;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), document.HtmlUrl.Replace("/", Path.DirectorySeparatorChar.ToString()));
            Console.WriteLine($"File path: {filePath}");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist at the specified path.");
                return null;
            }

            try
            {
                string htmlContent = await System.IO.File.ReadAllTextAsync(document.HtmlUrl);
                return new DocumentHtmlResponse
                    {
                        HtmlContent = htmlContent
                };
                
            }
            catch (Exception ex)
            {
                // Log any exceptions
                Console.WriteLine($"Error: {ex.Message}");
                return null; // Or handle the exception as needed
            }
        }
    }
}
