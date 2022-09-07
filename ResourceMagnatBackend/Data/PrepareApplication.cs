﻿using ResourceMagnat.Models;
using ResourceMagnat.Services;

namespace ResourceMagnat.Data;

public class PrepareApplication
{
    public static void Init(IApplicationBuilder app, ConfigurationManager config)
    {
        Config.SESSION_LENGTH_IN_SECONDS = config.GetValue<int>("SessionLengthInSeconds");

        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var sessionServise = serviceScope.ServiceProvider.GetService<SessionService>();
            sessionServise?.ClearEndedSessions();

            SetData(serviceScope.ServiceProvider.GetService<MainDbContext>());
        }
    }

    public static void SetData(MainDbContext? context)
    {
        if (context == null) return;
        if (context.BuildingTypes.Any()) return;

        context.BuildingTypes.AddRange(
            new BuildingType
            {
                Id = 1,
                Name = "Труба",
                Description = "Источник нефти",
                InitionalCoins = 1,
                InitionalCost = 10
            },
            new BuildingType
            {
                Id = 2,
                Name = "Насос",
                Description = "Увеличивает перекачку нефти по трубе",
                InitionalCoins = 5,
                InitionalCost = 100
            },
            new BuildingType
            {
                Id = 3,
                Name = "Перегонный завод",
                Description = "Превращает нефть в лёгкие фракции",
                InitionalCoins = 50,
                InitionalCost = 1000
            },
            new BuildingType
            {
                Id = 4,
                Name = "Нефтяная вышка",
                Description = "Добывает нефть из океанических глубин",
                InitionalCoins = 100,
                InitionalCost = 5000
            },
            new BuildingType
            {
                Id = 5,
                Name = "Поезд с нефтью",
                Description = "Привозит нефть из другой страны",
                InitionalCoins = 200,
                InitionalCost = 10000
            });

        context.SaveChanges();
    }
}