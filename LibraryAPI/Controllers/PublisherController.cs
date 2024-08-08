using AutoMapper;
using LibraryAPI.BusinessFacade;
using LibraryAPI.DomainObject;
using LibraryAPI.DomainObject.Publisher;
using LibraryAPI.Interface;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisher _publisher;
        private readonly IMapper _mapper;

        public PublisherController(IPublisher publisher, IMapper mapper)
        {
            _publisher = publisher;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<DataPublisher>> GetPublishers()
        {
                   
                var results = await _publisher.GetAll();
                var response = _mapper.Map<IEnumerable<DataPublisher>>(results);

                return response;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DataPublisher>> GetPublisher(int id) 
        {

            var result = await _publisher.GetById(id);
            var response = _mapper.Map<DataPublisher>(result);
            if (response == null)
            { 
                return BadRequest($"Data Dengan Id {id} Tidak Ditemukan");
            }
            return response;
        }

        [HttpGet("/name={name}")]
        public async Task<ActionResult<IEnumerable<DataPublisher>>> GetByName(string name)
        {
            var result = await _publisher.GetByName(name);
            var response = _mapper.Map<IEnumerable<DataPublisher>>(result);
            if (response.Count() == 0)
            {
                return BadRequest($"Data Dengan Nama {name} Tidak Ditemukan");
            }
            return Ok(response);
        }

        //Insert
        [HttpPost]
        public async Task<ActionResult> Insert(InsertPublisher obj) 
        {
            var newPublisher = _mapper.Map<Publisher>(obj);
            var result = await _publisher.Insert(newPublisher);

            var ReadData = _mapper.Map<DataPublisher>(result);
            return Ok(await _publisher.GetByName(obj.Name));
        }

        //update
        [HttpPut]
        public async Task<ActionResult> Put(DataPublisher dataPublisher)
        {
            try
            {
                var updatePublisher = _mapper.Map<Publisher>(dataPublisher);
                var result = await _publisher.Update(updatePublisher);
                return Ok(updatePublisher);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id) 
        {
            try
            {
                await _publisher.Delete(id);
                return Ok($"Data Dengan ID {id} Berhasil Dihapus");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //InsertV2 CombineStyle Code PSS
        [HttpPost("/Insertv2")]
        public async Task<ActionResult> InsertV2(InsertPublisher obj)
        {
           ResponseBase response = new();
            
            try
            {
                var result = await _publisher.InsertV2(obj);
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
    }
}
