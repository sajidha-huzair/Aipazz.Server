using MediatR;
using System;
using System.Collections.Generic;
using Aipazz.Domian.Matters;
using Aipazz.Application.Matters.DTO;

namespace Aipazz.Application.Matters.matter.Commands
{
    public class CreateMatterCommand : IRequest<MatterDto>
    {
        public string Title { get; set; } = string.Empty;
        public string CaseNumber { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string? Description { get; set; } = string.Empty;
        public CourtType? CourtType { get; set; }
        public string ClientNic { get; set; } = string.Empty;
        public string StatusId { get; set; } = string.Empty;
        public string MatterTypeName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; // Now this can be set in controller
        //public string? TeamId { get; set; } = null;
        public DateTime? UpdatedAt { get; set; }

        public List<string>? TeamMembers { get; set; } = new();
    }
}
