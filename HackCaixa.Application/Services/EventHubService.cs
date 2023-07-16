using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using HackCaixa.Application.Models.ViewModels;
using HackCaixa.Application.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace HackCaixa.Application.Services
{
    public class EventHubService : IEventHubService
    {

        private readonly IConfiguration _configuration;

        public EventHubService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //private const string connectionString = "Endpoint=sb://eventhack.servicebus.windows.net/;SharedAccessKeyName=hack;SharedAccessKey=HeHeVaVqyVkntO2FnjQcs2Ilh/4MUDo4y+AEhKp8z+g=;EntityPath=simulacoes";
        //private const string eventHubName = "simulacoes";

        public async Task EnviarEventoParaEventHub(SimulacaoHubViewModel resultado)
        {
            var json = JsonConvert.SerializeObject(resultado);
            await using (var producerClient = new EventHubProducerClient(_configuration.GetConnectionString("EventHubEndpoint"), _configuration.GetConnectionString("EventHubName")))
            {
                using var eventBatch = await producerClient.CreateBatchAsync();

                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(json)));

                await producerClient.SendAsync(eventBatch);
            }
        }

        public async Task EnviarEventoParaEventHubSemProduto(SimulacaoSemResultadoHubViewModel resultado)
        {
            var json = JsonConvert.SerializeObject(resultado);
            await using (var producerClient = new EventHubProducerClient(_configuration.GetConnectionString("EventHubEndpoint"), _configuration.GetConnectionString("EventHubName")))
            {
                using var eventBatch = await producerClient.CreateBatchAsync();

                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(json)));                

                await producerClient.SendAsync(eventBatch);
            }
        }


    }
}
