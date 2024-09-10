using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Repos;

namespace SatyamHealthCare
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IPatient, PatientService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<SatyamDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HospContr")));
            builder.Services.AddScoped<IAdmin, AdminService>();
            builder.Services.AddScoped<IDoctor, DoctorService>();
            builder.Services.AddScoped<IAppointment, AppointmentService>();
            builder.Services.AddScoped<IMedicalRecord, MedicalRecordService>();
            builder.Services.AddScoped<IPrescription, PrescriptionService>();
            builder.Services.AddScoped<ITest, TestService>();
            builder.Services.AddScoped<IPrescribedTest, PrescribedTestService>();


            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

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
        }
    }
}
