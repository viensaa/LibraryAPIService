using TransaksiService.DomainObject;
using TransaksiService.DomainObject.Transaction;

namespace TransaksiService.Interface
{
    public interface ITransaction
    {
        Task<IEnumerable<DataTransaction>> GetAllData(FilterRequest request);
        Task<BaseResponse> CreateTransaction (TransactionRequest request);
        
    }
}
