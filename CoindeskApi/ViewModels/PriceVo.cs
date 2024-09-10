namespace CoindeskApi.ViewModels;

public class PriceVo
{
    /// <summary>
    /// 更新時間
    /// </summary>
    public string UpdateTime { get; set; }

    /// <summary>
    /// 貨幣資料
    /// </summary>
    public List<CurrencyRateVo> Currencies { get; set; }

    public class CurrencyRateVo
    {
        /// <summary>
        /// 幣別代號
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 幣別名稱
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 匯率
        /// </summary>
        public float Rate { get; set; }
    }
}