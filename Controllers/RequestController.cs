using Microsoft.AspNetCore.Mvc;
using RequestManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.AspNetCore.ServerSentEvents;

namespace RequestManager.Controllers 
{
    public class RequestController : Controller 
    {
        private readonly RequestService _requestService;
        private readonly SSEService _sseService;

        public RequestController(RequestService requestService, SSEService sseService)
        {
            _requestService = requestService;
            _sseService = sseService;
        }

        [HttpGet]
        public IActionResult RequestView(int id)
        {
            try
            {
                var activeRequests = _requestService.GetRequests(id);
                return View("RequestView", activeRequests);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(RequestModel request) 
        {
            try 
            {
                if (ModelState.IsValid) 
                {
                    _requestService.CreateRequest(request);
                    await _sseService.SendEventAsync(new ServerSentEvent { Type = "requests_updated", Data = new List<string>{""} });
                    return Ok();
                } 
                else 
                {
                    return BadRequest(ModelState);
                }
            } 
            catch (Exception ex) 
            {
                return HandleError(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRequest(int id) 
        {
            try 
            {
                _requestService.DeleteRequest(id);
                await _sseService.SendEventAsync(new ServerSentEvent { Type = "requests_updated", Data = new List<string>{""} });
                return Ok();
            } 
            catch (Exception ex) 
            {
                return HandleError(ex);
            }
        }

        private IActionResult HandleError(Exception ex) 
{
            Console.WriteLine($"An error occurred: {ex.Message}");

            var errorViewModel = new ErrorViewModel
            {
                RequestId = Guid.NewGuid().ToString(),
                // You can add more properties to the ErrorViewModel if needed
            };

            return View("Error", errorViewModel);
        }
    }
}
