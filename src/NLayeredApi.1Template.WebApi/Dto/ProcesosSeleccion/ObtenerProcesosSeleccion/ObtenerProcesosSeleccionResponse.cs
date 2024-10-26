namespace NLayeredApi._1Template.WebApi.Dto.ProcesosSeleccion.ObtenerProcesosSeleccion
{
    public class ObtenerProcesosSeleccionResponse
    {
        public IReadOnlyList<ProcesoSeleccionDto> ProcesosSeleccion { get; }

        public ObtenerProcesosSeleccionResponse(IReadOnlyList<ProcesoSeleccionDto> procesosSeleccion) 
        {
            ProcesosSeleccion = procesosSeleccion;
        }
    }
}
