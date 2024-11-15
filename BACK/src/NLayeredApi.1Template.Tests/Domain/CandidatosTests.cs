using Microsoft.AspNetCore.Rewrite;
using NLayeredApi._1Template.WebApi.Dto.Candidatos.CrearCandidato;
using NSubstitute;
using NSubstitute.Core.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace NaviMente.Tests.Domain
{
    public class CandidatosTests(ITestOutputHelper outputHelper)
    {
        [Fact]
        public void UnicidadCandidatoTest()
        {
            ////Act
            //CrearCandidatoRequest request = new CrearCandidatoRequest();
            //var repo = Substitute.For<ICandidatoRepository>();
            //repo.ExisteCandidatoConEmail(Arg.Any<string>()).Returns(true);

            ////Arrange
            //var result = CrearCandidatoDomainService.CrearCandidato(request, repo);
            //outputHelper.WriteLine(result.IsSuccess.ToString());

            ////Asert
            //Assert.True(result.IsFailure);
        }

    }
}
