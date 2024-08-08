namespace LibraryAPI.DomainObject.StorageLocation
{
    public class StorageLocationData
    {
        public int ID { get; set; }
        public string Location { get; set; }

    }

    public class ListStorageLocation :ResponseBase
    {
        public ICollection<StorageLocationData> data { get; set; }
    }

}
