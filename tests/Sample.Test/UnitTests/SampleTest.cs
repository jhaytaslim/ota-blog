using API.Controllers;
using Application.Contracts;
using Application.DTOs;
using Application.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleService.Test.Mocks;

namespace SampleService.Tests.UnitTests;
public class SampleTest
{
    private Mock<IServiceManager> _mock;
    public SampleTest()
    {
        _mock = new Mock<IServiceManager>();
    }


    [Fact]
    public void CanDoUnitTests()
    {
        var setup = 1 + 1;

        Assert.Equal<int>(2, setup);
    }

}
