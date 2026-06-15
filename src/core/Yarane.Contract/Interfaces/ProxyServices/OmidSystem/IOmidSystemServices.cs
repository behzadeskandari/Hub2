using Yarane.Common.ReturnResults;
using Yarane.Contract.Dtos;

namespace Yarane.Contract.Interfaces.ProxyServices.OmidSystem
{
    public interface IOmidSystemProxyServices
    {
        Task<ProxyResult<SendOmidSystemSmsResponse>> SendSmsByQueue(List<SendOmidSystemSmsRequest> request);
    }
}