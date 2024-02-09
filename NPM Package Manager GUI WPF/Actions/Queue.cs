using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NPM_Package_Manager_GUI_WPF.Actions
{
    public static class Queue
    {
        static MainWindow mainWindow;
        static List<QueueAction> queue = new List<QueueAction>();
        static bool finished = true;

        public static async void Init(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;

            while (true)
            {
                await Task.Delay(1000); // Can make it faster later.
                if(!finished) { continue; }
                if(queue.Count != 0)
                {
                    //mainWindow.SBProgressText.Text = $"Preforming action {queue[0].ActionText} ({queue.Count})";
                    mainWindow.SBProgressText.Text = $"{queue[0].ActionText} ({queue.Count})";
                    mainWindow.SBProgressSpinner.Visibility = System.Windows.Visibility.Visible;
                    mainWindow.SBProgressBar.Visibility = System.Windows.Visibility.Visible;

                    mainWindow.SBProgressBar.Maximum = queue.Count;
                    mainWindow.SBProgressBar.Value = 1;

                    /*finished = false;
                    Task.Factory.StartNew(queue[0].Action).ContinueWith((Task task) =>
                    {
                        Dispatcher.FromThread(Thread.CurrentThread).Invoke(() =>
                        {
                            finished = true;
                        });
                    });*/

                    queue[0].Action(); // Temporary

                    queue.Remove(queue[0]);
                } else
                {
                    mainWindow.SBProgressText.Text = "Ready";
                    mainWindow.SBProgressSpinner.Visibility = System.Windows.Visibility.Collapsed;
                    mainWindow.SBProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public static void AddToQueue(QueueAction action)
        {
            queue.Add(action);
        }

        public class QueueAction
        {
            public Action Action { get; set; } = new Action(() => { });
            public string ActionText { get; set; } = "Unknown";
        }
    }
}
