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
using DocFlow.Core.Pattern.Oberserver;
using DocFlow.Services.Interfaces;
using DocFlow.Services.Services;

namespace DocFlow.Presentation.UserControls
{    
    /// <summary>
    /// Interaction logic for FormView.xaml
    /// </summary>
    public partial class FormView : UserControl
    {
        #region Fields
        //make this address relative to the project
        private string FormFieldFielAddress = "G:/Projects/FileTest/DocFlow.Presentation/xml/FormSettings.xml";
        IFileService _fileService;
        ISubject _subject;
        #endregion

        #region Methods

        protected void CreateFieldLabel(FormField field)
        {            
            if (field.ControlType == ControlType.RadioButtonList.ToString())
            {                
                pnlFormFieldContainer.Children.Add(new Label
                {
                    Content = field.Label,
                    Margin = new Thickness(10, 15, 15, 0)//radio buttons have 5 margin o the top so the bottom margin here is not needed
                });
            }
            else
            {
                pnlFormFieldContainer.Children.Add(new Label
                {
                    Content = field.Label,
                    Margin = new Thickness(10, 15, 15, 5)//controls that aren't radios need the margin bottom here
                });
            }
            
        }
        #endregion

        #region Constructor
        public FormView()
        {
            InitializeComponent();

            _fileService = new FileService();

            IEnumerable<FormField> formFields;

            //if we can find the directory
            if (_fileService.FileExists(FormFieldFielAddress))
            {
                //get the root of the document
                XElement formFieldsRoot = XElement.Load(FormFieldFielAddress);

                //get the form fields
                formFields = from root in formFieldsRoot.Elements()
                             select new FormField
                             {
                                 Name = root.Element("name").Value,
                                 Label = root.Element("label").Value,
                                 ControlType = root.Element("controltype").Value,
                                 Order = Convert.ToInt32(root.Element("order").Value),
                                 SubFields = root.Elements("option").Select(x => x.Value)
                             };


                //if there are form fields build the form
                if (formFields.Any())
                {
                    //foreach of the form fields retreived
                    foreach (var formField in formFields)
                    {

                        CreateFieldLabel(formField);

                        //based on the control Type
                        if (formField.ControlType == ControlType.TextBox.ToString())
                        {
                            //add a textbox to the stack panel
                            pnlFormFieldContainer.Children.Add(new TextBox
                            {
                                Name = formField.Name                                
                            });
                        }
                        else if (formField.ControlType == ControlType.RadioButtonList.ToString())
                        {
                            //create a stack panel into which the radio buttons will be placed
                            StackPanel radioContainer = new StackPanel
                            {
                                Orientation = Orientation.Vertical,
                                Margin = new Thickness(15,0,15,0)
                            };

                            //foreach radio option, create a radio button
                            foreach (var radio in formField.SubFields)
                            {
                                radioContainer.Children.Add(new RadioButton
                                {
                                    Content = radio,
                                    Name = formField.Name,                                                                        
                                });
                            }

                            //add the radio container to the panel
                            pnlFormFieldContainer.Children.Add(radioContainer);
                        }
                        else if (formField.ControlType == ControlType.DropDownList.ToString())
                        {
                            pnlFormFieldContainer.Children.Add(new ComboBox
                            {
                                Name = formField.Name,
                                ItemsSource = formField.SubFields                                
                            });
                        }
                        else if (formField.ControlType == ControlType.DatePicker.ToString())
                        {
                            pnlFormFieldContainer.Children.Add(new DatePicker
                            {
                                Name = formField.Name,
                                IsTodayHighlighted = true,
                                
                            });
                        }
                    }
                }   
            }
            else
            {
                //send notification about not finding the file

                //TODO - Implement notification system

                //var notification = new Notification("The form field settings could not be found.", NotificationType.Error);

                //notification.ShowDialog();

                //return;
            }                              
        }
        #endregion        
    }
}
