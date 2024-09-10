namespace CoindeskApi.ViewModels;

public class PriceVo
{
    public string UpdateTime { get; set; }

    public List<CurrencyRateVo> Currencies { get; set; }

    public class CurrencyRateVo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Rate { get; set; }
    }
}