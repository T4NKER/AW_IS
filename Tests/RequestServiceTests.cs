using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using RequestManager.Models;

namespace RequestManager.Tests.Models {
  public class RequestServiceTests { [Fact]
    public void CreateRequest_AddsRequestToList() {
      // Arrange
      var requestService = new RequestService();
      var newRequest = new RequestModel(13, "New Sample Request", DateTime.Now, DateTime.Now.AddDays(1));

      // Act
      requestService.CreateRequest(newRequest);
      var requests = requestService.GetAllRequests();

      // Assert
      Assert.Contains(newRequest, requests);
    }

    [Fact]
    public void CreateRequest_AddsEmptyDescriptionRequestToList() {
      // Arrange
      var requestService = new RequestService();
      var faultyRequest = new RequestModel(13, "", DateTime.Now, DateTime.Now.AddDays(1)); // Empty description

      // Act and Assert
      var exception = Assert.Throws < ArgumentException > (() =>requestService.CreateRequest(faultyRequest));

      // Assert
      Assert.Equal("Description cannot be empty", exception.Message);
    }

    [Fact]
    public void CreateRequest_AddsRequestWithPastDeadlineToList() {
      // Arrange
      var requestService = new RequestService();
      var pastDeadlineRequest = new RequestModel(13, "Sample Request", DateTime.Now, DateTime.Now.AddHours( - 1)); // Past deadline

      // Act and Assert
      var exception = Assert.Throws < ArgumentException > (() =>requestService.CreateRequest(pastDeadlineRequest));

      // Assert
      Assert.Equal("Deadline time cannot be before current time or within one hour", exception.Message);
    }

    [Fact]
    public void CreateRequest_AddsRequestWithDeadlineWithinOneHourToList() {
      // Arrange
      var requestService = new RequestService();
      var nearDeadlineRequest = new RequestModel(14, "Sample Request", DateTime.Now, DateTime.Now.AddMinutes(30));

      // Act
      var exception = Assert.Throws < ArgumentException > (() =>requestService.CreateRequest(nearDeadlineRequest));

      // Assert
      Assert.Equal("Deadline time cannot be before current time or within one hour", exception.Message);
    }

    [Fact]
    public void CreateRequest_AddsRequestWithDeadlineAfterOneHourToList() {
      // Arrange
      var requestService = new RequestService();
      var nearDeadlineRequest = new RequestModel(14, "Sample Request", DateTime.Now, DateTime.Now.AddMinutes(61));

      // Act
      requestService.CreateRequest(nearDeadlineRequest);

      // Assert
      var requests = requestService.GetAllRequests();
      Assert.Contains(nearDeadlineRequest, requests);
    }

    [Fact]
    public void DeleteRequest_RemovesRequestFromList() {
      // Arrange
      var requestService = new RequestService();
      var requestToDelete = requestService.GetRequests(1).First();

      // Act
      requestService.DeleteRequest(requestToDelete.Id);
      var requests = requestService.GetAllRequests();

      // Assert
      Assert.DoesNotContain(requestToDelete, requests);
    }

    [Theory][InlineData(1, 1, 10)] // First page
    [InlineData(2, 11, 2)] // Second page
    [InlineData(3, 0, 0)] // Non-existing page
    public void GetRequests_ReturnsCorrectPage(int pageNumber, int expectedStartId, int expectedCount) {
      // Arrange
      var requestService = new RequestService();

      // Act
      var requests = requestService.GetRequests(pageNumber);

      // Assert
      Assert.Equal(expectedCount, requests.Count());
      if (expectedCount > 0) {
        Assert.Equal(expectedStartId, requests.First().Id);
      }
    }
  }
}