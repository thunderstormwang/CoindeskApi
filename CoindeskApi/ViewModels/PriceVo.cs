namespace CoindeskApi.ViewModels;

public class PriceVo
{
    public string UpdateTime { get; set; }

    public List<Currency> Currencies { get; set; }

    public class Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Rate { get; set; }
    }
}