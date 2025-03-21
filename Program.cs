using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MacAdressContext>((options) =>
{
    options.UseMySQL("Server=localhost;Database=macadress;Uid=root;");
});

builder.Services.AddScoped<IValidator<MacAdress>, MacAdressValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/macadress", async (MacAdressContext context) =>
{
    return Results.Ok(await context.MacAdresses.ToListAsync());
});

app.MapGet("/macadress/{id}", async (MacAdressContext context, int id) =>
{
    var macAdress = await context.MacAdresses.FindAsync(id);
    if (macAdress == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(macAdress);
});

app.MapPost("/macadress", async (MacAdress macAdress, MacAdressContext context) =>
{
    var validator = new MacAdressValidator();
    var result = validator.Validate(macAdress);
    if (!result.IsValid)
    {
        return Results.BadRequest(result.Errors);
    }
    await context.MacAdresses.AddAsync(macAdress);
    await context.SaveChangesAsync();
    return Results.Created($"/macadress/{macAdress.Id}", macAdress);
});

app.MapDelete("/macadress/{id}", async (MacAdressContext context, int id) =>
{
    var macAdress = await context.MacAdresses.FindAsync(id);
    if (macAdress == null)
    {
        return Results.NotFound();
    }
    context.MacAdresses.Remove(macAdress);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPut("/macadress/{id}", async (MacAdressContext context, int id, MacAdress macAdress) =>
{
    var validator = new MacAdressValidator();
    var result = validator.Validate(macAdress);
    if (!result.IsValid)
    {
        return Results.BadRequest(result.Errors);
    }
    var existingMacAdress = await context.MacAdresses.FindAsync(id);
    if (existingMacAdress == null)
    {
        return Results.NotFound();
    }
    existingMacAdress.Mac = macAdress.Mac;
    existingMacAdress.expirationDate = macAdress.expirationDate;
    existingMacAdress.Email = macAdress.Email;
    await context.SaveChangesAsync();
    return Results.Ok(existingMacAdress);
});

app.MapPatch("/macadress/{id}", async (MacAdressContext context, int id, UpdateMacAdressDTO updateMacAdressDTO) =>
{
    var validator = new UpdateMacAdressDTO.Validator();
    var result = validator.Validate(updateMacAdressDTO);
    if (!result.IsValid)
    {
        return Results.BadRequest(result.Errors);
    }
    var existingMacAdress = await context.MacAdresses.FindAsync(id);
    if (existingMacAdress == null)
    {
        return Results.NotFound();
    }
    existingMacAdress.Mac = updateMacAdressDTO.Mac;
    existingMacAdress.expirationDate = updateMacAdressDTO.expirationDate;
    existingMacAdress.Email = updateMacAdressDTO.Email;
    await context.SaveChangesAsync();
    return Results.Ok(existingMacAdress);
});

app.Run();


