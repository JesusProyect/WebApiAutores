using API.Validations;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutores_UnitTest
{
    public class PrimeraLetraMayusculaAttributeTest
    {
        [Fact]
        public void PrimeraLetraMinuscula_ShouldBeError_IfFirstChar_isNot_Uppercase()
        {
            //Arrange
            PrimeraLetraMayusculaAttribute primeraLetraMayuscula = new();
            string valor = "jesus";

            var valContext = new ValidationContext(new { Nombre = valor });

            //Act
            var result = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Asset

            Assert.Equal("La primera letra debe ser mayuscula", result!.ErrorMessage); //cualquiera de las dos es igual
            result.ErrorMessage.Should().Be("La primera letra debe ser mayuscula");

        }

        [Fact]
        public void PrimeraLetraMinuscula_ShouldBeError_WithNull()
        {
            //Arrange
            PrimeraLetraMayusculaAttribute primeraLetraMayuscula = new();
            string? valor = null;

            var valContext = new ValidationContext(new { Nombre = valor });

            //Act
            var result = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Asset

            result.Should().BeNull();

        }

        [Fact]
        public void PrimeraLetraMinuscula_ShouldBe_OK()
        {
            //Arrange
            PrimeraLetraMayusculaAttribute primeraLetraMayuscula = new();
            string? valor = "Jesus";

            var valContext = new ValidationContext(new { Nombre = valor });

            //Act
            var result = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Asset

            result.Should().BeNull();

        }
    }
}