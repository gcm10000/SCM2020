using System;
using System.IO;
using System.IO.Packaging;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace SCM2020___Client
{
    static class Helper
    {
        public static Uri Server = new Uri("http://192.168.1.30:52991/api/");
        public static AuthenticationHeaderValue Authentication { get; set; } = null;
        private static readonly Regex NumberRegex = new Regex("[^0-9,]+"); //regex that matches disallowed text
        public static string TemplatePath = @"C:\Users\Gabriel\source\repos\gcm10000\SCM2020\SCM2020 - Client\Templates\"; //File.ReadAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "movement.cshtml"));
        //public static string SCMRegistration { get; set; }
        public static string SCMId { get; set; }
        public static ModelsLibraryCore.Sector CurrentSector { get; set; }
        public static bool IsTextAllowed(string text)
        {
            return !NumberRegex.IsMatch(text);
        }
        public static string GetTempFilePathWithExtension(string extension)
        {
            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + extension;
            return Path.Combine(path, fileName);
        }
        //public static int SaveAsXps(string fileName, string XAML)
        //{
        //    object doc;
        //    //FileInfo fileInfo = new FileInfo(fileName);
        //    //using (FileStream file = fileInfo.OpenRead())
        //    //{
        //    //    System.Windows.Markup.ParserContext context = new System.Windows.Markup.ParserContext();
        //    //    context.BaseUri = new Uri(fileInfo.FullName, UriKind.Absolute);
        //    //    doc = System.Windows.Markup.XamlReader.Load(file, context);
        //    //}

        //    using (var reader = new System.Xml.XmlTextReader(new StringReader(XAML)))
        //    {
        //        doc = System.Windows.Markup.XamlReader.Load(reader);
        //    }

        //    if (!(doc is IDocumentPaginatorSource))
        //    {
        //        throw new NotSupportedException("DocumentPaginatorSource expected");
        //    }
        //    using (Package container = Package.Open(fileName + ".xps", FileMode.Create))
        //    {
        //        using (XpsDocument xpsDoc = new XpsDocument(container, CompressionOption.Maximum))
        //        {
        //            XpsSerializationManager rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);

        //            DocumentPaginator paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
        //            // 8 inch x 6 inch, with half inch margin
        //            paginator = new DocumentPaginatorWrapper(paginator, new Size(768, 676), new Size(48, 48));
        //            rsm.SaveAsXaml(paginator);
        //        }
        //    }

        //    Console.WriteLine("{0} generated.", fileName + ".xps");
        //    return 0;
        //}
    }
}
