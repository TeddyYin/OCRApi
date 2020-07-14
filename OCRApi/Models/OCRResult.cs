using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPOCRSDK_CSharpDLL;

namespace OCRApi.Models
{
    public class OCRResult
    {
        public ushort left { get; set; }
        public ushort top { get; set; }
        public ushort right { get; set; }
        public ushort bottom { get; set; }
        public string RecognizedString { get; set; }
        public List<PPOCRCharResult> PPCharList { get; set; }
    }
}