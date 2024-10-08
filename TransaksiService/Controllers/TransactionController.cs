﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransaksiService.DomainObject;
using TransaksiService.DomainObject.Transaction;
using TransaksiService.Interface;

namespace TransaksiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _transaction;
        private readonly IMapper _mapper;

        public TransactionController(IMapper mapper,ITransaction transaction)
        {
            _mapper = mapper;
            _transaction = transaction;
        }

        [HttpPost("GetAllData")]
        public async Task<ActionResult<listDataTransaction>> GetAllData(FilterRequest request)
        {
            listDataTransaction response = new();
            response.StatusCode = Convert.ToInt32(enumStatusCode.success);            
            try
            {
                var results = await _transaction.GetAllData(request);
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
            
            return response;
        }
        [HttpPost("CreateTransaction")]
        public async Task<ActionResult> CreateTransaction(TransactionRequest request) 
        {
            BaseResponse response = new();
            try
            {
                var result = await _transaction.CreateTransaction(request);
                if (result.StatusCode != Convert.ToInt32(enumStatusCode.success))
                {
                    response.StatusCode = Convert.ToInt32(enumStatusCode.failure);
                    response.Message = result.Message;

                }
                else
                {
                    response.StatusCode = Convert.ToInt32(enumStatusCode.success);
                    response.Message = result.Message.ToString();
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
