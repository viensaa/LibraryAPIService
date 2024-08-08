using AutoMapper;
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

        public Task Delete(int id)
        {
            throw new NotImplementedException();
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
                .SingleOrDefaultAsync(x => x.Title == request.title);



            if (results == null) 
            {
                throw new Exception($"Data dengan Nama {request.title} Tidak Ditemukan");
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
    }
}
