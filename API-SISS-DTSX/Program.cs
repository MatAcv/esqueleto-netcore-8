using Data;
using Data.Repositories;
using Microsoft.SqlServer.Management.IntegrationServices;
using Microsoft.SqlServer.Management.Smo.Wmi;
using Microsoft.SqlServer.Server;
using MySql.Data.MySqlClient;

using System.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorPages();

//PARA MYSQL
var mySqlConfiguration = new Data.MySqlConfiguration(builder.Configuration.GetConnectionString("MySqlConn"));
builder.Services.AddSingleton(mySqlConfiguration);


//PARA SQL SERVER
SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
connectionStringBuilder.DataSource = ".";
connectionStringBuilder.InitialCatalog = "master";
connectionStringBuilder.IntegratedSecurity = true;

var cs = connectionStringBuilder.ConnectionString;
var SqlConfiguration = new Data.SqlConfiguration(cs);

builder.Services.AddSingleton(SqlConfiguration);



//using (SqlConnection connection = new SqlConnection(cs))
//{
//    connection.Open();
//    SqlCommand cmd = connection.CreateCommand();
//    cmd.CommandText = "Select * from dbo.cars";
//    var reader = cmd.ExecuteReader();
//    while (reader.Read())
//    {
      
//        int id = reader.GetInt32(0);
//        string nombre = reader.GetString(1); 

//        Console.WriteLine($"ID: {id}, Nombre: {nombre}");
//    }
//    connection.Close();
//}


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton(new MySqlConnection(builder.Configuration.GetConnectionString("MySqlConn")));



builder.Services.AddScoped<ICarRepository, CarRepository>();
//

var app = builder.Build();

//string targetServerName = "localhost";
//string folderName = "Project1Folder";
//string projectName = "Integration Services Project1";
//string packageName = "Package.dtsx";
////Microsoft.SqlServer.SqlManagementObjects.

//string sqlConnectionString = "Data Source=" + targetServerName +
//              ";Initial Catalog=master;Integrated Security=SSPI;";

//SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);

//IntegrationServices integrationServices = new IntegrationServices(sqlConnection);
//Catalog catalog = integrationServices.Catalogs["SSISDB"];



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/Notes/swagger.json", "Ejemplo de API"));

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ejemplo de API"));

app.MapControllers();
app.Run();
