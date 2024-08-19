using Application.Commands;
using Application.Mapper;
using Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Test
{
    public abstract class TestBase
    {
        protected IMediator Mediator { get; private set; }
        protected IMapper Mapper { get; private set; }

        protected TestBase()
        {
            var services = new ServiceCollection();

            services.AddMediatR(typeof(CreateMedicoCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateAgendaCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateConsultaCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(DeleteConsultaCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetConsultasQuery).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetAgendasQuery).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            var provider = services.BuildServiceProvider();
            Mediator = provider.GetRequiredService<IMediator>();
            Mapper = provider.GetRequiredService<IMapper>();
        }
    }
}