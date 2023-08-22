using AOTableModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDbContext;

namespace AOTableInterface.Repository
{
    public class AOTableRepository : IAOTableInterface
    {
        private readonly ApplicationDbContext _context;

        public AOTableRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #region Get Search Table
        public async Task<ICollection<AOTable>> GetTable(string? name, string[] typeList)
        {
            if (!string.IsNullOrEmpty(name)&&typeList.Length==0)
            {
                return await _context.Tables.Where(x=>x.Name.Contains(name)).ToListAsync();
            }
            else if(!string.IsNullOrEmpty(name)&&typeList.Length==1)
            {
                return await _context.Tables.Where(x => x.Type == typeList[0] && x.Name.Contains(name)).ToListAsync();
            }else if (typeList.Length > 0 && string.IsNullOrEmpty(name))
            {
                return await _context.Tables.Where(x=>typeList.Contains(x.Type)).ToListAsync();
            }
            else if(typeList.Length > 0 && !string.IsNullOrEmpty(name))
            {
                return await _context.Tables.Where(x => typeList.Contains(x.Type) && x.Name.Contains(name)).ToListAsync();
            }
            else
            {
                return null;
            }          
        }
        #endregion
    }
}

