using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.OtherDocuments.DTOs;
using Aipazz.Application.OtherDocuments.Interfaces;
using Aipazz.Application.OtherDocuments.Queries;
using Aipazz.Domian.OtherDocuments;
using MediatR;

namespace Aipazz.Application.OtherDocuments.Handlers
{
    public class GetOtherDocumentsByMatterIdHandler : IRequestHandler<GetOtherDocumentsByMatterIdQuery, IEnumerable<OtherDocumentResponse>>
    {
        private readonly IOtherDocumentRepository _otherDocumentRepository;
        public GetOtherDocumentsByMatterIdHandler(IOtherDocumentRepository otherDocumentRepository)
        {
            _otherDocumentRepository = otherDocumentRepository;
        }

        public async Task<IEnumerable<OtherDocumentResponse>> Handle(GetOtherDocumentsByMatterIdQuery request, CancellationToken cancellationToken)
        {

            Console.WriteLine($"GetOtherDocumentsByMatterIdHandler: Handling request for MatterId: {request.MatterId} and UserId: {request.UserId}");
            var response  =  await _otherDocumentRepository.GetByMatterIdAsync(request.MatterId, request.UserId);

            return response.Select(doc => new OtherDocumentResponse
            {
                Id = doc.id,
                FileName = doc.FileName,
                OriginalFileName = doc.OriginalFileName,
                ContentType = doc.ContentType,
                FileSize = doc.FileSize,
                UserName = doc.UserName,
                FileUrl = doc.FileUrl,
                MatterId = doc.MatterId,
                MatterName = doc.MatterName,
                CreatedAt = doc.CreatedAt,
                LastModifiedAt = doc.LastModifiedAt

            });
        }

       
    }
}
