using CommentsAPI.Data;
using CommentsAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CommentsDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CommentsDB"));
});
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>() //probaly not gonna use this, check Prog. folder for identity documentation
    .AddEntityFrameworkStores<CommentsDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<ApplicationUser>(); //probaly not gonna use this, check Prog. folder for identity documentation

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
