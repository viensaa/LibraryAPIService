using AutoMapper;
using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.Buku;
using LibraryAPI.Interface;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("ByTitle")]
        public async Task<ActionResult<dataBuku>> GetByTitle([FromBody]RequestDataBuku request)
        {
            dataBuku response = new();
            try
            {
                var result = await _buku.InfoByNama(request);
                response.StatusCode = true;

                if (result == null)
                {

                    response.Message = $"Buku Dengan Judul {request.title} Tidak Ditemukan";
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

    }
}
