using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using ModelsLibraryCore;
using System.Threading.Tasks;
using System.Linq;
using SCM2020___Client.Models;

namespace SCM2020___Client.Templates.Movement
{
    public class MaterialMovement
    {
        private string WorkOrder { get; set; }
        private string Situation { get; set; }
        private string Sector { get; set; }
        private DateTime WorkOrderDate { get; set; }
        private string RegisterApplication { get; set; }
        private string SolicitationEmployee { get; set; }
        private string ServiceLocalizationTextBox { get; set; }
        private DateTime? ClosingDate { get; set; }

        private string Html;

        List<MaterialMovementPrintExport> Products;
        public MaterialMovement(string workOrder)
        {
            Search(workOrder);
            var pathFileHtml = Path.Combine(Helper.CurrentDirectory, "templates", "Movement", "MaterialMovement.html");
            Html = System.IO.File.ReadAllText(pathFileHtml);
        }
        public void Search(string workOrder)
        {
            string userId = string.Empty;
            Monitoring Monitoring;
            InfoUser InfoUser;
            string Name = string.Empty;
            string Register = string.Empty;


            try
            {
                workOrder = System.Uri.EscapeDataString(workOrder);
                Monitoring = APIClient.GetData<ModelsLibraryCore.Monitoring>(new Uri(Helper.ServerAPI, $"monitoring/workorder/{workOrder}").ToString(), Helper.Authentication);
                userId = Monitoring.EmployeeId;
            }
            catch
            {
                //If doesn't exist work order, then shows error inside MessageBox 
                throw new NullReferenceException("Ordem de serviço inexistente.");
            }
            try
            {
                InfoUser = APIClient.GetData<InfoUser>(new Uri(Helper.ServerAPI, $"user/InfoUser/{userId}").ToString(), Helper.Authentication);
                Name = InfoUser.Name;
                Register = InfoUser.Register;
            }
            catch
            {
                Name = "Desconhecido";
                Register = "S/N";
            }

            //Show data in screen
            this.Situation = (Monitoring.Situation) ? "FECHADA" : "ABERTA";
            this.WorkOrder = Monitoring.Work_Order;
            this.Sector = APIClient.GetData<ModelsLibraryCore.Sector>(new Uri(Helper.ServerAPI, $"sector/{Monitoring.RequestingSector}").ToString(), Helper.Authentication).NameSector;
            this.WorkOrderDate = Monitoring.MovingDate;
            this.RegisterApplication = Register;
            this.SolicitationEmployee = Name;
            this.ServiceLocalizationTextBox = Monitoring.ServiceLocation;
            this.ClosingDate = Monitoring.ClosingDate;


            Products = ProductsAtWorkOrder(workOrder);
        }
        public List<MaterialMovementPrintExport> ProductsAtWorkOrder(string workOrder)
        {

            List<MaterialMovementPrintExport> ProductsToShow = new List<MaterialMovementPrintExport>();

            ModelsLibraryCore.MaterialOutput output = null;
            try
            {
                output = APIClient.GetData<ModelsLibraryCore.MaterialOutput>(new Uri(Helper.ServerAPI, $"output/workorder/{workOrder}").ToString(), Helper.Authentication);
            }
            catch //Doesn't exist output with that workorder
            { }

            ModelsLibraryCore.MaterialInput input = null;
            try
            {
                input = APIClient.GetData<ModelsLibraryCore.MaterialInput>(new Uri(Helper.ServerAPI, $"devolution/workorder/{workOrder}").ToString(), Helper.Authentication);
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
                        ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        MaterialMovementPrintExport product = new MaterialMovementPrintExport()
                        {
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Movement = "SAÍDA",
                            Quantity = item.Quantity,
                            Unity = infoProduct.Unity,
                            MoveDate = item.Date,
                            Patrimony = ""
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
                            ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                            MaterialMovementPrintExport product = new MaterialMovementPrintExport()
                            {
                                Code = infoProduct.Code,
                                Description = infoProduct.Description,
                                Movement = "ENTRADA",
                                Quantity = item.Quantity,
                                Unity = infoProduct.Unity,
                                MoveDate = item.Date,
                                Patrimony = ""
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
                        ModelsLibraryCore.PermanentProduct infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.ServerAPI, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                        MaterialMovementPrintExport product = new MaterialMovementPrintExport()
                        {
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Movement = "SAÍDA",
                            Quantity = 1,
                            Unity = infoProduct.Unity,
                            MoveDate = item.Date,
                            Patrimony = infoPermanentProduct.Patrimony
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

                        ModelsLibraryCore.PermanentProduct infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.ServerAPI, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                        MaterialMovementPrintExport product = new MaterialMovementPrintExport()
                        {
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Movement = "ENTRADA",
                            Quantity = 1,
                            Unity = infoProduct.Unity,
                            MoveDate = item.Date,
                            Patrimony = infoPermanentProduct.Patrimony
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

        public string RenderizeHtml()
        {
            string itemsContent = string.Empty;
            foreach (var product in Products)
            {
                itemsContent += "<tr>" +
                                    $"<td>{product.MoveDate.ToString("dd/MM/yyyy")}</td>" +
                                    $"<td>{product.Code}</td>" +
                                    $"<td>{product.Description}</td>" +
                                    $"<td>{product.Quantity}</td>" +
                                    $"<td>{product.Unity}</td>" +
                                    $"<td>{product.Movement}</td>" +
                                    $"<td>{product.Patrimony}</td>" +
                                "</tr>";
            }

            Html = Html.Replace("@WorkOrderDate", WorkOrderDate.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@WorkOrder", WorkOrder);
            Html = Html.Replace("@Situation", Situation);
            Html = Html.Replace("@RegisterApplicant", RegisterApplication);
            Html = Html.Replace("@NameApplicant", SolicitationEmployee);
            Html = Html.Replace("@Sector", Sector);
            Html = Html.Replace("@LISTOFITEMS", itemsContent);
            Html = Html.Replace("@BootstrapDirectory", new System.Uri(Path.Combine(Helper.CurrentDirectory, "templates", "css", "bootstrap.min.css")).AbsoluteUri);

            return Html.ToString();
        }
    }
}
