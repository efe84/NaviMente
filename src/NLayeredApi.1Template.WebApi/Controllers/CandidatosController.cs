
namespace NaviMente.WebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class CandidatosController : BaseController
    //{
    //    readonly DatabaseService _databaseService;

    //    public CandidatosController(DatabaseService databaseService, ILogger<CandidatosController> logger)
    //        : base(logger)
    //    {
    //        _databaseService = databaseService;
    //    }

    //    [HttpGet]
    //    public ActionResult<GetCandidatosResponse> GetList([FromQuery] GetCandidatosRequest request)
    //    {
    //        IReadOnlyList<CandidatoHeaderDto> candidatos = _databaseService.CandidatosQuery.GetAll(request.EdadMaxima);
    //        return Ok(new GetCandidatosResponse(candidatos));
    //    }

    //    [HttpGet("{idCandidato}")]
    //    public ActionResult<CandidatoDto> Get(long idCandidato)
    //    {
    //        CandidatoDto? candidato = _databaseService.CandidatosQuery.GetById(idCandidato);
    //        if (candidato is null)
    //            return NotFound($"Candidato con id {idCandidato} no encontrado");

    //        return Ok(candidato);
    //    }

    //    [HttpPost]
    //    public ActionResult<CrearCandidatoResponse> Create(CrearCandidatoRequest request)
    //    {
    //        Logger.LogInformation("Solicitud de registro de candidato con email {Email}", request.Email ?? "");
    //        Result<Candidato> result = CrearCandidatoDomainService.CrearCandidato(request, _databaseService.Candidatos);
    //        if (result.IsFailure)
    //            return BadRequest(result.Error);

    //        Candidato candidato = result.Value;
    //        _databaseService.Candidatos.Create(candidato);

    //        _databaseService.Commit();

    //        return Ok(new CrearCandidatoResponse(candidato.Id));
    //    }

    //    [HttpPatch("{idCandidato}")]
    //    public IActionResult Update(long idCandidato, ModificarCandidatoRequest request)
    //    {            
    //        Candidato? candidato = _databaseService.Candidatos.Find(idCandidato);
    //        if (candidato is null)
    //            return NotFound($"Candidato con id {idCandidato} no encontrado");

    //        Logger.LogInformation("Solicitud de actualización del candidato {IdCandidato}", request.Id);

    //        Result result = candidato.ModificarCandidato(request);
    //        if (result.IsFailure)
    //            return BadRequest(result.Error);

    //        _databaseService.Commit();

    //        return Ok();
    //    }

    //    [HttpDelete("{idCandidato}")]
    //    public IActionResult Delete(long idCandidato)
    //    {
    //        Candidato? candidato = _databaseService.Candidatos.Find(idCandidato);
    //        if (candidato == null)
    //            return NotFound($"Candidato con id {idCandidato} no encontrado");

    //        _databaseService.Candidatos.Delete(candidato);

    //        _databaseService.Commit();

    //        return Ok();
    //    }


    //    [HttpPost("{idCandidato}/experienciaslaborales")]
    //    public ActionResult<CrearExperienciaLaboralResponse> CreateExperienciaLaboral(long idCandidato, CrearExperienciaLaboralRequest request)
    //    {
    //        Candidato? candidato = _databaseService.Candidatos.Find(idCandidato);
    //        if (candidato is null)
    //            return NotFound($"Candidato con id {idCandidato} no encontrado");

    //        Result result = candidato.AddExperienciaLaboral(request);

    //        if (result.IsFailure)
    //            return BadRequest(result.Error);

    //        _databaseService.Commit();

    //        return Ok(new CrearExperienciaLaboralResponse(candidato.GetExperienciaLaboralLastId));
    //    }

    //    [HttpDelete("{idCandidato}/experienciaslaborales/{idExperienciaLaboral}")]
    //    public IActionResult DeleteExperienciaLaboral(long idCandidato, long idExperienciaLaboral)
    //    {
    //        Candidato? candidato = _databaseService.Candidatos.Find(idCandidato);
    //        if (candidato is null)
    //            return NotFound($"Candidato con Id {idCandidato} no encontrado");

    //        candidato.EliminarExperienciaLaboral(idExperienciaLaboral);

    //        _databaseService.Commit();

    //        return Ok();
    //    }

    //}
}
