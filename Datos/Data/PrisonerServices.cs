using Datos.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Datos.Data;

public class PrisonerServices : AbstracServices, IPrisonerServices
{
    public PrisonerServices(Repository repository, IMapper mapper, IConfiguration config) : base(repository, mapper, config)
    {
    }

    public async Task<Response> AddPrisonerAsync(PrisonerAddDto addDto)
    {
        try
        {
            await _repository.AddAsync(addDto);
            var entity = _mapper.Map<Prisoner>(addDto with { });
            return await _repository.AddAsync(entity);
        }
        catch (Exception ex)
        {
            return Response.Error(message: ex.Message);
        }
    }

    public async Task<Response> EditPrisonerAsync(PrisonerEditDto editDto)
    {
        try
        {
            var entity = _mapper.Map<Prisoner>(editDto);
            return await _repository.UpdateAsync(entity);
        }
        catch (Exception ex)
        {
            return Response.Error(message: ex.Message);
        }
    }

    public async Task<Response> DelPrisonerByIdAsync(string priKey)
    {
        try
        {
            var (prisoner, response) = await _repository.FindAsync<Prisoner>(x => x.PriKey.ToString().Equals(priKey));

            if (response == Response.Ok())
            {
                prisoner.Deleted = true;
                var result = await _repository.SaveAsync();

                return result >= 1 ? Response.Ok("Completadao") : Response.Fail("No se ha podido Eliminar");
            }

            return response;
        }
        catch (Exception ex)
        {
            return Response.Error(message: ex.Message);
        }
    }

    public async Task<(IEnumerable<Prisoner> prisoners, Response response)> GetPrisoners()
    {
        try
        {
            var result = await _repository.GetAllAsync<Prisoner>(x => x.Deleted == false);
            return result;
        }
        catch (Exception ex)
        {
            return (default, Response.Error(ex.Message));
        }
    }

    public async Task<Response> DelPrisonerByIdAsync(Prisoner prisoner)
    {
        return await DelPrisonerByIdAsync((prisoner.PriKey.ToString()));
    }
}