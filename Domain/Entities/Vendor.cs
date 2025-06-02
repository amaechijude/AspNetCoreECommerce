using System.ComponentModel.DataAnnotations;
using AspNetCoreEcommerce.Application.UseCases.VendorUseCase;
using AspNetCoreEcommerce.Shared;

namespace AspNetCoreEcommerce.Domain.Entities
{
    public class Vendor
    {
        [Key]
        public Guid VendorId { get; set; }
        public required string VendorName { get; set; }
        public required string VendorEmail { get; set; }
        public string VendorPhone {  get; set; } = string.Empty;
        public string VendorBannerUri { get; set; } = string.Empty;
        public required string Location { get; set; }
        public string? VerificationCode { get; set; } = string.Empty;
        public bool IsActivated { get; set; } = false;
        [Url]
        public string GoogleMapUrl { get; set; } = string.Empty;
        [Url]
        public string TwitterUrl { get;set; } = string.Empty;
        [Url]
        public string InstagramUrl { get; set; } = string.Empty;
        [Url]
        public string FacebookUrl { get; set; } = string.Empty;
        public DateTimeOffset DateJoined { get; set; }
        public DateTimeOffset DateUpdated { get; private set; }
        public required Guid UserId { get; set; }
        public required User User { get; set; }
        public ICollection<Product> Products { get; set; } = [];

        public async Task UpdateVendor(UpdateVendorDto update)
        {
            if (update.VendorBanner != null)
                VendorBannerUri = await GlobalConstants.SaveImageAsync(update.VendorBanner, GlobalConstants.vendorSubPath);
            if (!string.IsNullOrWhiteSpace(update.VendorPhone))
                VendorPhone = update.VendorPhone;
            if (!string.IsNullOrWhiteSpace(update.Location))
                Location = update.Location;
            if (!string.IsNullOrWhiteSpace(update.GoogleMapUrl))
                GoogleMapUrl = update.GoogleMapUrl;
            if (!string.IsNullOrWhiteSpace(update.TwitterUrl))
                TwitterUrl = update.TwitterUrl;
            if (!string.IsNullOrWhiteSpace(update.InstagramUrl))
                InstagramUrl = update.InstagramUrl;
            if (!string.IsNullOrWhiteSpace(update.FacebookUrl))
                FacebookUrl = update.FacebookUrl;

            DateUpdated = DateTimeOffset.UtcNow;
        }

        public void ActivateVendor()
        {
            IsActivated = true;
            VerificationCode = string.Empty;
        }

        public VerificationCode CreateVerificationCode(Vendor vendor)
        {
            return new VerificationCode
            {
                MinutesToExpire = 20,
                Code = GlobalConstants.GenerateVerificationCode(), // Generate a unique code
                Email = vendor.VendorEmail, // Use the vendor's email
                VendorId = vendor.VendorId, // Associate with the vendor
                Vendor = vendor, // Set the vendor reference
                ExpiresIn = DateTime.UtcNow.AddMinutes(20) // Set expiration to 20 minutes from now
            };
        }

    }
}
