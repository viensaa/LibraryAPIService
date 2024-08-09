namespace TransaksiService.DomainObject.Transaction
{
    public class TransactionRequest
    {
        public string NIM { get; set; }
        public int BukuID { get; set; }
        public int TransactionType { get; set; } 
        public int Quantity { get; set; }

        public DateTime BorrowDate { get; set; } 
        public DateTime ReturnDate { get; set; } 
    }
}
