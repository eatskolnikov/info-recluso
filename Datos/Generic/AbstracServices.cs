using Microsoft.Extensions.Configuration;
namespace Datos.Generic;

/// <summary>
/// La funcion de esta es para que la hereden y usen su constructor ya que en el  hay <br/>
/// inyecciones de uso muy necesario para otras funciones como obtener datos del json.config
/// </summary>
public abstract class AbstracServices
{
    /// <value> Instancia del repositorio usado para operaciones en base de datos///</value>
    protected readonly Repository _repository;
    /// <value> Instancia del Imapper usado para mapear una clase a otra ///</value>
    protected readonly IMapper _mapper;
    /// <value> Instancia del Iconfiguration usado para obtener datos de configuracion ///</value>
    protected readonly IConfiguration _config;
    /// <summary>
    /// Contructor de la clase <c>Repository</c>
    /// </summary>
    /// <param name="repository"> parametro de tipo <c> Repositorio</c> (servicio) </param>
    /// <param name="mapper">parametro de tipo <c>Imapper  </c>  </param>
    /// <param name="config">parametro de tipo iconfiguration   </param>
    public AbstracServices(Repository repository, IMapper mapper, IConfiguration config)
        => (_repository, _mapper, _config) = (repository, mapper, config);
}