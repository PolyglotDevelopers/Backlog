using Backlog.Core.Domain.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Masters
{
    public class ProjectMemberMapConfig : IEntityTypeConfiguration<ProjectMemberMap>
    {
        public void Configure(EntityTypeBuilder<ProjectMemberMap> builder)
        {
            builder.ToTable(nameof(ProjectMemberMap));

            builder.HasKey(x => x.Id);

            builder.HasOne(f => f.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmployeeId)
                .HasConstraintName("FK_ProjectMemberMap_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Project)
                .WithMany()
                .HasForeignKey(p => p.ProjectId)
                .HasConstraintName("FK_ProjectMemberMap_Project")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
        }
    }
}