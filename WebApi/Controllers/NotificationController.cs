using Business.Abstract;
using Business.SignalR.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ISectionService _sectionService;
        public NotificationController(IHubContext<NotificationHub> hub, ISectionService sectionService)
        {
            _hub = hub;
            _sectionService = sectionService; 
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
          await _hub.Clients.All.SendAsync("transferdata", _sectionService.GetAll());
            return Ok(new { Message = "Request Completed" });
        }
    }
}
