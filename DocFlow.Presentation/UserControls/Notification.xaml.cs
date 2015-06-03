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
using System.Windows.Shapes;
using DocFlow.Core.Domain.Enums;

namespace DocFlow.Presentation
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public Notification(string message, NotificationType type)
        {            
            //add the message

            switch (type)
            {
                case NotificationType.Success:
                    tbMessage.Foreground = Brushes.Green;
                    break;
                case NotificationType.Error:
                    tbMessage.Foreground = Brushes.Red;
                    break;
                case NotificationType.Warning:
                    tbMessage.Foreground = Brushes.Black;
                    tbMessage.Background = Brushes.GreenYellow;
                    break;
                case NotificationType.Confirmation:
                    tbMessage.Foreground = Brushes.Black;
                    tbMessage.Background = Brushes.LightBlue;
                    break;
                default:
                    break;
            }

            InitializeComponent();
        }


    }
}
