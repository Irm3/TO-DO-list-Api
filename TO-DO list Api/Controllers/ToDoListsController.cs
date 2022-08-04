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

        [Authorize(Roles = "role1")]
        [HttpGet("GetAllUserTasks")]
        public async Task<ActionResult<IEnumerable<ToDoList>>> GetToDoLists()
        {
          if (_context.ToDoLists == null)
          {
              return NotFound();
          }
            return await _context.ToDoLists.ToListAsync();
        }

        [Authorize(Roles = "role2")]
        [HttpGet("GetAllMyTasks")]
        public async Task<ActionResult<IEnumerable<ToDoList>>> GetMyLists()
        {
            var email = User.FindFirstValue(ClaimTypes.Email); // get email
            var user = await _context.Users.FirstOrDefaultAsync(acc => acc.Email == email); // find logged in user

            if (_context.ToDoLists == null)
            {
                return NotFound();
            }
            return await _context.ToDoLists.Where(a => a.FkUserId == user.UserId).ToListAsync();
        }

        [Authorize(Roles = "role1")]
        [HttpGet("Task/{id}")]
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

        [Authorize(Roles = "role2")]
        [HttpPut("EditMyTask/{id}")]
        public async Task<IActionResult> PutToDoList(int id, ListTask toDoList)
        {
            var email = User.FindFirstValue(ClaimTypes.Email); // get email
            var user = await _context.Users.FirstOrDefaultAsync(acc => acc.Email == email); // find logged in user
            var task = await _context.ToDoLists.FirstOrDefaultAsync(tsk => tsk.TodoListId == id); // find task by id

            if(task == null)
            {
                return NotFound();
            }

            if(user.UserId != task.FkUserId)
            {
                return Unauthorized();
            }

            task.Name = toDoList.Name;
            task.Status = toDoList.Status;

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

        [Authorize(Roles = "role2")]
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

        [Authorize(Roles = "role1")]
        [HttpDelete("DeleteTask/{id}")]
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

        [Authorize(Roles = "role2")]
        [HttpDelete("DeleteMyTask/{id}")]
        public async Task<IActionResult> DeleteMyDoList(int id)
        {

            var email = User.FindFirstValue(ClaimTypes.Email); // get email
            var user = await _context.Users.FirstOrDefaultAsync(acc => acc.Email == email); // find logged in user


            if (_context.ToDoLists == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoLists.FindAsync(id);

            if (toDoList == null)
            {
                return NotFound();
            }

            if (user.UserId != toDoList.FkUserId)
            {
                return Unauthorized();
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
