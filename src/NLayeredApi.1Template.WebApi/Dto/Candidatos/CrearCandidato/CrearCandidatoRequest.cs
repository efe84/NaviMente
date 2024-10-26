namespace NLayeredApi._1Template.WebApi.Dto.Candidatos.CrearCandidato
{
    public class CrearCandidatoRequest
    {
        public string Nif { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public int IdNacionalidad { get; set; }
        public DateTime? FechaValidezPermisoTrabajo { get; set; }
    }
}
