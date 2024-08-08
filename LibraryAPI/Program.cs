using LibraryAPI.BusinessFacade;
using LibraryAPI.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//menambahkan scpe unutk koneksi ke db
builder.Services.AddDbContext<DataContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibrayServiceConnection"));
});
//menambahkan configurasi auto mapper 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//menambah Controller agar terbaca
builder.Services.AddScoped<IPublisher, PublisherFacade>();
builder.Services.AddScoped<ICategory, CategoryFacade>();
builder.Services.AddScoped<IStorageLocation, StorageLocationFacade>();
builder.Services.AddScoped<IBuku, BukuFacade>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
