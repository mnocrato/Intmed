using Application.Commands;
using Domain.Models;
using Infrastructure.Data.Repositories;
using Moq;

namespace Test
{
    public class AgendaTests : TestBase
    {
        private Mock<IAgendaRepository> _agendaRepositoryMock;
        private Mock<IMedicoRepository> _medicoRepositoryMock;

        public AgendaTests()
        {
            _agendaRepositoryMock = new Mock<IAgendaRepository>();
            _medicoRepositoryMock = new Mock<IMedicoRepository>();
        }

        [Test]
        public async Task CreateAgendaCommandHandler_ShouldCreateAgenda()
        {
            // Arrange
            var command = new CreateAgendaCommand
            {
                MedicoId = 1,
                Dia = DateTime.Today,
                Horarios = new List<string> { "09:00", "10:00" }
            };

            _agendaRepositoryMock.Setup(repo => repo.GetByMedicoAndDiaAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync((Agenda)null);

            _agendaRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Agenda>()))
                .ReturnsAsync(1);

            _medicoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Medico { Id = 1, Nome = "Test", CRM = 123 });


            var handler = new CreateAgendaCommandHandler(_agendaRepositoryMock.Object, _medicoRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }
    }
}