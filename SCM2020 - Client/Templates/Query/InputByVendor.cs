using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace SCM2020___Client.Templates.Query
{
    public class InputByVendor
    {
        public string Invoice { get; set; }
        public string Vendor { get; set; }
        public string SCMRegistration { get; set; }
        public string SCMEmployee { get; set; }
        public DateTime InvoiceDate { get; set; }
        public class Product
        {
            public int Code { get; set; }
            public string Description { get; set; }
            public double Quantity { get; set; }
            public string Unity { get; set; }
            public string Patrimony { get; set; }
            public DateTime MoveDate { get; set; }
        }
        public List<Product> Products { get; private set; }
        private string Html;
        public InputByVendor(string invoice)
        {
            Search(invoice);

            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "query", "InputByVendor.html");
            Html = System.IO.File.ReadAllText(pathFileHtml);
        }
        private void Search(string invoice)
        {
            ModelsLibraryCore.InfoUser InfoUser;
            ModelsLibraryCore.MaterialInputByVendor inputByVendor = null;

            try
            {
                invoice = System.Uri.EscapeDataString(invoice);
                inputByVendor = APIClient.GetData<MaterialInputByVendor>(new Uri(Helper.ServerAPI, $"Input/Invoice/{invoice}").ToString(), Helper.Authentication);
            }
            catch
            {
                MessageBox.Show("Movimentação com nota fiscal inexistente.", "Movimentação inexistente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                InfoUser = APIClient.GetData<InfoUser>(new Uri(Helper.ServerAPI, $"user/InfoUser/{inputByVendor.SCMEmployeeId}").ToString(), Helper.Authentication);
            }
            catch
            {
                //HttpRequestException -> BadRequest
                MessageBox.Show("Funcionário não encontrado.", "Funcionário não encontrado", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Exibir dados na tela
            Invoice = inputByVendor.Invoice;
            InvoiceDate = inputByVendor.MovingDate;
            Vendor = APIClient.GetData<ModelsLibraryCore.Vendor>(new Uri(Helper.ServerAPI, $"vendor/{inputByVendor.VendorId}").ToString(), Helper.Authentication).Name;
            SCMEmployee = InfoUser.Name;
            SCMRegistration = InfoUser.Register;

            //Produtos na nota fiscal
            Products = new List<Product>();
            foreach (var item in inputByVendor.ConsumptionProducts)
            {
                ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                Product product = new Product()
                {
                    Code = infoProduct.Code,
                    Description = infoProduct.Description,
                    MoveDate = item.Date,
                    Quantity = item.Quantity,
                    Unity = infoProduct.Unity
                };
                this.Products.Add(product);
            }
        }
        public string RenderizeHtml()
        {
            string itemsContent = string.Empty;
            foreach (var product in Products)
            {
                itemsContent += "<tr>" +
                                    $"<td>{product.Code}</td>" +
                                    $"<td>{product.Description}</td>" +
                                    $"<td>{product.Patrimony}</td>" +
                                    $"<td>{product.Quantity}</td>" +
                                    $"<td>{product.Unity}</td>" +
                                    $"<td>{product.MoveDate.ToString("dd/MM/yyyy")}</td>" +
                                "</tr>";
            }

            Html = Html.Replace("@InvoiceDate", InvoiceDate.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@Invoice", Invoice);
            Html = Html.Replace("@Vendor", Vendor);
            Html = Html.Replace("@SCMRegistration", SCMRegistration);
            Html = Html.Replace("@SCMEmployee", SCMEmployee);
            Html = Html.Replace("@LISTOFPRODUCTS", itemsContent);
            Html = Html.Replace("@BootstrapDirectory", new System.Uri(Path.Combine(Helper.CurrentDirectory, "templates", "css", "bootstrap.min.css")).AbsoluteUri);

            return Html.ToString();
        }

    }
}
