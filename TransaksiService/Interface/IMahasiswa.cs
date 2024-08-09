using TransaksiService.DomainObject;
using TransaksiService.DomainObject.Mahasiswa;
using TransaksiService.Model;

namespace TransaksiService.Interface
{
    public interface IMahasiswa
    {
        Task<IEnumerable<DataMahasiswa>>GetAll();

        Task<IEnumerable<DataMahasiswa>> GetDatabyCustom (RequestSearch obj);
        Task<BaseResponse>CreateData(CreateMahasiswa obj);
        Task<BaseResponse>ChangeData(RequestChangeData obj);
        Task<BaseResponse> DeleteData(string nim);

    }
}
