namespace CoindeskApi.Models.Domains;

public class CurrencyEntity : IAggregateRoot
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Lang { get; set; }
    public string CurrencyName { get; set; }
}