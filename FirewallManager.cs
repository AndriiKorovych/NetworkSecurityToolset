using System;
using System.Diagnostics;

class FirewallManager
{
    public static void ConfigureFirewall()
    {
        Console.WriteLine("1. Заборонити вхідний трафік з IP");
        Console.WriteLine("2. Заборонити доступ до порту");
        Console.WriteLine("3. Дозволити трафік тільки з довірених IP");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                Console.WriteLine("Введіть IP:");
                string ip = Console.ReadLine();
                BlockIP(ip);
                break;
            case 2:
                Console.WriteLine("Введіть порт:");
                int port = int.Parse(Console.ReadLine());
                BlockPort(port);
                break;
            case 3:
                Console.WriteLine("Введіть довірені IP через кому:");
                string[] trustedIPs = Console.ReadLine().Split(',');
                AllowOnlyTrusted(trustedIPs);
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }

    private static void BlockIP(string ip)
    {
        RunCommand($"New-NetFirewallRule -DisplayName 'Block {ip}' -Direction Inbound -RemoteAddress {ip} -Action Block");
        Console.WriteLine($"IP {ip} заблоковано.");
        NotificationManager.NotifyAdmin("Брандмауер", $"IP {ip} заблоковано.");
    }

    private static void BlockPort(int port)
    {
        RunCommand($"New-NetFirewallRule -DisplayName 'Block Port {port}' -Direction Inbound -LocalPort {port} -Protocol TCP -Action Block");
        Console.WriteLine($"Порт {port} заблоковано.");
        NotificationManager.NotifyAdmin("Брандмауер", $"Порт {port} заблоковано.");
    }

    private static void AllowOnlyTrusted(string[] trustedIPs)
    {
        string trusted = string.Join(",", trustedIPs);
        RunCommand($"New-NetFirewallRule -DisplayName 'Allow Trusted Only' -Direction Inbound -RemoteAddress {trusted} -Action Allow");
        RunCommand("Set-NetFirewallProfile -Profile Domain,Public,Private -DefaultInboundAction Block");
        Console.WriteLine("Дозволено тільки трафік з довірених IP.");
        NotificationManager.NotifyAdmin("Брандмауер", "Дозволено тільки трафік з довірених IP.");
    }

    private static void RunCommand(string command)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = $"-Command \"{command}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }).WaitForExit();
    }
}
