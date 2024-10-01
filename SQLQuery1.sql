select * from Patients;
select * from Admins;
select * from Doctors;
select * from Appointments;
select * from MedicalRecords;

INSERT INTO MedicalRecords (PatientID, DoctorID, ConsultationDateTime, Diagnosis, PrescriptionID)
VALUES (1, 3, '2024-09-30 14:30:00', 'Common Cold', 101);
/*INSERT INTO Admins (FullName, Email, Password)
VALUES 
('John Admin', 'john.admin@satyamhealthcare.com', 'AdminPass123'),
('Jane Admin', 'jane.admin@satyamhealthcare.com', 'AdminPass456');


INSERT INTO Specializations (SpecializationName)
VALUES 
('Cardiology'),
('Dermatology'),
('Neurology'),
('Pediatrics'),
('Orthopedics');

INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, AppointmentTime, Status, Symptoms)
VALUES
    (1, 3, '2024-10-01', '09:00:00', 0, 'Headache'),
    (2, 4, '2024-10-02', '10:30:00', 0, 'Fever');


INSERT INTO Doctors (FullName, PhoneNo, Email, Password, Designation, Experience, SpecializationID, Qualification, AdminId)
VALUES 
('Dr. Paul Walker', '9876543210', 'paul.doctor@satyamhealthcare.com', 'DoctorPass123', 'Cardiologist', 10, 1, 'MBBS, MD', 1),
('Dr. Susan Doctor', '8765432109', 'susan.doctor@satyamhealthcare.com', 'DoctorPass456', 'Dermatologist', 8, 2, 'MBBS, MD', 2);


INSERT INTO Patients (FullName, DateOfBirth, Gender, BloodGroup, ContactNumber, Email, Address, Pincode, City, State, Password)
VALUES 
('Michael Doe', '1985-05-15', 'Male', 'O+', '+91 8825924958', 'sathyasai91202@gmail.com', '123 Main St', '110001', 'New Delhi', 'Delhi', 'PatientPass123'),
('Emily Smith', '1990-08-25', 'Female', 'A+', '8765432108', 'emily.smith@example.com', '456 Oak St', '560001', 'Bangalore', 'Karnataka', 'PatientPass456');*/
