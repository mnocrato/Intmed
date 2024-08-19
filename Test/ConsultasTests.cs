using Application.Commands;
using Application.Queries;
using Domain.Models;
using Infrastructure.Data.Repositories;
using Moq;

namespace Test
{
    public class ConsultasTests : TestBase
    {
        private readonly Mock<IConsultaRepository> _consultaRepositoryMock;
        private readonly Mock<IAgendaRepository> _agendaRepositoryMock;

        public ConsultasTests()
        {
            _consultaRepositoryMock = new Mock<IConsultaRepository>();
            _agendaRepositoryMock = new Mock<IAgendaRepository>();
        }

        [Test]
        public async Task ListarConsultasQueryHandler_ShouldReturnConsultas()
        {
            // Arrange
            var query = new GetConsultasQuery();
            var handler = new GetConsultasQueryHandler(_consultaRepositoryMock.Object, Mapper);
            _consultaRepositoryMock.Setup(repo => repo.GetConsultasFromDate(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Consulta>
                {
                        new() {
                            Id = 1,
                            Dia = DateTime.Today,
                            Horario = "10:00",
                            DataAgendamento = DateTime.Now,
                            Medico = new Medico { Id = 1, CRM = 123, Nome = "Dr. Test", Email = "test@example.com" }
                        }
                });

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Empty);
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(1, Is.EqualTo(result.First().Id));
            });
        }

        [Test]
        public async Task MarcarConsultaCommandHandler_ShouldMarcarConsulta()
        {
            // Arrange
            var command = new CreateConsultaCommand { AgendaId = 1, Horario = "09:00" };
            var novaConsulta = new Consulta
            {
                DataAgendamento = DateTime.UtcNow,
                Dia = DateTime.UtcNow,
                Horario = "09:00",
                MedicoId = 1,
                Id = 1
            };
            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Agenda
               {
                   Dia = DateTime.UtcNow,
                   Horarios = new List<string> { "09:00" },
                   Id = 1,
                   Medico = new Medico
                   {
                       Id = 1,
                       CRM = 2,
                       Nome = "Nome",
                       Email = "teste@gmail"
                   }
               });

            _consultaRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Consulta>()))
                .ReturnsAsync(1);
            _consultaRepositoryMock.Setup(repo => repo.GetConsultaByIdWithMedico(It.IsAny<int>()))
                .ReturnsAsync(novaConsulta);

            var handler = new CreateConsultaCommandHandler(_consultaRepositoryMock.Object, _agendaRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(1));
                Assert.That(result.Horario, Is.EqualTo("09:00"));
            });
        }

        [Test]
        public async Task DesmarcarConsultaCommandHandler_ShouldRemoveConsulta()
        {
            // Arrange
            var command = new DeleteConsultaCommand(1);
            var handler = new DeleteConsultaCommandHandler(_consultaRepositoryMock.Object);
            _consultaRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Consulta
               {
                   Id = 1,
                   Dia = DateTime.Today,
                   Horario = "10:00",
                   DataAgendamento = DateTime.Now,
                   Medico = new Medico { Id = 1, CRM = 123, Nome = "Dr. Test", Email = "test@example.com" }
               });

            // Act
            await handler.Handle(command, CancellationToken.None);
            Mock.Get(_consultaRepositoryMock.Object).Verify(repo => repo.DeleteAsync(It.Is<int>(c => c == 1)), Times.Once);
            Mock.Get(_consultaRepositoryMock.Object).Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

    }
}