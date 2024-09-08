using System.Text.Json.Serialization;

namespace CoindeskApi.Models;

public class BitcoinPriceIndex
{
    [JsonPropertyName("time")]
    public TimeInfo Time { get; set; }
    
    [JsonPropertyName("disclaimer")]
    public string Disclaimer { get; set; }
    
    [JsonPropertyName("chartName")]
    public string ChartName { get; set; }
    
    [JsonPropertyName("bpi")]
    public BpiInfo Bpi { get; set; }
}

public class BpiInfo
{
    [JsonPropertyName("USD")]
    public CurrencyInfo USD { get; set; }
    
    [JsonPropertyName("GBP")]
    public CurrencyInfo GBP { get; set; }
    
    [JsonPropertyName("EUR")]
    public CurrencyInfo EUR { get; set; }
}

public class CurrencyInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
    
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }
    
    [JsonPropertyName("rate")]
    public string Rate { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("rate_float")]
    public float RateFloat { get; set; }
}

public class TimeInfo
{
    [JsonPropertyName("updated")]
    public string Updated { get; set; }
    
    [JsonPropertyName("updatedISO")]
    public string UpdatedISO { get; set; }
    
    [JsonPropertyName("updateduk")]
    public string Updateduk { get; set; }
}