using FluentValidation;
using HackCaixa.Application.Repositories.Interfaces;
using HackCaixa.Application.Repositories;
using HackCaixa.Application.Data;
using HackCaixa.Application.Models.InputModels;
using HackCaixa.Application.Models.InputModels.Validations;
using HackCaixa.Application.Services;
using HackCaixa.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ProdutoDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"))
    );

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IEventHubService, EventHubService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IValidator<SimulacaoInputModel>, SimulacaoInputModelValidator>();
builder.Services.AddScoped<IValidator<SimulacaoContatoInputModel>, SimulacaoContatoInputModelValidator>();
builder.Services.AddScoped<IValidator<SimulacaoValorInputModel>, SimulacaoValorInputModelValidator>();
builder.Services.AddScoped<IValidator<SimulacaoPrazoInputModel>, SimulacaoPrazoInputModelValidator>();

var app = builder.Build();

//// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
