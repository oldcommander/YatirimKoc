using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Features.Listings.Commands;

public class UpdateListingCommandHandler : IRequestHandler<UpdateListingCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileUploadService _fileUploadService;

    public UpdateListingCommandHandler(IApplicationDbContext context, IFileUploadService fileUploadService)
    {
        _context = context;
        _fileUploadService = fileUploadService;
    }

    public async Task<bool> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
    {
        var listing = await _context.Listings
            .Include(x => x.Images)
            .Include(x => x.FeatureValues)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (listing == null) return false;

        // 1. Temel Bilgileri Güncelle
        listing.Title = request.Title;
        listing.Slug = request.Title.ToLower().Replace(" ", "-").Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");
        listing.Description = request.Description;
        listing.Price = request.Price;
        listing.City = request.City;
        listing.District = request.District;
        listing.TransactionTypeId = request.TransactionTypeId;
        listing.PropertyTypeId = request.PropertyTypeId;
        listing.IsPublished = request.IsPublished;
        listing.Latitude = request.Latitude;
        listing.Longitude = request.Longitude;

        // 2. Silinen Resimleri İşle (Fiziksel klasörden ve DB'den uçur)
        if (request.DeletedImageIds != null && request.DeletedImageIds.Any())
        {
            var imagesToDelete = listing.Images.Where(i => request.DeletedImageIds.Contains(i.Id)).ToList();
            foreach (var img in imagesToDelete)
            {
                await _fileUploadService.DeleteAsync(img.ImageUrl); // Sunucudan fiziksel olarak sil

                // GARANTİ YÖNTEM: listing.Images.Remove(img) YERİNE DOĞRUDAN CONTEXT'TEN SİLİYORUZ
                _context.ListingImages.Remove(img);
            }
        }

        // 3. Yeni Yüklenen Resimleri Ekle
        if (request.NewImageUrls != null && request.NewImageUrls.Any())
        {
            foreach (var url in request.NewImageUrls)
            {
                listing.Images.Add(new ListingImage
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = url,
                    IsCover = listing.Images.Count == 0 // Eğer hiç resim kalmadıysa ilk yüklenen kapak olur
                });
            }
        }

        // 4. Dinamik Özellikleri (Features) Güncelle
        var incomingFeatures = request.FeatureValues ?? new Dictionary<Guid, string>();

        // 4.1. Formdan Gelen Listede OLMAYAN (Silinmiş veya Boşaltılmış) Özellikleri Bul
        var featuresToRemove = listing.FeatureValues
            .Where(f => !incomingFeatures.ContainsKey(f.FeatureId) || string.IsNullOrWhiteSpace(incomingFeatures[f.FeatureId]))
            .ToList();

        foreach (var feature in featuresToRemove)
        {
            // ÇÖZÜM: list.Remove() metodunu kullanmıyoruz, sadece DbSet'ten uçuruyoruz.
            // Böylece EF Core sadece tek bir DELETE sorgusu üretecek ve patlamayacak.
            _context.ListingFeatureValues.Remove(feature);
        }

        // 4.2. Gelen Özellikleri Güncelle veya Yeni İse Ekle
        foreach (var kvp in incomingFeatures)
        {
            if (string.IsNullOrWhiteSpace(kvp.Value)) continue;

            // Bu özellik zaten ilanımızda kayıtlı mı?
            var existingFeature = listing.FeatureValues.FirstOrDefault(f => f.FeatureId == kvp.Key);

            if (existingFeature != null)
            {
                // Kayıtlıysa, sadece değerini (Value) güncelle (EF Core bu kısmı sadece UPDATE yapar)
                existingFeature.Value = kvp.Value;
            }
            else
            {
                // İlanda daha önce böyle bir özellik yoksa, yeni satır olarak ekle
                _context.ListingFeatureValues.Add(new ListingFeatureValue
                {
                    ListingId = listing.Id,
                    FeatureId = kvp.Key,
                    Value = kvp.Value
                });
            }
        }

        // 5. Kaydet ve Bitir
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}