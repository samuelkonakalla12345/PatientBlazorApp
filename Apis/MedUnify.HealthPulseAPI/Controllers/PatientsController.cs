namespace MedUnify.HealthPulseAPI.Controllers
{
    using AutoMapper;
    using MedUnify.Domain.HealthPulse;
    using MedUnify.HealthPulseAPI.Infrastructure.Filters;
    using MedUnify.HealthPulseAPI.Infrastructure.Handlers;
    using MedUnify.HealthPulseAPI.Services.Interface;
    using MedUnify.ResourceModel;
    using MedUnify.ResourceModel.HealthPulse;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IOrganizationHandler _organizationHandler;
        private readonly IPatientService _patientService;

        public PatientsController(IMapper mapper, IPatientService patientService, IOrganizationHandler organizationHandler)
        {
            this._mapper = mapper;
            _patientService = patientService;
            _organizationHandler = organizationHandler;
        }

        [Route("GetPatients")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PatientRM>))]
        public async Task<ActionResult<List<PatientRM>>> GetPatients()
        {
            int organizationId = _organizationHandler.GetOrganizationIdFromToken(User);

            List<PatientRM> patientsRM = new List<PatientRM>();

            var patients = await _patientService.GetAllPatientsAsync(organizationId);

            patientsRM = this._mapper.Map<List<PatientRM>>(patients);

            return Ok(patientsRM);
        }

        [Route("GetPatient")]
        [HttpGet]
        [ServiceFilter(typeof(OrganizationIdValidationFilter))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Patient))]
        public async Task<ActionResult<PatientRM>> GetPatient(int patientId)
        {
            PatientRM patientRM = new PatientRM();
            var patient = await _patientService.GetPatientByIdAsync(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            patientRM = this._mapper.Map<PatientRM>(patient);

            return Ok(patientRM);
        }

        [Route("UpdatePatient")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ReturnStatusResourseModel))]
        [ServiceFilter(typeof(OrganizationIdValidationFilter))]
        public async Task<ActionResult> UpdatePatient(int patientId, [FromBody] PatientRM patientRM)
        {
            ReturnStatusResourseModel returnStatusRM = new ReturnStatusResourseModel();

            if (patientId != patientRM.PatientId)
            {
                return BadRequest();
            }

            Patient patient = new Patient();

            patient = this._mapper.Map<Patient>(patientRM);

            await _patientService.UpdatePatientAsync(patient);

            returnStatusRM.Status = "success";
            returnStatusRM.Title = "Updated Successfully";
            returnStatusRM.Message = $"{patient.FirstName} details updated successfully.";
            return this.Ok(returnStatusRM);
        }

        [Route("AddPatient")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Patient))]
        public async Task<ActionResult> AddPatient([FromBody] Patient patient)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Use the handler to get the OrganizationId
            int organizationId = _organizationHandler.GetOrganizationIdFromToken(User);
            patient.OrganizationId = organizationId;

            await _patientService.AddPatientAsync(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientId }, patient);
        }

        [Route("DeletePatient")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReturnStatusResourseModel))]
        public async Task<ActionResult> DeletePatient(int patientId)
        {
            ReturnStatusResourseModel returnStatusRM = new ReturnStatusResourseModel();
            await _patientService.DeletePatientAsync(patientId);

            returnStatusRM.Status = "success";
            returnStatusRM.Title = "Deleted Successfully";
            returnStatusRM.Message = $"Patient details updated successfully.";
            return this.Ok(returnStatusRM);
        }
    }
}