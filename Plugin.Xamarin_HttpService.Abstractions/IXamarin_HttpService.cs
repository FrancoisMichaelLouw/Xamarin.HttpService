using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Plugin.Xamarin_HttpService.Abstractions
{
  /// <summary>
  /// Interface for Xamarin_HttpService
  /// </summary>
  public interface IXamarin_HttpService
  {
        Task<HttpResponseMessage> POSTAsync<T>(string Url, T data, Dictionary<string, string> Headers);
        Task<HttpResponseMessage> GETAsync(string Url, Dictionary<string, string> Headers);
    }
}
