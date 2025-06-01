using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aipazz.Application.Matters.DTO
{
    public class UpdateMatterStatusDto
    {
        public string NewStatusId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
