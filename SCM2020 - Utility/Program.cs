﻿using SCM2020___Utility.RequestingClient;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Authentication;

namespace SCM2020___Utility
{
    class Program
    {
        const string urlLogin = "http://localhost:52991/api/User/Login";
        const string urlAddUser = "http://localhost:52991/api/User/NewUser";
        const string urlGroups = "http://localhost:52991/api/Group/";
        const string urlAddGroup = "http://localhost:52991/api/Group/Add";
        const string urlAddProduct = "http://localhost:52991/api/GeneralProduct/Add";
        const string urlVendor = "http://localhost:52991/api/GeneralProduct/Add";


        static void Main(string[] args)
        {
            //CRUD
            //CREATE -> GENERIC POST V
            //READ -> GENERIC GET V
            //UPDATE -> GENERIC POST V
            //DELETE -> INT DELETE

            var start = Start();
            
            APIClient client = new APIClient(new Uri("http://localhost:52991/api/Monitoring/Add"), start);
            ModelsLibrary.Monitoring monitoring = new ModelsLibrary.Monitoring
            {
                Work_Order = "370484/19",
                Situation = false,
                MovingDate = DateTime.Now,
                EmployeeId = ""
                
            };


            var result = client.POSTData(monitoring);
            Console.WriteLine(result);
            //AddGroup(start);
            //SignUpAll();
            //RegisterVendors(start);
            //AddProduct(start);
            Pause();
        }
        static void AddGroup(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Grupos");
            var records = dbAccess.GetDataFromTable();
            //var lgroup = new List<Group>();
            foreach (var group in records)
            {
                var newgroup = new Group();

                foreach (var cell in group)
                {
                    newgroup.GroupName = cell.Value;
                    Console.WriteLine(cell.Value);
                }
                Group.AddOnServer(urlAddGroup, newgroup);
            }
        }
        static void AddProduct(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
    ConnectionString: SCMAccess.ConnectionString,
    TableName: "Produtos");
            var records = dbAccess.GetDataFromTable();
            var lProducts = new List<ConsumptionProduct>();
            foreach (var product in records)
            {
                var consumptionProduct = new ConsumptionProduct();
                foreach (var row in product)
                {
                    var key = row.Key.ToLower();

                    if (key == "codigo")
                    {
                        consumptionProduct.Code = int.Parse(row.Value);
                    }
                    else if (key == "unidade")
                    {
                        consumptionProduct.Unity = row.Value;
                    }
                    else if (key == "grupo")
                    {
                        consumptionProduct.Group = Group.GetGroup(urlGroups, row.Value);
                    }
                    else if (key == "descricao")
                    {
                        consumptionProduct.Description = row.Value;
                    }
                    else if (key == "qtdfisica")
                    {
                        consumptionProduct.Stock = Convert.ToDouble(row.Value);
                    }
                    else if (key == "qtdminima")
                    {
                        consumptionProduct.MininumStock = Convert.ToDouble(row.Value);
                    }
                    else if (key == "bloco")
                    {
                        consumptionProduct.Localization = row.Value;
                    }
                    else if (key == "npra gav")
                    {
                        consumptionProduct.NumberLocalization = (uint.TryParse(row.Value, out uint result)) ? result : 0;
                    }
                    else if (key == "foto do produto")
                    {
                        consumptionProduct.Photo = null;
                    }
                }
                lProducts.Add(consumptionProduct);
            }
            foreach (var p in lProducts)
            {
                Console.WriteLine($"{p.Code}, {p.Description}, {p.Unity}, {p.Group}, {p.Description}, {p.MininumStock}, {p.Stock}, {p.Localization}, {p.NumberLocalization}");
                Console.WriteLine(p.AddProduct(urlAddProduct, p, Authentication).ToString());
            }
            Console.WriteLine($"\nTotal de {lProducts.Count} produtos.");
            
        }
        static AuthenticationHeaderValue Start()
        {
            //Console.Write("Matrícula: ");
            //var Registration = Console.ReadLine();
            //Console.Write("Matrícula é do Tribunal? ");
            //var IsPJERJRegistration = bool.Parse(Console.ReadLine());
            //Console.Write("Senha: ");
            //var Password = Console.ReadLine();
            var Registration = "59450";
            var IsPJERJRegistration = true;
            var Password = "SenhaSecreta#2020";
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
                    return authentication;
                else
                    return null;
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        static void RegisterVendors(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Fornecedor");
            var records = dbAccess.GetDataFromTable();

            APIClient client = new APIClient(new Uri("http://localhost:52991/api/Vendor/Add"), Authentication);

            foreach (var employees in records)
            {
                var vendor = new Vendor();

                foreach (var row in employees)
                {
                    Console.WriteLine($"{row.Key}: {row.Value}");
                    if (row.Key == "Nome")
                    {
                        vendor.Name = row.Value;
                    }
                    else if (row.Key == "Forntel")
                    {
                        vendor.Telephone = row.Value;
                    }
                }
                try
                {
                    //RegisterVendors newvendor = new RegisterVendors(Authentication);
                    //newvendor.AddVendor("http://localhost:52991/api/Vendor/Add", vendor);
                    client.POSTData(vendor);
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine($"Total de {records.Count} registros.");

        }
        static void SignUpAll()
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Funcionario");
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
                    SignUp(signUp);
                }
                catch (AuthenticationException)
                {
                    Console.WriteLine($"A matricula {signUp.PJERJRegistration} já se encontra cadastrada.");
                }
            }
            Console.WriteLine($"Total de {records.Count} funcionários.");
        }
        static void SignUp(SignUpUserInfo userInfo)
        {
            SignUp signUp = new SignUp();
            signUp.MakeSignUp(urlAddUser, userInfo);
        }
        static void Pause()
        {
            Console.WriteLine("Pressione uma tecla para continuar. . .");
            Console.ReadKey();
        }
    }
}
