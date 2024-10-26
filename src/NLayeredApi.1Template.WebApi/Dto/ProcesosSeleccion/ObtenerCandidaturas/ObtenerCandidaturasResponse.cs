namespace NLayeredApi._1Template.WebApi.Dto.ProcesosSeleccion.ObtenerCandidaturas
{
    public class ObtenerCandidaturasResponse
    {
        public IReadOnlyList<CandidaturaDto> Candidaturas { get; }

        public ObtenerCandidaturasResponse(IReadOnlyList<CandidaturaDto> candidaturas)
        {
            Candidaturas = candidaturas;
        }   
    }
}
