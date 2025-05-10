using Aipazz.Domian.Matters;
using MediatR;
using System.Collections.Generic;

namespace Aipazz.Application.Matters.matter.Queries
{
    public record GetAllMattersQuery() : IRequest<List<Matter>>;
}
