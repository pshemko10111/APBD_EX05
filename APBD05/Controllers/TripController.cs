using APBD05.Context;
using APBD05.DTOs;
using APBD05.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD05.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class TripController : Controller
    {
        private readonly IDBService _dbservice;

        public TripController(IDBService service)
        {
            _dbservice = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTripsAsync()
        {
            
            return Ok(await _dbservice.GetTripsAsync());
        }

        [HttpPost("{idtrip}/clients")]
        public async Task<IActionResult> AddClientToTripAsync(int idtrip, [FromBody] ClientDTO client)
        {
            try
            {
                await _dbservice.AddClientToTripAsync(idtrip, client);
                return Ok();
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
    }
}
