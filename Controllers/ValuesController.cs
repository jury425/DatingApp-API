using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Data;
using DatingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataContext _context { get; set; }

        public ValuesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Values
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            List<Value> values = await _context.Values.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValueById(int id)
        {
            Value value = await _context.Values.FirstOrDefaultAsync(v => v.Id == id);
            return Ok(value);
        }


        // POST: api/Values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
