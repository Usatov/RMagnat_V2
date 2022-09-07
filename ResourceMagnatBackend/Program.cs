using Microsoft.EntityFrameworkCore;
using ResourceMagnat.Data;
using ResourceMagnat.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��������� �������� ��
builder.Services.AddDbContext<MainDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// ��������� ������
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ��������� �������
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BuildingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(x => 
         x.AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:3000"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ������������� ����������
PrepareApplication.Init(app, builder.Configuration);

app.Run();
