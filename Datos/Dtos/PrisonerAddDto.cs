namespace Datos.Dtos;

public record class PrisonerAddDto:Prisoner
{
    public PrisonerAddDto() => BirthDate = DateTime.Today.AddYears(-18);
}