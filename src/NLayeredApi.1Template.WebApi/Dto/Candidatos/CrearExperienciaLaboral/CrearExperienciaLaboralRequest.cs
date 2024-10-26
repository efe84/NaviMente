namespace NLayeredApi._1Template.WebApi.Dto.Candidatos.CrearExperienciaLaboral
{
    public class CrearExperienciaLaboralRequest
    {
        public long IdCandidato { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public int Sector { get; set; }
        public int Puesto { get; set; }
        public string Funciones { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
