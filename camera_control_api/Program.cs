using camera_control_project;

var builder =
    WebApplication.CreateBuilder(args);


// ------------------------------------
// Services
// ------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "FrontendPolicy",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// ------------------------------------
// Build
// ------------------------------------

var app = builder.Build();


// ------------------------------------
// Middleware
// ------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


// CORS

app.UseCors(
    "FrontendPolicy"
);


app.MapControllers();


// ------------------------------------
// Run
// ------------------------------------

app.Run();
