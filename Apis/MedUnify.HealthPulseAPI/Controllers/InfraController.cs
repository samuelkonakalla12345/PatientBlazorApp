namespace MedUnify.HealthPulseAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class InfraController : ControllerBase
    {
        private readonly ILogger<InfraController> _logger;

        public InfraController(ILogger<InfraController> logger)
        {
            _logger = logger;
        }

        [Route("IsUserAuthorized")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [Authorize]
        public IActionResult IsUserAuthorized()
        {
            return this.Ok("User is authorized");
        }

        [Route("GetErrorResponse")]
        [HttpGet]
        public IActionResult GetErrorResponse()
        {
            throw new Exception();
        }
    }
}