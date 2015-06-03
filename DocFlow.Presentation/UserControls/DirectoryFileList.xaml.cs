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
using DocFlow.Core.Pattern.Oberserver;
using DocFlow.Services.Interfaces;
using DocFlow.Services.Services;
using WPFFolderBrowser;

namespace DocFlow.Presentation.UserControls 
{
    /// <summary>
    /// Interaction logic for DirectoryFileList.xaml
    /// </summary>
    public partial class DirectoryFileList : UserControl, ISubject
    {
        #region Fields
        protected ICollection<IObserver> _observers;
        protected IFileService _fileService;
        protected string _directory = string.Empty;
        public string SelectedFilePath { get { return string.Format("{0}/{1}", _directory, lstFileQueue.SelectedItem); } }
        #endregion        

        #region Methods
        /// <summary>
        /// Refreshes the file list.
        /// </summary>
        private void RefreshFileList()
        {
            //clear the list if there's anything in it so that the
            //previously selected folder's files don't remain in the list
            if (lstFileQueue.Items.Count > 0)
            {
                lstFileQueue.Items.Clear();    
            }            

            //get the list of file names form the directory
            var fileNameList = _fileService.GetFilesFromDirectory(_directory);

            //if there are any
            if (fileNameList.Count() > 0)
            {
                //add each of them to the list
                foreach (var fileName in fileNameList)
                {
                    lstFileQueue.Items.Add(fileName);
                }

                //select the first one
                lstFileQueue.SelectedIndex = 0;
            }            
        }

        /// <summary>
        /// Display a folder browsers for the user to select a directory
        /// </summary>
        private void PromptForDirectory()
        {
            WPFFolderBrowserDialog dialog = new WPFFolderBrowserDialog("Select Directory");
            dialog.ShowHiddenItems = false;
            dialog.ShowPlacesList = true;

            if ((bool)dialog.ShowDialog())
            {
                _directory = dialog.FileName;
                this.RefreshFileList();
                lblSelectedDirectory.Content = _directory;
            }          
        }
        #endregion

        #region Constructor
        public DirectoryFileList()
        {
            InitializeComponent();

            _observers = new List<IObserver>();
            
            //TODO - implement some way of saving last selection and 
            //using that as the default the first time it boots up again
            if (string.IsNullOrEmpty(_directory))
            {
                //this.PromptForDirectory();
 
                _directory = "C:/Users/Jeff/Desktop/New folder";
            }

            lblSelectedDirectory.Content = _directory;

            //assign a new file service
            _fileService = new FileService();

            this.RefreshFileList();
        }
        #endregion       

        #region Events
        /// <summary>
        /// Handles the Click event of the btnBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //if the currently selected item is not the first item in the list
            if (lstFileQueue.SelectedIndex > 0)
            {
                //select the next lowest index
                lstFileQueue.SelectedIndex--;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSkip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            //if the selected index is the index of the last item in the list
            if (lstFileQueue.SelectedIndex != lstFileQueue.Items.Count - 1)
            {
                lstFileQueue.SelectedIndex++;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnChangeDirectory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnChangeDirectory_Click(object sender, RoutedEventArgs e)
        {
            this.PromptForDirectory();
        }
        /// <summary>
        /// Handles the SelectionChanged event of the lstFileQueue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void lstFileQueue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            notifyObservers();
        }
        #endregion

        #region Subject Behavior
        /// <summary>
        /// Adds the observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        public void addObserver(IObserver observer)
        {
            if (_observers.Contains(observer))
                return;

            _observers.Add(observer);
        }

        /// <summary>
        /// Removes the observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        public void removeObserver(IObserver observer)
        {
            if (_observers.Contains(observer))
                _observers.Remove(observer);
        }

        /// <summary>
        /// Notifies the observers.
        /// </summary>
        public void notifyObservers()
        {
            foreach (IObserver o in _observers)
            {
                o.Update(this, null);
            }
        }
        #endregion       
    }
}
