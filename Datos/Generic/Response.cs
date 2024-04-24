namespace Datos.Generic;

public class Response
{
    public CodeType Code { get; set; }
    public string Message { get; private set; }

    public static Response Ok(string message = "Exitoso") =>
        new Response() { Code = CodeType.OK, Message = message, };
    public static Response Fail(string message = "Ha ocurrido un error") =>
        new Response() { Code = CodeType.Fail, Message = message };

    public static Response Error(string message = "Ha ocurrido un error entro al catch") =>
        new Response() { Code = CodeType.Error, Message = message };
    public static Response NoFound(string message = "No encontrado") =>
        new Response() { Code = CodeType.NoFound, Message = message };

    public enum CodeType
    {
        OK,
        Fail,
        Error,
        NoFound
    }

    public static bool operator ==(Response left, Response right) => left?.Code.ToString().ToLower() == right?.Code.ToString().ToLower();
    public static bool operator !=(Response left, Response right) => left.Code.ToString().ToLower() != right.Code.ToString().ToLower();
    public override bool Equals(object o) => (o as Response)?.Code.ToString().ToLower() == Code.ToString().ToLower();
    public override int GetHashCode() => Code.GetHashCode();
    public override string ToString() => Code.ToString();
}