using Aipazz.Application.OtherDocuments.DTOs;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Commands
{
    public record UploadFileCommand(
        UploadFileRequest Request,
        string UserId,
        string UserName
    ) : IRequest<string>;
}