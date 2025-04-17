using Hub.Database;
using Hub.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hub.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationContext _db;

        public ClientsController(ApplicationContext dbContext)
        {
            _db = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Client client)
        {
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Client? msg = await _db.Clients.FindAsync(id);
            if (msg == null)
                return NotFound();
            _db.Clients.Remove(msg);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _db.Clients.ToListAsync());
        }
    }
}
