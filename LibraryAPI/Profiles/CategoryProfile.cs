using AutoMapper;
using LibraryAPI.DomainObject.Category;
using LibraryAPI.Models;

namespace LibraryAPI.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, DataCategory>();
            CreateMap<DataCategory, Category >();

        }
    }
}
