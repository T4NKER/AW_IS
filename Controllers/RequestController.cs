using Microsoft.AspNetCore.Mvc;
using RequestManager.Models;
using System.Threading.Tasks;
using Lib.AspNetCore.ServerSentEvents;


namespace RequestManager.Controllers {
    public class RequestController : Controller {
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
                Console.WriteLine("id: " + id);
                return View(activeRequests);
            }
            catch (Exception ex)
            {
                var requestId = Guid.NewGuid().ToString();

                var errorViewModel = new ErrorViewModel { RequestId = requestId };

                return View("Error", errorViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(RequestModel request) {
            try {
                if (ModelState.IsValid) {
                    _requestService.CreateRequest(request);
                    await _sseService.SendEventAsync(new ServerSentEvent { Type = "requests_updated", Data = new List<string>{""}  });
                    return Ok();
                } else {
                    return BadRequest(ModelState);
                }
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred while creating the request: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRequest(int id) {
            try {
                _requestService.DeleteRequest(id);
                await _sseService.SendEventAsync(new ServerSentEvent { Type = "requests_updated", Data = new List<string>{""} });
                return Ok();
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred while deleting the request: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}