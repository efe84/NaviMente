using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace NaviMente.WebApi.Infrastructure.Persistence.Configuration.Candidatos
{
    //internal class CandidatoConfiguration
    //       : IEntityTypeConfiguration<Candidato>
    //{
    //    public void Configure(EntityTypeBuilder<Candidato> builder)
    //    {
    //        builder.ToTable("Candidato");
    //        builder.HasKey(c => c.Id);
    //        builder.Property(c => c.NIF).HasColumnName("Nif");
    //        builder.Property(c => c.Apellidos).HasColumnName("Apellidos");
    //        builder.Property(c => c.Nombre).HasColumnName("Nombre");
    //        builder.Property(c => c.FechaNacimiento).HasColumnName("FechaNacimiento");
    //        builder.Property(c => c.IdNacionalidad).HasColumnName("IdNacionalidad");
    //        builder.Property(c => c.FechaValidezPermisoTrabajo).HasColumnName("FechaValidezPermisoTrabajo");

    //        builder.OwnsOne(
    //            p => p.Email,
    //            sa =>
    //            {
    //                sa.Property(p => p.Value).HasColumnName("Email");
    //            });

    //        builder
    //           .HasMany<ExperienciaLaboral>()
    //           .WithOne()
    //           .HasForeignKey(e => e.IdCandidato);
    //    }
    //}
}
