using Aipazz.Domian.Matters;
using MediatR;
using System;
using System.Collections.Generic;

namespace Aipazz.Application.Matters.matter.Commands
{
    public class UpdateMatterCommand : IRequest<Matter>
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty; // Partition Key
        public string CaseNumber { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public List<string> TeamMembers { get; set; } = new();
    }
}
