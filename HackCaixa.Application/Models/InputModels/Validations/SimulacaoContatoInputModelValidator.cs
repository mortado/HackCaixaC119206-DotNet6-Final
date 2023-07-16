
using FluentValidation;

namespace HackCaixa.Application.Models.InputModels.Validations
{
    public class SimulacaoContatoInputModelValidator : AbstractValidator<SimulacaoContatoInputModel>
    {
        public SimulacaoContatoInputModelValidator()
        {
            RuleFor(p => p.Simulacao).NotEmpty()
               .WithErrorCode("simulacao_required")
               .WithMessage("O objeto 'simulacao' é obrigatório");

            RuleFor(p => p.Contato).NotEmpty()
               .WithErrorCode("contato_required")
               .WithMessage("O campo 'contato' é obrigatório");

            RuleFor(p => p.Simulacao.ValorDesejado).NotEmpty()
               .WithErrorCode("valor_desejado_required")
               .WithMessage("O campo 'valorDesejado' é obrigatório");

            RuleFor(p => p.Simulacao.Prazo).NotEmpty()
                .WithErrorCode("prazo_required")
                .WithMessage("O campo 'prazo' é obrigatório");

            RuleFor(p => p.Contato.TipoPessoa).NotEmpty()
               .WithErrorCode("tipo_pessoa_required")
               .WithMessage("O campo 'TipoPessoa' é obrigatório");

            RuleFor(p => p.Contato.CpfCnpj).NotEmpty()
                .WithErrorCode("cpf_cnpj_required")
                .WithMessage("O campo 'CpfCnpj' é obrigatório");



            RuleFor(p => p.Contato.TipoPessoa).InclusiveBetween((short)1, (short)2)
               .WithErrorCode("tipo_pessoa_invalid")
               .WithMessage("'TipoPessoa' deve ser 1 para PF ou 2 para PJ.");
            

            RuleFor(p => p.Simulacao.Prazo).GreaterThan((short)0)
                .WithErrorCode("prazo_invalid")
                .WithMessage("O prazo informado deve ser maior que 0");

            RuleFor(p => p.Simulacao.ValorDesejado).GreaterThan((short)0)
                .WithErrorCode("valor_desejado_invalid")
                .WithMessage("O valorDesejado informado deve ser maior que 0");


            
        }
    }
}
