using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using TransaksiService.DomainObject;
using TransaksiService.DomainObject.Mahasiswa;
using TransaksiService.Interface;
using TransaksiService.Model;

namespace TransaksiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MahasiswaController : ControllerBase
    {
        private readonly IMahasiswa _mahasiswa;
        private readonly IMapper _mapper;

        public MahasiswaController(IMapper mapper,IMahasiswa mahasiswa )
        {
            _mapper = mapper;
            _mahasiswa = mahasiswa;
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<DataMahasiswa>> GetAll() 
        {
            var results = await _mahasiswa.GetAll();
            return results;
        }

        [HttpGet("GetDataByCustomSearch")]
        public async Task<ActionResult<listMahasiswa>> GetDataByCustomSearch(RequestSearch request)
        {
            listMahasiswa response = new();
            response.StatusCode = Convert.ToInt32(enumStatusCode.success);
            try
            {
                var results = await _mahasiswa.GetDatabyCustom(request);
                if (results == null || results.Count() <= 0) 
                {
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                    response.Message = "Data Tidak Ada yang Sesuai Kriteria Pencarian";
                }
                response.datas = results;
            }
            catch (Exception ex)
            {
                response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                response.Message = ex.Message;
            }

            return response.StatusCode== Convert.ToInt32(enumStatusCode.success) ? Ok(response) : BadRequest(response);
        }

        [HttpPost("CreateMahasiswa")]
        public async Task<ActionResult> CreateMahasiswa(CreateMahasiswa request) 
        {
            BaseResponse response = new();
            try
            {
                var result = await _mahasiswa.CreateData(request);
                if (result.StatusCode != Convert.ToInt32(enumStatusCode.success))
                {
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                    response.Message = result.Message;

                }
                else
                {
                    response.StatusCode = Convert.ToInt32(enumStatusCode.success);
                    response.Message = "Insert Success";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                response.Message = ex.Message;
            }
            return response.StatusCode == Convert.ToInt32(enumStatusCode.success) ? Ok(response) : BadRequest(response);

        }

        [HttpPut("ChangeData")]
        public async Task<ActionResult> ChangeData(RequestChangeData request)
        {
            BaseResponse response = new();
            try
            {
                var result = await _mahasiswa.ChangeData(request);
                if (result.StatusCode != Convert.ToInt32(enumStatusCode.success))
                {
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                    response.Message = result.Message;

                }
                else
                {
                    response.StatusCode = Convert.ToInt32(enumStatusCode.success);
                    response.Message = "Update Success";
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                response.Message = ex.Message;
            }
            return response.StatusCode == Convert.ToInt32(enumStatusCode.success) ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("DeleteData")]
        public async Task<ActionResult> DeleteData (string nim) 
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var result = await _mahasiswa.DeleteData(nim);
                if (result.StatusCode != Convert.ToInt32(enumStatusCode.success))
                {
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                    response.Message = result.Message;

                }
                else
                {
                    response.StatusCode = Convert.ToInt32(enumStatusCode.success);
                    response.Message = "Detele Success";
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                response.Message = ex.Message;
            }
            return response.StatusCode == Convert.ToInt32(enumStatusCode.success) ? Ok(response) : BadRequest(response);
        }

    }
}
