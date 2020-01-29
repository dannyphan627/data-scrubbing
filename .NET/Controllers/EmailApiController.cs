

namespace Web.Api.Controllers
{
    [Route("api/emails")]
    [ApiController]
    public class EmailApiController : BaseApiController
    {
        private IEmailService _service = null;

        public EmailApiController(IEmailService service
            , ILogger<BaseApiController> logger) : base(logger)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ItemResponse<EmailResponse>>> TestEmail(EmailAddRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                EmailResponse item = await _service.TestEmail(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public async Task<ActionResult<ItemResponse<EmailResponse>>> ConfirmEmail(string email)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Guid token = new Guid();
                EmailResponse item = await _service.ConfirmEmail(email, token);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }


        [HttpPost("contact")]
        public async Task<ActionResult<ItemResponse<EmailResponse>>> ContactUs(EmailAddRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                EmailResponse item = await _service.ContactUs(model);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
                code = 500;
            }

            return StatusCode(code, response);
        }

        [HttpPost("multiple")]
        public async Task<ActionResult<ItemResponse<EmailResponse>>> SurveyEmails(EmailsAddRequest emailsRequest)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                EmailResponse item = await _service.SurveyEmails(emailsRequest.Emails, emailsRequest.Link);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
    }
}
