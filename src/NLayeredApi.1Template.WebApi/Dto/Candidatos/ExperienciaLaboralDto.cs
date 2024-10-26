namespace NLayeredApi._1Template.WebApi.Dto.Candidatos
{
    public class ExperienciaLaboralDto
    {
        public long Id { set; get; }
        public DateTime FechaInicio { set; get; }
        public DateTime? FechaFin { set; get; }
        public string Sector { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string Funciones { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
    }
}
