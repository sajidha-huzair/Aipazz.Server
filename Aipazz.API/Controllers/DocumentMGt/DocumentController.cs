using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.DocumentMGt
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public DocumentController(IMediator mediator)
        {
            _mediatR = mediator;

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediatR.Send(new GetAllDcoumentsQuery());
            return Ok(result);
        }
    }
}
