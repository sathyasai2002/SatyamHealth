﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SatyamHealthCare.Models;

#nullable disable

namespace SatyamHealthCare.Migrations
{
    [DbContext(typeof(SatyamDbContext))]
    [Migration("20240930051956_casestudy3")]
    partial class casestudy3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Prescription", b =>
                {
                    b.Property<int>("PrescriptionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrescriptionID"));

                    b.Property<int?>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<string>("BeforeAfterFood")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Dosage")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("NoOfDays")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("PrescriptionID");

                    b.HasIndex("AppointmentId");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Appointment", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointmentId"));

                    b.Property<DateTime?>("AppointmentDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("AppointmentTime")
                        .IsRequired()
                        .HasColumnType("time");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symptoms")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AppointmentId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Doctor", b =>
                {
                    b.Property<int>("DoctorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DoctorId"));

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Qualification")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SpecializationID")
                        .HasColumnType("int");

                    b.HasKey("DoctorId");

                    b.HasIndex("AdminId");

                    b.HasIndex("SpecializationID");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.MedicalHistoryFile", b =>
                {
                    b.Property<int>("MedicalHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MedicalHistoryId"));

                    b.Property<string>("Allergies")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ChronicConditions")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FamilyHistory")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("HadSurgeries")
                        .HasColumnType("bit");

                    b.Property<bool>("HasAllergies")
                        .HasColumnType("bit");

                    b.Property<bool>("HasChronicConditions")
                        .HasColumnType("bit");

                    b.Property<bool>("HasFamilyHistory")
                        .HasColumnType("bit");

                    b.Property<bool>("HasLifestyleFactors")
                        .HasColumnType("bit");

                    b.Property<string>("LifestyleFactors")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Medications")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("Surgeries")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("TakesMedications")
                        .HasColumnType("bit");

                    b.Property<string>("VaccinationRecords")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("MedicalHistoryId");

                    b.HasIndex("PatientId");

                    b.ToTable("MedicalHistoryFiles");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.MedicalRecord", b =>
                {
                    b.Property<int>("RecordID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecordID"));

                    b.Property<DateTime>("ConsultationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Diagnosis")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("DoctorID")
                        .HasColumnType("int");

                    b.Property<int>("PatientID")
                        .HasColumnType("int");

                    b.Property<int>("PrescriptionID")
                        .HasColumnType("int");

                    b.HasKey("RecordID");

                    b.HasIndex("DoctorID");

                    b.HasIndex("PatientID");

                    b.ToTable("MedicalRecords");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Medicine", b =>
                {
                    b.Property<int>("MedicineID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MedicineID"));

                    b.Property<string>("MedicineName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("MedicineID");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Patient", b =>
                {
                    b.Property<int>("PatientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PatientID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("BloodGroup")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Pincode")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("PatientID");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.PrescriptionMedicine", b =>
                {
                    b.Property<int>("PrescriptionID")
                        .HasColumnType("int");

                    b.Property<int>("MedicineID")
                        .HasColumnType("int");

                    b.HasKey("PrescriptionID", "MedicineID");

                    b.HasIndex("MedicineID");

                    b.ToTable("PrescriptionMedicine");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.PrescriptionTest", b =>
                {
                    b.Property<int>("PrescriptionID")
                        .HasColumnType("int");

                    b.Property<int>("TestID")
                        .HasColumnType("int");

                    b.HasKey("PrescriptionID", "TestID");

                    b.HasIndex("TestID");

                    b.ToTable("PrescriptionTest");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Specialization", b =>
                {
                    b.Property<int>("SpecializationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SpecializationID"));

                    b.Property<string>("SpecializationName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("SpecializationID");

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Test", b =>
                {
                    b.Property<int>("TestID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TestID"));

                    b.Property<string>("TestName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("TestID");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Prescription", b =>
                {
                    b.HasOne("SatyamHealthCare.Models.Appointment", "Appointment")
                        .WithMany("Prescriptions")
                        .HasForeignKey("AppointmentId");

                    b.Navigation("Appointment");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Appointment", b =>
                {
                    b.HasOne("SatyamHealthCare.Models.Doctor", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SatyamHealthCare.Models.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Doctor", b =>
                {
                    b.HasOne("SatyamHealthCare.Models.Admin", "Admin")
                        .WithMany("Doctors")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SatyamHealthCare.Models.Specialization", "Specialization")
                        .WithMany("Doctors")
                        .HasForeignKey("SpecializationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.MedicalHistoryFile", b =>
                {
                    b.HasOne("SatyamHealthCare.Models.Patient", "Patient")
                        .WithMany("MedicalHistoryFiles")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.MedicalRecord", b =>
                {
                    b.HasOne("SatyamHealthCare.Models.Doctor", "Doctor")
                        .WithMany("MedicalRecords")
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SatyamHealthCare.Models.Patient", "Patient")
                        .WithMany("MedicalRecords")
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.PrescriptionMedicine", b =>
                {
                    b.HasOne("SatyamHealthCare.Models.Medicine", "Medicine")
                        .WithMany("PrescriptionMedicines")
                        .HasForeignKey("MedicineID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Prescription", "Prescription")
                        .WithMany("PrescriptionMedicines")
                        .HasForeignKey("PrescriptionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicine");

                    b.Navigation("Prescription");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.PrescriptionTest", b =>
                {
                    b.HasOne("Prescription", "Prescription")
                        .WithMany("PrescriptionTests")
                        .HasForeignKey("PrescriptionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SatyamHealthCare.Models.Test", "Test")
                        .WithMany("PrescriptionTests")
                        .HasForeignKey("TestID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Prescription");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("Prescription", b =>
                {
                    b.Navigation("PrescriptionMedicines");

                    b.Navigation("PrescriptionTests");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Admin", b =>
                {
                    b.Navigation("Doctors");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Appointment", b =>
                {
                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Doctor", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("MedicalRecords");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Medicine", b =>
                {
                    b.Navigation("PrescriptionMedicines");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Patient", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("MedicalHistoryFiles");

                    b.Navigation("MedicalRecords");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Specialization", b =>
                {
                    b.Navigation("Doctors");
                });

            modelBuilder.Entity("SatyamHealthCare.Models.Test", b =>
                {
                    b.Navigation("PrescriptionTests");
                });
#pragma warning restore 612, 618
        }
    }
}
