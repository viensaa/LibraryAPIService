namespace TransaksiService.DomainObject
{
    public enum StatusTransaction
    {
        complete = 0,
        OnBorrow = 1,
        Overdue = 2,
        pending = 3,
    }
    public enum TransactionType
    {
        returning = 0,
        borrow = 1,
    }
}
