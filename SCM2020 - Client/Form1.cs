using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCM2020___Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var urlBase = config.GetSection("API_Access:UrlBase").Value;
            var uriRelative = urlBase + "usuarios/login";
            
            var getToken = new SignIn(uriRelative);


        }
    }
}
