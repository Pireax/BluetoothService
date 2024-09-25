using BluetoothService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<LoggingExceptionHandler>();
builder.Services.AddSingleton<IBluetoothDeviceWatcher, BluetoothDeviceWatcher>();
builder.Services.AddSingleton<IBluetoothInterface, WindowsBluetoothInterface>();
builder.Services.AddSingleton<BluetoothService.BluetoothService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(o => { });

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
