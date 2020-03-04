using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SCM2020___Utility
{
    class SignIn
    {
        public SignIn()
        {
        }
        public HttpRequestHeaders MakeSignIn(string url, string Registration, bool IsPJERJRegistration, string Password)
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
                
                Console.WriteLine("Token recebido:");
                Console.WriteLine(content + "\n");

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
    }
}
