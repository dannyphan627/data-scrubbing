using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/insurances")]
    [ApiController]
    public class InsuranceProviderApiController : BaseApiController
    {
        private IInsuranceProviderService _service = null;
        private IAuthenticationService<int> _authService = null;
        public InsuranceProviderApiController(IInsuranceProviderService service
            , ILogger<BaseApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(InsuranceProviderAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                int id = _service.Add(model, userId);

                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpGet("paginate")] 
        public ActionResult<ItemResponse<Paged<InsuranceProvider>>> GetAllPagination(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<InsuranceProvider> page = _service.GetAllPagination(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<InsuranceProvider>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<InsuranceProvider>> Get(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                InsuranceProvider provider = _service.Get(id);

                if (provider == null)
                {
                    code = 404;
                    response = new ErrorResponse("Provider Not Found.");
                }
                else
                {
                    response = new ItemResponse<InsuranceProvider> { Item = provider };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(InsuranceProviderUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<InsuranceProvider>>> Search(string query, int pageSize, int pageIndex)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<InsuranceProvider> list = _service.SearchInsuranceProviders(query, pageSize, pageIndex);
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<InsuranceProvider>>() { Item = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(code, response);
        }

        [HttpGet]
        public ActionResult<ItemsResponse<InsuranceProvider>> GetAllList()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<InsuranceProvider> list = _service.GetAllList();
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<InsuranceProvider>() { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(code, response);
        }
    }

}