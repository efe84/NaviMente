namespace NLayeredApi._1Template.WebApi.Dto.ProcesosSeleccion
{
    public class CandidaturaDto
    {
        public long IdProcesoSeleccion { get; set; }
        public long IdCandidatura { get; set; }
        public long IdCandidato { get; set; }
        public string NIF { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
    }
}
