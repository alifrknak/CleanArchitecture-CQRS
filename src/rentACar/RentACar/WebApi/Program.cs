using Application;
using Core.CrossCuttingConcerns.Exceptions.Extensions;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddApplicationServices(); //
builder.Services.AddPersistenceService(builder.Configuration);//
builder.Services.AddDistributedMemoryCache();//
builder.Services.AddHttpContextAccessor(); //


// Redis 
/*builder.Services.AddStackExchangeRedisCache(opt =>
{
	opt.Configuration = "localhost:6379";

});//
*/


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//if (app.Environment.IsProduction()) //
	app.ConfigureCustomExceptionMiddleware(); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
