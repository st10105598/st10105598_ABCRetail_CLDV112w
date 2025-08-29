using st10105598_ABCRetail_CLDV112w.Models;
using st10105598_ABCRetail_CLDV112w.Services;

var builder = WebApplication.CreateBuilder(args);

// Get and validate connection string before registering services
var connectionString = builder.Configuration.GetConnectionString("AzureStorage");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'AzureStorage' is not configured.");
}

// Register services
builder.Services.AddControllersWithViews();

// Register TableStorageService as singleton
builder.Services.AddSingleton(new TableStorageService(connectionString));

var app = builder.Build();

// Seed sample data once on startup
using (var scope = app.Services.CreateScope())
{
    var tableService = scope.ServiceProvider.GetRequiredService<TableStorageService>();

    // Upsert sample customers (safe, will insert or update)
    tableService.UpsertCustomer(new CustomerEntity("Customer", "1", "John Doe", "john@example.com"));
    tableService.UpsertCustomer(new CustomerEntity("Customer", "2", "Alice Smith", "alice@example.com"));
    tableService.UpsertCustomer(new CustomerEntity("Customer", "3", "Bob Lee", "bob@example.com"));
    tableService.UpsertCustomer(new CustomerEntity("Customer", "4", "Sarah Khan", "sarah@example.com"));
    tableService.UpsertCustomer(new CustomerEntity("Customer", "5", "David Brown", "david@example.com"));

    // Upsert sample products (safe, will insert or update)
    tableService.UpsertProduct(new ProductEntity("Product", "1", "Laptop", 1200));
    tableService.UpsertProduct(new ProductEntity("Product", "2", "Phone", 800));
    tableService.UpsertProduct(new ProductEntity("Product", "3", "Headphones", 150));
    tableService.UpsertProduct(new ProductEntity("Product", "4", "Keyboard", 100));
    tableService.UpsertProduct(new ProductEntity("Product", "5", "Monitor", 300));
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
