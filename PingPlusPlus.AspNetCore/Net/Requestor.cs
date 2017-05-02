using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Pingpp.Exception;
using Pingpp.Models;
using Pingpp.Utils;

namespace Pingpp.Net {
    internal class Requestor: Pingpp {
        internal static HttpClient GetRequest(string sign) {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ApiBase);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            httpClient.DefaultRequestHeaders.Add("Pingplusplus-Version", ApiVersion);
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(AcceptLanguage));
            if(!string.IsNullOrEmpty(sign)) {
                httpClient.DefaultRequestHeaders.Add("Pingplusplus-Signature", sign);
            }
            httpClient.DefaultRequestHeaders.Add("UserAgent", "Pingpp C# SDK version" + Version);

            httpClient.Timeout = TimeSpan.FromMilliseconds(DefaultTimeout);
            return httpClient;
        }

        internal static async Task<string> DoRequest(string path,
            string method,
            Dictionary<string, object> param = null) {
            if(string.IsNullOrEmpty(ApiKey)) {
                throw new PingppException("No API key provided.  (HINT: set your API key using "
                                          + "\"Pingpp::setApiKey(<API-KEY>)\".  You can generate API keys from "
                                          + "the Pingpp web interface.  See https://pingxx.com/document/api for "
                                          + "details.");
            }
            try {
                HttpContent httpContent = null;
                string sign = string.Empty;
                if(method.ToUpper() == "POST" || method.ToUpper() == "PUT") {
                    if(param == null) {
                        throw new PingppException("Request params is empty");
                    }
                    string jsonBody = JsonConvert.SerializeObject(param, Formatting.Indented);

                    try {
                        sign = RsaUtils.RsaSign(jsonBody, PrivateKey);
                    } catch(System.Exception e) {
                        throw new PingppException("Sign request error." + e.Message);
                    }

                    httpContent = new StringContent(jsonBody);
                    httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=utf-8");
                }

                HttpClient req;
                HttpResponseMessage res;
                method = method.ToUpper();
                switch(method) {
                    case "GET":
                        req = GetRequest("");
                        using(res = await req.GetAsync(path)) {
                            return await res.Content.ReadAsStringAsync();
                        }
                    case "DELETE":
                        req = GetRequest("");
                        using(res = await req.DeleteAsync(path)) {
                            return await res.Content.ReadAsStringAsync();
                        }
                    case "POST":
                        req = GetRequest(sign);
                        using(res = await req.PostAsync(path, httpContent)) {
                            return await res.Content.ReadAsStringAsync();
                        }
                    case "PUT":
                        req = GetRequest(sign);
                        using(res = await req.PutAsync(path, httpContent)) {
                            return await res.Content.ReadAsStringAsync();
                        }
                    default: return null;
                }
            } catch(WebException e) {
                if(e.Response == null)
                    throw new WebException(e.Message);
                var statusCode = ((HttpWebResponse)e.Response).StatusCode;
                var errors = Mapper<Error>.MapFromJson(ReadStream(e.Response.GetResponseStream()), "error");

                throw new PingppException(errors, statusCode, errors.ErrorType, errors.Message);
            }
        }

        private static string ReadStream(Stream stream) {
            using(var reader = new StreamReader(stream, Encoding.UTF8)) {
                return reader.ReadToEnd();
            }
        }

        internal static Dictionary<string, string> FormatParams(Dictionary<string, object> param) {
            if(param == null) {
                return new Dictionary<string, string>();
            }
            var formattedParam = new Dictionary<string, string>();
            foreach(var dic in param) {
                var dicts = dic.Value as Dictionary<string, string>;
                if(dicts != null) {
                    var formatNestedDic = new Dictionary<string, object>();
                    foreach(var nestedDict in dicts) {
                        formatNestedDic.Add(string.Format("{0}[{1}]", dic.Key, nestedDict.Key), nestedDict.Value);
                    }

                    foreach(var nestedDict in FormatParams(formatNestedDic)) {
                        formattedParam.Add(nestedDict.Key, nestedDict.Value);
                    }
                } else if(dic.Value is Dictionary<string, object>) {
                    var formatNestedDic = new Dictionary<string, object>();

                    foreach(var nestedDict in (Dictionary<string, object>)dic.Value) {
                        formatNestedDic.Add(string.Format("{0}[{1}]", dic.Key, nestedDict.Key),
                            nestedDict.Value.ToString());
                    }

                    foreach(var nestedDict in FormatParams(formatNestedDic)) {
                        formattedParam.Add(nestedDict.Key, nestedDict.Value);
                    }
                } else if(dic.Value is IList) {
                    var li = (List<object>)dic.Value;
                    var formatNestedDic = new Dictionary<string, object>();
                    var size = li.Count();
                    for(var i = 0; i < size; i++) {
                        formatNestedDic.Add(string.Format("{0}[{1}]", dic.Key, i), li[i]);
                    }
                    foreach(var nestedDict in FormatParams(formatNestedDic)) {
                        formattedParam.Add(nestedDict.Key, nestedDict.Value);
                    }
                } else if("".Equals(dic.Value)) {
                    throw new PingppException(string.Format(
                        "You cannot set '{0}' to an empty string. " + "We interpret empty strings as null in requests. "
                        + "You may set '{0}' to null to delete the property.", dic.Key));
                } else if(dic.Value == null) {
                    formattedParam.Add(dic.Key, "");
                } else {
                    formattedParam.Add(dic.Key, dic.Value.ToString());
                }

            }
            return formattedParam;
        }

        internal static string CreateQuery(Dictionary<string, object> param) {
            var flatParams = FormatParams(param);
            var queryStringBuffer = new StringBuilder();
            foreach(var entry in flatParams) {
                if(queryStringBuffer.Length > 0) {
                    queryStringBuffer.Append("&");
                }

                queryStringBuffer.Append(UrlEncodePair(entry.Key, entry.Value));
            }
            return queryStringBuffer.ToString();
        }

        internal static string UrlEncodePair(string k, string v) {
            return string.Format("{0}={1}", UrlEncode(k), UrlEncode(v));
        }

        private static string UrlEncode(string str) {
            return string.IsNullOrEmpty(str) ? null : Uri.EscapeDataString(str).Replace("%20", "+");
        }

        internal static string FormatUrl(string url, string query) {
            return string.IsNullOrEmpty(query) ? url
                : string.Format("{0}{1}{2}", url, url.Contains("?") ? "&" : "?", query);
        }
    }
}