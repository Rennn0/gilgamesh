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
        private readonly ApplicationContext m_db;

        public ClientsController(ApplicationContext dbContext)
        {
            m_db = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Client client)
        {
            m_db.Clients.Add(client);
            await m_db.SaveChangesAsync();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Client? msg = await m_db.Clients.FindAsync(id);
            if (msg == null)
                return NotFound();
            m_db.Clients.Remove(msg);
            await m_db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await m_db.Clients.ToListAsync());
        }
    }
}