using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Web.Models;

public class BookFormModel
{
    [Required(ErrorMessage = "Název je povinný")]
    [StringLength(200, ErrorMessage = "Název může mít maximálně 200 znaků")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Autor je povinný")]
    [StringLength(200, ErrorMessage = "Autor může mít maximálně 200 znaků")]
    public string? Author { get; set; }

    [Range(1450, 2100, ErrorMessage = "Rok vydání musí být mezi 1450 a 2100")]
    public int Year { get; set; } = DateTime.Now.Year;

    [Required(ErrorMessage = "ISBN je povinné")]
    [StringLength(32, ErrorMessage = "ISBN může mít maximálně 32 znaků")]
    public string? Isbn { get; set; }

    [Range(0, 100000, ErrorMessage = "Počet kusů musí být mezi 0 a 100 000")]
    public int AvailableCount { get; set; }
}