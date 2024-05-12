using Kolos1.Models;

namespace Kolos1;

public interface IPrescriptionService
{
    public Task<IEnumerable<PrescriptionsWithNames>> GetPrescriptions(string DoctorName = "none");
    public Task<PrescriptionWithoutKey> AddPrescription(PrescriptionWithoutKey prescriptionWithoutKey);
}