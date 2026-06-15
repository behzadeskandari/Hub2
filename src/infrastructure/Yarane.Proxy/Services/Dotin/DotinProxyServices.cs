using CleanLog;
using Yarane.Common.ReturnResults;
using Yarane.Contract.Interfaces.ProxyServices.Dotin;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using Yarane.Contract.Dtos;
using Yarane.Contract.Dtos.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Yarane.Proxy.Services.Dotin.Dtos;
using Yarane.Proxy.Services.Dotin.Consts;

namespace Yarane.Proxy.Services.Dotin;

public class DotinProxyServices : IDotinProxyServices
{

    #region constructor

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Stopwatch _stopwatch;
    private readonly ICleanLogger _cleanLogger;
    //developer 
    //private string apiKey = "dc7694d6-2d0a-4e90-938e-60d41908a197";
    //production
    //private string apiKey = "55510a9c-5595-858e-d3c3-0389b1515755";

    private string apiVersion = "1";
    private string error = "سرویس دهنده مرکزی قطع می باشد";
    private readonly string apiKey;


    public DotinProxyServices(IHttpClientFactory httpClientFactory,
                         Stopwatch stopwatch
                         , ICleanLogger cleanLogger
                         , IConfiguration configuration
        )
    {
        _httpClientFactory = httpClientFactory;
        _stopwatch = stopwatch;
        _cleanLogger = cleanLogger;

        bool isProduction = bool.TryParse(configuration["PublishConfig:IsProduction"], out var parsedValue) && parsedValue;
        string configSection = isProduction ? "ConnectigConfigProduction" : "ConnectigConfigDevelopment";
        apiKey = configuration[configSection + ":ApiKey"];

    }

    #endregion



    public async Task<ProxyResult<SendSmsResponse>> SendSmsByQueue(SendSmsRequest request)
    {
        _stopwatch.Start();
        string gatewayTraceId = "";
        var inputProxy = request;

        var client = _httpClientFactory.CreateClient("dotin");
        client.DefaultRequestHeaders.TryAddWithoutValidation("apiKey", apiKey);
        client.DefaultRequestHeaders.TryAddWithoutValidation("apiVersion", apiVersion);
        var proxyRequest = new HttpRequestMessage(HttpMethod.Post, ApiAddresses.SmsLink);
        proxyRequest.Content = new StringContent(JsonConvert.SerializeObject(inputProxy), Encoding.UTF8, "application/json");
        var proxyResponse = new HttpResponseMessage();

        try
        {
            proxyResponse = await client.SendAsync(proxyRequest);
            gatewayTraceId = proxyResponse.Headers.GetValues("X-Request-Id").FirstOrDefault() ?? string.Empty;
        }
        catch (Exception ex)
        {
            await _cleanLogger.ProxyLog("", "SendSmsByQueue", "GET", request.ToString(), error + ex.ToString(), "");
            return new ProxyResult<SendSmsResponse>(false, 1038, new SendSmsResponse(), "سرویس قطع می باشد");
        }

        var proxyResponseString = await proxyResponse.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<SendSmsProxyResponse>(proxyResponseString);
        _stopwatch.Stop();
        if (proxyResponse.IsSuccessStatusCode)
        {
            if (result.IsSuccess)
            {
                await _cleanLogger.ProxyLog(gatewayTraceId, "SendSmsByQueue success", "POST", inputProxy.ToString(), result.Message, _stopwatch.ElapsedMilliseconds.ToString());
                return new ProxyResult<SendSmsResponse>(true, result.RsCode, null, result.Message);
            }
            else
            {
                await _cleanLogger.ProxyLog(gatewayTraceId, "SendSmsByQueue fail", "POST", inputProxy.ToString(), result.Message, _stopwatch.ElapsedMilliseconds.ToString());
                return new ProxyResult<SendSmsResponse>(false, result.RsCode, null, result.Message);
            }
        }
        else
        {
            await _cleanLogger.ProxyLog(gatewayTraceId, "SendSmsByQueue fail", "POST", inputProxy.ToString(), "ارسال پیامک با خطا مواجه شد", _stopwatch.ElapsedMilliseconds.ToString());
            return new ProxyResult<SendSmsResponse>(false, Convert.ToInt32(proxyResponse.StatusCode), null, "ارسال پیامک با خطا مواجه شد");
        }

    }

}
