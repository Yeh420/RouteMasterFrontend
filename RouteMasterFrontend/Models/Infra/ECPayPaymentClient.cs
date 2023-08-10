using RouteMasterFrontend.EFModels;
using System.Text;

namespace RouteMasterFrontend.Models.Infra
{
    public class ECPayPaymentClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _hashKey;
        private readonly string _hashIV;
        private readonly string _merchantID;

        public ECPayPaymentClient(string apiUrl, string hashKey, string hashIV, string merchantID)
        {
            _httpClient = new HttpClient();
            _apiUrl = apiUrl;
            _hashKey = hashKey;
            _hashIV = hashIV;
            _merchantID = merchantID;
        }

        public async Task<string> CallECPayApiAsync(Order order)
        {
            var requestData = new
            {
                MerchantID = _merchantID,
                MerchantTradeNo = order.Id.ToString(),
                MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                TotalAmount = order.Total,
                TradeDesc = "交易描述",
                ItemName = "商品名稱",
                ItemPrice = order.Total,
                ItemURL = "商品網址",
                ReturnURL = "YourReturnURL",
                ClientBackURL = "YourClientBackURL",

            };

            // 計算 CheckMacValue
            var checkMacValue = CalculateCheckMacValue(requestData);

            // 組合 POST 資料
            var postData = $"MerchantID={_merchantID}&CheckMacValue={checkMacValue}&...";

            // 建立 HTTP POST 請求
            var content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
            var formData = new FormUrlEncodedContent(new Dictionary<string, string>
    {
                { "MerchantID", _merchantID },
                { "MerchantTradeNo", requestData.MerchantTradeNo },
                { "MerchantTradeDate", requestData.MerchantTradeDate },
                { "TotalAmount", requestData.TotalAmount.ToString() },
                { "TradeDesc", requestData.TradeDesc },
                { "ItemName", requestData.ItemName },
                { "ReturnURL", requestData.ReturnURL },
                { "CheckMacValue", checkMacValue },
        // 其他參數...
    });
            var response = await _httpClient.PostAsync(_apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }

            return null;
        }

        

        private string CalculateCheckMacValue(object requestData)
        {
            // 根據綠界API文件計算 CheckMacValue
            // ...
            return "YourCalculatedCheckMacValue";
        }
    }
}
