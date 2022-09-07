using AutoMapper;
using ResourceMagnat.Data;
using ResourceMagnat.Dto;
using ResourceMagnat.Models;

namespace ResourceMagnat.Services;

public class BuildingService
{
    private readonly MainDbContext context;
    private readonly IMapper mapper;
    private readonly SessionService sessionService;

    public BuildingService(MainDbContext _context, IMapper _mapper, SessionService _sessionService)
    {
        context = _context;
        this.mapper = _mapper;
        sessionService = _sessionService;
    }

    public IEnumerable<BuildingTypeDto> List()
    {
        return mapper.Map<IEnumerable<BuildingTypeDto>>(context.BuildingTypes.OrderBy(i => i.Id));
    }

    public IEnumerable<BuildingDto> Get(int userId)
    {
        var buildings = context.Buildings.Where(i => i.UserId == userId);

        return mapper.Map<IEnumerable<BuildingDto>>(buildings);
    }

    public bool IsBuildingExist(int userId, int x, int y)
    {
        var existBuilding = context.Buildings.FirstOrDefault(i => i.UserId == userId && i.X == x && i.Y == y);
        return existBuilding != null;
    }

    public void AddBuilding(int userId, BuildingDto buildingDto)
    {
        var user = context.Users.FirstOrDefault(i => i.Id == userId);
        if (user == null) return;

        buildingDto.UserId = (int)userId;
        buildingDto.Level = 1;
        var building = mapper.Map<Building>(buildingDto);

        user.UpdateCoins();

        if (user.Coins >= building.GetCoinsCost(context))
        {
            context.Buildings.Add(building);
            context.SaveChanges();

            user.UpdateCoinsPerSecond(context);
            user.Coins -= building.GetCoinsCost(context);
        }
        context.SaveChanges();
    }

    public string CheckBuilding(string sid, int id)
    {
        var userId = sessionService.GetUserIdBySession(sid);
        if (userId == null) return "Session not found";

        var existBuilding = context.Buildings.FirstOrDefault(i => i.Id == id);
        if (existBuilding == null) return "Building not found";
        if (existBuilding.UserId != userId) return "Building is not yours";

        return "";
    }

    public void Up(string sid, int id)
    {
        var userId = sessionService.GetUserIdBySession(sid);

        var user = context.Users.FirstOrDefault(i => i.Id == userId);
        if (user == null) return;
        
        var existBuilding = context.Buildings.FirstOrDefault(i => i.Id == id);
        if (existBuilding == null) return;

        user.UpdateCoins();
        if (user.Coins >= existBuilding.GetCoinsCost(context))
        {
            user.Coins -= existBuilding.GetCoinsCost(context);
            existBuilding.Level++;
            user.UpdateCoinsPerSecond(context);
        }
        context.SaveChanges();
    }

    public void Down(string sid, int id)
    {
        var userId = sessionService.GetUserIdBySession(sid);

        var user = context.Users.FirstOrDefault(i => i.Id == userId);
        if (user == null) return;

        var existBuilding = context.Buildings.FirstOrDefault(i => i.Id == id);
        if (existBuilding == null) return;

        user.UpdateCoins();
        user.Coins += existBuilding.GetCoinsCost(context);
        existBuilding.Level--;
        user.UpdateCoinsPerSecond(context);

        context.SaveChanges();

        context.SaveChanges();
    }

    public void Remove(string sid, int id)
    {
        var userId = sessionService.GetUserIdBySession(sid);

        var user = context.Users.FirstOrDefault(i => i.Id == userId);
        if (user == null) return;

        var existBuilding = context.Buildings.FirstOrDefault(i => i.Id == id);
        if (existBuilding == null) return;

        user.UpdateCoins();
        while (existBuilding.Level >= 1)
        {
            user.Coins += existBuilding.GetCoinsCost(context);
            existBuilding.Level--;
        }

        context.Buildings.Remove(existBuilding);
        user.UpdateCoinsPerSecond(context);

        context.SaveChanges();
    }
}