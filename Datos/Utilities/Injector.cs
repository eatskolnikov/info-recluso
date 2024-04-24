using Microsoft.Extensions.DependencyInjection;

namespace Datos.Utilities;

public static class Injector
{
    public static IServiceProvider _proveedor;
    public static void GenerarProveedor(IServiceCollection servColector) => _proveedor = servColector.BuildServiceProvider();
    public static T GetService<T>() => _proveedor.GetService<T>();
    public static T GetService<T>(bool Mandatory) => _proveedor.GetRequiredService<T>();
}