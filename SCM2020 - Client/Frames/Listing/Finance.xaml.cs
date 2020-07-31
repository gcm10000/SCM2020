﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCM2020___Client.Frames.Listing
{
    /// <summary>
    /// Lógica interna para Finance.xaml
    /// </summary>
    public partial class Finance : Window
    {
        // Relatório financeiro apresenta os seguintes itens:
        // SKU
        // DESCRIÇÃO
        // DANFE
        // VALOR UNITÁRIO
        // VALOR TOTAL

        // Com os seguintes filtros:
        // PRODUTO: SKU ou DESCRIÇÃO
        // DATA
        public Finance()
        {
            InitializeComponent();
        }
    }
}
