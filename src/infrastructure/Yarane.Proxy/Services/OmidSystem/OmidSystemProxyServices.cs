using CleanLog;
using Microsoft.Extensions.Configuration;
using Yarane.Common.ReturnResults;
using Yarane.Contract.Dtos;
using Yarane.Contract.Interfaces.ProxyServices.OmidSystem;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using Yarane.Proxy.Services.OmidSystem.Consts;
using Yarane.Proxy.Services.OmidSystem.Dtos;

namespace Yarane.Proxy.Services.OmidSystem;

public class OmidSystemProxyServices : IOmidSystemProxyServices
{

    #region constructor

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Stopwatch _stopwatch;
    private readonly ICleanLogger _cleanLogger;

    //developer
    //private string apiKey = "17810a9c-5583-458e-b2c3-0389b1515741";
    //production
    //private string apiKey = "55510a9c-5595-858e-d3c3-0389b1515755";

    private string xApiKey = "maskanSJYjIME0oU8yWyFoKmamHvqF";
    private string apiVersion = "1";
    private string error = "سرویس پیامک امید سیستم قطع می باشد";
    private readonly string apiKey;
    public OmidSystemProxyServices(IHttpClientFactory httpClientFactory,
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


    public async Task<ProxyResult<SendOmidSystemSmsResponse>> SendSmsByQueue(List<SendOmidSystemSmsRequest> request)
    {
        _stopwatch.Start();
        string gatewayTraceId = "";
        var inputProxy = request;

        var client = _httpClientFactory.CreateClient("omidSystem");
        client.DefaultRequestHeaders.TryAddWithoutValidation("apiKey", apiKey);
        client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", xApiKey);
        client.DefaultRequestHeaders.TryAddWithoutValidation("apiVersion", apiVersion);
        var proxyRequest = new HttpRequestMessage(HttpMethod.Post, ApiAddresses.OmidSystemSmsLink);
        proxyRequest.Content = new StringContent(JsonConvert.SerializeObject(inputProxy), Encoding.UTF8, "application/json");
        var proxyResponse = new HttpResponseMessage();

        try
        {
            proxyResponse = await client.SendAsync(proxyRequest);
            gatewayTraceId = proxyResponse.Headers.GetValues("X-Request-Id").FirstOrDefault() ?? string.Empty;
        }
        catch (Exception ex)
        {
            await _cleanLogger.ProxyLog("", "OmidSystemProxyServices:SendSmsByQueue", "GET", request.ToString(), error + ex.ToString(), "");
            return new ProxyResult<SendOmidSystemSmsResponse>(false, 1038, new SendOmidSystemSmsResponse(), "سرویس پیامک امید سیستم قطع می باشد");
        }

        var proxyResponseString = await proxyResponse.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<SendOmidSystemSmsProxyResponse>(proxyResponseString);
        _stopwatch.Stop();
        if (proxyResponse.IsSuccessStatusCode)
        {
            if (result.ResultCode == 100)
            {
                await _cleanLogger.ProxyLog(gatewayTraceId, "OmidSystemProxyServices:SendSmsByQueue success", "POST", inputProxy.ToString(), result.Message, _stopwatch.ElapsedMilliseconds.ToString());
                return GenerateSuccessResponse(result);
            }
            else
            {
                await _cleanLogger.ProxyLog(gatewayTraceId, "OmidSystemProxyServices:SendSmsByQueue fail", "POST", inputProxy.ToString(), result.Message, _stopwatch.ElapsedMilliseconds.ToString());
                return new ProxyResult<SendOmidSystemSmsResponse>(false, result.ResultCode, null, result.Message);
            }
        }
        else
        {
            await _cleanLogger.ProxyLog(gatewayTraceId, "OmidSystemProxyServices:SendSmsByQueue fail", "POST", inputProxy.ToString(), "ارسال پیامک امید سیستم با خطا مواجه شد", _stopwatch.ElapsedMilliseconds.ToString());
            return new ProxyResult<SendOmidSystemSmsResponse>(false, Convert.ToInt32(proxyResponse.StatusCode), null, "ارسال پیامک امید سیستم با خطا مواجه شد");
        }

    }

    private static ProxyResult<SendOmidSystemSmsResponse> GenerateSuccessResponse(SendOmidSystemSmsProxyResponse result)
    {
        var response = new SendOmidSystemSmsResponse();

        response.Message = result.Message;
        response.Succeeded = result.Succeeded;
        response.ResultCode = result.ResultCode;
        response.Data = result.Data;

        return new ProxyResult<SendOmidSystemSmsResponse>(true, result.ResultCode, response, result.Message);
    }


}
