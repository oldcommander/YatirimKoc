using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Features.Listings.Commands;

public class UpdateListingCommandHandler : IRequestHandler<UpdateListingCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateListingCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
    {
        // 1. İlanı, Resimleri ve Özellik Değerlerini veritabanından çekiyoruz. (EF Core nesneyi izlemeye başlar)
        var listing = await _context.Listings
            .Include(l => l.Images)
            .Include(l => l.FeatureValues)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (listing == null)
            throw new Exception("Güncellenmek istenen ilan bulunamadı.");

        // 2. Ana özellikleri (Title, Price vb.) güncelliyoruz
        listing.Title = request.Title;
        listing.Description = request.Description;
        listing.Price = request.Price;
        listing.City = request.City;
        listing.District = request.District;
        listing.TransactionTypeId = request.TransactionTypeId;
        listing.PropertyTypeId = request.PropertyTypeId;
        listing.IsPublished = request.IsPublished;
        listing.Latitude = request.Latitude;
        listing.Longitude = request.Longitude;

        // NOT: Eğer slug alanını title'dan türetiyorsanız burada atamasını yapabilirsiniz. 
        // Örn: listing.Slug = GenerateSlug(request.Title);

        // 3. YENİ RESİMLERİ EKLEME İŞLEMİ
        if (request.NewImageUrls != null && request.NewImageUrls.Any())
        {
            foreach (var imageUrl in request.NewImageUrls)
            {
                listing.Images.Add(new ListingImage
                {
                    ListingId = listing.Id,
                    ImageUrl = imageUrl,
                    IsCover = listing.Images.Count == 0, // Eğer hiç resim yoksa ilkini kapak yapabiliriz
                    Order = listing.Images.Count + 1
                });
            }
        }

        // 4. SİLİNECEK RESİMLERİ ÇIKARMA İŞLEMİ
        if (request.DeletedImageIds != null && request.DeletedImageIds.Any())
        {
            var imagesToRemove = listing.Images
                .Where(img => request.DeletedImageIds.Contains(img.Id))
                .ToList();

            foreach (var img in imagesToRemove)
            {
                listing.Images.Remove(img);
                _context.ListingImages.Remove(img); // DB'den de silinmesi için EF'e bildiriyoruz
            }
        }

        // 5. FEATURE (ÖZELLİK) DEĞERLERİNİ GÜNCELLEME İŞLEMİ
        if (request.FeatureValues != null)
        {
            foreach (var featureValue in request.FeatureValues)
            {
                // İlgili özellik (FeatureId) ilanda zaten var mı diye bakıyoruz
                var existingFeature = listing.FeatureValues.FirstOrDefault(f => f.FeatureId == featureValue.Key);

                if (existingFeature != null)
                {
                    // Varsa sadece değerini güncelliyoruz
                    existingFeature.Value = featureValue.Value;
                }
                else
                {
                    // Yoksa listeye yeni bir özellik değeri olarak ekliyoruz
                    listing.FeatureValues.Add(new ListingFeatureValue
                    {
                        ListingId = listing.Id,
                        FeatureId = featureValue.Key,
                        Value = featureValue.Value
                    });
                }
            }
        }

        // DİKKAT: BURADA _context.Listings.Update(listing); KULLANMIYORUZ!
        // Entity'i Include ile çektiğimiz için Entity Framework yeni eklenenlerin INSERT,
        // silinenlerin DELETE ve değişenlerin UPDATE olması gerektiğini zaten biliyor.

        // 6. İşlemleri Veritabanına Kaydet
        var result = await _context.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}