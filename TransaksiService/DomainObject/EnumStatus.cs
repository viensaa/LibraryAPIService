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
        returning = 44,
        borrow = 45,
    }

    public enum enumStatusCode
    {
        success = 1,
        failure = 0,

    }
    public enum RowStatus
    {
        Active = 1,
        NonActive = 0,
    }

}
