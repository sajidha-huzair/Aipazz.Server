using Aipazz.Application.Billing.Interfaces;
using Aipazz.Application.Billing.Invoices.Commands;
using Aipazz.Application.Notification.Interfaces;
using Aipazz.Application.Notification.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Billing.Invoices.Handlers
{
    public class MarkInvoicePaidHandler : IRequestHandler<MarkInvoicePaidCommand, bool>
    {
        private readonly IInvoiceRepository _repo;
        private readonly INotificationService _notifier;
        private readonly INotificationRepository _notificationRepository;


        public MarkInvoicePaidHandler(IInvoiceRepository repo, INotificationService notifier, INotificationRepository notificationRepository)
        {
            _repo = repo;
            _notifier = notifier;
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(MarkInvoicePaidCommand request, CancellationToken ct)
        {
            var invoice = await _repo.GetByIdAsync(request.InvoiceId, request.UserId);
            if (invoice == null) return false;

            invoice.Status = "Paid";
            invoice.PaymentDate = DateTime.UtcNow;
            invoice.TransactionId = request.TransactionId;
            await _repo.UpdateAsync(invoice);

            // Use the service — not direct repository
            await _notifier.NotifyLawyerPaymentReceived(invoice);

            return true;
        }

    }
}


