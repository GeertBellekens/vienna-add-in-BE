using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace VIENNAAddInWpfUserControls
{
    /// <summary>
    /// Interaction logic for FileSelector.xaml
    /// </summary>
    public partial class ProgressBackgroundWorker
    {
        private static readonly RoutedEvent RunWorkerCompletedEvent =
            EventManager.RegisterRoutedEvent("RunWorkerCompleted", RoutingStrategy.Bubble, typeof (RunWorkerCompletedEventHandler),
                                             typeof (FileSelector));

        private readonly BackgroundWorker bw;

        private readonly RoutedEvent DoWorkEvent = EventManager.RegisterRoutedEvent("DoWork", RoutingStrategy.Bubble,
                                                                                    typeof (DoWorkEventHandler),
                                                                                    typeof (ProgressBackgroundWorker));

        public object parameter;

        public ProgressBackgroundWorker()
        {
            InitializeComponent();
            bw = new BackgroundWorker {WorkerReportsProgress = false, WorkerSupportsCancellation = false};
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

        public string Header
        {
            get { return (string) label.Content; }
            set
            {
                border.Width = 77 + label.ActualWidth;
                label.Content = value;
            }
        }

        public void RunWorkerAsync()
        {
            Visibility = Visibility.Visible;
            var sbdRotation = (Storyboard) FindResource("sbdRotation");
            sbdRotation.Begin(this);
            bw.RunWorkerAsync();
        }

        public void RunWorkerAsync(object argument)
        {
            Visibility = Visibility.Visible;
            var sbdRotation = (Storyboard) FindResource("sbdRotation");
            sbdRotation.Begin(this);
            parameter = argument;
            bw.RunWorkerAsync(this);
        }

        public event DoWorkEventHandler DoWork
        {
            add { AddHandler(DoWorkEvent, value); }
            remove { RemoveHandler(DoWorkEvent, value); }
        }

        public event RunWorkerCompletedEventHandler RunWorkerCompleted
        {
            add { AddHandler(RunWorkerCompletedEvent, value); }
            remove { RemoveHandler(RunWorkerCompletedEvent, value); }
        }

        public void RaiseBackgroundWorkerDoWorkEvent()
        {
            RaiseEvent(new RoutedEventArgs(DoWorkEvent));
        }

        public void RaiseBackgroundWorkerWorkCompletedEvent()
        {
            RaiseEvent(new RoutedEventArgs(RunWorkerCompletedEvent));
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var progressBackgroundWorker = (ProgressBackgroundWorker) e.Argument;
            progressBackgroundWorker.RaiseBackgroundWorkerDoWorkEvent();
            e.Result = e.Argument;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var progressBackgroundWorker = (ProgressBackgroundWorker)e.Result;
            var sbdRotation = (Storyboard) FindResource("sbdRotation");
            sbdRotation.Stop();
            Visibility = Visibility.Collapsed;
            progressBackgroundWorker.RaiseBackgroundWorkerWorkCompletedEvent();
        }
    }
}