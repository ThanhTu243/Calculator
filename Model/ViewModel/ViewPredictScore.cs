using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModel
{
    public class ViewPredictScore
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Province { get; set; }
        public float FromPoint { get; set; }
        public float ToPoint { get; set; }
        public string CodeMajor { get; set; }
        public int GroupMajor { get; set; }
        public string CodeCombination { get; set; }
    }
    public class DetailPredictScore
    {
        public string CodeMajor { get; set; }
        public string NameMajor { get; set; }
        public string CodeCombination { get; set; }
        public string Score { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
    }
}
