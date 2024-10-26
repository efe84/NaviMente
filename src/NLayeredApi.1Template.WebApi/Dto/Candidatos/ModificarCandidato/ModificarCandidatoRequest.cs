namespace NLayeredApi._1Template.WebApi.Dto.Candidatos.ModificarCandidato
{
    public class ModificarCandidatoRequest
    {
        public long Id { get; set; }
        public string NIF { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public int IdNacionalidad { get; set; }
        public DateTime? FechaValidezPermisoTrabajo { get; set; }
    }
}
