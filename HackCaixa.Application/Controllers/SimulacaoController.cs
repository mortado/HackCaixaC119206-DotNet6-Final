using FluentValidation;
using HackCaixa.Application.Models.InputModels;
using HackCaixa.Application.Models.Records;
using HackCaixa.Application.Models.ViewModels;
using HackCaixa.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace HackCaixa.Application.Controllers
{
    [Route("api")]
    [ApiController]
    public class SimulacaoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IEventHubService _eventHubService;
        private readonly IWebHostEnvironment _environment;
        private readonly IValidator<SimulacaoInputModel> _simulacaoValidator;
        private readonly IValidator<SimulacaoContatoInputModel> _simulacaoContatoValidator;
        private readonly IValidator<SimulacaoValorInputModel> _simulacaoValorValidator;
        private readonly IValidator<SimulacaoPrazoInputModel> _simulacaoPrazoValidator;
        private readonly FuncoesController _funcoes;


        public SimulacaoController(IProdutoService produtoService, IEventHubService eventHubService, IWebHostEnvironment environment, IValidator<SimulacaoInputModel> simulacaoValidator, IValidator<SimulacaoContatoInputModel> simulacaoContatoValidator, IValidator<SimulacaoValorInputModel> simulacaoValorValidator, IValidator<SimulacaoPrazoInputModel> simulacaoPrazoValidator)
        {
            _produtoService = produtoService;
            _eventHubService = eventHubService;
            _environment = environment;
            _simulacaoValidator = simulacaoValidator;
            _simulacaoValorValidator = simulacaoValorValidator;
            _simulacaoPrazoValidator = simulacaoPrazoValidator;
            _simulacaoContatoValidator = simulacaoContatoValidator;
            _funcoes = new FuncoesController();
        }


        [HttpPost("Simulacao")]
        [Description("Essa rota retorna o Resultado da Simulação para os sistemas PRICE e SAC." +
            "Usa o arredondamento das casas decimais como Padrão.")]
        public async Task<ActionResult<SimulacaoViewModel>> Simulacao([FromBody] SimulacaoInputModel solicitacao)
        {
            // Validações do Model e dos valores obrigatórios
            var validationResult = _simulacaoValidator.Validate(solicitacao);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ErroValidateRecord(e.ErrorCode, e.ErrorMessage));
                return BadRequest(errors);
            }

            // Chama a função Principal da Simulação
            return await Simulacao(solicitacao, true, new PessoaViewModel { });
        }


        [HttpPost("SimulacaoComContato")]
        [Description("Essa rota retorna o Resultado da Simulação para os sistemas PRICE e SAC." +
            "Usa o arredondamento das casas decimais. Nela é OBRIGATÓRIO enviar as informações de CONTATO")]
        public async Task<ActionResult<SimulacaoViewModel>> SimulacaoAPI([FromBody] SimulacaoContatoInputModel solicitacao)
        {
            // Validações do Model e dos valores obrigatórios
            var validationResult = _simulacaoContatoValidator.Validate(solicitacao);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ErroValidateRecord(e.ErrorCode, e.ErrorMessage));
                return BadRequest(errors);
            }

            // Chama a função Principal da Simulação
            return await Simulacao(solicitacao.Simulacao, true, solicitacao.Contato);
        }


        [HttpPost("SimulacaoPorValor")]
        [Description("Essa rota retorna uma simulação informando apenas o VALOR." +
            "O Prazo é localizado no banco de dados. As informações de contato são opcionais")]
        public async Task<ActionResult<SimulacaoCustomViewModel>> SimulacaoValor([FromBody] SimulacaoValorInputModel solicitacao)
        {
            // Validações do Model e dos valores obrigatórios
            var validationResult = _simulacaoValorValidator.Validate(solicitacao);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ErroValidateRecord(e.ErrorCode, e.ErrorMessage));
                return BadRequest(errors);
            }


            // Busca o Produto para o Valor Informado
            var produto = await _produtoService.ListarProdutoProximo(solicitacao.ValorDesejado, 0);

            if (produto.PorValor == null)
                return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = "Não há produtos disponiveis para os parâmetros informados" });


            // Faz as verificações dos Prazos Retornados para que o sistema possa calcular as prestações
            short prazo = 0;
            if (produto.PorValor.NuMinimoMeses == 0)
                prazo = produto.PorValor.NuMaximoMeses ?? 0;

            if (produto.PorValor.NuMaximoMeses == null)
                prazo = produto.PorValor.NuMinimoMeses ?? 0;

            if (prazo == 0)
                return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = "Não há produtos disponiveis para os parâmetros informados" });


            // Chama a função SimulacaoCustom para retornar a simulação requisitada.
            // O model de retorno é diferente da API principal pois ele adiciona os campos de VALOR e PRAZO calculados para que o front saiba os valores utilizados
            var resultado = await SimulacaoCustom(new SimulacaoInputModel { ValorDesejado = solicitacao.ValorDesejado, Prazo = prazo }, true, solicitacao.Contato ?? new PessoaViewModel { },produto.PorValor);

            if (resultado == null)
                return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = "Não há produtos disponiveis para os parâmetros informados" });

            return resultado;
        }


        [HttpPost("SimulacaoPorPrazo")]
        [Description("Essa rota retorna uma simulação informando apenas o PRAZO." +
            "O Valor é localizado no banco de dados. As informações de contato são opcionais")]
        public async Task<ActionResult<SimulacaoCustomViewModel>> SimulacaoPrazo([FromBody] SimulacaoPrazoInputModel solicitacao)
        {
            // Validações do Model e dos valores obrigatórios
            var validationResult = _simulacaoPrazoValidator.Validate(solicitacao);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ErroValidateRecord(e.ErrorCode, e.ErrorMessage));
                return BadRequest(errors);
            }

            // Faz as verificações dos Prazos Retornados para que o sistema possa calcular as prestações
            var produto = await _produtoService.ListarProdutoProximo(10000, solicitacao.Prazo);

            if (produto.PorPrazo == null)
                return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = "Não há produtos disponiveis para os parâmetros informados" });


            // Faz as verificações dos Prazos Retornados para que o sistema possa calcular as prestações
            decimal valor = 0;
            if (produto.PorPrazo.VrMaximo == null)
                valor = produto.PorPrazo.VrMinimo ?? 0;
            else
                valor = produto.PorPrazo.VrMaximo ?? 0;

            if (valor == 0)
                return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = "Não há produtos disponiveis para os parâmetros informados" });

            // Chama a função SimulacaoCustom para retornar a simulação requisitada.
            // O model de retorno é diferente da API principal pois ele adiciona os campos de VALOR e PRAZO calculados para que o front saiba os valores utilizados
            var resultado = await SimulacaoCustom(new SimulacaoInputModel { ValorDesejado = valor, Prazo = solicitacao.Prazo }, true, solicitacao.Contato ?? new PessoaViewModel { }, produto.PorPrazo);

            if (resultado == null)
                return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = "Não há produtos disponiveis para os parâmetros informados" });

            return resultado;
        }


        private async Task<ActionResult<SimulacaoViewModel>> Simulacao([FromBody] SimulacaoInputModel solicitacao, bool arredondarDesdeOInicio, PessoaViewModel contato)
        {
            

            if (!TryValidateModel(solicitacao))
                return BadRequest(ModelState);                      

            // Retorna a LISTA com as melhores condições fornecidas pelo banco de dados
            List<ProdutoViewModel> produtos = new List<ProdutoViewModel>();            
            try
            {
                produtos = await _produtoService.ListarMelhorProduto(solicitacao.ValorDesejado, solicitacao.Prazo);
            }
            catch (Exception ex)
            {
                // Trata o erro separadamente para DES e PROD
                if (_environment.IsDevelopment())
                    return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = $"{ex}. Valor: {solicitacao.ValorDesejado}. Prazo: {solicitacao.Prazo}" });
                else
                    return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = $"Estamos com instabilidade em nossos servidores, tente novamente em instantes!" });
            }


            // Se não tiver produto disponível, gera um EVENTO com essa informação e inclui os dados da simulação.
            // As informações de Contato serão enviadas para o EVENTHUB caso o Front tenha as enviado.
            if (produtos.Count  == 0)
            {
                ProdutoProximoViewModel produtosProximo = new ProdutoProximoViewModel();
                // Localiza as condições mais próximas para que a Central de Eventos saiba qual produto oferecer
                try
                {
                    produtosProximo = await _produtoService.ListarProdutoProximo(solicitacao.ValorDesejado, solicitacao.Prazo);                                                           
                }
                catch (Exception ex)
                {
                    // Se gerar erro, avisa a Central de Eventos o que o cliente estava tentando simular e não conseguiu
                    await _eventHubService.EnviarEventoParaEventHubSemProduto(new SimulacaoSemResultadoHubViewModel
                    {
                        ValorDesejado = solicitacao.ValorDesejado,                        
                        Prazo = solicitacao.Prazo,                        
                        Mensagem = "Cliente fez essa simulação e não encontrou um produto adequado. Tentamos conectar ao servidor para localizar as sugestões mais próximas, mas o servidor não conseguiu processar a requisição",                        
                        Sugestao = produtosProximo,                       
                        Contato = contato
                    });

                    // Se for em DEV, retorna o erro para tratamento
                    if (_environment.IsDevelopment())
                        return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = $"Ao tentar localizar o Produto Próximo, ocorreu o seguinte erro: {ex}. Retorno da API em Prod: 'Não há produtos disponiveis para os parâmetros informados'" });                    
                }


                try
                {
                    // Com as condições mais próximas, enviaremos ao EventHUB para oferecer ao cliente Produtos Semelhantes. 
                    // As sugestões vão para o EVENTHUB também.
                    await _eventHubService.EnviarEventoParaEventHubSemProduto(new SimulacaoSemResultadoHubViewModel {
                        ValorDesejado = solicitacao.ValorDesejado
                        , Prazo = solicitacao.Prazo
                        , Mensagem = "Cliente fez essa simulação e não encontrou um produto adequado. Existe duas sugestões: uma pelo valor escolhido e outra pelo prazo escolhido"
                        , Sugestao = produtosProximo
                        , Contato = contato 
                    });
                }
                catch (Exception ex)
                {
                    // Se gerar erro e for em DEV, retorna para tratamento
                    if (_environment.IsDevelopment())
                        return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = $"Ao tentar enviar para o EventHub, ocorreu o seguinte erro: {ex}. Retorno da API em Prod: 'Não há produtos disponiveis para os parâmetros informados'" });
                    
                }
                
                return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = "Não há produtos disponiveis para os parâmetros informados" });
            }
           

            // Gera a lista de Resultados para os produtos disponíveis (CASO TENHA MAIS DE UM) e seleciona o com Menor Juros para o cliente.
            // DETALHE: A função "_funcoes.Simular" possui como parâmetro se deseja utilizar o Arredondamento antes ou depois:
            // - Arredondamento antes: mesmo valor da API CAIXA (Por padrão)
            // - Arredondamento depois: mesmo valor do site CALCULADORA SAC E PRICE constante no regulamento.
            List<SimulacaoViewModel> resultados = new List<SimulacaoViewModel>();
            SimulacaoViewModel resultadoSelecionado = new SimulacaoViewModel();
            decimal menorJuros = produtos[0].PcTaxaJuros ?? 999;

            foreach (var prod in produtos)
            {
                SimulacaoViewModel resultado = _funcoes.Simular(arredondarDesdeOInicio, prod, solicitacao);

                // Faz a verificação da melhor taxa de juros
                decimal jurosProd = prod.PcTaxaJuros ?? 999;
                if (menorJuros >= jurosProd)
                {
                    menorJuros = jurosProd;
                    resultadoSelecionado = resultado;
                }
                
                // Adiciona os resultados de todas as simulações numa variável para tratamento
                resultados.Add(resultado);

                // Gera o Evento com o Resultado da e envia ao EVENTHUB
                try
                {
                    SimulacaoHubViewModel resultadoHub = new SimulacaoHubViewModel
                    {
                        CodigoProduto = resultado.CodigoProduto,
                        DescricaoProduto = resultado.DescricaoProduto,
                        TaxaJuros = resultado.TaxaJuros,
                        ResultadoSimulacao = resultado.ResultadoSimulacao,
                        Contato = contato
                    };
                    await _eventHubService.EnviarEventoParaEventHub(resultadoHub);                    
                }
                catch (Exception ex)
                {
                    // Se for em DEV, retorna o erro para tratamento
                    if (_environment.IsDevelopment())
                        return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = $"Ao tentar enviar para o EventHub, ocorreu o seguinte erro: {ex}. Retorno da API em Prod: 'Simulação normal enviada ao cliente'" });

                }                
            }
            
            return Ok(resultadoSelecionado);          
        }


        private async Task<ActionResult<SimulacaoCustomViewModel>> SimulacaoCustom([FromBody] SimulacaoInputModel solicitacao, bool arredondarDesdeOInicio, PessoaViewModel contato, ProdutoViewModel produto)
        {


            if (!TryValidateModel(solicitacao))
                return BadRequest(ModelState);

            // Gera o Resultados para o produto solicitado.
            // DETALHE: A função "_funcoes.Simular" possui como parâmetro se deseja utilizar o Arredondamento antes ou depois:
            // - Arredondamento antes: mesmo valor da API CAIXA (Por padrão)
            // - Arredondamento depois: mesmo valor do site CALCULADORA SAC E PRICE constante no regulamento.
            SimulacaoCustomViewModel resultado = new SimulacaoCustomViewModel();

            SimulacaoViewModel resultadoSelecionado = _funcoes.Simular(arredondarDesdeOInicio, produto, solicitacao);

            resultado.Simulacao = resultadoSelecionado;
            resultado.Prazo = solicitacao.Prazo;
            resultado.Valor = solicitacao.ValorDesejado;

            // Gera o Evento com o Resultado da Simulação e envia ao EVENTHUB
            try
            {
                SimulacaoHubViewModel resultadoHub = new SimulacaoHubViewModel
                {
                    CodigoProduto = resultadoSelecionado.CodigoProduto,
                    DescricaoProduto = resultadoSelecionado.DescricaoProduto,
                    TaxaJuros = resultadoSelecionado.TaxaJuros,
                    ResultadoSimulacao = resultadoSelecionado.ResultadoSimulacao,
                    Contato = contato
                };
                await _eventHubService.EnviarEventoParaEventHub(resultadoHub);
            }
            catch (Exception ex)
            {
                // Se for em DEV, retorna o erro para tratamento
                if (_environment.IsDevelopment())
                    return BadRequest(new ErroViewModel { Codigo = 400, Mensagem = $"Ao tentar enviar para o EventHub, ocorreu o seguinte erro: {ex}. Retorno da API em Prod: 'Simulação normal enviada ao cliente'" });
            }

            return Ok(resultado);
        }

    }
}
