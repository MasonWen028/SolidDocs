using SolidDocs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SolidDocs services
builder.Services.AddSolidDocs(options =>
{
    options.RootPath = "wwwroot/soliddocs";
    options.JwtSecret = "your-super-secret-key-with-at-least-32-characters-for-solid-docs-mvp";
    options.BaseUrl = "https://localhost:7001";
    options.RoutePrefix = "soliddocs";
    options.EnableCors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use SolidDocs middleware
app.UseSolidDocs();

app.Run(); 