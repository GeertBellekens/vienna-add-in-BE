// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using Visifire.Charts;
using MessageBox=System.Windows.Forms.MessageBox;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SchemaAnalyzer
    {
        private string file1 = "";
        private string file2 = "";
        private SchemaAnalyzerResults results1;
        private SchemaAnalyzerResults results2;
        private bool started;

        public SchemaAnalyzer()
        {
            InitializeComponent();
            var dummy = new Chart(); // workaround for assembly-error, we will never use this dummy object
            LoadFiles();
        }

        public SchemaAnalyzer(string file1, string file2)
        {
            InitializeComponent();
            var dummy = new Chart(); // workaround for assembly-error, we will never use this dummy object
            this.file1 = file1;
            this.file2 = file2;
            LoadFiles();
        }

        private void LoadFiles()
        {
            fileSelector1.FileName = file1;
            fileSelector2.FileName = file2;
            innerCanvas.Visibility = Visibility.Visible;
        }

        private bool SetFiles()
        {
            if (file1.Length == 0)
            {
                FileSelectorCancelled();
                return false;
            }
            if (!Analyze(file1, 1))
                return false;
            if (file2.Length > 0)
            {
                if (!Analyze(file2, 2))
                    return false;
                tab2.IsEnabled = true;
                tab3.IsEnabled = true;
            }
            else
            {
                tab1.Focus();
                tab2.IsEnabled = false;
                tab3.IsEnabled = false;
            }
            return true;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            file1 = fileSelector1.FileName;
            file2 = fileSelector2.FileName;
            if (SetFiles())
                innerCanvas.Visibility = Visibility.Collapsed;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            FileSelectorCancelled();
            innerCanvas.Visibility = Visibility.Collapsed;
        }

        private void FileSelectorCancelled()
        {
            if (!started) Close();
        }

        private bool Analyze(string file, int chart)
        {
            SchemaAnalyzerResults results;
            try
            {
                results = XMLSchemaReader.Read(file);
            }
            catch (FileNotFoundException fnfe)
            {
                MessageBox.Show("The given file could not be opened!\n" + fnfe.Message, Title, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            catch (DirectoryNotFoundException dnfe)
            {
                MessageBox.Show("The given path could not be opened!\n" + dnfe.Message, Title, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            catch (XmlException xe)
            {
                MessageBox.Show("The given file could not be read!\n" + xe.Message, Title, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            catch (XmlSchemaException xse)
            {
                MessageBox.Show("The given file could not be read!\n" + xse.Message, Title, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            started = true;
            (chart == 1 ? chart1 : chart2).Series[0].DataPoints.Clear();
            double curPos = 0.0;
            double deltaPos = 1/((double) results.Count - 1);
            foreach (SchemaAnalyzerResult dataset in results)
            {
                Color curColor = GetComplexityColor(curPos > 1 ? 1 : curPos);
                var item = new DataPoint
                               {
                                   YValue = dataset.Count,
                                   AxisXLabel = dataset.Caption,
                                   Color = new SolidColorBrush(Color.FromRgb(curColor.R, curColor.G, curColor.B))
                               };
                (chart == 1 ? chart1 : chart2).Series[0].DataPoints.Add(item);

                curPos += deltaPos;
            }
            //sort results for comparison view
            results.Sort(new SchemaAnalyzerResultComparerByName());

            chart3.Series[chart - 1].DataPoints.Clear();
            foreach (SchemaAnalyzerResult dataset in results)
            {
                var item2 = new DataPoint {YValue = dataset.Count, AxisXLabel = dataset.Caption};
                chart3.Series[chart - 1].DataPoints.Add(item2);
            }
            chart3.Series[chart - 1].LegendText = file.Substring(file.LastIndexOf("\\") + 1);
            //afterwards restore original sort
            results.Sort(new SchemaAnalyzerResultComparerByValue());

            (chart == 1 ? tab1 : tab2).Header = file.Substring(file.LastIndexOf("\\") + 1);
            (chart == 1 ? complexity1 : complexity2).Fill = new SolidColorBrush(GetComplexityColor(results.Complexity));
            (chart == 1 ? complexity1 : complexity2).Visibility = Visibility.Visible;
            (chart == 1 ? complexityMover1 : complexityMover2).Width = (int) (results.Complexity*canvas1.ActualWidth);
            if (chart == 1)
                results1 = results;
            else
                results2 = results;
            return true;
        }

        private static Color GetComplexityColor(double position)
        {
            if (position <= 0.5)
            {
                var green = (byte) (position*255);
                return Color.FromRgb(255, green, 0);
            }
            var red = (byte) ((Math.Abs(((position - 0.5)*2) - 1))*255);
            return Color.FromRgb(red, 128, 0);
        }

        public static void ShowForm(AddInContext context)
        {
            new SchemaAnalyzer().ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadFiles();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas actualCanvas = (tabControl.SelectedIndex == 0 ? canvas1 : canvas2);
            if (results1 != null)
            {
                complexityMover1.Width = (int) (results1.Complexity*actualCanvas.ActualWidth);
            }
            if (results2 != null)
            {
                complexityMover2.Width = (int) (results2.Complexity*actualCanvas.ActualWidth);
            }
        }
    }
}