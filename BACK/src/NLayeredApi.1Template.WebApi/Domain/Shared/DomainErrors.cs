

namespace NLayeredApi._1Template.WebApi.Domain.Shared
{
    public static class DomainErrors
    {
        //    public static class Candidato
        //    {
        //        public static Error EmailYaRegistrado(string email) =>
        //            new Error("email.already.registered", "Ya existe un candidato registrado con la dirección de correo " + email);

        //        public static Error PermisoTrabajoSoloExtranjeros()
        //            => new Error("permiso.trabajo.solo.extranjeros", "El permiso de trabajo solamente es necesario para trabajadores con nacionalidad extranjera");

        //        public static Error IdCandidatoNoEncontrado(long idCandidato)
        //            => new Error("id.candidato.not.found", $"Id candidato no encontrado: {idCandidato}");

        //    }

        //    public static class ProcesoSeleccion
        //    {
        //        public static Error ProcesoCerrado(long idProcesoSeleccion) =>
        //            new Error("proceso.cerrado", $"El proceso de selección {idProcesoSeleccion} está cerrado");

        //        public static Error PermisoTrabajoNoValido() =>
        //           new Error("permiso.trabajo.no.valido", "El candidato no tiene un permiso de trabajo válido para poder ser contratado");

        //        public static Error NumeroVacantesCubierto() =>
        //           new Error("permiso.trabajo.no.valido", "No se pueden seleccionar a más candidatos en el proceso debido a que ya se han cubierto el número de vacantes");

        //        public static Error CandidatoNoIncluidoEnProceso(long idCandidato, long idProceso) =>
        //           new Error("candidato.no.incluido.en.proceso", $"El candidato con id {idCandidato} no ha sido incluido en el proceso {idProceso}");

        //        public static Error CandidaturaNoEncontrada(long idCandidato, long idProceso) =>
        //           new Error("candidato.no.incluido.en.proceso", $"No se encuentra una candidatura con id {idCandidato} en el proceso {idProceso}");

        //        public static Error CandidatoYaIncluidoEnProceso(long idCandidato, long idProceso) =>
        //           new Error("candidato.ya.incluido.en.proceso", $"El candidato con id {idCandidato} ya está incluido en el proceso {idProceso}");

        //    }

        //    public static class General
        //    {
        //        public static Error Field1MustBeGreatherThanField2(string field1, string field2)
        //            => new Error("field1.must.be.greather.than.field2", $"{field1} debe ser mayor que {field2}");

        //        public static Error FieldMustBeGreatherThanValue(string field, int value)
        //            => new Error("field.must.be.greather.than.value", $"{field} debe ser mayor que {value}");

        //        public static Error ValueIsInvalid(string value) =>
        //            new Error("value.is.invalid", $"Value {value} is invalid");

        //        public static Error FieldIsRequired(string field) =>
        //            new Error("field.is.required", $"Field {field} is required");

        //        public static Error InvalidLength(string name = "")
        //        {
        //            string label = name == null ? " " : " " + name + " ";
        //            return new Error("invalid.string.length", $"Invalid{label}length");
        //        }

        //        public static Error CollectionIsTooSmall(int min, int current)
        //        {
        //            return new Error(
        //                "collection.is.too.small",
        //                $"The collection must contain {min} items or more. It contains {current} items.");
        //        }

        //        public static Error CollectionIsTooLarge(int max, int current)
        //        {
        //            return new Error(
        //                "collection.is.too.large",
        //                $"The collection must contain {max} items or more. It contains {current} items.");
        //        }

        //        public static Error InternalServerError(string message)
        //        {
        //            return new Error("internal.server.error", message);
        //        }
        //    }
    }

}
