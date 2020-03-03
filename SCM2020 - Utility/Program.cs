using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SCM2020___Utility
{
    class Program
    {
        const string urlLogin = "http://localhost:52991/api/User/Login";
        const string urlAddUser = "http://localhost:52991/api/User/NewUser";


        static void Main(string[] args)
        {
            Console.Write("Matrícula: ");
            var Registration = Console.ReadLine();
            Console.Write("Matrícula é do Tribunal? ");
            var IsPJERJRegistration = bool.Parse(Console.ReadLine());
            Console.Write("Senha: ");
            var Password = Console.ReadLine();
            System.Net.Http.Headers.AuthenticationHeaderValue authentication = null;
            try
            {
                SignIn signIn = new SignIn();
                authentication = signIn.MakeSignIn(
                    url: urlLogin,
                    Registration: Registration,
                    IsPJERJRegistration: IsPJERJRegistration,
                    Password: Password
                    ).Authorization;

                //MAKE
                if (authentication != null)
                    Console.WriteLine("Autenticado.");
                else
                    return;
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            RegisterVendors(authentication);
            Pause();
        }
        static void RegisterVendors(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Fornecedor");
            var records = dbAccess.GetDataFromTable();
            foreach (var employees in records)
            {
                var signUp = new Vendor();

                foreach (var row in employees)
                {
                    Console.WriteLine($"{row.Key}: {row.Value}");
                    if (row.Key == "Nome")
                    {
                        signUp.Name = row.Value;
                    }
                    else if (row.Key == "Forntel")
                    {
                        signUp.Telephone = row.Value;
                    }
                }
                try
                {
                    RegisterVendors newvendor = new RegisterVendors(Authentication);
                    newvendor.AddVendor("http://localhost:52991/api/Vendor/Add", signUp);
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine($"Total de {records.Count} registros.");

        }
        static void SignUpAll(System.Net.Http.Headers.AuthenticationHeaderValue authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Fornecedor");
            var records = dbAccess.GetDataFromTable();
            foreach (var employees in records)
            {
                var signUp = new SignUpUserInfo()
                {
                    IsPJERJRegistration = true,
                    Password = "@Tj_123456",
                    CPFRegistration = null,
                    Occupation = "",
                    Role = "DETEL"
                };

                foreach (var row in employees)
                {
                    Console.WriteLine($"{row.Key}: {row.Value}");
                    if (row.Key == "Matricula")
                    {
                        signUp.PJERJRegistration = row.Value;
                    }
                    else if (row.Key == "Funcionario")
                    {
                        signUp.Name = row.Value;
                    }
                }
                try
                {
                    SignUp(authentication, signUp);
                }
                catch (AuthenticationException)
                {
                    Console.WriteLine($"A matricula {signUp.PJERJRegistration} já se encontra cadastrada.");
                }
            }
            Console.WriteLine($"Total de {records.Count} funcionários.");
        }
        static void SignUp(System.Net.Http.Headers.AuthenticationHeaderValue authentication, SignUpUserInfo userInfo)
        {
            SignUp signUp = new SignUp(authentication);
            signUp.MakeSignUp(urlAddUser, userInfo);
        }
        static void Pause()
        {
            Console.WriteLine("Pressione uma tecla para continuar. . .");
            Console.ReadKey();
        }
    }
}
