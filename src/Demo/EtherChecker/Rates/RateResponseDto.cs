using System.Text.Json.Serialization;

namespace EtherChecker.Rates;

public class RateResponseDto
{
    [JsonPropertyName("asset_id_base")]
    public string BaseCurrency { get; set; }

    [JsonPropertyName("rates")]
    public List<RateDto> Rates { get; set; }

    public static implicit operator List<Rate>(RateResponseDto? response)
    {
        var result = new List<Rate>();

        if (response is null)
        {
            return result;
        }
        
        foreach (var responseRate in response.Rates)
        {
            result.Add(new Rate(response.BaseCurrency, responseRate.TargetCurrency, responseRate.RateValue, responseRate.TimeStamp));
        }

        return result;
    }
}

public class RateDto
{
    [JsonPropertyName("asset_id_quote")]
    public string TargetCurrency { get; set; }
    
    [JsonPropertyName("time")]
    public DateTime TimeStamp { get; set; }
    
    [JsonPropertyName("rate")]
    public decimal RateValue { get; set; }
}