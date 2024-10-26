using Microsoft.EntityFrameworkCore;
using NLayeredApi._1Template.WebApi.Infrastructure;
using NLayeredApi._1Template.WebApi.Dto.Candidatos;

namespace NLayeredApi._1Template.WebApi.Infrastructure.Persistence.Repositories.Query
{
    //public class CandidatoQueryRepository : QueryRepository<Candidato>
    //{
    //    public CandidatoQueryRepository(ApplicationContext context)
    //        : base(context)
    //    {            
    //    }

    //    public IReadOnlyList<CandidatoHeaderDto> GetAll(int? edadMaxima)
    //    {
    //        return dbSet
    //            .Include(c => c.Nacionalidad)
    //            .Where(c => edadMaxima == null || c.FechaNacimiento > DateTime.Now.AddYears(edadMaxima.Value * -1))
    //            .Select(c => new CandidatoHeaderDto()
    //            {
    //                Id = c.Id,
    //                Nif = c.NIF,
    //                Apellidos = c.Apellidos,
    //                Nombre = c.Nombre,
    //                FechaNacimiento = c.FechaNacimiento,
    //                Email = c.Email.Value,
    //                Nacionalidad = c.Nacionalidad.Descripcion
    //            }).ToList().AsReadOnly();
    //    }

    //    public CandidatoDto? GetById(long idCandidato)
    //    {
    //        return dbSet
    //            .Where(c => c.Id == idCandidato)
    //            .Include(c => c.ExperienciasLaborales)
    //                .ThenInclude(e => e.Sector)
    //            .Include(c => c.ExperienciasLaborales)
    //                .ThenInclude(e => e.Puesto)
    //            .Select(c => new CandidatoDto()
    //            {
    //                Id = c.Id,
    //                Nif = c.NIF,
    //                Apellidos = c.Apellidos,
    //                Nombre = c.Nombre,
    //                FechaNacimiento = c.FechaNacimiento,
    //                Email = c.Email.Value,
    //                IdNacionalidad = c.Nacionalidad.Id,
    //                FechaValidezPermisoTrabajo = c.FechaValidezPermisoTrabajo,
    //                ExperienciaLaboral = c.ExperienciasLaborales.Select(e => new ExperienciaLaboralDto()
    //                {
    //                    Id = e.Id,
    //                    Empresa = e.Empresa,
    //                    FechaInicio = e.FechaInicio,
    //                    FechaFin = e.FechaFin,
    //                    Funciones = e.Funciones,
    //                    Puesto = e.Puesto.Descripcion,
    //                    Sector = e.Sector.Descripcion
    //                })
    //            }).SingleOrDefault();
    //    }
    //}
}
