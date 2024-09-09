using System.ComponentModel.DataAnnotations;
using CoindeskApi.Models;
using FluentAssertions;

namespace CoindeskApi.Test;

public class CreateCurrencyDtoTest
{
    [Fact(DisplayName = $"測試 {nameof(CreateCurrencyDto.Code)} 為必填")]
    public void Code_Required_Test()
    {
        var request = new CreateCurrencyDto
        {
            Code = string.Empty,
            CurrencyName = "新台幣"
        };
        
        var actual = new List<ValidationResult>();
        var ctx = new ValidationContext(request);
        Validator.TryValidateObject(request, ctx, actual, true);
        
        actual.Should().NotBeNullOrEmpty();
        actual.Count().Should().Be(1);
        actual.First().MemberNames.Should().BeEquivalentTo(new List<string>() {$"{nameof(request.Code)}" });
        actual.First().ErrorMessage.Should().Contain("The Code field is required.");
    }
    
    [Fact(DisplayName = $"測試 {nameof(CreateCurrencyDto.CurrencyName)} 為必填")]
    public void CurrencyName_Required_Test()
    {
        var request = new CreateCurrencyDto
        {
            Code = "TWD",
            CurrencyName = string.Empty
        };
        
        var actual = new List<ValidationResult>();
        var ctx = new ValidationContext(request);
        Validator.TryValidateObject(request, ctx, actual, true);
        
        actual.Should().NotBeNullOrEmpty();
        actual.Count().Should().Be(1);
        actual.First().MemberNames.Should().BeEquivalentTo(new List<string>() {$"{nameof(request.CurrencyName)}" });
        actual.First().ErrorMessage.Should().Contain("The CurrencyName field is required.");
    }
}