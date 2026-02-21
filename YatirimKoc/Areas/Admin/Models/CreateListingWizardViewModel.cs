using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace YatirimKoc.Web.Areas.Admin.Models;

public class CreateListingWizardViewModel
{
    [Required(ErrorMessage = "İlan başlığı zorunludur.")]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Fiyat zorunludur.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Şehir seçimi zorunludur.")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "İlçe seçimi zorunludur.")]
    public string District { get; set; } = null!;

    [Required(ErrorMessage = "İşlem Tipi (Satılık/Kiralık) seçilmelidir.")]
    public Guid? TransactionTypeId { get; set; }

    [Required(ErrorMessage = "Emlak Tipi (Konut/Arsa) seçilmelidir.")]
    public Guid? PropertyTypeId { get; set; }

    public bool IsPublished { get; set; }

    public List<IFormFile>? Files { get; set; }
}