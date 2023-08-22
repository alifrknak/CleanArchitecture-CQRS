﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class CarConfiguration : BaseEntityConfiguration<Car, Guid>
{
	public void Configure(EntityTypeBuilder<Car> builder)
	{
		builder.ToTable("Cars").HasKey(b => b.Id);

		builder.Property(b => b.ModelId).HasColumnName("ModelId").IsRequired();
		builder.Property(b => b.Kilometer).HasColumnName("Kilometer").IsRequired();
		builder.Property(b => b.CarState).HasColumnName("State");
		builder.Property(b => b.ModelYear).HasColumnName("ModelYear");
		
		builder.HasOne(b => b.Model);

	}
}