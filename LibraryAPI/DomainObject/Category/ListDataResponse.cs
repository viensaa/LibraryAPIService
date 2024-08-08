using System.Collections.ObjectModel;

namespace LibraryAPI.DomainObject.Category
{
    public class ListDataResponse : ResponseBase
    {
        public IEnumerable<DataCategory> data { get; set; }
    }
}
