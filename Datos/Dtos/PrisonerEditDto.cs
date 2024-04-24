namespace Datos.Dtos;

public record class PrisonerEditDto : Prisoner
{
    public PrisonerEditDto() => Deleted = false;
}