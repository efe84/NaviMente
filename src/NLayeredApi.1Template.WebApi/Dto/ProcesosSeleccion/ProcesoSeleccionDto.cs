namespace NLayeredApi._1Template.WebApi.Dto.ProcesosSeleccion
{
    public class ProcesoSeleccionDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaIncorporacion { get; set; }
        public string Puesto { get; set; } = string.Empty;
        public int Vacantes { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
