using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOTableModel
{
    [Table("Form")]

    public class Form
    {
         
        public Guid id { get; set; }
        public Guid TableId { get; set; }
    }
}

