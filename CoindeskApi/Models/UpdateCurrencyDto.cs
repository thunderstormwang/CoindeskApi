namespace CoindeskApi.Models;

public class UpdateCurrencyDto
{
    /// <summary>
    /// 幣別代號
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 幣別名稱
    /// </summary>
    public string CurrencyName { get; set; }
}