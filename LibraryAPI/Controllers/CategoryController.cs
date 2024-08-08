using AutoMapper;
using LibraryAPI.BusinessFacade;
using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.Category;
using LibraryAPI.Interface;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategory _category;

        public CategoryController(ICategory category, IMapper mapper)
        {
            _mapper = mapper;
            _category = category;
        }

        CategoryFacade facade = new CategoryFacade();

        [HttpGet]
        public async Task<IEnumerable<DataCategory>> GetData()
        {

            var results = await _category.GetAll();
            var response = _mapper.Map<IEnumerable<DataCategory>>(results);

            return response;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DataCategory>> GetDataById(int id)
        {
            var result = await _category.GetById(id);
            var response = _mapper.Map<DataCategory>(result);
            if (response == null)
            {
                return BadRequest($"Data Dengan Id {id} Tidak DiTemukan");
            }
            return response;

        }

        [HttpPost("InsertV2")]
        public async Task<ActionResult> InsertV2(RequestCategory obj)
        {
            ResponseBase response = new();
            try
            {
                var result = await _category.InsertV2(obj);
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

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult> UpdateCategory(RequestCategory obj)
        {
            ResponseBase response = new();
            try
            {
                var result = await _category.UpdateV2(obj);
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

        [HttpDelete("DeleteCategory")]
        public async Task<ActionResult> Delete(int id)
        {
            ResponseBase response = new();
            try
            {
                await _category.Delete(id);
                response.Message = ($"Data Dengan ID {id} Berhasil Dihapus");
                response.StatusCode = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message=ex.Message;
                response.StatusCode = false;
                return BadRequest(response);
            }
        }

    }
}
