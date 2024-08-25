namespace MedUnify.HealthPulseAPI.Controllers
{
    using MedUnify.Domain.HealthPulse;
    using MedUnify.HealthPulseAPI.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class ProgressNotesController : ControllerBase
    {
        private readonly IProgressNoteService _progressNoteService;

        public ProgressNotesController(IProgressNoteService progressNoteService)
        {
            _progressNoteService = progressNoteService;
        }

        [HttpGet("visit/{visitId}")]
        public async Task<ActionResult<IEnumerable<ProgressNote>>> GetProgressNotesByVisitId(int visitId)
        {
            var progressNotes = await _progressNoteService.GetProgressNotesByVisitIdAsync(visitId);
            return Ok(progressNotes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProgressNote>> GetProgressNote(int id)
        {
            var progressNote = await _progressNoteService.GetProgressNoteByIdAsync(id);
            if (progressNote == null)
            {
                return NotFound();
            }
            return Ok(progressNote);
        }

        [HttpPost]
        public async Task<ActionResult> AddProgressNote(ProgressNote progressNote)
        {
            await _progressNoteService.AddProgressNoteAsync(progressNote);
            return CreatedAtAction(nameof(GetProgressNote), new { id = progressNote.ProgressNoteId }, progressNote);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProgressNote(int id, ProgressNote progressNote)
        {
            if (id != progressNote.ProgressNoteId)
            {
                return BadRequest();
            }

            await _progressNoteService.UpdateProgressNoteAsync(progressNote);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProgressNote(int id)
        {
            await _progressNoteService.DeleteProgressNoteAsync(id);
            return NoContent();
        }
    }
}