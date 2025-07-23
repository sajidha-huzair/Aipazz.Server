using Aipazz.Application.Calender.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Aipazz.API.Controllers.Calender;

[Route("api/[controller]")]
[ApiController]
public class EmailController(IEmailService emailService) : ControllerBase
{
    
    


    [HttpPost("clients/sendEmailtoClient")]
    public async Task<IActionResult> sendEmailAsync(string receptor, string subject, string body)
    {
        await emailService.sendEmaiToClient(receptor, subject, body);
        return Ok();
    }
}