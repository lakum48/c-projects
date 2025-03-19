using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using System.Net;
using System.Linq;



namespace Url
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadNetworkInterfaces();

        }

        private void LoadNetworkInterfaces()
        {
            var interfaces  = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var ni in interfaces)
            {
                InterfaceList.Items.Add(ni.Name);
            }
        }
        private void InterfaceList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedInterface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(ni => ni.Name == InterfaceList.SelectedItem.ToString());

            if (selectedInterface != null)
            {
                var ipProperties = selectedInterface.GetIPProperties();
                var ipInfo = ipProperties.UnicastAddresses.FirstOrDefault();

                StringBuilder info = new StringBuilder();
                info.AppendLine($"Имя: {selectedInterface.Name}");
                info.AppendLine($"Описание: {selectedInterface.Description}");
                info.AppendLine($"Тип: {selectedInterface.NetworkInterfaceType}");
                info.AppendLine($"Состояние: {selectedInterface.OperationalStatus}");
                info.AppendLine($"Скорость: {selectedInterface.Speed} bps");
                info.AppendLine($"MAC-адрес: {selectedInterface.GetPhysicalAddress()}");

                if (ipInfo != null)
                {
                    info.AppendLine($"IP-адрес: {ipInfo.Address}");
                    info.AppendLine($"Маска подсети: {ipInfo.IPv4Mask}");
                }

                InterfaceInfo.Text = info.ToString();
            }
        }

        // Анализ URL
        private void AnalyzeUrl_Click(object sender, RoutedEventArgs e)
        {
            if (Uri.TryCreate(UrlInput.Text, UriKind.Absolute, out Uri uri))
            {
                StringBuilder result = new StringBuilder();
                result.AppendLine($"Схема: {uri.Scheme}");
                result.AppendLine($"Хост: {uri.Host}");
                result.AppendLine($"Порт: {uri.Port}");
                result.AppendLine($"Путь: {uri.AbsolutePath}");
                result.AppendLine($"Параметры: {uri.Query}");
                result.AppendLine($"Фрагмент: {uri.Fragment}");

                UrlAnalysisResult.Text = result.ToString();
            }
            else
            {
                UrlAnalysisResult.Text = "Некорректный URL.";
            }
        }

        // Проверка доступности хоста (Ping)
        private async void PingHost_Click(object sender, RoutedEventArgs e)
        {
            if (Uri.TryCreate(UrlInput.Text, UriKind.Absolute, out Uri uri))
            {
                Ping ping = new Ping();
                try
                {
                    PingReply reply = await ping.SendPingAsync(uri.Host);
                    MessageBox.Show($"Ответ от {uri.Host}: {reply.Status}, время: {reply.RoundtripTime} мс");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Введите корректный URL для проверки доступности.");
            }
        }

        // Получение DNS-информации
        private void GetDnsInfo_Click(object sender, RoutedEventArgs e)
        {
            if (Uri.TryCreate(UrlInput.Text, UriKind.Absolute, out Uri uri))
            {
                try
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(uri.Host);
                    StringBuilder dnsInfo = new StringBuilder();
                    foreach (var ip in hostEntry.AddressList)
                    {
                        dnsInfo.AppendLine($"IP: {ip}");
                    }
                    MessageBox.Show(dnsInfo.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Введите корректный URL для получения DNS-информации.");
            }
        }
    }
}