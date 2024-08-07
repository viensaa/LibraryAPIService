using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    public class Buku
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        public int PublisherID { get; set; }
        [ForeignKey("PublisherID")]
        public Publisher Publisher { get; set; }

        public int StorageLocationID { get; set; }
        [ForeignKey("StorageLocationID")]
        public StorageLocation StorageLocation { get; set; }
        public int Jumlah { get; set; }
        public int InStock { get; set; }

        
        

        

    }
}
