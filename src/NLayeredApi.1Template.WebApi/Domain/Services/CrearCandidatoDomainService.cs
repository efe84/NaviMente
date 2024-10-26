

namespace NaviMente.WebApi.Domain.Services
{
    /// <summary>
    /// Para crear a un candidato se usa un servicio de dominio porque es necesario implementar la invariante que dice
    /// que no puede haber más de un candidato con el mismo correo electrónico. El lógica de dominio, por lo que no puede 
    /// ir al manejador del comando. 
    /// </summary>
    //public static class CrearCandidatoDomainService
    //{
    //    public static Result<Candidato> CrearCandidato(CrearCandidatoRequest request, ICandidatoRepository candidatoRepository)
    //    {
    //        bool existeCandidatoConEmail = candidatoRepository.ExisteCandidatoConEmail(request.Email);

    //        if (existeCandidatoConEmail)
    //            return DomainErrors.Candidato.EmailYaRegistrado(request.Email);

    //        return Candidato.CreateNew(request);
    //    }
    //}
}
