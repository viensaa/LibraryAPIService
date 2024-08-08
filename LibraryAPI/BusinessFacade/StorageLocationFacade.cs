using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.StorageLocation;
using LibraryAPI.Interface;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.BusinessFacade
{
    public class StorageLocationFacade : IStorageLocation
    {
        
        private readonly DataContext _context;

        public StorageLocationFacade(DataContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            try
            {
                var findData = await _context.storageLocations.SingleOrDefaultAsync(x => x.ID == id);
                if (findData == null)
                {
                    throw new Exception($"StorageLocation dengan ID {id} Tidak DiTemukan ");

                }
                _context.Remove(findData);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<StorageLocation>> GetAll()
        {
            var results = await _context.storageLocations.OrderBy(x => x.ID).ToListAsync();
            return results;
        }

        public async Task<StorageLocation> GetById(int id)
        {
            var result = await _context.storageLocations.FirstOrDefaultAsync(x=> x.ID == id);
            return result;
        }

        public Task<StorageLocation> Insert(StorageLocation entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseBase> InsertV2(RequestStorageLocation obj)
        {
            ResponseBase response = new();
            try
            {
                var checkDuplicate = await _context.storageLocations.FirstOrDefaultAsync(x=>x.Location == obj.Location);
                if (checkDuplicate != null) 
                {
                    response.Message = $"Storage Location {obj.Location} Sudah Terdaftar";
                    response.StatusCode = false;

                    return response;
                }
                StorageLocation dataInput = new StorageLocation();
                dataInput.Location = obj.Location;
                _context.storageLocations.Add(dataInput);
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

        public Task<StorageLocation> Update(StorageLocation entity)
        {
            throw new NotImplementedException();
        }

        //Tidak Jadi digunakan
        //public async Task<ResponseBase> UpdateV2(RequestStorageLocation obj)
        //{
        //    ResponseBase response = new();
        //    try
        //    {
        //        var FindData = await _context.storageLocations.FirstOrDefaultAsync(x => x.Location == obj.Location);
        //        if (FindData == null)
        //        {
        //            response.Message = $"Storage Location {obj.Location} Tidak Terdaftar";
        //            response.StatusCode = false;

        //            return response;
        //        }
        //        FindData.Location = obj.Location;                
        //        await _context.SaveChangesAsync();

        //        response.Message = $"Update Data {obj.Location} Success";
        //        response.StatusCode = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return response;
        //}
    }
}
