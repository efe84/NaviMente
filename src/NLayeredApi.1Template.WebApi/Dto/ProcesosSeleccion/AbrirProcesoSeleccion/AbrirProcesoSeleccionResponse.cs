namespace NLayeredApi._1Template.WebApi.Dto.ProcesosSeleccion.AbrirProcesoSeleccion
{
    public class AbrirProcesoSeleccionResponse
    {
        public AbrirProcesoSeleccionResponse(long idProcesoSeleccion)
        {
            IdProcesoSeleccion = idProcesoSeleccion;
        }

        public long IdProcesoSeleccion { get; set; }
    }
}
