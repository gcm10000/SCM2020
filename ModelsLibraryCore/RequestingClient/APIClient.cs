using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibraryCore.RequestingClient
{
    public static class APIClient
    {
        //http://localhost:52991/api/Vendor
        //CRUD
        //CREATE -> GENERIC POST
        //READ -> GENERIC GET
        //UPDATE -> GENERIC POST
        //DELETE -> INT DELETE
        public static string PostData(string RequestUrl, object ObjectData, AuthenticationHeaderValue authentication = null)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = authentication;

                var objToJson = JsonConvert.SerializeObject(ObjectData);
                HttpResponseMessage respToken = client.PostAsync(RequestUrl, new StringContent(objToJson, Encoding.UTF8, "application/json")).Result;
                string content = respToken.Content.ReadAsStringAsync().Result;
                return content;
            }
        }
        public static string DeleteData(string RequestUrl, AuthenticationHeaderValue authentication = null)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = authentication;

                HttpResponseMessage respToken = client.DeleteAsync(RequestUrl).Result;
                string content = respToken.Content.ReadAsStringAsync().Result;
                return content;
            }
        }
        /// <summary>
        /// Requisição de leitura da API.
        /// </summary>
        /// <typeparam name="T">Modelo de dados em questão.</typeparam>
        /// <returns></returns>
        public static T GetData<T>(string RequestUrl, AuthenticationHeaderValue authentication = null) 
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = authentication;

                    HttpResponseMessage respToken = client.GetAsync(RequestUrl).Result;

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
