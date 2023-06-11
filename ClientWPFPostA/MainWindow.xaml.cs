using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace ClientWPFPostA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            Title = "PosteA";

            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7271/hubData") //this url is local and should be change with a real one
                .WithAutomaticReconnect()
                .Build();

            Task.Factory.StartNew(async () =>
            {
                await connection.StartAsync();
            });

            connection.Reconnecting += (sender) =>
            {
                Dispatcher.Invoke(() =>
                {
                    Debug.WriteLine("Reconnecting...");
                });
                return Task.CompletedTask;
            };

            CommandBinding copyCommandBinding = new CommandBinding(ApplicationCommands.Copy, CopyCommand);
            textToCopy.CommandBindings.Add(copyCommandBinding);
        }

        private async void CopyCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (textToCopy.SelectionLength > 0)
            {
                await connection.InvokeAsync("CopyData", textToCopy.SelectedText);
            }
        }
    }
}