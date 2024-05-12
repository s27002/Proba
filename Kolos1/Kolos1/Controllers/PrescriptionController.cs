
using Kolos1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kolos1;

[ApiController]
[Route("api/[Controller]")]

public class PrescriptionController : ControllerBase
{
    private IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }
    
    [HttpGet("GetPrescriptions")]
    public async Task<IActionResult> GetPrescriptions(string DoctorName = "none")
    {
        var list = await _prescriptionService.GetPrescriptions(DoctorName);
        if (list.Count() == 0)
            return BadRequest("There is no receptions for that doctor");
        else
            return Ok(list);
    }

    [HttpPost("AddPrescription")]
    public async Task<IActionResult> AddPrescription(PrescriptionWithoutKey prescriptionWithoutKey)
    {
        var res = await _prescriptionService.AddPrescription(prescriptionWithoutKey);
        return Ok(res);
    }
}