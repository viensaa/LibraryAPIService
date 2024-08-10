using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
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
        private static readonly HttpClient httpClient = new HttpClient();

        public TransactionFacade(IMapper mapper, DataContext context)
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
                bool isValidRequest = CheckValidRequest(ref request);
                if (!isValidRequest)
                {
                    response.Message = ("Request Data Not Valid");
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);

                    return response;
                }
                RequestSearch requestMahasiswa = new RequestSearch();
                requestMahasiswa.NIM = request.NIM;
                requestMahasiswa.Nama = string.Empty;
                requestMahasiswa.Kelas = string.Empty;
                bool isMahasiswaValid = await CheckValidMahasiswa(requestMahasiswa);
                if (!isMahasiswaValid)
                {
                    response.Message = ("Data Mahasiswa Not Valid");
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);

                    return response;
                }


                RequestBuku requestBuku = new RequestBuku();
                requestBuku.Bukuid = request.BukuID;
                InfoBuku buku = await GetBuku(requestBuku);
                var debug = "testingDebug";


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return response;
        }

        public async Task<bool> CheckValidMahasiswa(RequestSearch request) 
        {
            var result = await _context.Mahasiswas.Where(x => x.NIM == request.NIM).ToListAsync();
            if (result == null || result.Count() <= 0) 
            {
                return false;
            }
            return true;
        }

        public  async Task<InfoBuku> GetBuku(RequestBuku obj) 
        {
            InfoBuku Data = new();
            var URL = "http://localhost:5164/api/Buku/ById";//di development lanjutannanti akan di mauskkan ke table config di DB
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, URL)
            {
                Content = content
            };
            try
            {
                HttpResponseMessage responseAPI = await httpClient.SendAsync(request);
                responseAPI.EnsureSuccessStatusCode();
                string APIResponse =await responseAPI.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<InfoBuku>(APIResponse);

                //using (var httpClient = new HttpClient()) 
                //{
                //    StringContent content = new StringContent(json)
                //}


            }
            catch (Exception ex)
            {

                throw new Exception($"Request Error : {ex.Message}");
            }


            return Data;
        }

        public bool CheckValidRequest(ref TransactionRequest request)
        {
            /*
            di sini untuk ngecek data terisi atau tidak 
            buat ngecek apakah transaction type terisi selain data peminjaman atau pemgembalian
            jika peminjaman(45) maka returndate harus terisi dan harus lebih besar dari tainggal hari ini
            jika kondisi pengembalian(40) maka returndate harus tanggal hari ini 
            */

            //pengeceken request kosong
            if (string.IsNullOrEmpty(request.NIM) || string.IsNullOrEmpty(request.BukuID.ToString())

                || string.IsNullOrEmpty(request.TransactionType.ToString()) || string.IsNullOrEmpty(request.Quantity.ToString())) 
            {
                return false;
            }
            if (request.TransactionType == 45)
            {
                request.BorrowDate = DateTime.Now;
                if (string.IsNullOrEmpty(request.ReturnDate.ToString()) || request.ReturnDate < DateTime.Today || request.Quantity <= 0 || request.Quantity > 1)
                {
                    return false;
                }
            }
            else if (request.TransactionType == 40)
            {
                request.BorrowDate = null;
                if (string.IsNullOrEmpty(request.ReturnDate.ToString()) || request.ReturnDate != DateTime.Today
                    || !string.IsNullOrEmpty(request.BorrowDate.ToString()) || request.Quantity <= 0 || request.Quantity > 1)
                {
                    return false;
                }
            }
            else { 
                return false;
            }

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
