using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using TransaksiService.DomainObject;
using TransaksiService.DomainObject.Mahasiswa;
using TransaksiService.DomainObject.Transaction;
using TransaksiService.Interface;

namespace TransaksiService.BusinessFacade
{
    public class TransactionFacade : ITransaction
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TransactionFacade(IMapper mapper,DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BaseResponse> CreateTransaction(TransactionRequest request)
        {
            BaseResponse response = new();
            response.StatusCode = Convert.ToInt32(enumStatusCode.success);            
            try
            {
                bool isValidRequest = CheckValidRequest(request);
                if (!isValidRequest)
                {
                    response.Message = ("Request Data Not Valid");
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);

                    return response;
                }
                

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
            return response;
        }

        public bool CheckValidRequest(TransactionRequest request) 
        {
            /*
            di sini untuk ngecek data terisi atau tidak 
            buat ngecek apakah transaction type terisi selain data peminjaman atau pemgembalian
            jika peminjaman maka returndate harus terisi dan borrowdate ambil dari hari ini
            jika kondisi pengembalian maka returndate harus kosong atau di isi dengna tanggal hari ini dan borrow date harus kosong
            */
            return true;
        }

        public async Task<IEnumerable<DataTransaction>> GetAllData(FilterRequest request)
        {
            try
            {
                var query = _context.Transactions.Include(td => td.TransactionDetails).Include(m => m.Mahasiswa).AsQueryable();

                if (!string.IsNullOrEmpty(request.TransactionType))
                {
                    query = query.Where(x => x.TransactionType == Convert.ToInt32(request.TransactionType));
                }
                if (!string.IsNullOrEmpty(request.transactionID))
                {
                    query = query.Where(x => x.TransactionId == Convert.ToInt32(request.transactionID));
                }
                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(request.Status)); 
                }

                var results = await query.ToListAsync();
                if (results == null || results.Count() <= 0)
                {
                    throw new Exception("Tidak Ada Data Yang Sesuai Kriteria Pencarian");
                }
                //mapping Data
                List<DataTransaction> datas = new List<DataTransaction>();
                foreach (var item in results)
                {
                    List<DataTransactionDetail> dataTrasactionDetails = new List<DataTransactionDetail>();
                    foreach (var itemDetails in item.TransactionDetails)
                    {
                        dataTrasactionDetails.Add(new DataTransactionDetail
                        {
                            TransactionDetailId = itemDetails.TransactionId,
                            TransactionId = itemDetails.TransactionId,
                            BookId = itemDetails.BookId,
                            Quantity = itemDetails.Quantity,
                            DueDate = itemDetails.DueDate,
                            ReturnDate = itemDetails.ReturnDate,
                            Status = itemDetails.Status,
                        });
                    }

                    //populate Data Mahasiswa
                    DataMahasiswa mahasiswa = new DataMahasiswa();
                    mahasiswa.Nama = item.Mahasiswa.Nama;
                    mahasiswa.Id = item.Mahasiswa.Id;
                    mahasiswa.NIM = item.Mahasiswa.NIM;
                    mahasiswa.Kelas = item.Mahasiswa.Kelas;

                    datas.Add(new DataTransaction
                    {
                        TransactionId = item.TransactionId,
                        TransactionType = item.TransactionType,
                        TransactionDate = item.TransactionDate,
                        Status = item.Status,
                        Details = dataTrasactionDetails,
                        Mahasiswa = mahasiswa

                    });
                }

                return datas;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }
    }
}
