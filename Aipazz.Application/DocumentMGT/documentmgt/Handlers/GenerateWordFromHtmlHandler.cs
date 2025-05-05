using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Application.DocumentMGT.documentmgt.Queries;
using Aipazz.Application.DocumentMGT.Interfaces;
using MediatR;

namespace Aipazz.Application.DocumentMGT.documentmgt.Handlers
{
    public class GenerateWordFromHtmlHandler : IRequestHandler<GenerateWordFromHtmlQuery, byte[]>
    {
        private readonly IWordGenerator _wordGenerator;

        public GenerateWordFromHtmlHandler(IWordGenerator wordGenerator)
        {
            _wordGenerator = wordGenerator;

        }
        public Task<byte[]> Handle(GenerateWordFromHtmlQuery request, CancellationToken cancellationToken)
        {
            var fileBytes = _wordGenerator.GenerateFromHtml(request.HtmlContent);
            return Task.FromResult(fileBytes);
        }
    }
}
