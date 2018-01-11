using Plugin.Xamarin_HttpService.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Xamarin_HttpService.Settings;

namespace Plugin.Xamarin_HttpService.Implementation
{
    /// <summary>
    /// Current settings to use for PCL
    /// </summary>
    public class Xamarin_HttpServicePCLImplementation : IXamarin_HttpService
    {
        /// <summary>
        /// Current settings to use for PCL
        /// </summary>
        public async Task<HttpResponseMessage> GETAsync(string Url, Dictionary<string, string> Headers)
        {
            try
            {
                var client = new HttpClient();
                
                client.BaseAddress = new Uri(Startup_Settings.Base_URL);
                if (Headers != null)
                {
                    if (Headers.Keys.Count > 0)
                    {
                        foreach (var item in Headers)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }
                }
                
                var response = await client.GetAsync(Url);

                client.Dispose();
                validateStatusCodes(response);

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Connection lost", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("No data recieved", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Error occured in the operation", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Current settings to use for PCL
        /// </summary>
        public async Task<HttpResponseMessage> POSTAsync<T>(string Url, T data, Dictionary<string, string> Headers)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);
                HttpContent byteContent = null;

                if (data.GetType() == typeof(string))
                {
                    if (data != null)
                    {
                        byteContent = new StringContent(data.ToString());
                    }
                }
                else
                {
                    if (data != null)
                    {
                        var contentobj = JsonConvert.SerializeObject(data);
                        byteContent = new StringContent(contentobj);
                    }
                }
                
                client.BaseAddress = new Uri(Startup_Settings.Base_URL);
                if (Headers != null)
                {
                    if (Headers.Keys.Count > 0)
                    {
                        foreach (var item in Headers)
                        {
                            if (item.Key == "Content-Type")
                            {
                                byteContent.Headers.ContentType.MediaType = item.Value;
                            }
                            else
                            {
                                client.DefaultRequestHeaders.Add(item.Key, item.Value);
                            }
                        }
                    }
                }
                
                var response = await client.PostAsync(Url, byteContent);

                client.Dispose();
                validateStatusCodes(response);

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Connection lost", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("No data recieved", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("No response from server, connectivity issues.", ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception("Operation timed out", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Current settings to use for PCL
        /// </summary>
        public void validateStatusCodes(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    throw new Exception("Bad request");

                case System.Net.HttpStatusCode.Forbidden:
                    throw new Exception("Forbidden");

                case System.Net.HttpStatusCode.GatewayTimeout:
                    throw new Exception("Connection timed out");

                case System.Net.HttpStatusCode.InternalServerError:
                    throw new Exception("Internal server error");

                case System.Net.HttpStatusCode.NoContent:
                    throw new Exception("No Content");

                case System.Net.HttpStatusCode.NotAcceptable:
                    throw new Exception("Not Acceptable");

                case System.Net.HttpStatusCode.NotFound:
                    throw new Exception("Not found");

                case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                    throw new Exception("Proxy authentication required");

                case System.Net.HttpStatusCode.RequestTimeout:
                    throw new Exception("Request timed out");

                case System.Net.HttpStatusCode.ServiceUnavailable:
                    throw new Exception("Service is unavailable");

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new Exception("Not Authorized");

                case System.Net.HttpStatusCode.Unused:
                    throw new Exception("Unused");

                case System.Net.HttpStatusCode.UseProxy:
                    throw new Exception("Proxy should be used");

                default:
                    break;
            }
        }
    }
}
