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

            try
            {
                using var httpClient = new HttpClient();
                var htmlContent = await httpClient.GetStringAsync(document.HtmlUrl);

                return new DocumentHtmlResponse
                {
                    HtmlContent = htmlContent,
                    DocumentId = document.id,
                    UserId = document.Userid,
                    UserName = document.UserName, // Add this
                    FileName = document.FileName,
                    MatterId = document.MatterId, // Add this
                    TeamId = document.TeamId, // Add this
                    Url = document.Url,
                    HtmlUrl = document.HtmlUrl,
                    CreatedAt = document.CreatedAt,
                    LastModifiedAt = document.LastModifiedAt,

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving HTML from blob: {ex.Message}");
                return null;
            }
        }
    }
}
