using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//NOTE: DockerコンテナではHttps証明書を用意していないのでリダイレクトしない
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// rooting api by minimal api
app.MapGet("/health", () => Results.Ok(new { ok = true }));
app.MapGet("/test", () => Results.Ok(new { ok = "test" }));

app.Run();