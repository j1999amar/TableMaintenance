using AOTableModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOTableInterface
{
    public interface IAOTableInterface
    {
        public Task<ICollection<AOTable>> GetTable(string name, string[] typeList);
        public Task<AOTable> AddTable(AOTable table);
        public Task<AOTable> ViewTable(Guid id);
        public bool  IsExists(Guid id);
        public bool IsTypeExists(string typeName);
    }
}
