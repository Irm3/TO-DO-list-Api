using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TO_DO_list_Api.Database;
using TO_DO_list_Api.Models;

namespace TO_DO_list_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {
        private readonly ToDoListDBContext _context;

        public ToDoListsController(ToDoListDBContext context)
        {
            _context = context;
        }

        // GET: api/ToDoLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoList>>> GetToDoLists()
        {
          if (_context.ToDoLists == null)
          {
              return NotFound();
          }
            return await _context.ToDoLists.ToListAsync();
        }

        // GET: api/ToDoLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoList>> GetToDoList(int id)
        {
          if (_context.ToDoLists == null)
          {
              return NotFound();
          }
            var toDoList = await _context.ToDoLists.FindAsync(id);

            if (toDoList == null)
            {
                return NotFound();
            }

            return toDoList;
        }

        // PUT: api/ToDoLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoList(int id, ToDoList toDoList)
        {
            if (id != toDoList.TodoListId)
            {
                return BadRequest();
            }

            _context.Entry(toDoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("AddTask")]
        public async Task<ActionResult<ToDoList>> PostToDoList([FromBody] ListTask toDoList)
        {

            var email = User.FindFirstValue(ClaimTypes.Email); // get email
            var user = await _context.Users.FirstOrDefaultAsync(acc => acc.Email == email); // find logged in user

            var task = new ToDoList
            {
                Name = toDoList.Name,
                Status = toDoList.Status,
                FkUserId = user.UserId,
                FkUser = user
            };

            _context.ToDoLists.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoList", new { id = task.TodoListId }, task);
        }

        // DELETE: api/ToDoLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoList(int id)
        {
            if (_context.ToDoLists == null)
            {
                return NotFound();
            }
            var toDoList = await _context.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }

            _context.ToDoLists.Remove(toDoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoListExists(int id)
        {
            return (_context.ToDoLists?.Any(e => e.TodoListId == id)).GetValueOrDefault();
        }
    }
}
