using AOTableModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public async Task<ICollection<AOTable>> GetTable(string name, string[] typeList)
        {
            if (!string.IsNullOrEmpty(name)&&typeList.Length==0)
            {
                return await _context.Tables.Where(x => x.Name.Contains(name)).ToListAsync();
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

        public async Task<AOTable> AddTable(AOTable table)
        {
             await _context.Tables.AddAsync(table);
            var saved=await _context.SaveChangesAsync();
            return saved > 0 ? table:null ;

        }
        public async Task<AOTable> ViewTable(Guid id)
        {
            var table=await _context.Tables.Where(x => x.Id == id).SingleOrDefaultAsync();
            return table;
        }
        public async Task<AOTable> EditTable(AOTable table)
        {
            _context.Tables.Entry(table).State = EntityState.Modified;
            var change=await _context.SaveChangesAsync();
            return change > 0 ? table : null;
        }
        public bool IsExists(Guid id)
        {
            return _context.Tables.Any(x => x.Id == id);
        }

        public bool IsTypeExists(string typeName)
        {
            typeName = typeName.ToLower();
            switch (typeName)
            {
                case "coverage":
                    return true;
                case "form":
                    return true;
                case "policy":
                    return true;
                case "risk":
                    return true;
                case "schedule":
                    return true;
                default: return false;

            }
        }

        public  bool DeleteTable(Guid id)
        {
            if (DeleteTableCheck(id))
            {
                var table = _context.Tables.Find(id);
                _context.Tables.Remove(table);
                return _context.SaveChanges() > 0 ? true : false;
            }
            else
            {
                return false;
            }
        }
         
        public bool DeleteTableCheck(Guid id)
        {
           var column=_context.AOColumns.Where(x=>x.TableId==id).SingleOrDefault();
           var form=_context.Forms.Where(x => x.TableId == id).SingleOrDefault();
            if( column!=null | form != null)
            {
                return false;
            }
           
                return true;
            

        }
    }
}

