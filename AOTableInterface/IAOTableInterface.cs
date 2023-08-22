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
    }
}
