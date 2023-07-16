
using FluentValidation;

namespace HackCaixa.Application.Models.InputModels.Validations
{
    public class SimulacaoInputModelValidator : AbstractValidator<SimulacaoInputModel>
    {
        public SimulacaoInputModelValidator()
        {
            RuleFor(p => p.ValorDesejado).NotEmpty()
               .WithErrorCode("valor_desejado_required")
               .WithMessage("O campo 'valorDesejado' é obrigatório");            

            RuleFor(p => p.Prazo).NotEmpty()
                .WithErrorCode("prazo_required")
                .WithMessage("O campo 'prazo' é obrigatório");

            RuleFor(p => p.ValorDesejado).GreaterThan((short)0)
                .WithErrorCode("valor_invalid")
                .WithMessage("O valor informado deve ser maior que 0");

            RuleFor(p => p.Prazo).GreaterThan((short)0)
                .WithErrorCode("prazo_invalid")
                .WithMessage("O prazo informado deve ser maior que 0");
        }
    }
}
