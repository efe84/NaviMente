namespace NLayeredApi._1Template.WebApi.Dto.Candidatos.GetCandidatos
{
    public class GetCandidatosResponse
    {
        public IReadOnlyList<CandidatoHeaderDto> Candidatos { get; }
       
        public GetCandidatosResponse(IReadOnlyList<CandidatoHeaderDto> candidatos) 
        { 
            Candidatos = candidatos;
        }
    }
}
