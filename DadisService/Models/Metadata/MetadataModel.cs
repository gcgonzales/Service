using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models.Metadata
{
    public class MetadataModel : Attribute
    {
        private List<ClaseModel> clases;

        public List<ClaseModel> Clases
        {
            get { return clases; }
            set { clases = value; }
        }

        public MetadataModel()
        { clases = new List<ClaseModel>(); }
    }
}