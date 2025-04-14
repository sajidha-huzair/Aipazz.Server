//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Aipazz.Application.DocumentMGT.Interfaces;
//using Aipazz.Application.DocumentMGT.TemplateMgt.Queries;
//using MediatR;

//namespace Aipazz.Application.DocumentMGT.TemplateMgt.Handlers
//{
//    public class GenerateWordFromHtmlHandler : IRequestHandler<GenerateWordFromHtmlQuery, byte[]>
//    {
//        private readonly IWordGenerator _wordGenerator;

//        public GenerateWordFromHtmlHandler(IWordGenerator wordGenerator)
//        {
//            _wordGenerator = wordGenerator;
//        }


//        public Task<byte[]> Handle(GenerateWordFromHtmlQuery request, CancellationToken cancellationToken)
//        {
//            var fileBytes = _wordGenerator.GenerateFromHtml(request.HtmlContent);
//            return Task.FromResult(fileBytes);
//        }
//    }
//}
