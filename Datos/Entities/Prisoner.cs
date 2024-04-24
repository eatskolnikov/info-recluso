using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datos.Entities;

public record class Prisoner
{
    public int PriKey { get; set; }

    [Required(ErrorMessage = "Obligatorio")]
    public string Id { get; set; }
    
    [Required (ErrorMessage ="Obligatorio")]
    public string Names { get; set; }

    [Required(ErrorMessage = "Obligatorio")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Obligatorio")]

    public string Gender { get; set; }

    [Required(ErrorMessage = "Obligatorio")]
    public string Crimes { get; set; }

    [Required(ErrorMessage = "Obligatorio")]
    public string ConvictionStatus { get; set; }

    [Required(ErrorMessage = "Obligatorio")]
    public string Prison { get; set; }
    public string Notes { get; set; }

    [Required(ErrorMessage = "Obligatorio")]
    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = "Obligatorio")]
    public DateTime? AdmissionDate { get; set; }= DateTime.Today;

    [NotUpdate]
    public DateTime? CreateDate { get; }

    [NotMapped]
    public virtual bool? Deleted { get; set; }

}