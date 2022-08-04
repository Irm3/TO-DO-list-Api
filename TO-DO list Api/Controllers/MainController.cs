using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TO_DO_list_Api.Models;
using TO_DO_list_Api.JWT;
using TO_DO_list_Api.Database;

namespace TO_DO_list_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly ToDoListDBContext _context;

    }
}
