using Backend.Api.Application.DTOs.Request;
using Backend.Api.Application.Interfaces;
using Backend.Api.Domain.Entities;
using Backend.Api.Presentation;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1
{
    [TestClass]
    public sealed class ClienteControllerTest
    {
        private Mock<IClienteRepository> _mockRepository;
        private ClienteController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IClienteRepository>();
            _controller = new ClienteController(_mockRepository.Object);
        }

        [TestMethod]
        public async Task Add_ReturnsOk_WhenClienteAdded()
        {
            var cliente = new AddClienteDTO
            {
                Nombre = "Nuevo Cliente",
                Genero = "M",
                Edad = 1,
                Identificacion = "000000",
                Direccion = "Direccion",
                Telefono = "Telefono",
                Contrasena = "12345",
            };

            _mockRepository.Setup(r => r.AddAsync(cliente)).Returns(Task.CompletedTask);

            var result = await _controller.Add(cliente);

            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetById_ReturnsCliente_WhenFound()
        {
            // Arrange
            var cliente = new Cliente { PersonaId = 1, Nombre = "Cliente" };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cliente);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(cliente, okResult.Value);
        }
    }
}
