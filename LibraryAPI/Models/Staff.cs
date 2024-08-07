using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Staff
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
