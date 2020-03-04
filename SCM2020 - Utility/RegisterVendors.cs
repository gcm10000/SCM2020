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
    class RegisterVendors
    {
        AuthenticationHeaderValue Authentication;
        public RegisterVendors(AuthenticationHeaderValue Authentication)
        {
            this.Authentication = Authentication;
        }
        public HttpStatusCode AddVendor(string url, Vendor vendor)
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
                        JsonConvert.SerializeObject(vendor), Encoding.UTF8, "application/json")).Result;

                //obtem o token gerado
                string content = respToken.Content.ReadAsStringAsync().Result;

                if (respToken.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new AuthenticationException(content);
                }
                return respToken.StatusCode;
            }

        }
    }
}
