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
    public static class APIClient
    {
        //http://localhost:52991/api/Vendor

        public static string POSTData(Uri requestUri, object ObjectData, AuthenticationHeaderValue authentication)
        {

            //CRUD
            //CREATE -> GENERIC POST
            //READ -> GENERIC GET
            //UPDATE -> GENERIC POST
            //DELETE -> INT DELETE
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = authentication;

                var objToJson = JsonConvert.SerializeObject(ObjectData);
                HttpResponseMessage respToken = client.PostAsync(requestUri, new StringContent(objToJson, Encoding.UTF8, "application/json")).Result;
                string content = respToken.Content.ReadAsStringAsync().Result;
                return content;
            }
        }
        public static string DELETEData(Uri requestUri, AuthenticationHeaderValue authentication)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = authentication;

                HttpResponseMessage respToken = client.DeleteAsync(requestUri).Result;
                string content = respToken.Content.ReadAsStringAsync().Result;
                return content;
            }
        }
        /// <summary>
        /// Requisição de leitura da API.
        /// </summary>
        /// <typeparam name="T">Modelo de dados em questão.</typeparam>
        /// <returns></returns>
        public static T GETData<T>(Uri requestUri, AuthenticationHeaderValue authentication) 
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = authentication;

                    HttpResponseMessage respToken = client.GetAsync(requestUri).Result;

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
