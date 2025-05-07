using Aipazz.Application.DocumentMGT.Interfaces;
using Aipazz.Application.DocumentMGT.TemplateMgt.Commands;
using MediatR;

public class CreateTemplateHandler : IRequestHandler<CreateTemplateCommand, Unit>
{
    private readonly ITemplateRepository _repository;
    public CreateTemplateHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        await _repository.CreateTemplate(request.Template);
        return Unit.Value;
    }
}
