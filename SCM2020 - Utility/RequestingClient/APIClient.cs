using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SCM2020___Utility.RequestingClient
{
    class APIClient
    {
        string url;
        AuthenticationHeaderValue authentication;
        public APIClient(string url) { this.url = url; }
        public APIClient(string url, AuthenticationHeaderValue authentication) { this.url = url; this.authentication = authentication; }
        private string POSTData(object ObjectData)
        {
            //CRUD
            //CREATE -> GENERIC POST
            //READ -> GENERIC GET
            //UPDATE -> GENERIC POST
            //DELETE -> INT DELETE

            var objToJson = JsonConvert.SerializeObject(vendor);
            var content = new StringContent(objToJson, Encoding.UTF8, "application/json");
            return "";
        }
        /// <summary>
        /// Requisição de leitura da API.
        /// </summary>
        /// <typeparam name="T">Modelo de dados em questão.</typeparam>
        /// <returns></returns>
        public T GETData<T>() 
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = authentication;

                    HttpResponseMessage respToken = client.GetAsync(url).Result;

                    string content = respToken.Content.ReadAsStringAsync().Result;

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        var result = JsonConvert.DeserializeObject<T>(content);
                        return result;
                    }
                    else
                        throw new HttpRequestException($"{respToken.StatusCode}: {respToken.Content}.\n{content}");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
