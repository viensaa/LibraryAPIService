using AutoMapper;
using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.Buku;
using LibraryAPI.DomainObject.Category;
using LibraryAPI.DomainObject.Publisher;
using LibraryAPI.DomainObject.StorageLocation;
using LibraryAPI.Interface;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.BusinessFacade
{
    
    public class BukuFacade : IBuku
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BukuFacade(DataContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Delete(int id)
        {
            try
            {
                var findData = await _context.Bukus.SingleOrDefaultAsync(x => x.ID == id);
                if (findData == null)
                {
                    throw new Exception($"Buku dengan ID {id} Tidak DiTemukan ");

                }
                _context.Remove(findData);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Buku>> GetAll()
        {
            var results = await _context.Bukus.
                OrderBy(x=>x.ID)
                .Include(p => p.Publisher)
                .Include(c => c.Category)
                .Include(s => s.StorageLocation)
                .ToListAsync();
            return results;
        }



        public Task<Buku> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Buku> Insert(Buku entity)
        {
            throw new NotImplementedException();
        }

        public Task<Buku> Update(Buku entity)
        {
            throw new NotImplementedException();
        }





        public async Task<IEnumerable<Data>> GetAllInfoBuku()
        {
            var results = await _context.Bukus.OrderBy(x => x.ID)
                .Include(p => p.Publisher)
                 .Include(c => c.Category)
                  .Include(s => s.StorageLocation)
                .ToListAsync();

            //proses Mapping Data Manual
            List<Data> dataBukus = new List<Data>();
            foreach (var item in results) 
            {
                //Mapping Data Category
                DataCategory categoryBuku = new DataCategory();
                categoryBuku.Id = item.Category.ID;
                categoryBuku.CategoryCode = item.Category.CategoryCode;
                categoryBuku.Description = item.Category.Description;

                //Mapping Data Publisher
                DataPublisher PublisherBuku = new DataPublisher();
                PublisherBuku.Id = item.Publisher.ID;
                PublisherBuku.Name = item.Publisher.Name;
                PublisherBuku.Address = item.Publisher.Address;
                PublisherBuku.Phone = item.Publisher.Phone;

                //Mapping Data StorageLocation
                StorageLocationData LokasiBuku = new StorageLocationData();
                LokasiBuku.ID = item.StorageLocation.ID;
                LokasiBuku.Location = item.StorageLocation.Location;

                dataBukus.Add(new Data
                {
                   ID= item.ID,
                   InStock= item.InStock,
                   Jumlah= item.Jumlah,
                   Title= item.Title,
                   Category = categoryBuku,
                   Publisher = PublisherBuku,
                   Lokasi = LokasiBuku,

                });
            }

            return dataBukus;
        }

        public async Task<Data> InfoByNama(RequestDataBuku request)
        {
            try
            {
                 var results = await _context.Bukus.OrderBy(x => x.ID)
                .Include(p => p.Publisher)
                 .Include(c => c.Category)
                  .Include(s => s.StorageLocation)
                .SingleOrDefaultAsync(x => x.ID == request.BukuId);



            if (results == null) 
            {
                throw new Exception($"Buku dengan ID {request.BukuId} Tidak Ditemukan");
            }
            //proses Mapping Data Manual
            Data dataBukus = new Data();
           
                //Mapping Data Category
                DataCategory categoryBuku = new DataCategory();
                categoryBuku.Id = results.Category.ID;
                categoryBuku.CategoryCode = results.Category.CategoryCode;
                categoryBuku.Description = results.Category.Description;

                //Mapping Data Publisher
                DataPublisher PublisherBuku = new DataPublisher();
                PublisherBuku.Id = results.Publisher.ID;
                PublisherBuku.Name = results.Publisher.Name;
                PublisherBuku.Address = results.Publisher.Address;
                PublisherBuku.Phone = results.Publisher.Phone;

                //Mapping Data StorageLocation
                StorageLocationData LokasiBuku = new StorageLocationData();
                LokasiBuku.ID = results.StorageLocation.ID;
                LokasiBuku.Location = results.StorageLocation.Location;

                dataBukus = new Data
                {
                    ID = results.ID,
                    InStock = results.InStock,
                    Jumlah = results.Jumlah,
                    Title = results.Title,
                    Category = categoryBuku,
                    Publisher = PublisherBuku,
                    Lokasi = LokasiBuku,

                };
            
            return dataBukus;

            }
            catch (Exception ex)
            {

                throw new Exception (ex.Message);
            }
        }

        public async Task<IEnumerable<Data>> InfoByCustom(RequestDataCustom request)
        {
            try
            {
                var query = _context.Bukus.OrderBy(x => x.ID)
                        .Include(p => p.Publisher)
                        .Include(c => c.Category)
                        .Include(s => s.StorageLocation)
                        .AsQueryable();
                //custom Query
                if (!string.IsNullOrEmpty(request.Title))
                {
                    query = query.Where(x => x.Title == request.Title);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(request.CategoryId)))
                {
                    query = query.Where(x => x.CategoryID == Convert.ToInt32(request.CategoryId));
                }
                if (!string.IsNullOrEmpty(Convert.ToString(request.storageLocationId)))
                {
                    query = query.Where(x => x.StorageLocationID == Convert.ToInt32(request.storageLocationId));
                }
                if (!string.IsNullOrEmpty(Convert.ToString(request.PublisherId)))
                {
                    query = query.Where(x => x.PublisherID == Convert.ToInt32(request.PublisherId));
                }

                var results = await query.ToListAsync();

                //proses Mapping Data Manual
                List<Data> dataBukus = new List<Data>();
                foreach (var item in results)
                {
                    //Mapping Data Category
                    DataCategory categoryBuku = new DataCategory();
                    categoryBuku.Id = item.Category.ID;
                    categoryBuku.CategoryCode = item.Category.CategoryCode;
                    categoryBuku.Description = item.Category.Description;

                    //Mapping Data Publisher
                    DataPublisher PublisherBuku = new DataPublisher();
                    PublisherBuku.Id = item.Publisher.ID;
                    PublisherBuku.Name = item.Publisher.Name;
                    PublisherBuku.Address = item.Publisher.Address;
                    PublisherBuku.Phone = item.Publisher.Phone;

                    //Mapping Data StorageLocation
                    StorageLocationData LokasiBuku = new StorageLocationData();
                    LokasiBuku.ID = item.StorageLocation.ID;
                    LokasiBuku.Location = item.StorageLocation.Location;

                    dataBukus.Add(new Data
                    {
                        ID = item.ID,
                        InStock = item.InStock,
                        Jumlah = item.Jumlah,
                        Title = item.Title,
                        Category = categoryBuku,
                        Publisher = PublisherBuku,
                        Lokasi = LokasiBuku,

                    });
                }

                return dataBukus;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public async Task<ResponseBase> InsertV2(RequestInsert request)
        {
            ResponseBase response = new();
            try
            {
                var CheckDuplicate = await _context.Bukus.SingleOrDefaultAsync(x => x.Title == request.Title);
                if (CheckDuplicate != null)
                {
                    response.Message = ($"Judul Buku {request.Title} Sudah Tersedia");
                    response.StatusCode = false;

                    return response;
                }
                var isValidStorage = await _context.storageLocations.SingleOrDefaultAsync(x => x.ID == request.StorageLocationId);
                var isValidCategory = await _context.categories.SingleOrDefaultAsync(x => x.ID == request.CategoryId);
                var isValidPublisher = await _context.publishers.SingleOrDefaultAsync(x => x.ID == request.PublisherId);
                if (isValidCategory == null || isValidPublisher == null || isValidStorage == null || request.Jumlah <= 0 || request.InStock > request.Jumlah)
                {
                    response.Message = ($"Data Insert Tidak Valid ");
                    response.StatusCode = false;

                    return response;
                }
                               
                Buku dataInput = new Buku();
                dataInput.Title = request.Title;
                dataInput.CategoryID = request.CategoryId;
                dataInput.PublisherID = request.PublisherId;
                dataInput.StorageLocationID = request.StorageLocationId;
                dataInput.Jumlah = request.Jumlah;
                dataInput.InStock = request.InStock;
                
                _context.Bukus.Add(dataInput);
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

        public async Task<ResponseBase> UpdateV2(RequestUpdate request)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var findData = await _context.Bukus.SingleOrDefaultAsync(x=> x.ID == request.Id);
                if (findData == null) 
                {
                    response.Message = ($"Data Buku dengan ID {request.Id} Title {request.Title} Tidak Tersedia");
                    response.StatusCode = false;

                    return response;
                }
                var isValidStorage = await _context.storageLocations.SingleOrDefaultAsync(x => x.ID == request.StorageLocationId);
                var isValidCategory = await _context.categories.SingleOrDefaultAsync(x => x.ID == request.CategoryId);
                var isValidPublisher = await _context.publishers.SingleOrDefaultAsync(x => x.ID == request.PublisherId);
                if (isValidCategory == null || isValidPublisher == null || isValidStorage == null || request.Jumlah <= 0 || request.InStock > request.Jumlah)
                {
                    response.Message = ($"Data Update Tidak Valid ");
                    response.StatusCode = false;

                    return response;
                }
                findData.Title = request.Title;
                findData.CategoryID = request.CategoryId;
                findData.PublisherID = request.PublisherId;
                findData.StorageLocationID = request.StorageLocationId;
                findData.Jumlah = request.Jumlah;
                findData.InStock = request.InStock;

                await _context.SaveChangesAsync();
                response.Message = "Update Success";
                response.StatusCode = true;
            }
            catch (Exception ex)
            {

                throw new Exception (ex.Message);
            }
            return response;
        }


    }
}
