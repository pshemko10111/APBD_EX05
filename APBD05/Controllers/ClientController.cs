using APBD05.Context;
using APBD05.DTOs;
using APBD05.Models;
using APBD05.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD05.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly IDBService _dBService;

        public ClientController(IDBService service)
        {
            _dBService = service;
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClientAsync([FromRoute]int clientId)
        {
            if (!_dBService.DeleteClientAsync(clientId).Result) return BadRequest("Client is assigned to trips");
            return Ok();
        }
    }
}
