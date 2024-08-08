using AutoMapper;
using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.StorageLocation;
using LibraryAPI.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageLocationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStorageLocation _storageLocation;

        public StorageLocationController(IStorageLocation storageLocation, IMapper mapper)
        {
            _storageLocation = storageLocation;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<StorageLocationData>> GetData()
        {
            var result = await _storageLocation.GetAll();
            var response = _mapper.Map<IEnumerable<StorageLocationData>>(result);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StorageLocationData>> GetDataByID(int id)
        {
            var result = await _storageLocation.GetById(id);
            var response = _mapper.Map<StorageLocationData>(result);
            if (response == null)
            {
                return BadRequest($"Data Dengan Id {id} Tidak DiTemukan");
            }
            return response;

        }

        [HttpPost("InsertStorageLocation")]
        public async Task<ActionResult> InsertData(RequestStorageLocation request)
        {
            ResponseBase response = new();
            try
            {
                var result = await _storageLocation.InsertV2(request);
                response = result;
                return result.StatusCode ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = false;
                return BadRequest(response);
            }
        }


        //[HttpPut("Update")]
        //public async Task<ActionResult> PutData(RequestStorageLocation request)
        //{
        //    ResponseBase response = new();
        //    try
        //    {
        //        var result = await _storageLocation.UpdateV2(request);
        //        response = result;
        //        return result.StatusCode ? Ok(response) : BadRequest(response);
        //    }
        //    catch (Exception ex)
        //    {

        //        response.Message = ex.Message;
        //        response.StatusCode = false;
        //        return BadRequest(response);
        //    }
        //}

        [HttpDelete("DeleteStorageLocation")]
        public async Task<ActionResult> delete(int id) 
        {
            ResponseBase response = new();
            try
            {
               await _storageLocation.Delete(id);
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
