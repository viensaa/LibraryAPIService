using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Mahasiswa
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string NIM { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Class { get; set; }
    }
}
