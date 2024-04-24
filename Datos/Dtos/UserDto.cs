using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Datos.Dtos;
public class UserDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Obligatorio")]
    public string Email { get; set; }

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).+$", ErrorMessage = "Debe tener letra , numeros y simbolos")]
    [Required(ErrorMessage ="Numeros /Mayus / Simbolos")]
    public string Pass { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "La cadena debe contener solo letras y números")]
    [Required(ErrorMessage = "Obligatorio")]
    public string UserName { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    public static implicit operator IdentityUser(UserDto userDto)
    {
        return new IdentityUser()
        {
            Email = userDto.Email,
            UserName = userDto.UserName,
            EmailConfirmed = true,
            PhoneNumber = userDto.PhoneNumber,      
        };
    }
}