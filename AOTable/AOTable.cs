using System.ComponentModel.DataAnnotations.Schema;

namespace AOTableModel
{
    [Table("AOTable")]
    public class AOTable
    {
        public Guid Id { get; set; }
        public string ?Name { get; set; }
        public string ?Type { get; set; }
        public string ?Description { get; set; }
        public string ?Comment { get; set; }
        public bool preminum=false;

    }
}