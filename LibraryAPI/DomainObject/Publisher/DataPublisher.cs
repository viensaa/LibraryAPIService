namespace LibraryAPI.DomainObject.Publisher
{
    public class DataPublisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
    public class objDataPublisher : ResponseBase
    {
        public DataPublisher data { get; set; }

    }

}
