using Application.Commands;
using Domain.Models;
using Infrastructure.Data.Repositories;
using Moq;

namespace Test
{
    public class MedicoTests : TestBase
    {
        private Mock<IMedicoRepository> _medicoRepositoryMock;

        [SetUp]
        public void SetUp()
            => _medicoRepositoryMock = new Mock<IMedicoRepository>();

        [Test]
        public async Task CadastrarMedico_DeveCadastrarComSucesso()
        {
            // Arrange
            var command = new CreateMedicoCommand("Drauzio Varella", 3711, "drauzinho@globo.com");
            Medico medico = null;

            _medicoRepositoryMock
                .Setup(repo => repo.GetByCRMAsync(It.IsAny<int>()))
                .ReturnsAsync(medico);

            _medicoRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Medico>()))
                .ReturnsAsync(1);


            // Act
            var handler = new CreateMedicoCommandHandler(_medicoRepositoryMock.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }
    }
}