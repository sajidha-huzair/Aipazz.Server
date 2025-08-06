using MediatR;
using Aipazz.Domian.Calender;

public class GetCourtDatesByMatterIdQuery : IRequest<List<CourtDateForm>>
{
    public string UserId { get; set; }
    public string MatterId { get; set; }

    public GetCourtDatesByMatterIdQuery(string userId, string matterId)
    {
        UserId = userId;
        MatterId = matterId;
    }
}
