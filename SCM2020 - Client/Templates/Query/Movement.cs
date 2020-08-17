using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SCM2020___Client.Templates.Query
{
    public class Movement
    {
        public class Product
        {
            public int ProductId { get; set; }
            public int Code { get; set; }
            public string Description { get; set; }
            public double Quantity { get; set; } = 0.00d;
            public string Unity { get; set; }
            public string Patrimony { get; set; }
            public string Movement { get; set; }
            public DateTime MoveDate { get; set; }
        }

        public string WorkOrder;
        public int RegisterApplication;
        public string Application;
        public string Sector;
        public string Situation;
        public string ServiceLocalization;
        public DateTime WorkOrderDate;
        public DateTime? ClosureWorkOrder;

        private ModelsLibraryCore.Monitoring Monitoring = null;
        private ModelsLibraryCore.InfoUser InfoUser = null;

        public List<Product> Products = null;
        private string Html;
        public Movement(string workOrder)
        {
            Search(workOrder);

            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "query", "Movement.html");
            Html = System.IO.File.ReadAllText(pathFileHtml);
        }
        public Movement(string WorkOrder, int RegisterApplication, string Application, string Sector, string Situation, string ServiceLocalization, DateTime WorkOrderDate, DateTime? ClosureWorkOrder, List<Product> Products)
        {
            this.WorkOrder = WorkOrder;
            this.RegisterApplication = RegisterApplication;
            this.Application = Application;
            this.Sector = Sector;
            this.Situation = Situation;
            this.ServiceLocalization = ServiceLocalization;
            this.WorkOrderDate = WorkOrderDate;
            this.ClosureWorkOrder = ClosureWorkOrder;
            this.Products = Products;

            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "query", "Movement.html");
            Html = System.IO.File.ReadAllText(pathFileHtml);
        }
        public string RenderizeHtml()
        {
            string itemsContent = string.Empty;
            foreach (var product in Products)
            {
                itemsContent += "<tr>" +
                                    $"<td>{product.Code}</td>" +
                                    $"<td>{product.Description}</td>" +
                                    $"<td>{product.Quantity}</td>" +
                                    $"<td>{product.Unity}</td>" +
                                    $"<td>{product.Patrimony}</td>" +
                                    $"<td>{product.Movement}</td>" +
                                    $"<td>{product.MoveDate.ToString("dd/MM/yyyy")}</td>" +
                                "</tr>";
            }

            Html = Html.Replace("@WorkOrderDate", WorkOrderDate.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@WorkOrder", WorkOrder); 
            Html = Html.Replace("@RegisterApplication", RegisterApplication.ToString());
            Html = Html.Replace("@Application", Application);
            Html = Html.Replace("@Sector", Sector);
            Html = Html.Replace("@Situation", Situation);
            Html = Html.Replace("@ServiceLocalization", ServiceLocalization);
            Html = Html.Replace("@ClosureDate", (ClosureWorkOrder == null) ? "" : ClosureWorkOrder.Value.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@LISTOFPRODUCTS", itemsContent);
            return Html.ToString();
        }
        public void Search(string workOrder)
        {
            string userId;
            try
            {
                workOrder = System.Uri.EscapeDataString(workOrder);
                Monitoring = APIClient.GetData<ModelsLibraryCore.Monitoring>(new Uri(Helper.Server, $"monitoring/workorder/{workOrder}").ToString(), Helper.Authentication);
                userId = Monitoring.EmployeeId;
            }
            catch
            {
                throw new Exception("Ordem de serviço inexistente.", new NullReferenceException());
            }
            try
            {
                InfoUser = APIClient.GetData<ModelsLibraryCore.InfoUser>(new Uri(Helper.Server, $"user/InfoUser/{userId}").ToString(), Helper.Authentication);
            }
            catch
            {
                //HttpRequestException -> BadRequest
                throw new Exception("Funcionário não encontrado.", new NullReferenceException());
            }


            //Show data in screen

            this.WorkOrder = Monitoring.Work_Order;
            this.RegisterApplication = int.Parse(InfoUser.Register);
            this.Application = InfoUser.Name;
            this.Sector = APIClient.GetData<ModelsLibraryCore.Sector>(new Uri(Helper.Server, $"sector/{Monitoring.RequestingSector}").ToString(), Helper.Authentication).NameSector;
            this.Situation = (Monitoring.Situation) ? "FECHADA" : "ABERTA";
            this.ServiceLocalization = Monitoring.ServiceLocation;
            this.WorkOrderDate = Monitoring.MovingDate;
            this.ClosureWorkOrder = Monitoring.ClosingDate;
            this.Products = ProductsAtWorkOrder(workOrder);

        }
        public List<Product> ProductsAtWorkOrder(string workOrder)
        {

            List<Product> ProductsToShow = new List<Product>();

            ModelsLibraryCore.MaterialOutput output = null;
            try
            {
                output = APIClient.GetData<ModelsLibraryCore.MaterialOutput>(new Uri(Helper.Server, $"output/workorder/{workOrder}").ToString(), Helper.Authentication);
            }
            catch //Doesn't exist output with that workorder
            { }

            ModelsLibraryCore.MaterialInput input = null;
            try
            {
                input = APIClient.GetData<ModelsLibraryCore.MaterialInput>(new Uri(Helper.Server, $"devolution/workorder/{workOrder}").ToString(), Helper.Authentication);
            }
            catch //Doesn't exist input with that workorder
            { }

            ManualResetEvent manualReset1 = new ManualResetEvent(false);
            ManualResetEvent manualReset2 = new ManualResetEvent(false);
            ManualResetEvent manualReset3 = new ManualResetEvent(false);
            ManualResetEvent manualReset4 = new ManualResetEvent(false);

            //CONSUMPTERS
            Task.Run(() =>
            {
                if (output != null)
                {

                    foreach (var item in output.ConsumptionProducts.ToList())
                    {
                        //Task.Run for each
                        ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        Product product = new Product()
                        {
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Movement = "SAÍDA",
                            Quantity = item.Quantity,
                            Unity = infoProduct.Unity,
                            MoveDate = item.Date,
                            Patrimony = "",
                            ProductId = infoProduct.Id
                        };
                        ProductsToShow.Add(product);
                    }
                }
                manualReset1.Set();
            });

            Task.Run(() =>
            {
                if (input != null)
                {
                    foreach (var item in input.ConsumptionProducts.ToList())
                    {
                        //Task.Run for each
                        {
                            ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                            Product product = new Product()
                            {
                                Code = infoProduct.Code,
                                Description = infoProduct.Description,
                                Movement = "ENTRADA",
                                Quantity = item.Quantity,
                                Unity = infoProduct.Unity,
                                MoveDate = item.Date,
                                Patrimony = "",
                                ProductId = infoProduct.Id
                            };
                            ProductsToShow.Add(product);
                        }
                    }
                }
                manualReset2.Set();
            });

            //PERMANENTS
            Task.Run(() =>
            {
                if (output != null)
                {
                    foreach (var item in output.PermanentProducts.ToList())
                    {
                        //Task.Run for each
                        ModelsLibraryCore.PermanentProduct infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.Server, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                        Product product = new Product()
                        {
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Movement = "SAÍDA",
                            Quantity = 1,
                            Unity = infoProduct.Unity,
                            MoveDate = item.Date,
                            Patrimony = infoPermanentProduct.Patrimony,
                            ProductId = infoPermanentProduct.Id,
                        };
                        ProductsToShow.Add(product);
                    }
                }
                manualReset3.Set();
            });

            Task.Run(() =>
            {
                if (input != null)
                {
                    foreach (var item in input.PermanentProducts.ToList())
                    {
                        //Task.Run for each

                        ModelsLibraryCore.PermanentProduct infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.Server, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                        Product product = new Product()
                        {
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Movement = "ENTRADA",
                            Quantity = 1,
                            Unity = infoProduct.Unity,
                            MoveDate = item.Date,
                            Patrimony = infoPermanentProduct.Patrimony,
                            ProductId = infoPermanentProduct.Id
                        };
                        ProductsToShow.Add(product);
                    }
                }
                manualReset4.Set();
            });

            //Esperando todos os sinais
            manualReset1.WaitOne();
            manualReset2.WaitOne();
            manualReset3.WaitOne();
            manualReset4.WaitOne();

            ProductsToShow = ProductsToShow.OrderBy(x => x.MoveDate).ToList();
            return ProductsToShow;
        }

    }
}
