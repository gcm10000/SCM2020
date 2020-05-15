using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;

namespace SCM2020___Utility
{
    public class Group
    {
        public Group() { }
        public static HttpStatusCode AddOnServer(string url, Group group)
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
                        JsonConvert.SerializeObject(group), Encoding.UTF8, "application/json")).Result;

                //obtem o token gerado
                string content = respToken.Content.ReadAsStringAsync().Result;

                if (respToken.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new AuthenticationException(content);
                }
                return respToken.StatusCode;
            }

        }
        public static int GetGroup(string url, string group)
        {
            group = group.ToUpper();
            using (var client = new HttpClient())
            {
                //limpa o header
                client.DefaultRequestHeaders.Accept.Clear();

                // Envio da requisição a fim de autenticar
                // e obter o token de acesso
                HttpResponseMessage respToken = client.GetAsync(url).Result;

                //obtem o token gerado
                string content = respToken.Content.ReadAsStringAsync().Result;

                if (respToken.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new AuthenticationException(content);
                }
                List<Group> g = JsonConvert.DeserializeObject<List<Group>>(content);
                try
                {
                    int id = 0;
                    if (g.Any(x => x.GroupName == group))
                        id = g.FirstOrDefault(x => x.GroupName == group).Id;
                    else
                    {
                        AddOnServer("http://192.168.1.30:52991/api/Group/Add", new Group() { GroupName = group });
                        return GetGroup(url, group);
                    }

                    return id;
                }
                catch (NullReferenceException)
                {
                    AddOnServer("http://192.168.1.30:52991/api/Group/Add", new Group() { GroupName = group});
                    return GetGroup(url, group);
                }
            }

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string GroupName { get; set; }
    }
}
