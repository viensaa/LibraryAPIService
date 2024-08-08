using LibraryAPI.DomainObject.Publisher;
using LibraryAPI.Interface;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.BusinessFacade
{
    public class PublisherFacade : IPublisher
    {
        private readonly DataContext _context;

        public PublisherFacade(DataContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            try
            {
                var findData = await _context.publishers.SingleOrDefaultAsync(x => x.ID == id);
                if (findData == null)
                {
                    throw new Exception($"Publisher dengan ID {id} Tidak DiTemukan ");

                }
                _context.Remove(findData);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            

        }

        public async Task<IEnumerable<Publisher>> GetAll()
        {
            var result = await _context.publishers.OrderBy(x=> x.ID).ToListAsync();
            return result;
        }

        public async Task<Publisher> GetById(int id)
        {
            var result = await _context.publishers.SingleOrDefaultAsync(x => x.ID == id);
            //if (result == null) throw new Exception ($"Data Dengan ID {id} Tidak Ditemukan");
            return result;
        }

        public async Task<IEnumerable<Publisher>> GetByName(string name)
        {
            var result = await _context.publishers.Where(x=> x.Name.Contains(name)).ToListAsync();
            return result;
        }

        public async Task<Publisher> Insert(Publisher entity)
        {
            try
            {
                Publisher existingData = new Publisher();               
                existingData = await _context.publishers.SingleOrDefaultAsync(x => x.Name == entity.Name);
                DataPublisher Result = new DataPublisher();

                if (existingData != null)
                {
                    //throw new Exception ($"Publisher dengan nama {entity.Name} Sudah Tersedia");
                    //
                }

                _context.publishers.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Publisher> Update(Publisher entity)
        {
            try
            {
                var findData = await _context.publishers.SingleOrDefaultAsync( x => x.ID == entity.ID);
                if (findData == null) 
                {
                    throw new Exception($"Publisher dengan ID {entity.ID} Tidak DiTemukan ");
                }
                findData.Name = entity.Name;
                findData.Address = entity.Address;
                findData.Phone = entity.Phone;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<objDataPublisher> InsertV2(InsertPublisher obj)
        {
            objDataPublisher existingData = new objDataPublisher();
            try
            {

                var chekDuplicate = await _context.publishers.SingleOrDefaultAsync(x => x.Name == obj.Name);
                
                if (chekDuplicate != null)
                {
                    
                    existingData.Message = ($"Publisher dengan nama {obj.Name} Sudah Tersedia");
                    existingData.StatusCode = false;
                    
                    return existingData;
                }
                //populate data insert
                Publisher dataInput = new Publisher();
                dataInput.Name = obj.Name;
                dataInput.Address = obj.Address;
                dataInput.Phone = obj.Phone;

                _context.publishers.Add(dataInput);
                await _context.SaveChangesAsync();

                existingData.Message = "Insert Success";
                existingData.StatusCode = true;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return existingData;


        }

        
    }
}
