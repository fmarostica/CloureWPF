using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Globalization;
using CloureLib.Core;
using System.Threading.Tasks;

namespace CloureLib
{
    public class CloureLib
    {
        private string protocol = "https://";
        private string host = "cloure.com";
        private string appToken = string.Empty;
        private string userToken = string.Empty;
        private string lang = "es";
        private string ApiVersion = "v1";
        private int BuildVersion = 36;
        private List<CloureParam> cparams = new List<CloureParam>();
        private string sendedParams = "";

        /// <summary>
        /// Add a cloure parameter will be executed
        /// </summary>
        /// <param name="Name">The name of the parameter ie: topic</param>
        /// <param name="Value">The value of the parameter ie: login</param>
        public void AddParameter(string Name, object Value)
        {
            CloureParam cloureParam = new CloureParam(Name, Value);
            cparams.Add(cloureParam);
        }

        public async Task<string> ExecuteCommand()
        {
            string Response = "";
            string url_str = protocol + host + "/api/" + ApiVersion+"/";

            MultipartFormDataContent dataContent = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url_str);
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

            if (appToken != null)
            {
                if (appToken.Length == 32)
                    cparams.Add(new CloureParam("app_token", appToken));
            }
            if (userToken != null)
            {
                if (userToken.Length == 32) cparams.Add(new CloureParam("user_token", userToken));
            }

            cparams.Add(new CloureParam("language", lang));
            cparams.Add(new CloureParam("referer", "wpf"));

            foreach (CloureParam param in cparams)
            {
                if (param != null)
                {
                    if (param.value.GetType() == typeof(string))
                    {
                        HttpContent httpContent = new StringContent((string)param.value);
                        dataContent.Add(httpContent, param.name);
                    }
                    else if (param.value.GetType() == typeof(double))
                    {
                        HttpContent httpContent = new StringContent(((double)param.value).ToString("F2", CultureInfo.InvariantCulture));
                        dataContent.Add(httpContent, param.name);
                    }
                    else if (param.value.GetType() == typeof(int))
                    {
                        HttpContent httpContent = new StringContent(((int)param.value).ToString());
                        dataContent.Add(httpContent, param.name);
                    }
                    else if (param.value.GetType() == typeof(CloureImage))
                    {
                        string option = "api_image_object";
                        CloureImage cloureImage = (CloureImage)param.value;
                        HttpContent httpContent = new StringContent("");

                        if (option == "byte")
                        {
                            httpContent = new ByteArrayContent(cloureImage.GetBytes());
                            dataContent.Add(httpContent, param.name, "image");
                        }
                        else if (option == "base64")
                        {
                            string base64String = Convert.ToBase64String(cloureImage.GetBytes());
                            httpContent = new StringContent(base64String);
                            dataContent.Add(httpContent, param.name);
                        }
                        else if (option == "api_image_object")
                        {
                            httpContent = new StringContent(cloureImage.ToJSONString());
                            dataContent.Add(httpContent, param.name);
                        }
                    }
                    else if (param.value.GetType() == typeof(List<CloureImage>))
                    {
                        List<CloureImage> cloureImages = (List<CloureImage>)param.value;
                        string str_content = "[";
                        foreach (CloureImage cloureImage in cloureImages)
                        {
                            str_content += cloureImage.ToJSONString() + ",";
                        }
                        str_content = str_content.TrimEnd(',');
                        str_content += "]";
                        HttpContent httpContent = new StringContent(str_content);
                        dataContent.Add(httpContent, param.name);
                    }
                    else if (param.value.GetType() == typeof(bool))
                    {
                        bool value = (bool)param.value;
                        if (value)
                        {
                            HttpContent httpContent = new StringContent("1");
                            dataContent.Add(httpContent, param.name);
                        }
                        else
                        {
                            HttpContent httpContent = new StringContent("0");
                            dataContent.Add(httpContent, param.name);
                        }

                    }
                }
            }

            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url_str, dataContent);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Response = await httpResponseMessage.Content.ReadAsStringAsync();
            }

            return Response;
        }
        

        
        public async Task<bool> login(string user, string pass)
        {
            bool Response = false;

            cparams.Clear();
            cparams.Add(new CloureParam("topic", "login"));
            cparams.Add(new CloureParam("user", user));
            cparams.Add(new CloureParam("pass", pass));

            string res = await ExecuteCommand();


            return Response;
        }
    }
}
