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
    public partial class FormView : UserControl, IObserver
    {
        #region Fields
        //make this address relative to the project
        private string FormFieldFieldAddress = "G:/Projects/FileTest/DocFlow.Presentation/xml/FormSettings.xml";
        private IFileService _fileService;
        private ISubject _subject;
        #endregion

        #region Methods
        /// <summary>
        /// Handles the Checked event of the rdoOther control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void rdoOther_Checked(object sender, RoutedEventArgs e)
        {
            //figure out which field sent the request
            //make the correct formfield visible
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the Unchecked event of the rdoOther control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void rdoOther_Unchecked(object sender, RoutedEventArgs e)
        {
            //figure out which field sent the request
            //make the correct formfield hide
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the comboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //figure out which combobox is the sender
            //display the appropriate other textbox
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the other field.
        /// </summary>
        /// <param name="parentField">The parent field.</param>
        protected void CreateOtherField(FormField parentField)
        {
            var otherFormField = new FormField { 
                ControlType = ControlType.TextBox.ToString(),
                HasOther = false,
                Label = string.Format("Other {0}", parentField.Label),
                Name = string.Format("other{0}", parentField.Name)            
            };

            CreateFieldLabel(otherFormField);
            CreateTextBox(otherFormField);
        }

        /// <summary>
        /// Creates the field label.
        /// </summary>
        /// <param name="field">The field.</param>
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
                    Margin = new Thickness(10, 15, 15, 5),//controls that aren't radios need the margin bottom here,
                    Visibility = field.Name.StartsWith("other") ? Visibility.Hidden : Visibility.Visible
                });
            }            
        }

        /// <summary>
        /// Creates the text box field.
        /// </summary>
        /// <param name="textboxField">The textbox field.</param>
        /// <exception cref="ArgumentNullException">textboxField</exception>
        protected void CreateTextBox(FormField textboxField)
        {
            if (textboxField == null)
                throw new ArgumentNullException("textboxField");
            //add a textbox to the stack panel

            pnlFormFieldContainer.Children.Add(new TextBox
            {
                Name = textboxField.Name,
                Visibility = textboxField.Name.StartsWith("other") ? Visibility.Hidden : Visibility.Visible
            });
        }

        /// <summary>
        /// Creates the RadioButton list.
        /// </summary>
        /// <param name="radioField">The radio field.</param>
        protected void CreateRadioButtonList(FormField radioField)
        {
            //create a stack panel into which the radio buttons will be placed
            StackPanel radioContainer = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(15, 0, 15, 0)
            };

            //foreach radio option, create a radio button
            foreach (var radio in radioField.SubFields)
            {
                var radioBtn = new RadioButton
                {
                    Content = radio,
                    Name = radioField.Name
                };

                if (radio == "Other")
                {
                    radioBtn.Checked += rdoOther_Checked;
                    radioBtn.Unchecked += rdoOther_Unchecked;  
                }
                
                radioContainer.Children.Add(radioBtn);
            }
            
            //add the radio container to the panel
            pnlFormFieldContainer.Children.Add(radioContainer);
            
            if (radioField.HasOther)
            {
                CreateOtherField(radioField);                 
            }
        }        

        /// <summary>
        /// Creates the drop down list.
        /// </summary>
        /// <param name="dropdownField">The dropdown field.</param>
        /// <exception cref="ArgumentNullException">dropdownField</exception>
        protected void CreateDropDownList(FormField dropdownField)
        {
            if (dropdownField == null)
                throw new ArgumentNullException("dropdownField");

            var comboBox = new ComboBox
            {
                Name = dropdownField.Name,
                ItemsSource = dropdownField.SubFields
            };

            pnlFormFieldContainer.Children.Add(comboBox);

            if (dropdownField.HasOther)
            {
                comboBox.SelectionChanged += comboBox_SelectionChanged; 
                CreateOtherField(dropdownField);
            }
        }        

        /// <summary>
        /// Creates the date picker.
        /// </summary>
        /// <param name="datepickerField">The datepicker field.</param>
        /// <exception cref="ArgumentNullException">datepickerField</exception>
        protected void CreateDatePicker(FormField datepickerField)
        {
            if (datepickerField == null)
                throw new ArgumentNullException("datepickerField");

            pnlFormFieldContainer.Children.Add(new DatePicker
            {
                Name = datepickerField.Name,
                IsTodayHighlighted = true,
            });
        }

        /// <summary>
        /// Clears all form fields.
        /// </summary>
        protected void ClearAllFormFields()
        {
            foreach (var control in pnlFormFieldContainer.Children)
            {
                if (control.GetType() == typeof(TextBox))
                {
                    ((TextBox)control).Text = string.Empty;
                }
                else if (control.GetType() == typeof(RadioButton))
                {
                    ((RadioButton)control).IsChecked = false;
                }
                else if (control.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)control).SelectedIndex = -1;
                }
                else if (control.GetType() == typeof(DatePicker))
                {
                    ((DatePicker)control).Text = string.Empty;
                }
            }
        }
        #endregion

        #region Constructor
        public FormView(ISubject subject)
        {
            InitializeComponent();
            
            //register with the subject
            _subject = subject;            
            _subject.addObserver(this);

            _fileService = new FileService();

            IEnumerable<FormField> formFields;

            //if we can find the directory
            if (_fileService.FileExists(FormFieldFieldAddress))
            {
                //get the root of the document
                XElement formFieldsRoot = XElement.Load(FormFieldFieldAddress);

                //get the form fields
                formFields = from root in formFieldsRoot.Elements()
                             select new FormField
                             {
                                 Name = root.Element("name").Value,
                                 Label = root.Element("label").Value,
                                 ControlType = root.Element("controltype").Value,
                                 Order = Convert.ToInt32(root.Element("order").Value),  
                                 HasOther = Convert.ToBoolean(root.Element("hasother").Value),
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
                            CreateTextBox(formField);
                        }
                        else if (formField.ControlType == ControlType.RadioButtonList.ToString())
                        {
                            CreateRadioButtonList(formField);
                        }
                        else if (formField.ControlType == ControlType.DropDownList.ToString())
                        {
                            CreateDropDownList(formField);
                        }
                        else if (formField.ControlType == ControlType.DatePicker.ToString())
                        {
                            CreateDatePicker(formField);
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
    
        #region Observer Behavior
        public void Update(ISubject subject, object args)
        {
            if (subject.GetType() == typeof(DirectoryFileList))
            {
                ClearAllFormFields();
            }
        }
        #endregion
    }
}
