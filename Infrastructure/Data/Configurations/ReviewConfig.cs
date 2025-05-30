﻿using AspNetCoreEcommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreEcommerce.Infrastructure.Data.Configurations
{
    public class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(f => f.Id);
            builder.ToTable("Reviews");
        }
    }
}
