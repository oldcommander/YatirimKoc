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

            // ==========================================
            // 3. DİNAMİK ÖZELLİKLERİ (FEATURES) OLUŞTUR
            // ==========================================

            // ORTAK VE GENEL ÖZELLİKLER
            var takas = new Feature { Id = Guid.NewGuid(), Name = "Takas", InputType = "Select", Options = "Evet, Hayır", IsActive = true };
            var kimden = new Feature { Id = Guid.NewGuid(), Name = "Kimden", InputType = "Select", Options = "Emlak Ofisinden, Sahibinden, İnşaat Firmasından", IsActive = true };
            var krediyeUygunluk = new Feature { Id = Guid.NewGuid(), Name = "Krediye Uygunluk", InputType = "Select", Options = "Evet, Hayır, Bilinmiyor", IsActive = true };
            var aidat = new Feature { Id = Guid.NewGuid(), Name = "Aidat (TL)", InputType = "Number", IsActive = true };
            var binaYasi = new Feature { Id = Guid.NewGuid(), Name = "Bina Yaşı", InputType = "Select", Options = "0, 1, 2, 3, 4, 5-10 arası, 11-15 arası, 16-20 arası, 21-25 arası, 26-30 arası, 31 ve üzeri", IsActive = true };
            var isitma = new Feature { Id = Guid.NewGuid(), Name = "Isıtma", InputType = "Select", Options = "Yok, Soba, Doğalgaz Sobası, Kat Kaloriferi, Merkezi, Merkezi (Pay Ölçer), Kombi (Doğalgaz), Yerden Isıtma, Klima, Fancoil Ünitesi, Güneş Enerjisi", IsActive = true };

            // KONUT ÖZELLİKLERİ
            var m2Brut = new Feature { Id = Guid.NewGuid(), Name = "m² (Brüt)", InputType = "Number", IsActive = true };
            var m2Net = new Feature { Id = Guid.NewGuid(), Name = "m² (Net)", InputType = "Number", IsActive = true };
            var odaSayisi = new Feature { Id = Guid.NewGuid(), Name = "Oda Sayısı", InputType = "Select", Options = "Stüdyo (1+0), 1+1, 1.5+1, 2+0, 2+1, 2.5+1, 3+1, 3.5+1, 4+1, 4.5+1, 5+1 ve üzeri", IsActive = true };
            var bulunduguKat = new Feature { Id = Guid.NewGuid(), Name = "Bulunduğu Kat", InputType = "Select", Options = "Bodrum Kat, Zemin Kat, Giriş Katı, Yüksek Giriş, Kot 1, Kot 2, Kot 3, 1. Kat, 2. Kat, 3. Kat, 4. Kat, 5. Kat, 6. Kat ve Üzeri, Çatı Katı, Teras Katı", IsActive = true };
            var katSayisi = new Feature { Id = Guid.NewGuid(), Name = "Kat Sayısı", InputType = "Number", IsActive = true };
            var banyoSayisi = new Feature { Id = Guid.NewGuid(), Name = "Banyo Sayısı", InputType = "Select", Options = "Yok, 1, 2, 3, 4, 5 ve üzeri", IsActive = true };
            var mutfak = new Feature { Id = Guid.NewGuid(), Name = "Mutfak Tipi", InputType = "Select", Options = "Açık Mutfak, Kapalı Mutfak", IsActive = true };
            var balkon = new Feature { Id = Guid.NewGuid(), Name = "Balkon", InputType = "Select", Options = "Var, Yok", IsActive = true };
            var asansor = new Feature { Id = Guid.NewGuid(), Name = "Asansör", InputType = "Select", Options = "Var, Yok", IsActive = true };
            var otopark = new Feature { Id = Guid.NewGuid(), Name = "Otopark", InputType = "Select", Options = "Açık Otopark, Kapalı Otopark, Yok", IsActive = true };
            var esyali = new Feature { Id = Guid.NewGuid(), Name = "Eşyalı", InputType = "Select", Options = "Evet, Hayır", IsActive = true };
            var kullanimDurumu = new Feature { Id = Guid.NewGuid(), Name = "Kullanım Durumu", InputType = "Select", Options = "Boş, Kiracılı, Mülk Sahibi", IsActive = true };
            var siteIcerisinde = new Feature { Id = Guid.NewGuid(), Name = "Site İçerisinde", InputType = "Select", Options = "Evet, Hayır", IsActive = true };
            var tapuKonut = new Feature { Id = Guid.NewGuid(), Name = "Tapu Durumu", InputType = "Select", Options = "Kat Mülkiyetli, Kat İrtifaklı, Hisseli Tapulu, Müstakil Tapulu, Arsa Tapulu", IsActive = true };

            // ARSA ÖZELLİKLERİ
            var imarDurumu = new Feature { Id = Guid.NewGuid(), Name = "İmar Durumu", InputType = "Select", Options = "Ada, Bağ & Bahçe, Depo, Eğitim, Enerji, Konut, Sanayi, SİT Alanı, Tarla, Ticari, Turizm, Turizm + Ticari", IsActive = true };
            var m2Tek = new Feature { Id = Guid.NewGuid(), Name = "m²", InputType = "Number", IsActive = true };
            var m2Fiyati = new Feature { Id = Guid.NewGuid(), Name = "m² Fiyatı", InputType = "Number", IsActive = true };
            var adaNo = new Feature { Id = Guid.NewGuid(), Name = "Ada No", InputType = "Text", IsActive = true };
            var parselNo = new Feature { Id = Guid.NewGuid(), Name = "Parsel No", InputType = "Text", IsActive = true };
            var paftaNo = new Feature { Id = Guid.NewGuid(), Name = "Pafta No", InputType = "Text", IsActive = true };
            var emsal = new Feature { Id = Guid.NewGuid(), Name = "Kaks (Emsal)", InputType = "Text", IsActive = true };
            var gabari = new Feature { Id = Guid.NewGuid(), Name = "Gabari", InputType = "Text", IsActive = true };
            var tapuArsa = new Feature { Id = Guid.NewGuid(), Name = "Tapu Durumu", InputType = "Select", Options = "Hisseli Tapulu, Müstakil Tapulu, Tahsis, Zilyetlik", IsActive = true };

            // İŞYERİ ÖZELLİKLERİ
            var isyeriTuru = new Feature { Id = Guid.NewGuid(), Name = "Türü", InputType = "Select", Options = "Atölye, AVM, Büfe, Büro & Ofis, Depo & Antrepo, Dükkan & Mağaza, Fabrika, İmalathane, Kafe, Klinik, Mağaza, Plaza Katı", IsActive = true };
            var bolumOdaSayisi = new Feature { Id = Guid.NewGuid(), Name = "Bölüm & Oda Sayısı", InputType = "Select", Options = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10 ve üzeri", IsActive = true };
            var durumuIsyeri = new Feature { Id = Guid.NewGuid(), Name = "Durumu", InputType = "Select", Options = "Sıfır, İkinci El", IsActive = true };

            // Özellikleri Veritabanına Ekle
            context.Features.AddRange(
                takas, kimden, krediyeUygunluk, aidat, binaYasi, isitma,
                m2Brut, m2Net, odaSayisi, bulunduguKat, katSayisi, banyoSayisi, mutfak, balkon, asansor, otopark, esyali, kullanimDurumu, siteIcerisinde, tapuKonut,
                imarDurumu, m2Tek, m2Fiyati, adaNo, parselNo, paftaNo, emsal, gabari, tapuArsa,
                isyeriTuru, bolumOdaSayisi, durumuIsyeri
            );
            await context.SaveChangesAsync();

            // ==========================================
            // 4. ÖZELLİKLERİ EMLAK TİPLERİNE BAĞLAMA
            // ==========================================

            // ---> KONUT ÖZELLİKLERİ BAĞLANTILARI <---
            context.PropertyFeatures.AddRange(
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = m2Brut.Id, IsRequired = true, Order = 1 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = m2Net.Id, IsRequired = true, Order = 2 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = odaSayisi.Id, IsRequired = true, Order = 3 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = binaYasi.Id, IsRequired = true, Order = 4 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = bulunduguKat.Id, IsRequired = true, Order = 5 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = katSayisi.Id, IsRequired = true, Order = 6 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = isitma.Id, IsRequired = true, Order = 7 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = banyoSayisi.Id, IsRequired = true, Order = 8 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = mutfak.Id, IsRequired = false, Order = 9 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = balkon.Id, IsRequired = false, Order = 10 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = asansor.Id, IsRequired = false, Order = 11 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = otopark.Id, IsRequired = false, Order = 12 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = esyali.Id, IsRequired = true, Order = 13 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = kullanimDurumu.Id, IsRequired = true, Order = 14 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = siteIcerisinde.Id, IsRequired = false, Order = 15 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = aidat.Id, IsRequired = false, Order = 16 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = krediyeUygunluk.Id, IsRequired = true, Order = 17 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = tapuKonut.Id, IsRequired = true, Order = 18 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = kimden.Id, IsRequired = true, Order = 19 },
                new PropertyFeature { PropertyTypeId = konut.Id, FeatureId = takas.Id, IsRequired = true, Order = 20 }
            );

            // ---> ARSA ÖZELLİKLERİ BAĞLANTILARI <---
            context.PropertyFeatures.AddRange(
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = imarDurumu.Id, IsRequired = true, Order = 1 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = m2Tek.Id, IsRequired = true, Order = 2 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = m2Fiyati.Id, IsRequired = false, Order = 3 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = adaNo.Id, IsRequired = false, Order = 4 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = parselNo.Id, IsRequired = false, Order = 5 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = paftaNo.Id, IsRequired = false, Order = 6 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = emsal.Id, IsRequired = false, Order = 7 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = gabari.Id, IsRequired = false, Order = 8 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = krediyeUygunluk.Id, IsRequired = true, Order = 9 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = tapuArsa.Id, IsRequired = true, Order = 10 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = kimden.Id, IsRequired = true, Order = 11 },
                new PropertyFeature { PropertyTypeId = arsa.Id, FeatureId = takas.Id, IsRequired = true, Order = 12 }
            );

            // ---> İŞYERİ ÖZELLİKLERİ BAĞLANTILARI <---
            context.PropertyFeatures.AddRange(
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = isyeriTuru.Id, IsRequired = true, Order = 1 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = m2Tek.Id, IsRequired = true, Order = 2 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = bolumOdaSayisi.Id, IsRequired = true, Order = 3 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = isitma.Id, IsRequired = false, Order = 4 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = binaYasi.Id, IsRequired = true, Order = 5 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = aidat.Id, IsRequired = false, Order = 6 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = krediyeUygunluk.Id, IsRequired = true, Order = 7 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = tapuKonut.Id, IsRequired = true, Order = 8 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = durumuIsyeri.Id, IsRequired = true, Order = 9 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = kimden.Id, IsRequired = true, Order = 10 },
                new PropertyFeature { PropertyTypeId = isyeri.Id, FeatureId = takas.Id, IsRequired = true, Order = 11 }
            );

            await context.SaveChangesAsync();
        }
    }
}