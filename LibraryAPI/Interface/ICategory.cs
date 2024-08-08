using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.Category;
using LibraryAPI.Models;

namespace LibraryAPI.Interface
{
    public interface ICategory : ICRUD<Category>
    {
        Task<ResponseBase> InsertV2(RequestCategory obj);
        Task<ResponseBase> UpdateV2(RequestCategory obj);

    }
}
