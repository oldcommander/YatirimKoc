using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Interfaces;

namespace YatirimKoc.Application.Features.Listings.Commands;

public class DeleteListingCommandHandler : IRequestHandler<DeleteListingCommand, bool>
{
    private readonly IApplicationDbContext _context;
    // DOSYA SERVİSİNİ BURAYA ÇAĞIRIYORUZ
    private readonly IFileUploadService _fileUploadService;

    public DeleteListingCommandHandler(IApplicationDbContext context, IFileUploadService fileUploadService)
    {
        _context = context;
        _fileUploadService = fileUploadService;
    }

    public async Task<bool> Handle(DeleteListingCommand request, CancellationToken cancellationToken)
    {
        // 1. İlanı veritabanında bul (Resimleriyle birlikte ÇEKMEK ZORUNDAYIZ: .Include(x => x.Images))
        var listing = await _context.Listings
            .Include(x => x.Images) // RESİMLER BURADA DAHİL EDİLİYOR
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        // 2. İlan yoksa false dön (Controller'da hata mesajı göstereceğiz)
        if (listing == null)
        {
            return false;
        }

        // 3. EĞER İLANIN RESİMLERİ VARSA ÖNCE SUNUCUDAN FİZİKSEL OLARAK SİL
        if (listing.Images != null && listing.Images.Any())
        {
            foreach (var image in listing.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    await _fileUploadService.DeleteAsync(image.ImageUrl);
                }
            }
        }

        // 4. İlanı ve bağlı özelliklerini/resim kayıtlarını veritabanından sil
        _context.Listings.Remove(listing);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}