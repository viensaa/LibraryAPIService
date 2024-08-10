namespace TransaksiService.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int MahasiswaId { get; set; }
       // public int StaffId { get; set; }
        public int TransactionType { get; set; } // "1. borrow" or "2. return"
        public DateTime TransactionDate { get; set; }
       // public int Status { get; set; }


        //public Staff Staff { get; set; }
        public Mahasiswa Mahasiswa { get; set; }
        public ICollection<TransactionDetail> TransactionDetails { get; set; }

    }
}
