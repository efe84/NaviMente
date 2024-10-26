namespace NLayeredApi._1Template.WebApi.Dto.ProcesosSeleccion.IncluirCandidatoEnProceso
{
    public class IncluirCandidatoEnProcesoResponse
    {
        public long IdCandidatura { get; set; }

        public IncluirCandidatoEnProcesoResponse(long idCandidatura)
        {
            IdCandidatura = idCandidatura;
        }

    }
}
