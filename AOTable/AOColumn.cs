using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOTableModel
{
    [Table("AOColumn")]

    public class AOColumn
    {

        public Guid Id { get; set; }
        public Guid TableId { get; set; }
    }
}
