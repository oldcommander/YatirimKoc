using YatirimKoc.Domain.Entities.Admin;

namespace YatirimKoc.Domain.Entities.Listings;

public class Listing
{
    public Guid Id { get; set; }

    // --------------------
    // BASIC INFO
    // --------------------
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
    public string Currency { get; set; } = "TL";

    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }

    // --------------------
    // PROPERTY DETAILS
    // --------------------
    public int? SquareMeter { get; set; }
    public int? RoomCount { get; set; }
    public int? BathroomCount { get; set; }
    public int? Floor { get; set; }
    public int? TotalFloor { get; set; }
    public int? BuildingAge { get; set; }

    // --------------------
    // LOCATION
    // --------------------
    public string City { get; set; } = null!;
    public string District { get; set; } = null!;
    public string? Neighborhood { get; set; }


    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public bool IsDeleted { get; set; } = false;


    // --------------------
    // RELATIONS
    // --------------------
    public Guid ListingTypeId { get; set; }
    public ListingType ListingType { get; set; } = null!;

    public Guid ListingCategoryId { get; set; }
    public ListingCategory ListingCategory { get; set; } = null!;

    // --------------------
    // AUTHORIZATION
    // --------------------
    public Guid CreatedByUserId { get; set; }

    public Guid AdminProfileId { get; set; }
    public AdminProfile AdminProfile { get; set; } = null!;

    // --------------------
    // SYSTEM
    // --------------------
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();

}
