namespace NLayeredApi._1Template.WebApi.Dto.ProcesosSeleccion.AbrirProcesoSeleccion
{
    public class AbrirProcesoSeleccionRequest
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaIncorporacion { get; set; }
        public int Vacantes { get; set; }
        public int IdPuesto { get; set; }
        public int IdSector { get; set; }
    }
}
