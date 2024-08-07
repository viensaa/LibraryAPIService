using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        public string CategoryCode { get; set; }
        public string Description { get; set; }

        public ICollection<Buku> Bukus { get; set; }
    }
}
