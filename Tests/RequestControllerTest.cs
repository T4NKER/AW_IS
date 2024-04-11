using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RequestManager.Controllers;
using RequestManager.Models;
using Lib.AspNetCore.ServerSentEvents;
using Xunit;

namespace RequestManager.Tests.Controllers
{
    public class RequestControllerTests
    {
        [Fact]
        public void RequestView_ValidId_ReturnsViewResult()
        {
            // Arrange
            var requestService = new RequestService(); // Actual implementation

            // Create mock IServerSentEventsService
            var serverSentEventsServiceMock = new Mock<IServerSentEventsService>();

            // Create SSEService with the mock IServerSentEventsService
            var sseService = new SSEService(serverSentEventsServiceMock.Object); 

            var controller = new RequestController(requestService, sseService);

            // Act
            var result = controller.RequestView(1); // Assuming id = 1 exists in the database

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void RequestView_IdZero_ReturnsErrorView()
{
            // Arrange
            var requestService = new RequestService(); 
            var serverSentEventsServiceMock = new Mock<IServerSentEventsService>();
            var sseService = new SSEService(serverSentEventsServiceMock.Object);
            var controller = new RequestController(requestService, sseService);

            // Act
            var result = controller.RequestView(0); 

            // Assert
            // Assert
var viewResult = Assert.IsType<ViewResult>(result);
Console.WriteLine("Actual View Name: " + viewResult.ViewName);
Console.WriteLine("View Data Count: " + viewResult.ViewData.Count);

foreach (var key in viewResult.ViewData.Keys)
{
    Console.WriteLine($"ViewData[{key}]: {viewResult.ViewData[key]}");
}

Console.WriteLine("Model: " + viewResult.Model); 
        }

        [Fact]
        public void RequestView_IdNotInDatabase_ReturnsNotFound()
        {
            // Arrange
            var requestService = new RequestService(); // Actual implementation
            var serverSentEventsServiceMock = new Mock<IServerSentEventsService>();
            var sseService = new SSEService(serverSentEventsServiceMock.Object); 
            var controller = new RequestController(requestService, sseService);

            // Act
            var result = controller.RequestView(999); // Assuming id = 999 does not exist in the database

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        
    }
}
