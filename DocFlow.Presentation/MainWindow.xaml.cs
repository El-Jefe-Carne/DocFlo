using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using DocFlow.Core.Domain;
using DocFlow.Core.Domain.Enums;
using Services.Interfaces;
using DocFlow.Core.Pattern;
using DocFlow.Core.Pattern.Oberserver;
using DocFlow.Services.Interfaces;
using DocFlow.Services.Services;
using DocFlow.Presentation.UserControls;

namespace DocFlow.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver  
    {

        #region fields                
        ISubject _subject;
        #endregion                

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
             
            //observer pattern setup
            _subject = uDirectoryFileList;
            _subject.addObserver(this);            

            wfhAdobeReader.Child = new AdobeViewer(uDirectoryFileList.SelectedFilePath);                                                  
        }
        #endregion

        #region Observer Behavior

        public void Update(ISubject subject, object args)
        {
            if (subject != null)
            {
                var fileListView = (DirectoryFileList)subject;

                wfhAdobeReader.Child = new AdobeViewer(fileListView.SelectedFilePath);                
            }
        }
       
        #endregion

       
    }
}
