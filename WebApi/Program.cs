using Application.Commands;
using Application.Interfaces.Repositories;
using Application.Mapper;
using Application.Queries;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

AddMediatr(builder);
AddRepositories(builder);

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.MapGet("/", (HttpContext context) =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();

static void AddRepositories(WebApplicationBuilder builder)
{
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped(typeof(IMedicoRepository), typeof(MedicoRepository));
    builder.Services.AddScoped(typeof(IAgendaRepository), typeof(AgendaRepository));
    builder.Services.AddScoped(typeof(IConsultaRepository), typeof(ConsultaRepository));
}

static void AddMediatr(WebApplicationBuilder builder)
{
    builder.Services.AddMediatR(typeof(CreateMedicoCommand).GetTypeInfo().Assembly);
    builder.Services.AddMediatR(typeof(CreateAgendaCommand).GetTypeInfo().Assembly);
    builder.Services.AddMediatR(typeof(CreateConsultaCommand).GetTypeInfo().Assembly);
    builder.Services.AddMediatR(typeof(DeleteConsultaCommand).GetTypeInfo().Assembly);
    builder.Services.AddMediatR(typeof(GetConsultasQuery).GetTypeInfo().Assembly);
    builder.Services.AddMediatR(typeof(GetAgendasQuery).GetTypeInfo().Assembly);
}