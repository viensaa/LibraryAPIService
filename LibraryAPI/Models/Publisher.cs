using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Publisher
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public ICollection<Buku> Bukus { get; set; }
    }
}
