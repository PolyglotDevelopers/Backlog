using Backlog.Core.Domain.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Masters
{
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable(nameof(Company));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.TradeName)
                .HasMaxLength(750)
                .IsRequired();

            builder.Property(p => p.RegisteredName)
                .HasMaxLength(750)
                .IsRequired();

            builder.Property(p => p.ContactPerson)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(f => f.Address)
                .WithMany()
                .HasForeignKey(p => p.AddressId)
                .HasConstraintName("FK_Company_Address")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Currency)
                .WithMany()
                .HasForeignKey(p => p.CurrencyId)
                .HasConstraintName("FK_Company_Currency")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Language)
                .WithMany()
                .HasForeignKey(p => p.LanguageId)
                .HasConstraintName("FK_Company_Language")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
        }
    }
}