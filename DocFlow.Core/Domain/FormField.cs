using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFlow.Core.Domain
{
    public class FormField
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string ControlType { get; set; }
        public int Order { get; set; }
        public string Response { get; set; }

        public IEnumerable<string> SubFields { get; set; }
    }
}
