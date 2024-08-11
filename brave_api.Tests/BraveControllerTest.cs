using System.ComponentModel.DataAnnotations;
using brave_api.Classes;
using brave_api.Controllers;
using brave_api.Requests;
using Microsoft.AspNetCore.Mvc;

namespace brave_api.Tests;

public class BraveControllerTest
{
    BraveController braveController;

    public BraveControllerTest()
    {
        braveController = new BraveController();
    }

    public static IEnumerable<object[]> ValidInputTestData()
    {
        yield return new object[]{ new Person { AgeOfDeath = 10, YearOfDeath = 12 }, new Person() { AgeOfDeath = 13, YearOfDeath = 17 }, 4.5};
        yield return new object[]{ new Person { AgeOfDeath = 35, YearOfDeath = 40 }, new Person() { AgeOfDeath = 38, YearOfDeath = 40 }, 7};
        yield return new object[]{ new Person { AgeOfDeath = 44, YearOfDeath = 50 }, new Person() { AgeOfDeath = 55, YearOfDeath = 60 }, 16};
        yield return new object[]{ new Person { AgeOfDeath = 40, YearOfDeath = 50 }, new Person() { AgeOfDeath = 20, YearOfDeath = 28 }, 98.5};
        yield return new object[]{ new Person { AgeOfDeath = 17, YearOfDeath = 30 }, new Person() { AgeOfDeath = 17, YearOfDeath = 25 }, 331.5};
        yield return new object[]{ new Person { AgeOfDeath = 13, YearOfDeath = 12 }, new Person() { AgeOfDeath = 20, YearOfDeath = 17 }, -1};
        yield return new object[]{ new Person { AgeOfDeath = 5, YearOfDeath = 15 }, new Person() { AgeOfDeath = 7, YearOfDeath = 7 }, -1};
    }

    [Theory]
    [MemberData(nameof(ValidInputTestData))]
    public void CountKill_WhenInputValid_ReturnResult(Person pA, Person pB, decimal expectedResult)
    {
        BraveRequest request = new BraveRequest
        {
            PersonA = pA,
            PersonB = pB
        };

        OkObjectResult? actionResult = braveController.CountKill(request) as OkObjectResult;

        Assert.NotNull(actionResult);
        Assert.Equal(actionResult.StatusCode, 200);
        BraveResponse? response = actionResult.Value as BraveResponse;
        Assert.NotNull(response);
        Assert.Equal(expectedResult, response.Result);
    }

    public static IEnumerable<object[]> NegativeInputTestData()
    {
        yield return new object[]{ new Person { AgeOfDeath = -10, YearOfDeath = 12 }, new Person { AgeOfDeath = 13, YearOfDeath = 17 }, "AgeOfDeathPersonA", "Age cannot be negative"};
        yield return new object[]{ new Person { AgeOfDeath = 10, YearOfDeath = 12 }, new Person { AgeOfDeath = -13, YearOfDeath = 17 }, "AgeOfDeathPersonB", "Age cannot be negative"};
        yield return new object[]{ new Person { AgeOfDeath = 10, YearOfDeath = -12 }, new Person { AgeOfDeath = 13, YearOfDeath = 17 }, "YearDeathPersonA", "Year cannot be negative"};
        yield return new object[]{ new Person { AgeOfDeath = 10, YearOfDeath = 12 }, new Person { AgeOfDeath = 13, YearOfDeath = -17 }, "YearDeathPersonB", "Year cannot be negative"};
         yield return new object[]{ new Person { AgeOfDeath = (int)10.12345, YearOfDeath = 12 }, new Person { AgeOfDeath = 13, YearOfDeath = 17 }, "AgeDeathPersonA", "Age cannot be decimal"};
        yield return new object[]{ new Person { AgeOfDeath = 10, YearOfDeath = 12 }, new Person { AgeOfDeath = (int)13.12345, YearOfDeath = 17 }, "AgeDeathPersonB", "Age cannot be decimal"};
        yield return new object[]{ new Person { AgeOfDeath = 10, YearOfDeath = (int)12.12345 }, new Person { AgeOfDeath = 13, YearOfDeath = 17 }, "YearDeathPersonA", "Year cannot be decimal"};
        yield return new object[]{ new Person { AgeOfDeath = 10, YearOfDeath = 12 }, new Person { AgeOfDeath = 13, YearOfDeath = (int)17.12345 }, "YearDeathPersonB", "Year cannot be decimal"};
        yield return new object[]{ new Person { AgeOfDeath =  (int)-10.12345, YearOfDeath = (int)-12.5845 }, new Person { AgeOfDeath = (int)13.121, YearOfDeath = (int)17.12345 }, "All input", "All input fail"};
    }

    [Theory]
    [MemberData(nameof(NegativeInputTestData))]
    public void CountKill_WhenInputNegative_ReturnMinusOne(Person pA, Person pB, string errorKey, string errorMessage)
    {
        BraveRequest request = new BraveRequest
        {
            PersonA = pA,
            PersonB = pB
        };

        braveController.ModelState.AddModelError(errorKey, errorMessage);

        OkObjectResult? actionResult = braveController.CountKill(request) as OkObjectResult;

        Assert.NotNull(actionResult);
        Assert.Equal(actionResult.StatusCode, 200);
        BraveResponse? response = actionResult.Value as BraveResponse;
        Assert.NotNull(response);
        Assert.Equal(-1, response.Result);
    }
}