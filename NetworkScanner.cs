using System;
using System.Net.Sockets;

class NetworkScanner
{
    public static void ScanRange()
    {
        Console.WriteLine("Введіть діапазон IP (наприклад, 192.168.1.1-192.168.1.255):");
        string range = Console.ReadLine();
        Console.WriteLine("Введіть порти для сканування (наприклад, 22,80,443):");
        string[] portsInput = Console.ReadLine().Split(',');
        int[] ports = Array.ConvertAll(portsInput, int.Parse);

        string[] parts = range.Split('-');
        string baseIP = parts[0].Substring(0, parts[0].LastIndexOf('.') + 1);
        int start = int.Parse(parts[0].Split('.')[3]);
        int end = int.Parse(parts[1].Split('.')[3]);

        for (int i = start; i <= end; i++)
        {
            string ip = $"{baseIP}{i}";
            foreach (int port in ports)
            {
                ScanPort(ip, port);
            }
        }
    }

    private static void ScanPort(string ip, int port)
    {
        using TcpClient client = new TcpClient();
        try
        {
            client.Connect(ip, port);
            Console.WriteLine($"Хост {ip} відкрив порт {port}");
        }
        catch
        {
            // Порт недоступний
        }
    }
}
