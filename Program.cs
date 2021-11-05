using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

DatabaseSettings.ConnectionString = builder.Configuration.GetSection("TodoDatabaseSettings:ConnectionString").Value;
DatabaseSettings.DatabaseName = builder.Configuration.GetSection("TodoDatabaseSettings:DatabaseName").Value;
DatabaseSettings.CollectionName = builder.Configuration.GetSection("TodoDatabaseSettings:CollectionName").Value;
DatabaseSettings.IsSSL = Convert.ToBoolean(builder.Configuration.GetSection("TodoDatabaseSettings:IsSSL").Value);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();
builder.Services.AddScoped<DatabaseSettings>();
builder.Services.AddCors(options => options.AddDefaultPolicy(builder => 
{ 
    builder.WithOrigins(
 "https://mini-todo-appz.herokuapp.com/v1/todos", 
        "https://mini-todo-appz.herokuapp.com/v1/todos/id",
        "https://mini-todo-appz.herokuapp.com",
        "https://localhost:7252/v1/todos",
        "https://localhost:7252");
}));

var app = builder.Build();

app.UseCors(builder => builder
.AllowAnyOrigin());


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Post API v1"));

app.MapGet("/", () => "Mini To Do by Assis Zang");

app.MapGet("v1/todos", (DatabaseSettings settings) =>
{
    var todos = settings.Todos.FindAsync(x => true).Result.ToList();
    return Results.Ok(todos);
});

app.MapGet("v1/todos/id", (DatabaseSettings settings, string id) =>
{
    var todo = settings.Todos.FindAsync(x => x._id == id).Result.FirstOrDefault();
    return Results.Ok(todo);
});

app.MapPost("v1/todos", (DatabaseSettings settings, Todo todo) =>
{
    todo._id = ObjectId.GenerateNewId().ToString() ?? todo._id;
    settings.Todos.InsertOne(todo);
    return Results.Created($"/v1/todos/{todo._id}", todo);
});

app.MapPut("v1/todos", (DatabaseSettings settings, Todo todo) => 
{
    settings.Todos.ReplaceOne(x => x._id == todo._id, todo);
    return Results.Ok(todo);
});

app.MapDelete("v1/todos", (DatabaseSettings settings, string id) =>
{
    settings.Todos.DeleteOne(x => x._id == id);
    return Results.Ok($"Deleted id: {id}");
});

app.Run();