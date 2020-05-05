using ModelsLibrary;
using SCM2020___Utility.RequestingClient;
using System;
using System.Collections.Generic;
using System.Linq;
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

            //var start = Start();

            //ModelsLibrary.AuxiliarConsumption auxiliar1 = new ModelsLibrary.AuxiliarConsumption()
            //{
            //    Date = DateTime.Now,
            //    ProductId = 2,
            //    Quantity = 3d,
            //};
            //ModelsLibrary.AuxiliarConsumption auxiliar2 = new ModelsLibrary.AuxiliarConsumption()
            //{
            //    Date = DateTime.Now,
            //    ProductId = 41,
            //    Quantity = 21d,
            //};
            //List<ModelsLibrary.AuxiliarConsumption> auxiliarConsumptions = new List<ModelsLibrary.AuxiliarConsumption>()
            //{
            //    auxiliar1,
            //    auxiliar2
            //};
            //ModelsLibrary.MaterialInputByVendor input = new ModelsLibrary.MaterialInputByVendor()
            //{
            //    Invoice = "0271867",
            //    MovingDate = DateTime.Now,
            //    VendorId = 2,

            //    AuxiliarConsumptions = auxiliarConsumptions
            //};

            //ModelsLibrary.Monitoring monitoring = new ModelsLibrary.Monitoring()
            //{
            //    EmployeeId = "c7f5aef6-b8c7-4b73-b815-07d9d9eb7b52",
            //    MovingDate = DateTime.Now,
            //    Situation = false,
            //    Work_Order = "123456/20",
            //};

            //ModelsLibrary.MaterialOutput output = new ModelsLibrary.MaterialOutput()
            //{
            //    WorkOrder = "123456/20",
            //    MovingDate = DateTime.Now,
            //    ServiceLocation = "",
            //    RequestingSector = 2,
            //    EmployeeRegistration = "",
            //    ConsumptionProducts = new List<ModelsLibrary.AuxiliarConsumption>()
            //    {
            //        auxiliar1,
            //        auxiliar2
            //    },
            //    PermanentProducts = new List<ModelsLibrary.AuxiliarPermanent>()
            //    {
            //        new ModelsLibrary.AuxiliarPermanent() { Date = DateTime.Now, ProductId = 2 }
            //    }
            //};

            //APIClient client1 = new APIClient(new Uri("http://localhost:52991/api/Output/Remove/1"),
            //    null);
            //var data = client1.GETData<ModelsLibrary.Monitoring>();
            //data.Work_Order = "TESTE1234";

            //APIClient client2 = new APIClient(new Uri("http://localhost:52991/api/Input/Add"),
            //    null);

            AddMonitoring(null);
            //var result = client1.DELETEData();
            //Console.WriteLine(result);
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
        static void AddMonitoring(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Monitoramento");
            var records = dbAccess.GetDataFromTable();

            var lMonitoring = new List<ModelsLibrary.Monitoring>();
            foreach (var oldMonitoring in records)
            {
                ModelsLibrary.Monitoring monitoring = new ModelsLibrary.Monitoring();
                monitoring.Work_Order = oldMonitoring.First(x => x.Key.ToLower() == "ordem de seriço").Value;
                if (DateTime.TryParse(oldMonitoring.First(x => x.Key.ToLower() == "data da movimentação").Value, out DateTime movingDate))
                    monitoring.MovingDate = movingDate;
                if (DateTime.TryParse(oldMonitoring.First(x => x.Key.ToLower() == "data do fechamento").Value, out DateTime closingDate))
                    monitoring.ClosingDate = closingDate;
                monitoring.SCMEmployeeId = oldMonitoring.First(x => x.Key.ToLower() == "matricula do almo").Value;
                monitoring.EmployeeId = oldMonitoring.First(x => x.Key.ToLower() == "mat do tecnico").Value;
                monitoring.Situation = oldMonitoring.First(x => x.Key.ToLower() == "situação").Value == "FECHADA";

                    //EDITAR
                lMonitoring.Add(monitoring);
            }
        }
        static void AddInputByVendor(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "EntradaNovo");
            var records = dbAccess.GetDataFromTable();

            List<string> NFs = new List<string>();
            List<MaterialInputByVendor> InputByVendors = new List<MaterialInputByVendor>();
            foreach (var oldInputByVendor in records)
            {
                var NF = oldInputByVendor.First(x => x.Key.ToLower() == "nfdoc").Value;
                if (!InputByVendors.Any(x => x.Invoice == NF))
                {
                    MaterialInputByVendor materialInputByVendor = new MaterialInputByVendor();
                    materialInputByVendor.Invoice = NF;
                    materialInputByVendor.MovingDate = DateTime.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "data da movimentação").Value);
                    materialInputByVendor.AuxiliarConsumptions = new List<AuxiliarConsumption>();
                    

                    var auxiliarConsumption = new AuxiliarConsumption();
                    //EDITAR MODELO DE DADOS DE ID PARA CÓDIGO
                    auxiliarConsumption.ProductId = int.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "codigo").Value);
                    auxiliarConsumption.Date = DateTime.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "data da movimentação").Value);
                    auxiliarConsumption.Quantity = int.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "qtd").Value);
                    
                    materialInputByVendor.AuxiliarConsumptions.Add(auxiliarConsumption);

                    InputByVendors.Add(materialInputByVendor);
                }
                else
                {
                    MaterialInputByVendor materialInputByVendor = InputByVendors.First(x => x.Invoice == NF);

                    var auxiliarConsumption = new AuxiliarConsumption();
                    auxiliarConsumption.ProductId = int.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "codigo").Value);
                    auxiliarConsumption.Date = DateTime.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "data da movimentação").Value);
                    auxiliarConsumption.Quantity = int.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "qtd").Value);

                    materialInputByVendor.AuxiliarConsumptions.Add(auxiliarConsumption);

                }
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
