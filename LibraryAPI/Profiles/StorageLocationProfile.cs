using AutoMapper;
using LibraryAPI.DomainObject.StorageLocation;
using LibraryAPI.Models;

namespace LibraryAPI.Profiles
{
    public class StorageLocationProfile : Profile
    {
        public StorageLocationProfile() 
        {
            CreateMap<StorageLocationData, StorageLocation>();
            CreateMap<StorageLocation, StorageLocationData> ();

        }
    }
}
