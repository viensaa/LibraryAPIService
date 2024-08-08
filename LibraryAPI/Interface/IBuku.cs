using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.Buku;
using LibraryAPI.Models;

namespace LibraryAPI.Interface
{
    public interface IBuku :ICRUD <Buku>
    {
        Task<IEnumerable<Data>> GetAllInfoBuku();

        Task<Data> InfoByNama(RequestDataBuku request);
        Task<IEnumerable<Data>> InfoByCustom(RequestDataCustom request);

        Task<ResponseBase>InsertV2(RequestInsert request);

        Task<ResponseBase> UpdateV2(RequestUpdate request);
    }
}
