using Microsoft.EntityFrameworkCore;

namespace NaviMente.WebApi.Infrastructure.Persistence
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext() { }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new Persistence.Configuration.Candidatos.CandidatoConfiguration());
            //modelBuilder.ApplyConfiguration(new Persistence.Configuration.ProcesosSeleccion.CandidatoConfiguration());
            //modelBuilder.ApplyConfiguration(new ExperienciaLaboralConfiguration());
            //modelBuilder.ApplyConfiguration(new CandidaturaConfiguration());
            //modelBuilder.ApplyConfiguration(new NacionalidadConfiguration());
            //modelBuilder.ApplyConfiguration(new ProcesoSeleccionConfiguration());
            //modelBuilder.ApplyConfiguration(new EstadoProcesoSeleccionConfiguration());
            //modelBuilder.ApplyConfiguration(new EstadoCandidaturaConfiguration());
            //modelBuilder.ApplyConfiguration(new SectorConfiguration());
            //modelBuilder.ApplyConfiguration(new PuestoConfiguration());
        }


    }
}
