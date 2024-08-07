using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class StorageLocation
    {
        [Key]
        public int ID { get; set; }
        public string Location { get; set; }
    }
}
