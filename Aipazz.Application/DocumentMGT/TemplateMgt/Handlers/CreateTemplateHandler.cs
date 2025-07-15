using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.TemplateMgt.Commands;
using MediatR;

public class CreateTemplateHandler : IRequestHandler<CreateTemplateCommand, Unit>
{
    private readonly ITemplateRepository _repository;
    private readonly IFileStorageService _fileStorage;
    public CreateTemplateHandler(ITemplateRepository repository, IFileStorageService fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<Unit> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid().ToString();
        var url = await _fileStorage.SaveTemplateAsync(id, request.Template.Name, request.Template.ContentHtml);
        
        request.Template.id = id;
        request.Template.Url = url;
        request.Template.CreatedAt = DateTime.UtcNow;
        request.Template.LastModified = DateTime.UtcNow;
        
        await _repository.CreateTemplate(request.Template);
        return Unit.Value;
    }
}
