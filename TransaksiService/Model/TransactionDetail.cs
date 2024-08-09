using TransaksiService.DomainObject;

namespace TransaksiService.Model
{
    public class TransactionDetail
    {
        public int TransactionDetailId { get; set; }
        public int TransactionId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int Status { get; set; } = Convert.ToInt32(StatusTransaction.pending); // Default to "Pending"

        
        public  Transaction Transaction { get; set; }
        
       
    }
}
