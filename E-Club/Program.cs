var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // في Production، شغل Swagger برضه للـ Testing
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Club API V1");
    });
}

// ◀️ مهم: تحويل HTTP إلى HTTPS
app.UseHttpsRedirection();

// ◀️ ملفات ثابتة (لو عندك)
app.UseStaticFiles();

// ◀️ Cors لازم يكون قبل Authentication
app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

app.Run();