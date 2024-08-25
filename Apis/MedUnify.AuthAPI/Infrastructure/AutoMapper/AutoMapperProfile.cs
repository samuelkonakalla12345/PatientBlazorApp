namespace MedUnify.AuthAPI.Infrastructure.AutoMapper
{
    using global::AutoMapper;
    using MedUnify.Domain.HealthPulse;
    using MedUnify.ResourceModel.HealthPulse;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            AllowNullDestinationValues = true;

            #region HealthPulse

            CreateMap<PatientRM, Patient>().ReverseMap();

            #endregion HealthPulse
        }
    }
}