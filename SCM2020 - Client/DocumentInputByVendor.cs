using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Text;
using ModelsLibraryCore;
using System.Windows;
using System.Data.Odbc;
using System.ServiceModel.Channels;

namespace SCM2020___Client
{
    public class DocumentInputByVendor
    {
        public class QueryInputByVendor
        {
            public string Invoice { get; set; }
            public string Vendor { get; set; }
            public string SCMRegistration { get; set; }
            public string SCMEmployee { get; set; }
            public DateTime InvoiceDate { get; set; }
        }
        public class Product
        {
            public int Code { get; set; }
            public string Description { get; set; }
            public double Quantity { get; set; }
            public string Unity { get; set; }
            public DateTime MoveDate { get; set; }
        }
        public class ResultSearch
        {
            public QueryInputByVendor InformationMovement { get; }
            public List<Product> Products { get; }
            public ResultSearch(List<Product> Products, QueryInputByVendor InformationMovement)
            {
                this.Products = Products;
                this.InformationMovement = InformationMovement;
            }
        }
        public List<DocumentMovement.Product> Products { get; }

        public DocumentInputByVendor()
        {
            
        }
        public static ResultSearch Search(string invoice)
        {
            InfoUser InfoUser;
            ModelsLibraryCore.MaterialInputByVendor inputByVendor = null;

            try
            {
                invoice = System.Uri.EscapeDataString(invoice);
                inputByVendor = APIClient.GetData<MaterialInputByVendor>(new Uri(Helper.Server, $"Input/Invoice/{invoice}").ToString(), Helper.Authentication);
            }
            catch
            {
                MessageBox.Show("Não há movimentação com esta nota fiscal.", "Movimentação inexistente", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            try
            {
                InfoUser = APIClient.GetData<InfoUser>(new Uri(Helper.Server, $"user/InfoUser/{inputByVendor.SCMEmployeeId}").ToString(), Helper.Authentication);
            }
            catch
            {
                //HttpRequestException -> BadRequest
                MessageBox.Show("Funcionário não encontrado.", "Funcionário não encontrado", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            //Show data in screen
            QueryInputByVendor InformationMovement = new DocumentInputByVendor.QueryInputByVendor()
            {
                Invoice = inputByVendor.Invoice,
                InvoiceDate = inputByVendor.MovingDate,
                Vendor = APIClient.GetData<ModelsLibraryCore.Vendor>(new Uri(Helper.Server, $"vendor/{inputByVendor.VendorId}").ToString(), Helper.Authentication).Name,
                SCMEmployee = InfoUser.Name,
                SCMRegistration = InfoUser.Register
            };

            //Produtos na nota fiscal
            List<DocumentInputByVendor.Product> ProductsToShow = new List<DocumentInputByVendor.Product>();

            foreach (var item in inputByVendor.AuxiliarConsumptions)
            {
                ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                Product product = new Product()
                {
                    Code = infoProduct.Code,
                    Description = infoProduct.Description,
                    MoveDate = item.Date,
                    Quantity = item.Quantity,
                    Unity = infoProduct.Unity
                };
                ProductsToShow.Add(product);
            }
            return new ResultSearch(ProductsToShow, InformationMovement);
        }
    }
}
