using ModelsLibrary;
using Newtonsoft.Json;
using SCM2020___Utility.RequestingClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Authentication;

namespace SCM2020___Utility
{
    class Program
    {
        static Uri uriServer = new Uri("http://192.168.1.30:52991/api/");
        const string urlLogin = "http://192.168.1.30:52991/api/User/Login";
        const string urlAddUser = "http://192.168.1.30:52991/api/User/NewUser";
        const string urlGroups = "http://192.168.1.30:52991/api/Group/";
        const string urlAddGroup = "http://192.168.1.30:52991/api/Group/Add";
        const string urlAddProduct = "http://192.168.1.30:52991/api/GeneralProduct/Add";
        const string urlVendor = "http://192.168.1.30:52991/api/Vendor/Add";


        static void Main(string[] args)
        {
            //CRUD
            //CREATE -> GENERIC POST V
            //READ -> GENERIC GET V
            //UPDATE -> GENERIC POST V
            //DELETE -> INT DELETE

            SignUpAdministrator();
            var start = Start();

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


            RegisterVendors(start);
            AddGroup(start);
            AddSector(start);
            SignUpSCMEmployees();
            SignUpAll();
            AddProduct(start);
            //AddMonitoring(start);
            //AddInputByVendor(start);
            //AddOutput(start);
            //AddInput(start);

            //var result = client1.DELETEData();
            //Console.WriteLine(result);
            //
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
        static void AddSector(AuthenticationHeaderValue Authentication)
        {
            //Sector sector = new Sector()
            //{
                
            //}
            //Console.WriteLine(APIClient.POSTData(new Uri(uriServer, new Uri("/api/Monitoring/Add")), monitoring, Authentication));

        }
        static void AddMonitoring(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Monitoramento");
            var records = dbAccess.GetDataFromTable();
            
            SCMAccess dbAccess2 = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Saida");
            var records2 = dbAccess2.GetDataFromTable();

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
                monitoring.EmployeeId = oldMonitoring.First(x => x.Key.ToLower() == "funcionario").Value.Trim();
                monitoring.Situation = oldMonitoring.First(x => x.Key.ToLower() == "situação").Value == "FECHADA";
                //monitoring.RequestingSector = int.Parse(oldMonitoring.First(x => x.Key.ToLower() == "tipo de saida").Value);
                foreach (var oldOutput in records2)
                {
                    if (oldOutput.Any(x => (x.Key.ToLower() == "ordem de seriço") && (x.Value == monitoring.Work_Order)))
                    {
                        monitoring.RequestingSector = int.Parse(oldOutput.First(x => x.Key.ToLower() == "tipo de saida").Value);
                    }
                }
                lMonitoring.Add(monitoring);
                //Console.WriteLine(APIClient.POSTData(new Uri(uriServer, "Vendor/Add/"), vendor, Authentication));

                Console.WriteLine(APIClient.POSTData(new Uri(uriServer, "Monitoring/Migrate/"), monitoring, Authentication));
            }
        }
        static void AddInputByVendor(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "EntradaNovo");
            var records = dbAccess.GetDataFromTable();

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
                    var register = oldInputByVendor.First(x => x.Key.ToLower() == "matricula do almo").Value;
                    var resultSCMId = APIClient.GETData<string>(new Uri(uriServer, $"User/UserId/{register}"), Authentication);
                    materialInputByVendor.SCMEmployeeId = resultSCMId;
                    var vendor = oldInputByVendor.First(x => x.Key.ToLower() == "nome").Value;
                    string strResultVendor = null;
                    Vendor _vendor = null;

                    try
                    {
                        //OK
                        strResultVendor = APIClient.POSTData(new Uri(uriServer, $"Vendor/Name/"), vendor, Authentication);
                        _vendor = JsonConvert.DeserializeObject<Vendor>(strResultVendor);

                    }
                    catch
                    {
                        Vendor newVendor = new Vendor();
                        newVendor.Name = vendor;
                        newVendor.Telephone = string.Empty;
                        var resultPOST = APIClient.POSTData(new Uri(uriServer, $"Vendor/Add/"), newVendor, Authentication);
                        Console.WriteLine(resultPOST);
                        strResultVendor = APIClient.POSTData(new Uri(uriServer, $"Vendor/Name/"), newVendor.Name, Authentication);
                        _vendor = JsonConvert.DeserializeObject<Vendor>(strResultVendor);

                    }
                    //Vendor _vendor = JsonConvert.DeserializeObject<Vendor>(strResultVendor);
                    materialInputByVendor.VendorId = _vendor.Id;

                    try
                    {
                        var auxiliarConsumption = new AuxiliarConsumption();
                        var code = int.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "codigo").Value);
                        var resultProduct = APIClient.GETData<ConsumptionProduct>(new Uri(uriServer, $"GeneralProduct/Code/{code}"), Authentication);
                        var resultProductId = resultProduct.Id;
                        auxiliarConsumption.ProductId = resultProductId;
                        auxiliarConsumption.Date = DateTime.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "data da movimentação").Value);
                        auxiliarConsumption.Quantity = int.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "qtd").Value);
                        auxiliarConsumption.SCMEmployeeId = resultSCMId;
                        materialInputByVendor.AuxiliarConsumptions.Add(auxiliarConsumption);

                        InputByVendors.Add(materialInputByVendor);
                    }
                    catch (System.Net.Http.HttpRequestException ex)
                    {
                        string error = JsonConvert.SerializeObject(new
                        {
                            Message = ex.Message,
                            Invoice = materialInputByVendor.Invoice,
                        });
                        string fullName = Path.Combine(Directory.GetCurrentDirectory(), $"log-{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.txt");
                        File.WriteAllText(fullName, error);
                    }
                    
                }
                else
                {
                    MaterialInputByVendor materialInputByVendor = InputByVendors.First(x => x.Invoice == NF);

                    var auxiliarConsumption = new AuxiliarConsumption();
                    var code = int.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "codigo").Value);
                    ConsumptionProduct resultProduct = null;

                    try
                    {
                        resultProduct = APIClient.GETData<ConsumptionProduct>(new Uri(uriServer, $"GeneralProduct/Code/{code}"), Authentication);
                        var resultProductId = resultProduct.Id;
                        auxiliarConsumption.ProductId = resultProductId;
                        auxiliarConsumption.Date = DateTime.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "data da movimentação").Value);
                        auxiliarConsumption.Quantity = double.Parse(oldInputByVendor.First(x => x.Key.ToLower() == "qtd").Value);
                        var register = oldInputByVendor.First(x => x.Key.ToLower() == "matricula do almo").Value;
                        var resultSCMId = APIClient.GETData<string>(new Uri(uriServer, $"User/UserId/{register}"), Authentication);
                        materialInputByVendor.SCMEmployeeId = resultSCMId;

                        materialInputByVendor.AuxiliarConsumptions.Add(auxiliarConsumption);
                    }
                    catch (System.Net.Http.HttpRequestException ex)
                    {
                        string error = JsonConvert.SerializeObject(new 
                        { 
                            Message = ex.Message,
                            Invoice = materialInputByVendor.Invoice,
                        });
                        string fullName = Path.Combine(Directory.GetCurrentDirectory(), $"MATERIALINPUTBYVENDORLOG-{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.txt");
                        File.WriteAllText(fullName, error);
                    }


                }
            }
            foreach (var item in InputByVendors)
            {
                Console.WriteLine(APIClient.POSTData(new Uri(uriServer, "/api/Input/Migrate"), item, Authentication));
            }
        }
        static void AddOutput(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Saida");
            var records = dbAccess.GetDataFromTable();
            List<MaterialOutput> materialOutputs = new List<MaterialOutput>();
            //MATRICULA DO ALMO -> TROCAR
            //a ideia é que confira na ordem de serviço 
            foreach (var oldMaterialOutput in records)
            {
                var WorkOrder = oldMaterialOutput.First(x => x.Key.ToLower() == "ordem de seriço").Value;
                var code = int.Parse(oldMaterialOutput.First(x => x.Key.ToLower() == "codigo").Value);
                ConsumptionProduct resultProduct = null;
                try
                {
                    resultProduct = APIClient.GETData<ConsumptionProduct>(new Uri(uriServer, $"GeneralProduct/Code/{code}"), Authentication);



                    var resultProductId = resultProduct.Id;

                    var SCMEmployeeRegistration = oldMaterialOutput.First(x => x.Key.ToLower() == "matricula do almo").Value;
                    var resultSCMId = APIClient.GETData<string>(new Uri(uriServer, $"User/UserId/{SCMEmployeeRegistration}"), Authentication);
                    var EmployeeRegistration = oldMaterialOutput.First(x => x.Key.ToLower() == "matricula").Value;
                    var resultEmployeeId = APIClient.GETData<string>(new Uri(uriServer, $"User/UserId/{SCMEmployeeRegistration}"), Authentication);

                    if (!materialOutputs.Any(x => x.WorkOrder == WorkOrder))
                    {
                        MaterialOutput materialOutput = new MaterialOutput()
                        {
                            MovingDate = DateTime.Parse(oldMaterialOutput.First(x => x.Key.ToLower() == "data da movimentação").Value),
                            //SCMEmployeeId = resultSCMId,
                            //EmployeeId = resultEmployeeId,
                            ServiceLocation = oldMaterialOutput.First(x => x.Key.ToLower() == "local do seriço").Value,
                            WorkOrder = oldMaterialOutput.First(x => x.Key.ToLower() == "ordem de seriço").Value,
                            ConsumptionProducts = new List<AuxiliarConsumption>()
                        {
                            new AuxiliarConsumption()
                            {
                                Date = DateTime.Parse(oldMaterialOutput.First(x => x.Key.ToLower() == "mov data").Value),
                                Quantity = double.Parse(oldMaterialOutput.First(x => x.Key.ToLower() == "qtd").Value),
                                SCMEmployeeId = resultSCMId,
                                ProductId = resultProductId
                            }
                        }
                        };
                        materialOutputs.Add(materialOutput);
                        Console.WriteLine($"Total de movimentação de saída resgatadas: {materialOutputs.Count}");
                    }
                    else
                    {
                        MaterialOutput materialOutput = materialOutputs.First(x => x.WorkOrder == WorkOrder);
                        var auxiliarConsumption = new AuxiliarConsumption();
                        auxiliarConsumption.ProductId = resultProductId;
                        auxiliarConsumption.Date = DateTime.Parse(oldMaterialOutput.First(x => x.Key.ToLower() == "data da movimentação").Value);
                        auxiliarConsumption.Quantity = double.Parse(oldMaterialOutput.First(x => x.Key.ToLower() == "qtd").Value);
                        auxiliarConsumption.SCMEmployeeId = resultSCMId;

                        materialOutput.ConsumptionProducts.Add(auxiliarConsumption);
                    }
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    string error = JsonConvert.SerializeObject(new
                    {
                        Message = ex.Message,
                        WorkOrder = oldMaterialOutput.First(x => x.Key.ToLower() == "ordem de seriço"),
                    });
                    string fullName = Path.Combine(Directory.GetCurrentDirectory(), $"MATERIALOUTPUTLOG-{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.txt");
                    File.WriteAllText(fullName, error);
                }
            }

            foreach (var item in materialOutputs)
            {
                Console.WriteLine(APIClient.POSTData(new Uri(uriServer, "/api/Output/Migrate"), item, Authentication));
            }
        }
        static void AddInput(AuthenticationHeaderValue Authentication)
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "Entrada");
            var records = dbAccess.GetDataFromTable();
            //MATRICULA DO ALMO -> TROCAR
            //a ideia é que confira na ordem de serviço 
            List<MaterialInput> materialInputs = new List<MaterialInput>();

            foreach (var oldMaterialInput in records)
            {
                var workOrder = oldMaterialInput.First(x => x.Key.ToLower() == "ordem de seriço").Value;

                var SCMregister = oldMaterialInput.First(x => x.Key.ToLower() == "fun setor").Value;
                var resultSCMId = APIClient.GETData<string>(new Uri(uriServer, $"User/UserId/{SCMregister}"), Authentication);
                var code = int.Parse(oldMaterialInput.First(x => x.Key.ToLower() == "codigo").Value);

                if (!materialInputs.Any(x => x.WorkOrder == workOrder))
                {
                    var register = oldMaterialInput.First(x => x.Key.ToLower() == "resp os").Value;
                    var resultEmployeeId = APIClient.GETData<string>(new Uri(uriServer, $"User/UserId/{register}"), Authentication);

                    var dbRegarding = oldMaterialInput.First(x => x.Key.ToLower() == "referente a").Value;
                    Regarding regarding = Regarding.NotUsed;
                    if (dbRegarding == "Transferência Interna")
                    {
                        regarding = Regarding.InternalTransfer;
                    }
                    else if (dbRegarding == "Outra Comarca")
                    {
                        regarding = Regarding.AnotherCounty;
                    }

                    try
                    {

                        var resultProduct = APIClient.GETData<ConsumptionProduct>(new Uri(uriServer, $"GeneralProduct/Code/{code}"), Authentication);
                        var resultProductId = resultProduct.Id;
                        MaterialInput materialInput = new MaterialInput()
                        {
                            MovingDate = DateTime.Parse(oldMaterialInput.First(x => x.Key.ToLower() == "data da movimentação").Value),
                            WorkOrder = oldMaterialInput.First(x => x.Key.ToLower() == "ordem de seriço").Value,
                            SCMEmployeeId = resultSCMId,
                            EmployeeId = resultEmployeeId,
                            Regarding = regarding,
                            ConsumptionProducts = new List<AuxiliarConsumption>()
                        {
                            new AuxiliarConsumption()
                            {
                                Date = DateTime.Parse(oldMaterialInput.First(x => x.Key.ToLower() == "data da movimentação").Value),
                                ProductId = resultProductId,
                                Quantity = double.Parse(oldMaterialInput.First(x => x.Key.ToLower() == "qtd").Value),
                                SCMEmployeeId = resultSCMId,
                            }
                        }
                        };
                        materialInputs.Add(materialInput);
                        Console.WriteLine($"Total de movimentação de entrada resgatadas: {materialInputs.Count}");
                    }
                    catch (System.Net.Http.HttpRequestException ex)
                    {
                        string error = JsonConvert.SerializeObject(new
                        {
                            Message = ex.Message,
                            WorkOrder = oldMaterialInput.First(x => x.Key.ToLower() == "ordem de seriço"),
                        });
                        string fullName = Path.Combine(Directory.GetCurrentDirectory(), $"MATERIALINPUTLOG-{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.txt");
                        File.WriteAllText(fullName, error);
                    }
                }
                else
                {
                    try
                    {
                        MaterialInput materialInput = materialInputs.First(x => x.WorkOrder == workOrder);
                        var auxiliarConsumption = new AuxiliarConsumption();
                        var resultProduct = APIClient.GETData<ConsumptionProduct>(new Uri(uriServer, $"GeneralProduct/Code/{code}"), Authentication);
                        var resultProductId = resultProduct.Id;
                        auxiliarConsumption.ProductId = resultProductId;
                        auxiliarConsumption.Date = DateTime.Parse(oldMaterialInput.First(x => x.Key.ToLower() == "data da movimentação").Value);
                        auxiliarConsumption.Quantity = double.Parse(oldMaterialInput.First(x => x.Key.ToLower() == "qtd").Value);
                        auxiliarConsumption.SCMEmployeeId = resultSCMId;
                        materialInput.ConsumptionProducts.Add(auxiliarConsumption);
                    }
                    catch (System.Net.Http.HttpRequestException ex)
                    {
                        string error = JsonConvert.SerializeObject(new
                        {
                            Message = ex.Message,
                            WorkOrder = oldMaterialInput.First(x => x.Key.ToLower() == "ordem de seriço"),
                        });
                        string fullName = Path.Combine(Directory.GetCurrentDirectory(), $"MATERIALINPUTLOG-{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.txt");
                        File.WriteAllText(fullName, error);
                    }
                }
            }
            foreach (var item in materialInputs)
            {
                Console.WriteLine(APIClient.POSTData(new Uri(uriServer, "Devolution/Migrate"), item, Authentication));
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
                    Console.WriteLine(APIClient.POSTData(new Uri(uriServer, "Vendor/Add/"), vendor, Authentication));
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine($"Total de {records.Count} registros.");

        }
        static void SignUpSCMEmployees()
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                TableName: "[Func do almox]");
            var records = dbAccess.GetDataFromTable();
            foreach (var employees in records)
            {
                var signUp = new SignUpUserInfo()
                {
                    IsPJERJRegistration = true,
                    Password = "@Tj_123456",
                    CPFRegistration = null,
                    Occupation = "Técnico",
                    Role = "SCM"
                };

                foreach (var row in employees)
                {
                    Console.WriteLine($"{row.Key}: {row.Value}");
                    if (row.Key == "Matricula")
                    {
                        signUp.PJERJRegistration = row.Value;
                    }
                    else if (row.Key == "Nonfunalm")
                    {
                        signUp.Name = row.Value.Trim();
                    }
                }
                try
                {
                    SignUp(signUp);
                }
                catch (AuthenticationException)
                {
                    //NOMES NÃO CADASTRADOS PORQUE TEM MATRÍCULAS REPETIDAS!
                    Console.WriteLine($"A matricula {signUp.PJERJRegistration} já se encontra cadastrada.");
                    signUp.PJERJRegistration = "9" + signUp.PJERJRegistration;
                }
            }
            Console.WriteLine($"Total de {records.Count} funcionários.");
        }
        static void SignUpAll()
        {
            SCMAccess dbAccess = new SCMAccess(
                ConnectionString: SCMAccess.ConnectionString,
                SelectCommand: "SELECT * FROM Funcionario ORDER BY Matricula DESC", true);
            var records = dbAccess.GetDataFromTable();
            foreach (var employees in records)
            {
                var signUp = new SignUpUserInfo()
                {
                    IsPJERJRegistration = true,
                    Password = "@Tj_123456",
                    CPFRegistration = null,
                    Occupation = "Técnico",
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
                        signUp.Name = row.Value.Trim();
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

            foreach (var employees in records)
            {
                var nameEmployee = employees.First(x => x.Key == "Funcionario").Value.Trim();
                var registrationEmployee = employees.First(x => x.Key == "Matricula").Value;
                if (!CheckIfExistsName(null, nameEmployee))
                {
                    Console.WriteLine($"O nome {nameEmployee} será cadastrado.");
                    var signUp = new SignUpUserInfo()
                    {
                        IsPJERJRegistration = true,
                        Password = "@Tj_123456",
                        CPFRegistration = null,
                        Occupation = "Técnico",
                        Role = "DETEL",
                        Name = nameEmployee,
                        PJERJRegistration = "9" + registrationEmployee
                    };
                    SignUp(signUp);
                    Console.WriteLine($"O nome {nameEmployee} foi cadastrado.");
                }

            }
                Console.WriteLine($"Total de {records.Count} funcionários.");
        }
        static bool CheckIfExistsName(AuthenticationHeaderValue Authentication, string name)
        {
            var result = APIClient.POSTData(new Uri(uriServer, "User/ExistsName/"), name, Authentication);
            return bool.Parse(result);
        }
        static void SignUpAdministrator()
        {
            Sector sector = new Sector() 
            {
                NameSector = "Administrador",
                NumberSector = 100
            };
            APIClient.POSTData(new Uri(uriServer, "sector"), sector, null);
            var signUp = new SignUpUserInfo()
            {
                IsPJERJRegistration = true,
                CPFRegistration = null,
                Occupation = "Desenvolvedor",
                //O PRIMEIRO "SETOR" SERÁ ADMINISTRADOR
                IdSector = 1,
                Name = "Gabriel Machado",
                PJERJRegistration = "59450",
                Password = "SenhaSecreta#2020",
            };
            try
            {
                SignUp(signUp);
            }
            catch (AuthenticationException)
            {
                Console.WriteLine($"A matricula {signUp.PJERJRegistration} já se encontra cadastrada.");
            }
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
