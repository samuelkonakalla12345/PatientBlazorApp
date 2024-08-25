namespace MedUnify.HealthPulseAPI.Controllers
{
    using MedUnify.Domain.HealthPulse;
    using MedUnify.HealthPulseAPI.Services.Interface;
    using MedUnify.ResourceModel.HealthPulse;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpGet("patient/{patientId}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PatientRM>))]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<List<Visit>>> GetVisitsByPatientId(int patientId)
        {
            var visits = await _visitService.GetVisitsByPatientIdAsync(patientId);
            return Ok(visits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> GetVisit(int id)
        {
            var visit = await _visitService.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            return Ok(visit);
        }
        [Route("AddVisit")]
        [HttpPost]
        public async Task<ActionResult> AddVisit(Visit visit)
        {
            await _visitService.AddVisitAsync(visit);
            return CreatedAtAction(nameof(GetVisit), new { id = visit.VisitId }, visit);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVisit(int id, Visit visit)
        {
            if (id != visit.VisitId)
            {
                return BadRequest();
            }

            await _visitService.UpdateVisitAsync(visit);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVisit(int id)
        {
            await _visitService.DeleteVisitAsync(id);
            return NoContent();
        }
    }
}