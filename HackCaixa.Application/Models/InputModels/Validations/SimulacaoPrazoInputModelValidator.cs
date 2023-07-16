
using FluentValidation;

namespace HackCaixa.Application.Models.InputModels.Validations
{
    public class SimulacaoPrazoInputModelValidator : AbstractValidator<SimulacaoPrazoInputModel>
    {
        public SimulacaoPrazoInputModelValidator()
        {
            RuleFor(p => p.Prazo).NotEmpty()
               .WithErrorCode("valor_desejado_required")
               .WithMessage("O campo 'valorDesejado' é obrigatório");

            RuleFor(p => p.Prazo).GreaterThan((short)0)
                 .WithErrorCode("prazo_invalid")
                 .WithMessage("O prazo informado deve ser maior que 0");
        }
    }
}
