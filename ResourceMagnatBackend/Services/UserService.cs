using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ResourceMagnat.Data;
using ResourceMagnat.Dto;
using ResourceMagnat.Models;

namespace ResourceMagnat.Services;

public class UserService
{
    private readonly MainDbContext context;
    private readonly IMapper mapper;
    private readonly SessionService sessionService;

    public UserService(MainDbContext _context, IMapper _mapper, SessionService _sessionService)
    {
        context = _context;
        this.mapper = _mapper;
        sessionService = _sessionService;
    }

    public IEnumerable<User> GetUsers() => context.Users.OrderBy(i => i.Id).ToList();

    public UserDto GetOrCreate(string uid)
    {
        var user = context.Users.FirstOrDefault(i => i.Uid == uid) ?? CreateUser(uid);

        user.UpdateCoins();
        context.SaveChanges();

        var result = mapper.Map<UserDto>(user);
        result.SessionId = sessionService.CreateSession(user);

        return result;
    }

    private User CreateUser(string uid)
    {
        // Создаём пользователя
        var user = new User(uid);

        context.Users.Add(user);
        context.SaveChanges();

        // Добавляем ему начальное здание
        var building = new Building
        {
            UserId = user.Id,
            BuildingTypeId = 1,
            X = 3,
            Y = 2,
            Level = 1
        };
        context.Buildings.Add(building);
        context.SaveChanges();

        // Пересчитываем доход в секунду
        user.UpdateCoinsPerSecond(context);
        // context.Entry(user).State = EntityState.Modified;
        context.SaveChanges();

        return user;
    }

}