using Newtonsoft.Json;
using SCM2020___Client.Models;
using System.Net.Http;
using System.Text;

namespace SCM2020___Client
{
    class SignIn
    {
        private string UrlToSignIn { get; set; }
        public SignIn(string uriRelative)
        {
            //Resgata a URL para ter referência para linkar.
            UrlToSignIn = uriRelative;
        }
        public ResultSignIn GetToken(string Registration, string Password)
        {
            using (var client = new HttpClient())
            {
                //Limpa o header
                client.DefaultRequestHeaders.Accept.Clear();
                //Inclui no cabeçalho Accept que será enviado requisição RAW
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //Envio da requisição a fim de autenticar e obter o token de acesso
                var respToken = client.PostAsync(UrlToSignIn, new StringContent(
                                JsonConvert.SerializeObject(new
                                {
                                    registration = Registration,
                                    password = Password
                                }), Encoding.UTF8, "application/json")).Result;

                var content = respToken.Content.ReadAsStringAsync().Result;
                Token token = (respToken.StatusCode == System.Net.HttpStatusCode.OK) ? JsonConvert.DeserializeObject<Token>(content) : null;
                return new ResultSignIn(respToken.StatusCode, token);
            }
        }
    }
}
