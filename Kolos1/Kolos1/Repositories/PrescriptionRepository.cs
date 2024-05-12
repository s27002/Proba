using System.Data.SqlClient;
using System.Runtime.InteropServices.JavaScript;
using Kolos1.Models;

namespace Kolos1.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly IConfiguration _configuration;

    public PrescriptionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<IEnumerable<PrescriptionsWithNames>> GetPrescriptions(string DoctorName = "none")
    {
        var prescriptions = new List<PrescriptionsWithNames>();
        SqlCommand command;
        
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                await connection.OpenAsync();
                if (DoctorName.Equals("none")) 
                    command = new SqlCommand(
                        "SELECT pr.IdPrescription, pr.Date, pr.DueDate, pa.LastName PN, pd.LastName DN\nFROM Prescription pr, Doctor pd, Patient pa \nWHERE pr.IdPatient = pa.IdPatient and pr.IdDoctor = pd.IdDoctor\nORDER BY pr.Date DESC",
                        connection);
                else
                {
                    command = new SqlCommand(
                        "SELECT pr.IdPrescription, pr.Date, pr.DueDate, pa.LastName PN, pd.LastName DN\nFROM Prescription pr, Doctor pd, Patient pa \nWHERE pr.IdPatient = pa.IdPatient and pr.IdDoctor = pd.IdDoctor and pd.LastName = @DoctorName\nORDER BY pr.Date DESC",
                        connection);
                    command.Parameters.AddWithValue("@DoctorName", DoctorName);
                }
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var prescription = new PrescriptionsWithNames
                        {
                            IdPrescription = (int)reader["IdPrescription"],
                            Date = reader["Date"].ToString(),
                            DueDate = reader["DueDate"].ToString(),
                            PatientLastName = reader["PN"].ToString(),
                            DoctorLastName = reader["DN"].ToString()
                        };
                        prescriptions.Add(prescription);
                    }
                }
            }
        

        return prescriptions;
    }

    public async Task<PrescriptionWithoutKey> AddPrescription(PrescriptionWithoutKey prescriptionWithoutKey)
    {
        PrescriptionWithKey prescriptionWithKey = new PrescriptionWithKey();
        using (var connect = new SqlConnection(_configuration.GetConnectionString("Default"))){
        await connect.OpenAsync();
        var command =
            new SqlCommand(
                "INSERT INTO Prescription VALUES (@Date, @DueDate, @IdPatient, @IdDoctor) SELECT SCOPE_IDENTITY()", connect);
        command.Parameters.AddWithValue("@Date", prescriptionWithoutKey.Date);
        command.Parameters.AddWithValue("@DueDate", prescriptionWithoutKey.DueDate);
        command.Parameters.AddWithValue("@IdPatient", prescriptionWithoutKey.IdPatient);
        command.Parameters.AddWithValue("@IdDoctor", prescriptionWithoutKey.IdDoctor);
        var resId = Convert.ToInt32(await command.ExecuteScalarAsync());

        var secondCommand =
            new SqlCommand("SELECT Date, DueDate, IdDoctor, IdPatient FROM Prescription WHERE IDPrescription = @IdPrescription", connect);
        secondCommand.Parameters.AddWithValue("@IdPrescription", resId);
        var reader = await secondCommand.ExecuteReaderAsync();
        
            while (await reader.ReadAsync())
            {
                var perscription = new PrescriptionWithoutKey
                {
                    Date = reader["Date"].ToString(),
                    DueDate = reader["DueDate"].ToString(),
                    IdDoctor = (int)reader["IdDoctor"],
                    IdPatient = (int)reader["IdPatient"]
                };
                return perscription;
            }
        
        
        }
        return null;
    }
}