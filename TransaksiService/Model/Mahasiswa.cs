namespace TransaksiService.Model
{
    public class Mahasiswa
    {
        public int Id { get; set; }
        public string NIM { get; set; }
        public string Nama { get; set; }
        public string Kelas { get; set; }
        public int RowStatus { get; set; }

        public ICollection<Transaction> transactions { get; set; }
    }
}
