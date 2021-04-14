
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Castle.DynamicProxy;

namespace Nice.RPC
{
    public class HttpRequestInterceptor : IInterceptor
    {
        private readonly HttpClient _httpClient;
        public HttpRequestInterceptor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.Method;
            var requestModel = new RequestModel
            {
                TypeFullName = methodInfo.DeclaringType.FullName,
                MethodName = methodInfo.Name,
                Paramters = invocation.Arguments
            };
            HttpContent httpContent = new StringContent(requestModel.ToJson());
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseMessage = _httpClient.PostAsync("/DotNetCoreRpc/ServerRequest", httpContent).GetAwaiter().GetResult();
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                string result = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrEmpty(result))
                {
                    ResponseModel responseModel = result.FromJson<ResponseModel>();
                    if (responseModel.Code != (int)HttpStatusCode.OK)
                    {
                        throw new Exception($"请求出错,返回内容:{result}");
                    }
                    if (responseModel.Data != null)
                    {
                        invocation.ReturnValue = responseModel.Data.ToJson().FromJson(methodInfo.ReturnType);
                    }
                    return;
                }
            }
            throw new Exception($"请求异常,StatusCode:{responseMessage.StatusCode}");
        }
    }
    public class RequestModel
    {
        public string TypeFullName { get; set; }
        public string MethodName { get; set; }
        public object[] Paramters { get; set; }
    }
    public class ResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}