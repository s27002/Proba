
using Kolos1.Models;
using Kolos1.Repositories;

namespace Kolos1;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _repository;

    public PrescriptionService(IPrescriptionRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<PrescriptionsWithNames>> GetPrescriptions(string DoctorName = "none")
    {
        return await _repository.GetPrescriptions(DoctorName);
    }

    public async Task<PrescriptionWithoutKey> AddPrescription(PrescriptionWithoutKey prescriptionWithoutKey)
    {
        return await _repository.AddPrescription(prescriptionWithoutKey);
    }
}