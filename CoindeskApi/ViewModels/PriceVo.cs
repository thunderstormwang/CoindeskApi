namespace CoindeskApi.ViewModels;

public class PriceVo
{
    public string UpdateTime { get; set; }

    public List<CurrencyVo> Currencies { get; set; }

    public class CurrencyVo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Rate { get; set; }
    }
}