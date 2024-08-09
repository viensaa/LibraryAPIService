using AutoMapper;
using TransaksiService.DomainObject.Transaction;
using TransaksiService.Model;

namespace TransaksiService.Profiles
{
    public class TransactionProfile :Profile
    {
        public TransactionProfile()
        {
            CreateMap<DataTransaction, Transaction>();
            CreateMap<Transaction, DataTransaction>();

            CreateMap<Transaction, DataTransactionDetail>();
            CreateMap<DataTransactionDetail, Transaction>();
        }
    }
}
