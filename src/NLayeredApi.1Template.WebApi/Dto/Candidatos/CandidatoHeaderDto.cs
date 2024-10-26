namespace NaviMente.WebApi.Dto.Candidatos
{
    public class CandidatoHeaderDto
    {
        public long Id { get; set; }
        public string Nif { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public int IdNacionalidad { get; set; }
        public string Nacionalidad { get; set; } = string.Empty;
    }
}
