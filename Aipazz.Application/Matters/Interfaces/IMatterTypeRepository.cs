using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aipazz.Domian.Matters;

namespace Aipazz.Application.Matters.Interfaces
{
    public interface IMatterTypeRepository
    {
        Task<List<MatterType>> GetAllMatterTypes(string userId);
        Task<MatterType?> GetMatterTypeById(string id, string userId);
        Task<MatterType?> GetMatterTypeByName(string name, string userId);
        Task AddMatterType(MatterType type);
        Task UpdateMatterType(MatterType type);
        Task DeleteMatterType(string id, string userId);
    }
}

