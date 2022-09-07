using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceMagnat.Data;
using ResourceMagnat.Dto;
using ResourceMagnat.Models;
using ResourceMagnat.Services;

namespace ResourceMagnat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MainDbContext context;
        private readonly IMapper mapper;
        private readonly SessionService sessionService;
        private readonly UserService userService;

        public UserController(MainDbContext _context, IMapper _mapper, SessionService _sessionService, UserService _userService)
        {
            context = _context;
            mapper = _mapper;
            sessionService = _sessionService;
            userService = _userService;
        }

        /// <summary>
        /// Получаем список всех пользователей
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        [HttpGet]
        public IEnumerable<User> Get() => userService.GetUsers();

        /// <summary>
        /// Возвращает пользователя по его UID. Если такого пользователя нет - создаёт его.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("get/{uid}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public IActionResult GetOrCreate(string uid)
        {
            return Ok(userService.GetOrCreate(uid));
        }

    }
}
