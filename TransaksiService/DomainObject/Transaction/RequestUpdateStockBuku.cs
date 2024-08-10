namespace TransaksiService.DomainObject.Transaction
{
    public class RequestUpdateStockBuku
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public int StorageLocationId { get; set; }
        public int Jumlah { get; set; }
        public int InStock { get; set; }
    }
}
