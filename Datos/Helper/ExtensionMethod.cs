using Microsoft.EntityFrameworkCore.ChangeTracking;

public static class ExtensionMethod
{
    /// <summary>
    /// prohibe la modificacion del campo de la entidad  
    /// que tenga el atributo [NotUpdate]
    /// </summary>
    /// <param name="entityEntry"></param>
    public static void PropertyCheckNotUpdate(this EntityEntry entityEntry, object myobject)
    {
        var propiedades = myobject.GetType().GetProperties()
            .Where(x => x.GetCustomAttributes(false).
             Any(y => y.Equals(new NotUpdateAttribute())));

        foreach (var item in propiedades)
            entityEntry.Property(item.Name).IsModified = false;
    }
}