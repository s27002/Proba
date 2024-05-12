using Kolos1.Models;

namespace Kolos1.Repositories;

public interface IPrescriptionRepository
{
    public Task<IEnumerable<PrescriptionsWithNames>> GetPrescriptions(string DoctorName = "none");
    public Task<PrescriptionWithoutKey> AddPrescription(PrescriptionWithoutKey prescriptionWithoutKey);
}