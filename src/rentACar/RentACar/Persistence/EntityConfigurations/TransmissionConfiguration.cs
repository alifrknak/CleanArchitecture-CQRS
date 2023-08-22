using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data.SqlTypes;

namespace Persistence.EntityConfigurations;

public class TransmissionConfiguration : BaseEntityConfiguration<Transmission, Guid>
{
	public void Configure(EntityTypeBuilder<Transmission> builder)
	{
		builder.ToTable("Transmissions").HasKey(b => b.Id);

		builder.Property(b => b.Name).HasColumnName("Name").IsRequired();
		builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
		builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
		builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");

		builder.HasIndex(indexExpression: b => b.Name, name: "UK_Tranmissions_Name").IsUnique();

		builder.HasMany(b => b.Models);

	}
}
