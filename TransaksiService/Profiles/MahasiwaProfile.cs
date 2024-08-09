using AutoMapper;
using TransaksiService.DomainObject.Mahasiswa;
using TransaksiService.Model;

namespace TransaksiService.Profiles
{
    public class MahasiwaProfile : Profile
    {
        public MahasiwaProfile()
        {
            CreateMap<DataMahasiswa, Mahasiswa>();
            CreateMap<Mahasiswa, DataMahasiswa>();
        }
    }
}
