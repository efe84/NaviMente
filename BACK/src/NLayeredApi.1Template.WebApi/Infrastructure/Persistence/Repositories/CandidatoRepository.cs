using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories
{
    //public class CandidatoRepository : Repository<Candidato>, ICandidatoRepository
    //{
    //    public CandidatoRepository(ApplicationContext context) : base(context)
    //    {
    //    }

    //    public override Candidato? Find(long key)
    //    {
    //        return dbSet.Include(c => c.ExperienciasLaborales).Where(c => c.Id.Equals(key)).FirstOrDefault();
    //    }

    //    public bool ExisteCandidatoConEmail(string email)
    //    {
    //        return dbSet.Any(c => c.Email.Value.Equals(email));
    //    }

    //    public bool ExisteCandidatoConId(long id)
    //    {
    //        return dbSet.Any(c => c.Id.Equals(id));
    //    }
    //}
}
