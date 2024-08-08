using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.StorageLocation;
using LibraryAPI.Models;

namespace LibraryAPI.Interface
{
    public interface IStorageLocation :ICRUD<StorageLocation>
    {
        // Task<ListStorageLocation> GetAll();
        Task<ResponseBase> InsertV2(RequestStorageLocation obj);
       // Task<ResponseBase> UpdateV2(RequestStorageLocation obj);
    }
}
