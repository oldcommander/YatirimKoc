using Microsoft.EntityFrameworkCore;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Infrastructure.Persistence.Seed;

public static class ListingSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // 1. İşlem Tiplerini (Transaction Types) Ekle
        if (!await context.TransactionTypes.AnyAsync())
        {
            context.TransactionTypes.AddRange(
                new TransactionType { Id = Guid.NewGuid(), Name = "Satılık", Slug = "satilik", IsActive = true },
                new TransactionType { Id = Guid.NewGuid(), Name = "Kiralık", Slug = "kiralik", IsActive = true },
                new TransactionType { Id = Guid.NewGuid(), Name = "Kat Karşılığı", Slug = "kat-karsiligi", IsActive = true },
                new TransactionType { Id = Guid.NewGuid(), Name = "Devren", Slug = "devren", IsActive = true }
            );
            await context.SaveChangesAsync();
        }

        // 2. Emlak Tiplerini (Property Types) Ekle
        if (!await context.PropertyTypes.AnyAsync())
        {
            var konut = new PropertyType { Id = Guid.NewGuid(), Name = "Konut", Slug = "konut", IsActive = true };
            var arsa = new PropertyType { Id = Guid.NewGuid(), Name = "Arsa", Slug = "arsa", IsActive = true };
            var isyeri = new PropertyType { Id = Guid.NewGuid(), Name = "İşyeri", Slug = "isyeri", IsActive = true };

            context.PropertyTypes.AddRange(konut, arsa, isyeri);
            await context.SaveChangesAsync();

            // 3. Dinamik Özellikleri (Features) Ekle
            var odaSayisi = new Feature { Id = Guid.NewGuid(), Name = "Oda Sayısı", InputType = "Select", Options = "Stüdyo (1+0), 1+1, 2+1, 3+1, 4+1, 5+1 ve üzeri", IsActive = true };
            var banyoSayisi = new Feature { Id = Guid.NewGuid(), Name = "Banyo Sayısı", InputType = "Number", IsActive = true };
            var isitma = new Feature { Id = Guid.NewGuid(), Name = "Isıtma Tipi", InputType = "Select", Options = "Kombi (Doğalgaz), Merkezi Sistem, Yerden Isıtma, Klima, Soba, Yok", IsActive = true };
            var binaYasi = new Feature { Id = Guid.NewGuid(), Name = "Bina Yaşı", InputType = "Number", IsActive = true };
            var esyali = new Feature { Id = Guid.NewGuid(), Name = "Eşyalı mı?", InputType = "Select", Options = "Evet, Hayır", IsActive = true };

            var imarDurumu = new Feature { Id = Guid.NewGuid(), Name = "İmar Durumu", InputType = "Select", Options = "Konut, Ticari, Sanayi, Tarla, Bahçe, SİT Alanı", IsActive = true };
            var emsal = new Feature { Id = Guid.NewGuid(), Name = "Emsal (KAKS)", InputType = "Text", IsActive = true }; // 0.20, 1.50 gibi değerler girilir
            var gabari = new Feature { Id = Guid.NewGuid(), Name = "Gabari", InputType = "Text", IsActive = true }; // Serbest, 9.50 gibi

            context.Features.AddRange(odaSayisi, banyoSayisi, isitma, binaYasi, esyali, imarDurumu, emsal, gabari);
            await context.SaveChangesAsync();

            // 4. Özellikleri Emlak Tiplerine Bağlama (PropertyFeatures)

            // ---> KONUT ÖZELLİKLERİ <---
            context.PropertyFeatures.AddRange(
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = odaSayisi.Id, IsRequired = true, Order = 1 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = banyoSayisi.Id, IsRequired = true, Order = 2 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = isitma.Id, IsRequired = false, Order = 3 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = binaYasi.Id, IsRequired = true, Order = 4 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = esyali.Id, IsRequired = false, Order = 5 }
            );

            // ---> ARSA ÖZELLİKLERİ <---
            context.PropertyFeatures.AddRange(
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = imarDurumu.Id, IsRequired = true, Order = 1 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = emsal.Id, IsRequired = false, Order = 2 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = gabari.Id, IsRequired = false, Order = 3 }
            );

            // ---> İŞYERİ ÖZELLİKLERİ <---
            context.PropertyFeatures.AddRange(
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = odaSayisi.Id, IsRequired = false, Order = 1 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = isitma.Id, IsRequired = false, Order = 2 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = binaYasi.Id, IsRequired = false, Order = 3 }
            );

            await context.SaveChangesAsync();
        }
    }
}