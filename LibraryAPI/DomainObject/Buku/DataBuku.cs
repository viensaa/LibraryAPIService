using LibraryAPI.DomainObject.Category;
using LibraryAPI.DomainObject.Publisher;
using LibraryAPI.DomainObject.StorageLocation;

namespace LibraryAPI.DomainObject.Buku
{
    public class Data
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DataCategory Category { get; set; }
        public DataPublisher  Publisher { get; set; }
        public StorageLocationData Lokasi { get; set; }
        public int Jumlah { get; set; }
        public int InStock { get; set; }
    }

    public class dataBuku : ResponseBase
    {
        public Data Data { get; set; }
    }

    public class listofbuku : ResponseBase 
    {
        public IEnumerable<Data> Data { get;set; }
    }

}
