using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibraryCore.RequestingClient
{
    public class SignIn
    {
        public HttpRequestHeaders Headers { get; private set; }
        public Token Token { get; private set; }
        public JwtSecurityToken JwtSecurityToken { get; private set; }
        public Sector Sector { get; private set; }
        public SignIn(HttpRequestHeaders Headers, Token Token, string url)
        {
            Uri uri = new Uri(url);

            this.Headers = Headers;
            this.Token = Token;
            this.JwtSecurityToken = new JwtSecurityToken(this.Token.token);
            string idSector = JwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + "/api/";
            this.Sector = APIClient.GetData<Sector>(new Uri(new Uri(requested), $"sector/{idSector}").ToString(), Headers.Authorization);
        }
    }
    public static class APIClient
    {

        //http://localhost:52991/api/Vendor
        //CRUD
        //CREATE -> GENERIC POST
        //READ -> GENERIC GET
        //UPDATE -> GENERIC POST
        //DELETE -> INT DELETE
        public static SignIn MakeSignIn(string url, string Register, string Password)
        {
            using (var client = new HttpClient())
            {
                //limpa o header
                client.DefaultRequestHeaders.Accept.Clear();

                //incluir o cabeçalho Accept que será envia na requisição
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Envio da requisição a fim de autenticar
                // e obter o token de acesso
                HttpResponseMessage respToken = client.PostAsync(url, new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            Register = Register,
                            Password = Password
                        }), Encoding.UTF8, "application/json")).Result;

                //obtem o token gerado
                string content = respToken.Content.ReadAsStringAsync().Result;
                Token token = null;

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    //deserializa o token e data de expiração para o objeto Token
                    token = JsonConvert.DeserializeObject<Token>(content);
                    // Associar o token aos headers do objeto
                    // do tipo HttpClient
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
                }
                else if (respToken.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new AuthenticationException(content);
                }
                return new SignIn(client.DefaultRequestHeaders, token, url);
            }
        }
        private static HttpRequestHeaders MakeSignIn2(string url, string Registration, bool IsPJERJRegistration, string Password)
        {
            using (var client = new HttpClient())
            {
                //limpa o header
                client.DefaultRequestHeaders.Accept.Clear();

                //incluir o cabeçalho Accept que será envia na requisição             
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Envio da requisição a fim de autenticar
                // e obter o token de acesso
                HttpResponseMessage respToken = client.PostAsync(url, new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            Registration = Registration,
                            IsPJERJRegistration = IsPJERJRegistration,
                            Password = Password
                        }), Encoding.UTF8, "application/json")).Result;

                //obtem o token gerado
                string content = respToken.Content.ReadAsStringAsync().Result;

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    //deserializa o token e data de expiração para o objeto Token
                    Token token = JsonConvert.DeserializeObject<Token>(content);
                    // Associar o token aos headers do objeto
                    // do tipo HttpClient
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
                }
                else if (respToken.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new AuthenticationException(content);
                }
                return client.DefaultRequestHeaders;
            }
        }

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

        public static ResponseMessage PostData(Uri RequestUri, object ObjectData, AuthenticationHeaderValue authentication = null)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = authentication;

                var objToJson = JsonConvert.SerializeObject(ObjectData);
                HttpResponseMessage respToken = client.PostAsync(RequestUri, new StringContent(objToJson, Encoding.UTF8, "application/json")).Result;
                string content = respToken.Content.ReadAsStringAsync().Result;
                return new ResponseMessage(content, respToken);
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
        public static string GetData(string RequestUrl, AuthenticationHeaderValue authentication = null)
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
                    return content;
                }
                else
                    throw new HttpRequestException($"{respToken.StatusCode}: {respToken.Content}.\n{content}");
            }
        }
    }
}