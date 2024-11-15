using Microsoft.EntityFrameworkCore;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories.Query
{
    //public class CandidaturaQueryRepository : QueryRepository<Candidatura>
    //{
    //    public CandidaturaQueryRepository(ApplicationContext context) 
    //        : base(context)
    //    {
    //    }

    //    public IReadOnlyList<CandidaturaDto> ObtenerCandidaturasPorProceso(long idProceso)
    //    {
    //        return dbSet
    //            .Include(c => c.Candidato)
    //            .Include(c => c.Estado)
    //            .Where(c => c.IdProcesoSeleccion.Equals(idProceso))
    //            .Select(c => new CandidaturaDto()
    //            {
    //                IdCandidatura = c.Id,
    //                IdCandidato = c.Candidato.Id,
    //                Apellidos = c.Candidato.Apellidos,
    //                Nombre = c.Candidato.Nombre,
    //                IdProcesoSeleccion = c.IdProcesoSeleccion,
    //                Estado = c.Estado.Descripcion,
    //                Observaciones = c.Observaciones ?? string.Empty
    //            }).ToList().AsReadOnly();
    //    }
    //}
}
