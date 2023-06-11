using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace ClientWPFPostB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;

        string ClipBoardData;
        public MainWindow()
        {

            InitializeComponent();
            Title = "PosteB";

            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7271/hubData") //this url is local and should be change with a real one
                .WithAutomaticReconnect()
                .Build();

            CommandBinding pasteCommandBinding = new CommandBinding(ApplicationCommands.Paste, PasteCommand);
            textToPaste.CommandBindings.Add(pasteCommandBinding);
            Loaded += MainWindow_Loaded;

        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            connection.On<string>("DataCopied", pasteData =>
            {
                Debug.WriteLine(pasteData);
                Dispatcher.Invoke(() =>
                {
                    ClipBoardData = pasteData;
                });
            });

            try
            {
                await connection.StartAsync();
                Debug.WriteLine("Connection started");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to start connection: {ex.Message}");
            }
        }

        private void PasteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            textToPaste.Text = ClipBoardData;
        }
    }
}