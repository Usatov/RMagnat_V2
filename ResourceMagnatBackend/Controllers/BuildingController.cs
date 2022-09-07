using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResourceMagnat.Data;
using ResourceMagnat.Dto;
using ResourceMagnat.Models;
using ResourceMagnat.Services;

namespace ResourceMagnat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly MainDbContext context;
        private readonly IMapper mapper;
        private readonly SessionService sessionService;
        public BuildingService buildingService;

        public BuildingController(MainDbContext _context, IMapper _mapper, SessionService _sessionService, BuildingService _buildingService)
        {
            context = _context;
            mapper = _mapper;
            sessionService = _sessionService;
            buildingService = _buildingService;
        }

        /// <summary>
        /// Возвращает список всех возможных зданий
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BuildingTypeDto>), StatusCodes.Status200OK)]
        public IEnumerable<BuildingTypeDto> List()
        {
            return buildingService.List();
        }

        /// <summary>
        /// Возвращает список зданий для пользователя
        /// </summary>
        /// <param name="sid">Идентификатор сессии</param>
        /// <returns></returns>
        [HttpGet("own/{sid}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        public IEnumerable<BuildingDto> Get(string sid)
        {
            var userId = sessionService.GetUserIdBySession(sid);
            if (userId == null) return new List<BuildingDto>();

            return buildingService.Get((int)userId);
        }

        [HttpPost("add/{sid}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Add(string sid, BuildingDto buildingDto)
        {
            var userId = sessionService.GetUserIdBySession(sid);
            if (userId == null) return NotFound("Session not found");

            if (buildingService.IsBuildingExist((int)userId, buildingDto.X, buildingDto.Y))
                return BadRequest("Building already exist");

            buildingService.AddBuilding((int)userId, buildingDto);

            return Ok();
        }

        [HttpGet("up/{sid}/{id}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Up(string sid, int id)
        {
            var checkResult = buildingService.CheckBuilding(sid, id);
            if (!string.IsNullOrEmpty(checkResult)) return NotFound(checkResult);

            buildingService.Up(sid, id);

            return Ok();
        }

        [HttpGet("down/{sid}/{id}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Down(string sid, int id)
        {
            var checkResult = buildingService.CheckBuilding(sid, id);
            if (!string.IsNullOrEmpty(checkResult)) return NotFound(checkResult);

            buildingService.Down(sid, id);

            return Ok();
        }

        [HttpGet("remove/{sid}/{id}")]
        [ProducesResponseType(typeof(IEnumerable<BuildingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Remove(string sid, int id)
        {
            var checkResult = buildingService.CheckBuilding(sid, id);
            if (!string.IsNullOrEmpty(checkResult)) return NotFound(checkResult);

            buildingService.Remove(sid, id);

            return Ok();
        }
    }
}
