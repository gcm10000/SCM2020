using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client
{
    public class SummaryInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public PackIconKind Icon { get; set; }
        public SummaryInfo(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
        public SummaryInfo(string Key, string Value, PackIconKind Icon)
        {
            this.Key = Key;
            this.Value = Value;
            this.Icon = Icon;
        }
    }
}
