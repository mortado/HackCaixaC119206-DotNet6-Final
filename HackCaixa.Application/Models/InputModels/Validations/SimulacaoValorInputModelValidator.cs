
using FluentValidation;

namespace HackCaixa.Application.Models.InputModels.Validations
{
    public class SimulacaoValorInputModelValidator : AbstractValidator<SimulacaoValorInputModel>
    {
        public SimulacaoValorInputModelValidator()
        {
            RuleFor(p => p.ValorDesejado).NotEmpty()
               .WithErrorCode("valor_desejado_required")
               .WithMessage("O campo 'valorDesejado' é obrigatório");

            RuleFor(p => p.ValorDesejado).GreaterThan((short)0)
                 .WithErrorCode("valor_invalid")
                 .WithMessage("O valor informado deve ser maior que 0");

        }
    }
}
