
/// <summary>
/// Atributo que sera usado para evitar actualizacion en base de datos
/// la propiedad debe existir en la entidad o se provoca una excepcion
/// </summary>
[AttributeUsage(AttributeTargets.Property,AllowMultiple =false,Inherited =true)]
public class NotUpdateAttribute: Attribute
{
}