using AutoMapper;
using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.Json;
using TransaksiService.DomainObject;
using TransaksiService.DomainObject.Mahasiswa;
using TransaksiService.DomainObject.Transaction;
using TransaksiService.Interface;
using TransaksiService.Model;

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
            response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
            try
            {
                #region validation Data
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

                Mahasiswa dataMahaswa = new Mahasiswa();

                bool isMahasiswaValid = CheckValidMahasiswa(requestMahasiswa,ref dataMahaswa);
                if (!isMahasiswaValid)
                {
                    response.Message = ("Data Mahasiswa Not Valid");
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);

                    return response;
                }


                RequestBuku requestBuku = new RequestBuku();
                requestBuku.Bukuid = request.BukuID;
                InfoBuku buku = await GetBuku(requestBuku);
                if (buku == null || buku.statusCode == false) 
                {
                    response.Message = ("Data Buku Not Valid");
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                    return response;
                }
                #endregion
                if (request.TransactionType == Convert.ToInt32(TransactionType.borrow))
                {

                    if (buku.data.inStock == 0)
                    {
                        response.Message = ("Buku Yang Ingin Dipinjam Sudah Habis");
                        response.StatusCode = Convert.ToInt32(enumStatusCode.failure);

                        return response ;
                    }
                    #region Populate data Insert to Transaction
                    Transaction inputTransaction = new Transaction();
                    inputTransaction.MahasiswaId = dataMahaswa.Id;
                    inputTransaction.TransactionType = request.TransactionType;
                    inputTransaction.TransactionDate = DateTime.Now;
                    //inputTransaction.Status = Convert.ToInt32(StatusTransaction.OnBorrow);
                    inputTransaction.TransactionDetails =
                        new List<TransactionDetail>
                        {
                            new TransactionDetail
                        {
                            BookId = request.BukuID,
                            Quantity = request.Quantity,
                            DueDate = DateTime.Now,
                            ReturnDate = request.ReturnDate,
                            Status = Convert.ToInt32(StatusTransaction.OnBorrow)
                        } //jika ada 2 data tinggal lanjutkan saja
                        };
                    #endregion

                    #region update stock buku
                    RequestUpdateStockBuku stockBuku = new RequestUpdateStockBuku();
                    stockBuku.Id = buku.data.id;
                    stockBuku.Title = buku.data.title;
                    stockBuku.CategoryId = buku.data.category.id;
                    stockBuku.PublisherId = buku.data.publisher.id;
                    stockBuku.StorageLocationId = buku.data.lokasi.id;
                    stockBuku.Jumlah = buku.data.jumlah;
                    stockBuku.InStock = buku.data.inStock - request.Quantity;


                    bool IsSuccessUpdateStock = await UpdateStockbuku(stockBuku);
                    if (!IsSuccessUpdateStock)
                    {
                        response.Message = ($"Gagal Update Stock Buku");
                        response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                        return response;
                    }
                    #endregion

                    _context.Transactions.Add(inputTransaction);
                    await _context.SaveChangesAsync();

                    response.Message = ("Create Transaction For Borrow Success");
                    response.StatusCode = Convert.ToInt32(enumStatusCode.success);
                }
                else if (request.TransactionType == Convert.ToInt32(TransactionType.returning)) 
                {
                    //ngambil data dari table transaction by mahasiswa, transactiontype = 45, status = onborrow , bukuID
                    //jika datanya tidak ada munuclkan errr mahasiswa tidak pernah meminjam buku tersebut, jika ada maka insert baru dan update stock buku(hit api stock)

                    #region validasi mahasiswa meminjam buku yang ingin di kembalikan
                    ValidasiBorrow obj = new ValidasiBorrow();
                    obj.MahasiswaId = dataMahaswa.Id;
                    obj.TransactionType = Convert.ToInt32(TransactionType.borrow);
                    obj.Status = Convert.ToInt32(StatusTransaction.OnBorrow);
                    obj.BookId = request.BukuID;
                    bool isValidBorrow = await ValidasiBorrow(obj);
                    if (!isValidBorrow)
                    {
                        response.Message = ($"Mahasiswa Dengan Nim {dataMahaswa.NIM} Tidak Pernah Meminjam Buku ID {request.BukuID} Judul {buku.data.title}");
                        response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                        return response;
                    }
                    #endregion

                    #region populate data input for transaction
                    Transaction inputTransaction = new Transaction();
                    inputTransaction.MahasiswaId = dataMahaswa.Id;
                    inputTransaction.TransactionType = request.TransactionType;
                    inputTransaction.TransactionDate = DateTime.Now;
                 //   inputTransaction.Status = Convert.ToInt32(StatusTransaction.complete);
                    inputTransaction.TransactionDetails =
                        new List<TransactionDetail>
                        {
                            new TransactionDetail
                        {
                            BookId = request.BukuID,
                            Quantity = request.Quantity,
                            DueDate = null,
                            ReturnDate = DateTime.Now,
                            Status = Convert.ToInt32(StatusTransaction.complete)
                        } //jika ada 2 data tinggal lanjutkan saja
                        };
                    #endregion
                    #region update stock buku
                    RequestUpdateStockBuku stockBuku = new RequestUpdateStockBuku();
                    stockBuku.Id = buku.data.id;
                    stockBuku.Title = buku.data.title;
                    stockBuku.CategoryId = buku.data.category.id;
                    stockBuku.PublisherId = buku.data.publisher.id;
                    stockBuku.StorageLocationId = buku.data.lokasi.id;
                    stockBuku.Jumlah = buku.data.jumlah;
                    stockBuku.InStock = buku.data.inStock + request.Quantity;
                    

                    bool IsSuccessUpdateStock = await UpdateStockbuku(stockBuku);
                    if (!IsSuccessUpdateStock) 
                    {
                        response.Message = ($"Gagal Update Stock Buku");
                        response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                        return response;
                    }
                    #endregion

                    _context.Transactions.Add(inputTransaction);
                    await _context.SaveChangesAsync();
                    

                    response.Message = ("Create Transaction For Return Success");
                    response.StatusCode = Convert.ToInt32(enumStatusCode.success);

                }




            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return response;
        }

        public async Task<bool> UpdateStockbuku(RequestUpdateStockBuku obj)
        {
            bool response = false;
            var url = "http://localhost:5164/api/Buku/UpdateBuku";//afr2024:di development lanjutannanti akan di mauskkan ke table config di DB
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = content
            };
            try
            {
                HttpResponseMessage responseAPI = await httpClient.SendAsync(request);
                if (responseAPI.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return false;
                }
                response = true;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return response;
        }

        public  bool CheckValidMahasiswa(RequestSearch request,ref Mahasiswa mahasiswa) 
        {
            var result =  _context.Mahasiswas.FirstOrDefault(x => x.NIM == request.NIM);
            if (result == null || result.Id <= 0) 
            {
                return false;
            }
            mahasiswa = result;
            return true;
        }

        public  async Task<InfoBuku> GetBuku(RequestBuku obj) 
        {
            InfoBuku Data = new();
            var URL = "http://localhost:5164/api/Buku/ById";//afr2024:di development lanjutannanti akan di mauskkan ke table config di DB
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, URL)
            {
                Content = content
            };
            try
            {
                HttpResponseMessage responseAPI = await httpClient.SendAsync(request);
                if (responseAPI.StatusCode == System.Net.HttpStatusCode.BadRequest) 
                {
                    Data.statusCode = false;
                    return Data;
                }
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

        public async Task<bool> ValidasiBorrow(ValidasiBorrow validasi) 
        {
            var query = _context.Transactions.Include(t => t.TransactionDetails).Include(m => m.Mahasiswa)
                .Where(t=> t.MahasiswaId == validasi.MahasiswaId &&
                        t.TransactionType == validasi.TransactionType &&                        
                        t.TransactionDetails.Any(td => td.BookId == validasi.BookId && td.Status == validasi.Status));

            var results = await query.ToListAsync();
            if (results == null || results.Count() <= 0)
            {
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
                //if (!string.IsNullOrEmpty(request.Status))
                //{
                //    query = query.Where(x => x.Status == Convert.ToInt32(request.Status)); 
                //}

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
                       // Status = item.Status,
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
