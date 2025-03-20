using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;

namespace Url
{
    public partial class MainWindow : Window
    {
        private const string HistoryFilePath = @"C:\Users\baska\Documents\url_history";       
        private List<UrlHistoryEntry> _urlHistory;

        public MainWindow()
        {
            InitializeComponent();
            LoadNetworkInterfaces();
            LoadUrlHistory();
        }

        private void LoadNetworkInterfaces()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var ni in interfaces)
            {
                InterfaceList.Items.Add(ni.Name);
            }
        }

        private void LoadUrlHistory()
        {
            _urlHistory = new List<UrlHistoryEntry>();

            if (File.Exists(HistoryFilePath))
            {
                try
                {
                    var lines = File.ReadAllLines(HistoryFilePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 3)
                        {
                            _urlHistory.Add(new UrlHistoryEntry
                            {
                                Url = parts[0].Replace("URL: ", ""),
                                PingResult = parts[1].Replace("Ping: ", ""),
                                DnsInfo = parts[2].Replace("DNS: ", "")
                            });
                        }
                    }
                    HistoryList.ItemsSource = _urlHistory;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке истории: {ex.Message}");
                }
            }
        }

        private void SaveUrlHistory()
        {
            try
            {
                
                if (!File.Exists(HistoryFilePath))
                {
                    File.Create(HistoryFilePath).Close(); 
                }

                
                var lines = _urlHistory.Select(entry => entry.ToString()).ToArray();
                File.WriteAllLines(HistoryFilePath, lines);

                MessageBox.Show($"История сохранена в файл: {Path.GetFullPath(HistoryFilePath)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении истории: {ex.Message}");
            }
        }

        private void AddUrlToHistory(string url, string pingResult, string dnsInfo)
        {
            // Ищем запись с таким же URL
            var existingEntry = _urlHistory.FirstOrDefault(entry => entry.Url == url);

            if (existingEntry != null)
            {
                // Обновляем существующую запись
                if (!string.IsNullOrEmpty(pingResult))
                {
                    existingEntry.PingResult = pingResult;
                }
                if (!string.IsNullOrEmpty(dnsInfo))
                {
                    existingEntry.DnsInfo = dnsInfo;
                }
            }
            else
            {
                // Создаем новую запись
                var entry = new UrlHistoryEntry
                {
                    Url = url,
                    PingResult = pingResult,
                    DnsInfo = dnsInfo
                };
                _urlHistory.Add(entry);
            }

            // Обновляем ListBox
            HistoryList.ItemsSource = null;
            HistoryList.ItemsSource = _urlHistory;

            // Сохраняем историю в файл
            SaveUrlHistory();
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

                // Определяем тип IP-адреса
                try
                {
                    IPAddress[] addresses = Dns.GetHostAddresses(uri.Host);
                    if (addresses.Length > 0)
                    {
                        string addressType = GetAddressType(addresses[0]); // Определяем тип первого адреса
                        result.AppendLine($"Тип адреса: {addressType}");
                        UrlAnalysisResult.Text = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    result.AppendLine($"Ошибка при определении типа адреса: {ex.Message}");
                    UrlAnalysisResult.Text = result.ToString();
                }

                AddUrlToHistory(uri.ToString(), "", "");
            }
            else
            {
                UrlAnalysisResult.Text = "Некорректный URL.";
            }
        }

        private async void PingHost_Click(object sender, RoutedEventArgs e)
        {
            if (Uri.TryCreate(UrlInput.Text, UriKind.Absolute, out Uri uri))
            {
                Ping ping = new Ping();
                try
                {
                    PingReply reply = await ping.SendPingAsync(uri.Host);
                    string pingResult = $"Ответ от {uri.Host}: {reply.Status}, время: {reply.RoundtripTime} мс";
                    MessageBox.Show(pingResult);

                    // Добавляем результат ping в историю
                    AddUrlToHistory(uri.ToString(), pingResult, "");
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
                        string addressType = GetAddressType(ip); // Определяем тип адреса
                        dnsInfo.AppendLine($"IP: {ip} (Тип: {addressType})");
                    }
                    string dnsResult = dnsInfo.ToString();
                    MessageBox.Show(dnsResult);

                    // Добавляем результат DNS в историю
                    AddUrlToHistory(uri.ToString(), "", dnsResult);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveUrlHistory();
        }

        public static string GetAddressType(IPAddress ipAddress)
        {
            if (IPAddress.IsLoopback(ipAddress))
            {
                return "Loopback";
            }

            byte[] addressBytes = ipAddress.GetAddressBytes();

            // Локальные адреса:
            // 10.0.0.0 — 10.255.255.255
            if (addressBytes[0] == 10)
            {
                return "Локальный (Private)";
            }

            // 172.16.0.0 — 172.31.255.255
            if (addressBytes[0] == 172 && addressBytes[1] >= 16 && addressBytes[1] <= 31)
            {
                return "Локальный (Private)";
            }

            // 192.168.0.0 — 192.168.255.255
            if (addressBytes[0] == 192 && addressBytes[1] == 168)
            {
                return "Локальный (Private)";
            }

            // Все остальные адреса считаются публичными
            return "Публичный (Public)";
        }
    }

    public class UrlHistoryEntry
    {
        public string Url { get; set; }
        public string PingResult { get; set; }
        public string DnsInfo { get; set; }

        public override string ToString()
        {
            return $"URL: {Url}\nPing: {PingResult}\nDNS: {DnsInfo}\n";
        }
    }
}