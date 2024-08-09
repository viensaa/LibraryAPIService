using TransaksiService.DomainObject.Mahasiswa;
using TransaksiService.Model;

namespace TransaksiService.DomainObject.Transaction
{
    public class DataTransaction
    {
        public int TransactionId { get; set; }
        public int TransactionType { get; set; }
        public DataMahasiswa Mahasiswa { get; set; }
        public ICollection<DataTransactionDetail> Details { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Status { get; set; }

    }

    public class listDataTransaction : BaseResponse
    {
        public IEnumerable<DataTransaction> datas { get; set; }
    }
}
