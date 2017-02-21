using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using Microsoft.Win32;
using Binding=System.Windows.Data.Binding;
using TextBox=System.Windows.Controls.TextBox;
using UserControl=System.Windows.Controls.UserControl;

namespace VIENNAAddInWpfUserControls
{
    /// <summary>
    /// Interaction logic for FileSelector.xaml
    /// </summary>
    public partial class DirectorySelector : UserControl
    {
        public DirectorySelector()
        {
            InitializeComponent();
            fileNameTextBox.SetBinding(TextBox.TextProperty, new Binding("DirectoryName")
                                                             {
                                                                 Source = this,
                                                                 UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                                                             });
        }

        public bool ShowNewFolderButton { get; set; }

        public string Description { get; set; }

        public static readonly DependencyProperty DirectoryNameProperty = DependencyProperty.Register("DirectoryName", typeof(string), typeof(DirectorySelector));

        public string DirectoryName
        {
            get
            {
                return (string) GetValue(DirectoryNameProperty);
            }
            set
            {
                SetValue(DirectoryNameProperty, value);
            }
        }

        private void openFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog
                      {
                          Description = Description,
                          ShowNewFolderButton = ShowNewFolderButton
                      };
            RaiseEvent(new RoutedEventArgs(BeforeDialogOpenedEvent));
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileNameTextBox.Text = dlg.SelectedPath;
            }
            RaiseEvent(new RoutedEventArgs(AfterDialogClosedEvent));
        }

        public new double Width
        {
            get
            {
                return dockPanel.Width;
            }
            set
            {
                dockPanel.Width = value;
            }
        }

        public new double Height
        {
            get
            {
                return dockPanel.Height;
            }
            set
            {
                dockPanel.Height = value;
            }
        }

        private static readonly RoutedEvent BeforeDialogOpenedEvent = EventManager.RegisterRoutedEvent("BeforeDialogOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DirectorySelector));

        public event RoutedEventHandler BeforeDialogOpened
        {
            add { AddHandler(BeforeDialogOpenedEvent, value); }
            remove { RemoveHandler(BeforeDialogOpenedEvent, value); }
        }

        private static readonly RoutedEvent AfterDialogClosedEvent = EventManager.RegisterRoutedEvent("AfterDialogClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DirectorySelector));

        public event RoutedEventHandler AfterDialogClosed
        {
            add { AddHandler(AfterDialogClosedEvent, value); }
            remove { RemoveHandler(AfterDialogClosedEvent, value); }
        }

        private static readonly RoutedEvent DirectoryNameChangedEvent = EventManager.RegisterRoutedEvent("DirectoryNameChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DirectorySelector));

        public event RoutedEventHandler DirectoryNameChanged
        {
            add { AddHandler(DirectoryNameChangedEvent, value); }
            remove { RemoveHandler(DirectoryNameChangedEvent, value); }
        }

        private void fileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DirectoryNameChangedEvent));
        }
    }
}