namespace MedUnify.HealthPulseAPI.Infrastructure.Filters
{
    using MedUnify.Domain.HealthPulse;
    using MedUnify.HealthPulseAPI.Infrastructure.Handlers;
    using MedUnify.HealthPulseAPI.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class OrganizationIdValidationFilter : IAsyncActionFilter
    {
        private readonly IOrganizationHandler _organizationHandler;
        private readonly IPatientService _patientService;

        public OrganizationIdValidationFilter(IOrganizationHandler organizationHandler, IPatientService patientService)
        {
            _organizationHandler = organizationHandler;
            _patientService = patientService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Extract organizationId from the token
            var user = context.HttpContext.User;
            int organizationId = 0;

            try
            {
                organizationId = _organizationHandler.GetOrganizationIdFromToken(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Result = new UnauthorizedObjectResult(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                context.Result = new BadRequestObjectResult(ex.Message);
                return;
            }

            // Check if patientId is present in the parameter object
            var patientIdParameter = context.ActionArguments.Values
                .FirstOrDefault(arg => arg is int patientId) as int?;



            // Attempt to find the PatientId property in the action arguments
            var patientIdProperty = context.ActionArguments.Values
                .Select(v => v.GetType().GetProperty("PatientId"))
                .FirstOrDefault(p => p != null);

            if (patientIdParameter != null)
            {
                if (patientIdParameter.HasValue)
                {
                    var patientId = patientIdParameter.Value;
                    var patient = await _patientService.GetPatientByIdAsync(patientId);

                    // If the patient does not exist or the patient is not in the same organization, return Unauthorized
                    if (patient == null || patient.OrganizationId != organizationId)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }

                // Check if any parameter is of type Patient
                var patientParam = context.ActionArguments.Values
                    .FirstOrDefault(arg => arg is Patient) as Patient;

                if (patientParam != null)
                {
                    // Validate the patient object
                    if (patientParam.OrganizationId != organizationId)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
            }
            else if (patientIdProperty != null)
            {
                var patientArgument = context.ActionArguments.Values.FirstOrDefault();
                if (patientArgument != null)
                {
                    try
                    {
                        var patientId = Convert.ToInt32(patientArgument);
                        var patient = await _patientService.GetPatientByIdAsync(patientId);

                        // If the patient does not exist or the patient is not in the same organization, return Unauthorized
                        if (patient == null || patient.OrganizationId != organizationId)
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception details
                        Console.WriteLine($"Error retrieving PatientId: {ex.Message}");
                        context.Result = new BadRequestResult();
                    }
                }
                else
                {
                    // No argument found
                    context.Result = new BadRequestResult();
                }
            }
            else
            {
                // PatientId property not found
                context.Result = new BadRequestResult();
            }

            //// Check if any parameter is of type Patient
            //foreach (var argument in context.ActionArguments.Values)
            //{
            //    if (argument is Patient patient)
            //    {
            //        // Check if the patient belongs to the same organization
            //        if (patient.OrganizationId != organizationId)
            //        {
            //            context.Result = new UnauthorizedResult();
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        // Check if patientId is present in the parameter object
            //        var patientIdProperty = argument.GetType().GetProperty("PatientId", BindingFlags.Public | BindingFlags.Instance);
            //        if (patientIdProperty != null)
            //        {
            //            var patientIdValue = patientIdProperty.GetValue(argument);
            //            if (patientIdValue is int patientId)
            //            {
            //                var patientFromDb = await _patientService.GetPatientByIdAsync(patientId);

            //                // If the patient does not exist or the patient is not in the same organization, return Unauthorized
            //                if (patientFromDb == null || patientFromDb.OrganizationId != organizationId)
            //                {
            //                    context.Result = new UnauthorizedResult();
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}

            // Continue to the action method if validation is successful
            await next();
        }
    }
}