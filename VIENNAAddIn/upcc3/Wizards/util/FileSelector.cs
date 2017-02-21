using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public class FileSelector : StackPanel
    {
        private const int ButtonWidth = 30;
        private const int DefaultWidth = 415;
        private const int GapBetweenElements = 5;
        private readonly string DefaultExt;
        private readonly string Filter;
        private readonly TextBox TextBoxFileName;

        public FileSelector(string defaultExt, string filter)
        {
            DefaultExt = defaultExt;
            Filter = filter;

            Orientation = Orientation.Horizontal;

            TextBoxFileName = new TextBox
                              {
                                  Margin = new Thickness(0, 0, GapBetweenElements, 0),
                                  VerticalAlignment = VerticalAlignment.Stretch,
                              };
            TextBoxFileName.TextChanged += textbox_TextChanged;
            Children.Add(TextBoxFileName);

            var button = new Button
                         {
                             Content = "...",
                             Width = ButtonWidth,
                             VerticalAlignment = VerticalAlignment.Stretch,
                         };
            button.Click += browseButton_Click;
            Children.Add(button);

            Width = DefaultWidth;
        }

        public new double Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                TextBoxFileName.Width = value - ButtonWidth - GapBetweenElements;
            }
        }

        public string FileName
        {
            get { return TextBoxFileName.Text; }
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                      {
                          DefaultExt = DefaultExt,
                          Filter = Filter,
                      };
            if (dlg.ShowDialog() == true)
            {
                TextBoxFileName.Text = dlg.FileName;
            }
        }

        public event Action FileNameChanged;

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FileNameChanged != null)
            {
                FileNameChanged();
            }
            TextBoxFileName.TextChanged -= textbox_TextChanged;
        }
    }

    public class ChainedFileSelector : FileSelector
    {
        public ChainedFileSelector(string defaultExt, string filter) : base(defaultExt, filter)
        {
        }
    }

    public class MultipleFilesSelector : StackPanel
    {
        private readonly string defaultExt;
        private readonly string filter;
        private readonly List<FileSelector> fileSelectors = new List<FileSelector>();

        public MultipleFilesSelector(string defaultExt, string filter)
        {
            this.defaultExt = defaultExt;
            this.filter = filter;

            AddFileSelector();
        }

        private void AddFileSelector()
        {
            var fileSelector = new FileSelector(defaultExt, filter)
                               {
                                   Margin = new Thickness(0, 5, 0, 0),
                               };
            fileSelector.FileNameChanged += AddFileSelector;
            fileSelectors.Add(fileSelector);
            Children.Add(fileSelector);
        }

        public new double Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                foreach (var fileSelector in fileSelectors)
                {
                    fileSelector.Width = value;
                }
            }
        }

        public IEnumerable<string> FileNames
        {
            get
            {
                foreach (var fileSelector in fileSelectors)
                {
                    if (!string.IsNullOrEmpty(fileSelector.FileName))
                    {
                        yield return fileSelector.FileName;
                    }
                }
            }
        }
    }

}