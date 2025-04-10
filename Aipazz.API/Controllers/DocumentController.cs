using Aipazz.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        // Constructor injection of the service
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        [HttpGet]
        public IActionResult GetAllDocuments()
        {
            var documents = _documentService.GetAllDocuments();
            return Ok(documents);
        }
    }
}
