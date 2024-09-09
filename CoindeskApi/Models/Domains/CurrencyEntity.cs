namespace CoindeskApi.Models.Domains;

public class CurrencyEntity : IAggregateRoot
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Lang { get; set; }
    public string CurrencyName { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime? ModifiedTime { get; set; }

    public void SetCurrencyName(string currencyName)
    {
        CurrencyName = currencyName;
        SetModifiedTime();
    }

    private void SetModifiedTime()
    {
        ModifiedTime = DateTime.Now;
    }
}