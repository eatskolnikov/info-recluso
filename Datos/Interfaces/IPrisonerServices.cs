namespace Datos.Interfaces;

public interface IPrisonerServices
{
    public Task<Response> AddPrisonerAsync(PrisonerAddDto addDto);
    public Task<Response> EditPrisonerAsync(PrisonerEditDto editDto);
    public Task<Response> DelPrisonerByIdAsync(string priKey);
    public Task<Response> DelPrisonerByIdAsync(Prisoner prisoner);
    public Task<(IEnumerable<Prisoner> prisoners, Response response)> GetPrisoners();
}