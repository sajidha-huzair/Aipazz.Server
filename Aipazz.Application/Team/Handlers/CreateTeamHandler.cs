using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Notification.Services;
using Aipazz.Application.Team.Commands;
using Aipazz.Application.Team.Interfaces;
using Aipazz.Domian.Team;
using MediatR;

namespace Aipazz.Application.Team.Handlers
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, string>
    {
        private readonly ITeamRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public CreateTeamHandler(ITeamRepository repository, INotificationService notificationService,IEmailService emailService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        public async Task<string> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = new Aipazz.Domian.Team.Team
            {
                id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                OwnerId = request.UserId,
                OwnerName = request.OwnerName,
                Members = request.Members ?? new List<TeamMember>(),
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow,
                IsActive = true
            };

            // Create the team
            var teamId = await _repository.CreateTeamAsync(team);

            // Send notifications to owner and members
            var memberIds = team.Members.Select(m => m.UserId).ToList();
            await _notificationService.SendTeamCreatedNotificationAsync(
                teamId, 
                team.Name, 
                team.OwnerName, 
                team.OwnerId, 
                memberIds
            );
            await _emailService.SendEmailToTeamCreator(
                request.OwnerEmail, 
                "Your team has been created", 
                $"<p>Dear {team.OwnerName},</p><p>Your team '{team.Name}' has been successfully created.</p>"
            );

            return teamId;
        }
    }
}