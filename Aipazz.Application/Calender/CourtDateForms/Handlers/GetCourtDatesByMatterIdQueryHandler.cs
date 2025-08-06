using MediatR;
using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;

public class GetCourtDatesByMatterIdQueryHandler : IRequestHandler<GetCourtDatesByMatterIdQuery, List<CourtDateForm>>
{
    private readonly ICourtDateFormRepository _repository;

    public GetCourtDatesByMatterIdQueryHandler(ICourtDateFormRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CourtDateForm>> Handle(GetCourtDatesByMatterIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByMatterIdAsync(request.UserId, request.MatterId);
    }
}
