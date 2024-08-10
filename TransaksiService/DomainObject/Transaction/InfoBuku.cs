namespace TransaksiService.DomainObject.Transaction
{
    public class InfoBuku
    {
        public Data data;
        public string message { get; set; }
        public bool statusCode { get; set; }
    }
    public class Data
    {
        public int id { get; set; }
        public string title { get; set; }
        public Category category { get; set; }
        public Publisher publisher { get; set; }
        public Lokasi lokasi { get; set; }
        public int jumlah { get; set; }
        public int inStock { get; set; }
    }
    public class Category
    {
        public int id { get; set; }
        public string categoryCode { get; set; }
        public string description { get; set; }
    }
    public class Publisher
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
    }


    public class Lokasi
    {
        public int id { get; set; }
        public string location { get; set; }
    }


}
