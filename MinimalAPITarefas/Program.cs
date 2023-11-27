using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registrar o contexto
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TarefasDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Olá mundo!");

//Mapeamento
//EndPoint para retornar uma lita de tarefas
app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

//EndPoint para criar tarefas
app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
});

app.Run();

class Tarefa
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsConcluida { get; set; }
}

class AppDbContext : DbContext
{
    //Construtor
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
    {
    }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}
