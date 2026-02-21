using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class CreateListingWizardViewModel
{
    // ================================
    // STEP 1 - Temel Bilgiler
    // ================================
    [Required(ErrorMessage = "Başlık zorunludur")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Fiyat zorunludur")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Kategori seçmelisiniz")]
    public Guid? ListingCategoryId { get; set; }

    [Required(ErrorMessage = "Tip seçmelisiniz")]
    public Guid? ListingTypeId { get; set; }

    // ================================
    // STEP 2 - Konum
    // ================================
    [Required(ErrorMessage = "Şehir zorunludur")]
    public string City { get; set; }

    [Required(ErrorMessage = "İlçe zorunludur")]
    public string District { get; set; }

    // ================================
    // STEP 3 - Özellikler
    // ================================
    [Required(ErrorMessage = "Oda sayısı zorunludur")]
    [Range(1, 20, ErrorMessage = "Geçerli bir oda sayısı girin")]
    public int? Bedrooms { get; set; }

    [Required(ErrorMessage = "Salon sayısı zorunludur")]
    [Range(0, 10, ErrorMessage = "Geçerli bir salon sayısı girin")]
    public int? LivingRooms { get; set; }

    [Required(ErrorMessage = "Banyo sayısı zorunludur")]
    [Range(1, 10, ErrorMessage = "Geçerli bir banyo sayısı girin")]
    public int? Bathrooms { get; set; }

    [Required(ErrorMessage = "Alan bilgisi zorunludur")]
    [Range(1, 10000, ErrorMessage = "Geçerli bir alan girin")]
    public int? Area { get; set; }

    public bool IsPublished { get; set; }

    // ================================
    // STEP 4 - Görseller
    // ================================
    public List<IFormFile> Files { get; set; }
}
