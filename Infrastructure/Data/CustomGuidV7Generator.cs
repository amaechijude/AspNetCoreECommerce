using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AspNetCoreEcommerce.Infrastructure.Data
{
    public class CustomGuidV7Generator : ValueGenerator<Guid>
    {
        public override bool GeneratesTemporaryValues => false; // This ensures permanent value

        public override Guid Next(EntityEntry entry)
        {
            if (entry == null)
            {
                throw new InValidGuidException("Invalid Guid Generation");
            }

            return Guid.CreateVersion7();
        }
    }

    public class InValidGuidException(string message) : Exception(message);
}
