using Microsoft.EntityFrameworkCore;
using NLayeredApi._1Template.WebApi.Infrastructure.Persistence.Repositories;
using NLayeredApi._1Template.WebApi.Infrastructure.Persistence.Repositories.Query;
using System.Net.Mail;

namespace NaviMente.WebApi.Infrastructure.Persistence
{
    public class DatabaseService : DbContext
    {
        //private readonly ApplicationContext _context;

        //public DatabaseService(ApplicationContext context)
        //{
        //    _context = context;
        //}

        //ICandidatoRepository? _candidatos;
        //public ICandidatoRepository Candidatos { 
        //    get
        //    {
        //        _candidatos ??= new CandidatoRepository(_context);

        //        return _candidatos;
        //    }
        //}

        //IProcesoSeleccionRepository? _procesosSeleccion;
        //public IProcesoSeleccionRepository ProcesosSeleccion
        //{
        //    get
        //    {
        //        _procesosSeleccion ??= new ProcesoSeleccionRepository(_context);

        //        return _procesosSeleccion;
        //    }
        //}

        //#region Query repositories

        //CandidatoQueryRepository? _candidatosQuery;
        //public CandidatoQueryRepository CandidatosQuery
        //{
        //    get
        //    {
        //        _candidatosQuery ??= new CandidatoQueryRepository(_context);

        //        return _candidatosQuery;
        //    }
        //}

        //ProcesoSeleccionQueryRepository? _procesosSeleccionQuery;
        //public ProcesoSeleccionQueryRepository ProcesosSeleccionQuery
        //{
        //    get
        //    {
        //        _procesosSeleccionQuery ??= new ProcesoSeleccionQueryRepository(_context);

        //        return _procesosSeleccionQuery;
        //    }
        //}

        //CandidaturaQueryRepository? _candidaturasQuery;
        //public CandidaturaQueryRepository CandidaturasQuery
        //{
        //    get
        //    {
        //        _candidaturasQuery ??= new CandidaturaQueryRepository(_context);

        //        return _candidaturasQuery;
        //    }
        //}

        //ExperienciaLaboralQueryRepository? _experienciasLaboralesQuery;
        //public ExperienciaLaboralQueryRepository ExperienciasLaboralesQuery
        //{
        //    get
        //    {
        //        _experienciasLaboralesQuery ??= new ExperienciaLaboralQueryRepository(_context);

        //        return _experienciasLaboralesQuery;
        //    }
        //}

        //SectorQueryRepository? _sectoresQuery;
        //public SectorQueryRepository SectoresQuery
        //{
        //    get
        //    {
        //        _sectoresQuery ??= new SectorQueryRepository(_context);

        //        return _sectoresQuery;
        //    }
        //}

        //PuestoQueryRepository? _puestosQuery;
        //public PuestoQueryRepository PuestosQuery
        //{
        //    get
        //    {
        //        _puestosQuery ??= new PuestoQueryRepository(_context);

        //        return _puestosQuery;
        //    }
        //}

        //#endregion

        //public void Commit()
        //{
        //    _context.SaveChanges();
        //}

        //public new void Attach<T>(T entity) where T : Entity
        //{
        //    _context.Set<T>().Attach(entity);
        //}
    }
}
