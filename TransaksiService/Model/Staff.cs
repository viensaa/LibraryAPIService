
namespace TransaksiService.Model
{
    public class Staff
    {
        public int NIP { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        // Navigation property
        public ICollection<Transaction> Transactions { get; set; }
    }
}
