using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TransaksiService.DomainObject;
using TransaksiService.DomainObject.Mahasiswa;
using TransaksiService.Interface;
using TransaksiService.Model;

namespace TransaksiService.BusinessFacade
{
    public class MahasiswaFacade : IMahasiswa
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        

        public MahasiswaFacade(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse> ChangeData(RequestChangeData obj)
        {
            BaseResponse response = new();
            try
            {
                var findData = await _context.Mahasiswas.SingleOrDefaultAsync(x=> x.NIM == obj.NIM);
                if (findData == null)
                {
                    response.Message = ($"Data Mahasiswa dengan NIM {obj.NIM} Tidak Tersedia");
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);

                    return response;
                }
                if (obj.NIM.IsNullOrEmpty() || obj.Nama.IsNullOrEmpty() || obj.Kelas.IsNullOrEmpty())
                {
                    response.Message = $"Data Update Tidak Valid";
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);

                    return response;
                }
                findData.Nama = obj.Nama;
                findData.NIM = obj.NIM;
                findData.Kelas = obj.Kelas;
                findData.RowStatus = obj.RowStatus;

                await _context.SaveChangesAsync();
                response.Message = "Update Success";
                response.StatusCode = Convert.ToInt32(enumStatusCode.success);


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> CreateData(CreateMahasiswa obj)
        {
            BaseResponse response = new();
            try
            {
                var checkDuplicate = await _context.Mahasiswas.SingleOrDefaultAsync(x=> x.NIM == obj.NIM);
                if (checkDuplicate != null)
                {
                    response.Message = $"Mahasiswa dengan NIM {obj.NIM} Sudah Terdaftar";
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                    
                    return response;
                }
                if(obj.NIM.IsNullOrEmpty() || obj.Nama.IsNullOrEmpty() || obj.Kelas.IsNullOrEmpty()) 
                {
                    response.Message = $"Data Insert Tidak Valid";
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);

                    return response;
                }
                Mahasiswa dataInput = new Mahasiswa();
                dataInput.NIM = obj.NIM;
                dataInput.Nama = obj.Nama;
                dataInput.Kelas = obj.Kelas;
                dataInput.RowStatus = obj.RowStatus;

                _context.Mahasiswas.Add(dataInput);
                await _context.SaveChangesAsync();

                response.Message = "Create Success";
                response.StatusCode = Convert.ToInt32(enumStatusCode.success);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> DeleteData(string nim)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var findData = await _context.Mahasiswas.SingleOrDefaultAsync(x => x.NIM == nim);
                if (findData == null)
                {
                    response.Message =  ($"Mahasiswa dengan Nim {nim} Tidak DiTemukan ");
                    response.StatusCode= Convert.ToInt32(enumStatusCode.failure);

                    return response;
                }
                _context.Remove(findData);
                await _context.SaveChangesAsync();
                response.Message = ($"Mahasiswa dengan Nim {nim} Telah Berhasil Di Delete ");
                response.StatusCode = Convert.ToInt32(enumStatusCode.success);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return response;
        }

        public async Task<IEnumerable<DataMahasiswa>> GetAll()
        {
            var results = await _context.Mahasiswas.OrderBy(x=> x.Nama)
                .Where(x=> x.RowStatus == Convert.ToInt32(RowStatus.Active)).ToListAsync();
            var datas = _mapper.Map<IEnumerable<DataMahasiswa>>(results);
            return datas;
        }

        public async Task<IEnumerable<DataMahasiswa>> GetDatabyCustom(RequestSearch obj)
        {
            var query = _context.Mahasiswas.OrderBy(x=> x.Id).Where(x => x.RowStatus == Convert.ToInt32(RowStatus.Active)).AsQueryable();
            if (!string.IsNullOrEmpty(obj.NIM))
            {
                query = query.Where(x => x.NIM == obj.NIM);
            }
            if (!string.IsNullOrEmpty(obj.Nama))
            {
                query = query.Where(x => x.Nama == obj.Nama);
            }
            if (!string.IsNullOrEmpty(obj.Kelas))
            {
                query = query.Where(x => x.Kelas == obj.Kelas);
            }
            var results = await query.ToListAsync();
            var datas = _mapper.Map<IEnumerable<DataMahasiswa>>(results);
            return datas;
        }

    }
}
