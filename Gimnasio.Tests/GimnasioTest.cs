using System;
using System.Collections.Generic;
using NUnit.Framework;
using Gimnasio;
using Moq;

namespace Gimnasio.Tests
{

    [TestFixture]
    public class GimnasioTests
    {
        [Test]
        public void AgregarSala_DeberiaAgregarSalaCorrectamente()
        {
            // arrange
            Gimnasio gimnasio = new Gimnasio("Gimnasio Test", "Direccion Test");
            Sala sala = new Sala("Sala de Yoga", 20);

            //act
            gimnasio.AgregarSala(sala); // agregar la sala bien

            // assert
            Assert.Contains(sala, gimnasio.Salas);
        }

        [Test]
        public void RegistrarSocio_DeberiaRegistrarSocioCorrectamente()
        {
            // arrange
            Gimnasio gimnasio = new Gimnasio("Gimnasio Test", "Direccion Test");
            Socio socio = new Socio("1", "Juan", "Perez", "123456789", "VIP", 5);

            // act
            gimnasio.RegistrarSocio(socio); // registrar el socio bien

            // assert
            Assert.Contains(socio, gimnasio.Socios);
            
        }

        [Test]
        public void RegistrarSocio_NoDeberiaRegistrarSocioDuplicado()
        {
            //arrange
            Gimnasio gimnasio = new Gimnasio("Gimnasio Test", "Direccion Test");
            Socio socio = new Socio("1", "Juan", "Perez", "123456789", "VIP", 5);

            // act
            gimnasio.RegistrarSocio(socio);
            gimnasio.RegistrarSocio(socio); // registra el mismo socio de nuvo

            // assert
            Assert.AreEqual(1, gimnasio.Socios.Count);
        }

    }

    // integration test
    public interface ILogger
    {
        void Log(string message);
    }

    [TestFixture]
    public class GimnasioIntegrationTests
    {
        [Test]
        public void RegistrarSocio_LogsMessageWhenSocioIsRegistered()
        {
            // arrange
            var mockLogger = new Mock<ILogger>();
            var gimnasio = new Gimnasio("Gimnasio Central", "Av. Principal 123");
            var socio = new Socio("001", "Carlos", "Lopez", "987654321", "Premium", 3);

            // act
            gimnasio.RegistrarSocio(socio);
            mockLogger.Object.Log($"Socio {socio.Nombre} registrado en el gimnasio {gimnasio}");

            // assert
            mockLogger.Verify(logger => logger.Log(It.Is<string>(msg => msg.Contains("Socio Carlos registrado"))), Times.Once);
        }
    }
}

