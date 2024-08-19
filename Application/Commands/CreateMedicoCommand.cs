using Domain.Models;
using Infrastructure.Data.Repositories;
using MediatR;

namespace Application.Commands
{
    public record CreateMedicoCommand(string Nome, int CRM, string? Email) : IRequest<int>;

    public class CreateMedicoCommandHandler : IRequestHandler<CreateMedicoCommand, int>
    {
        private readonly IMedicoRepository _repository;

        public CreateMedicoCommandHandler(IMedicoRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateMedicoCommand request, CancellationToken cancellationToken)
        {
            var existingMedico = await _repository.GetByCRMAsync(request.CRM);
            if (existingMedico != null)
                throw new InvalidOperationException("Já existe um médico cadastrado com esse CRM.");

            var medico = new Medico
            {
                Nome = request.Nome,
                CRM = request.CRM,
                Email = request.Email
            };

            return await _repository.AddAsync(medico);
        }
    }
}