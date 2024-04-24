namespace Datos.Utilities;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<PrisonerAddDto, Prisoner>().ReverseMap();
        CreateMap<PrisonerEditDto, Prisoner>().ReverseMap();
    }
}