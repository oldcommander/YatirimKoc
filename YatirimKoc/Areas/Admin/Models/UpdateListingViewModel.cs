using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Web.Areas.Admin.Models;

public class UpdateListingViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "İlan başlığı zorunludur.")]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Fiyat zorunludur.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Şehir seçimi zorunludur.")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "İlçe seçimi zorunludur.")]
    public string District { get; set; } = null!;

    [Required(ErrorMessage = "İşlem Tipi seçilmelidir.")]
    public Guid TransactionTypeId { get; set; }

    [Required(ErrorMessage = "Emlak Tipi seçilmelidir.")]
    public Guid PropertyTypeId { get; set; }

    public bool IsPublished { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // MEVCUT RESİMLER (Sayfada göstermek için)
    public List<ListingImage>? ExistingImages { get; set; }

    // SİLİNECEK RESİMLERİN ID'LERİ (Kullanıcı çarpıya basınca buraya atılacak)
    public List<Guid> DeletedImageIds { get; set; } = new();

    // YENİ YÜKLENECEK DOSYALAR
    public List<IFormFile>? NewFiles { get; set; }

    // MEVCUT DİNAMİK ÖZELLİKLER (Sözlük Formatında)
    public Dictionary<Guid, string>? FeatureValues { get; set; } = new();
}