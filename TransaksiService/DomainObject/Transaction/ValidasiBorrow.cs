namespace TransaksiService.DomainObject.Transaction
{
    public class ValidasiBorrow
    {
        public int MahasiswaId { get; set; }
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public int BookId { get; set; }
    }
}
