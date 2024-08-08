using AutoMapper;
using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.Category;
using LibraryAPI.Interface;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace LibraryAPI.BusinessFacade
{
    public class CategoryFacade :ICategory
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoryFacade()
        {
        }

        public CategoryFacade(DataContext dataContext,IMapper mapper) 
        {
            _context = dataContext;
            _mapper = mapper;
        }

        public async Task Delete(int id)
        {
            try
            {
                var findData = await _context.categories.SingleOrDefaultAsync(x => x.ID == id);
                if (findData == null)
                {
                    throw new Exception($"Category dengan ID {id} Tidak DiTemukan ");

                }
                _context.Remove(findData);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var results = await _context.categories.OrderBy(x => x.ID).ToListAsync();
            return results;
        }


        public async Task<Category> GetById(int id)
        {
           var result =  await _context.categories.FirstOrDefaultAsync(x => x.ID == id);
            return result;
        }

        public Task<Category> Insert(Category entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseBase> InsertV2(RequestCategory obj)
        {
            ResponseBase response = new();
            try
            {
                var CheckDuplicate = await _context.categories.SingleOrDefaultAsync(x=> x.CategoryCode == obj.CategoryCode);
                if(CheckDuplicate != null) 
                {
                    response.Message = ($"Category Code{obj.CategoryCode} Sudah Tersedia");
                    response.StatusCode = false;
                    
                    return response;
                }
                Category dataInput = new Category();
                dataInput.CategoryCode = obj.CategoryCode;
                dataInput.Description = obj.Description;

                _context.categories.Add(dataInput);
                await _context.SaveChangesAsync();

                response.Message = "Insert Success";
                response.StatusCode = true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return response;
        }

        public Task<Category> Update(Category entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseBase> UpdateV2(RequestCategory obj)
        {
            ResponseBase response = new();
            try
            {
                var findData = await _context.categories.SingleOrDefaultAsync(x => x.CategoryCode == obj.CategoryCode);
                if (findData == null)
                {
                    response.Message = ($"Category Code{obj.CategoryCode} Tidak Tersedia");
                    response.StatusCode = false;

                    return response;
                }
                findData.Description = obj.Description;
                await _context.SaveChangesAsync();
                response.Message = "Update Success";
                response.StatusCode = true;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return response;
        }
    }
}
