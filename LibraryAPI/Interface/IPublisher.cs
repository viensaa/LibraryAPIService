using LibraryAPI.DomainObject.Publisher;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Interface
{
    public interface IPublisher :ICRUD<Publisher>
    {
        Task<IEnumerable<Publisher>>GetByName(string name);
        Task<objDataPublisher> InsertV2(InsertPublisher obj);
    }
}
