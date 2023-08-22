﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations;

public class BrandConfiguration : BaseEntityConfiguration<Brand, Guid>
{
	public void Configure(EntityTypeBuilder<Brand> builder)
	{
		builder.ToTable("Brands").HasKey(b => b.Id);
		builder.Property(b => b.Name).HasColumnName("Name").IsRequired();
		builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
		builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
		builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");
		
		builder.HasIndex(b => b.Name, name: "UK_Brands_Name").IsUnique();

		builder.HasMany(b => b.Models);


	}
}
