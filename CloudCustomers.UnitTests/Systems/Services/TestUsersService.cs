using CloudCustomers.API.Config;
using CloudCustomers.API.Models;
using CloudCustomers.API.Services;
using CloudCustomers.UnitTests.Fixtures;
using CloudCustomers.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CloudCustomers.UnitTests.Systems.Services;

public class TestUsersService
{
    [Fact]
    public async Task GetAll_WhenCalled_InvokesHttpGetRequest()
    {
        //Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "https://example.com/users";

        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        //Act
        await sut.GetAll();

        //Assert
        //Verify HTTP request is made!
        handlerMock
            .Protected()
            .Verify("SendAsync", Times.Exactly(1), ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get), ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task GetAll_WhenHits404_ReturnsEmptyListOfUsers()
    {
        //Arrange
        var endpoint = "https://example.com/users";

        var handlerMock = MockHttpMessageHandler<User>.SetupReturn404();
        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        //Act
        var result = await sut.GetAll();

        //Assert
        result.Count().Should().Be(0);
    }

    [Fact]
    public async Task GetAll_WhenCalled_ReturnsListOfUsersOfExpectedSize()
    {
        //Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "https://example.com/users";

        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        //Act
        var result = await sut.GetAll();

        //Assert
        result.Count().Should().Be(expectedResponse.Count);
    }

    [Fact]
    public async Task GetAll_WhenCalled_InvokesConfiguredExternalUrl()
    {
        //Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "https://example.com/users";

        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse, endpoint);
        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        //Act
        var result = await sut.GetAll();

        //Assert
        handlerMock
            .Protected()
            .Verify("SendAsync", Times.Exactly(1), ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri(endpoint)), ItExpr.IsAny<CancellationToken>());
    }
}
