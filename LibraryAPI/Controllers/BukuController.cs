using AutoMapper;
using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.Buku;
using LibraryAPI.Interface;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BukuController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBuku _buku;

        public BukuController(IBuku buku,IMapper mapper)
        {
            _buku = buku;
            _mapper = mapper;
        }

        [HttpGet("GetAllInfoBuku")]
        public async Task<IEnumerable<Data>> GetAllInfoBuku() 
        {
            var results = await _buku.GetAllInfoBuku();
            return results;
        }

        [HttpPost("ById")]
        public async Task<ActionResult<dataBuku>> GetByTitle([FromBody]RequestDataBuku request)
        {
            dataBuku response = new();
            try
            {
                var result = await _buku.InfoByNama(request);
                response.StatusCode = true;

                if (result == null)
                {

                    response.Message = $"Buku Dengan ID {request.BukuId} Tidak Ditemukan";
                    response.StatusCode = false;
                    response.Data = result;

                    return  BadRequest(response);
                }

                response.Data = result;
                
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = false;

            }
            return response.StatusCode ? Ok(response) : BadRequest(response);
            
        }

        [HttpPost("InfoByCustom")]
        public async Task<ActionResult<listofbuku>> InfoByCustom(RequestDataCustom request)
        {
            listofbuku response = new();

            try
            {
                var results = await _buku.InfoByCustom(request);
                if (results == null)
                {
                    response.StatusCode = false;
                    response.Message = "Data Tidak Ada yang Sesuai Kriteria Pencarian";
                }
                response.Data = results;
                
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = false;

            }
            return response;
        }

        [HttpPost("InsertData")]
        public async Task<ActionResult> InsertData(RequestInsert request) 
        {
            ResponseBase response = new();
            try
            {
                var result = await _buku.InsertV2(request);
                response.StatusCode = result.StatusCode;
                response.Message = result.Message;
                return result.StatusCode ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {

                response.Message = ex.Message;
                response.StatusCode = false;
                return BadRequest(response);
            }
        }

        [HttpPut("UpdateBuku")]
        public async Task<ActionResult> UpdateBuku(RequestUpdate request) 
        {
            ResponseBase response = new();
            try
            {
                var result = await _buku.UpdateV2(request);
                response.StatusCode = result.StatusCode;
                response.Message = result.Message;
                return result.StatusCode ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = false;
                return BadRequest(response);
            }
        }

        [HttpDelete("DeleteBuku")]
        public async Task<ActionResult> DeleteBuku(int id) 
        {
            ResponseBase response = new();
            try
            {
                await _buku.Delete(id);
                response.Message = ($"Data Dengan ID {id} Berhasil Dihapus");
                response.StatusCode = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = false;
                return BadRequest(response);
            }
        }


    }
}
