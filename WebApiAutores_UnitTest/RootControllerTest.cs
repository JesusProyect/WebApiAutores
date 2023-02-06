using API.Controllers.V1;
using API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiAutores_UnitTest.Mocks;

namespace WebApiAutores_UnitTest
{
    public class RootControllerTest
    {

        [Fact]
        public async Task Get_WithUser_Admin_ShouldBe_4Links()
        {
            //Arrange
            var usuarioService = new UsuarioServiceMock
            {
                Resultado = AuthorizationResult.Success()
            };

            RootController rootcontroller = new(usuarioService)
            {
                Url = new UrlHelperMock()
            };

            //Act

            var resultado = await rootcontroller.Get();

            //Assert

            resultado.Value!.Count().Should().Be(4);
        }

        [Fact]
        public async Task Get_WithNoUser_Admin_ShouldBe_2Links()
        {
            //Arrange
            var usuarioService = new UsuarioServiceMock();
            usuarioService.Resultado = AuthorizationResult.Failed();
            RootController rootcontroller = new(usuarioService);
            rootcontroller.Url = new UrlHelperMock();

            //Act

            var resultado = await rootcontroller.Get();

            //Assert

            resultado.Value!.Count().Should().Be(2);
        }


        [Fact]
        public async Task Get_WithNoUser_Admin_ShouldBe_2Links_UsandoMoq()
        {
            //Arrange
            var mock = new Mock<IUsuarioService>();
            mock.Setup(x => x.EsAdmin(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var mockUrl = new Mock<IUrlHelper>();
            mockUrl.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(string.Empty);

            RootController rootcontroller = new(mock.Object)
            {
                Url = mockUrl.Object
            };

            //Act

            var resultado = await rootcontroller.Get();

            //Assert

            resultado.Value!.Count().Should().Be(2);
        }
    }
}
