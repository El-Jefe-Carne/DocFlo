using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocFlow.Presentation
{
    public partial class AdobeViewer : UserControl
    {
        public AdobeViewer(string fileName)
        {
            InitializeComponent();

            this.acrobatViewer.LoadFile(fileName);
        }
    }
}
