using System;

class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Network Security Toolset:");
            Console.WriteLine("1. Перехоплення мережевого трафіку");
            Console.WriteLine("2. Налаштування брандмауера");
            Console.WriteLine("3. Сканування IP-адрес і портів");
            Console.WriteLine("4. Визначення сервісів на відкритих портах");
            Console.WriteLine("5. Вихід");
            Console.WriteLine("Оберіть опцію:");

            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 5)
            {
                Console.WriteLine("Неправильний вибір. Спробуйте ще раз.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    NetworkSniffer.StartSniffing();
                    break;
                case 2:
                    FirewallManager.ConfigureFirewall();
                    break;
                case 3:
                    NetworkScanner.ScanRange();
                    break;
                case 4:
                    ServiceIdentifier.ScanAndIdentifyServices();
                    break;
                case 5:
                    Console.WriteLine("Вихід...");
                    return;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }
}
