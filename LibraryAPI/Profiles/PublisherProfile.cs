using AutoMapper;
using LibraryAPI.DomainObject.Publisher;
using LibraryAPI.Models;

namespace LibraryAPI.Profiles
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile() 
        {
            CreateMap<Publisher, DataPublisher>();
            CreateMap<DataPublisher, Publisher>();
            CreateMap<InsertPublisher, Publisher>();
            //CreateMap<InsertPublisher, DataPublisher>();
            //CreateMap<DataPublisher, InsertPublisher>();





        }
    }
}
