using Service;
using System;
using System.Web.Http;

namespace TaxCalculator.Controllers
{
    [RoutePrefix("invoice")]
    public class InvoiceController : ApiController
    {
        IMessageProcessor messageProcessor;
        public InvoiceController(IMessageProcessor _messageProcessor)
        {
            messageProcessor = _messageProcessor;
        }

        [Route]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("Ok");
        }
        
        [Route]
        [HttpPost]
        public IHttpActionResult ProcessMessage(string inputMessage)
        {
            try
            {
                var isValidMessage = messageProcessor.ValidateMessage(inputMessage);
                if (!isValidMessage)
                {
                    return BadRequest("Invalid input message");
                }
                else
                {
                    var objProcessed = messageProcessor.ProcessMessage(inputMessage);
                    return Ok(objProcessed);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}