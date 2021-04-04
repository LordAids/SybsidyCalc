using System;

namespace SybsidiiCalc
{
    class Program
    {
        private static void Notify(object? sender, string message)
        {
            Console.WriteLine(message);
        }

        private static void Exception(object? sender, Tuple<string, Exception> tuple)
        {
            Console.WriteLine(tuple.Item2);
        }

        static void Main(string[] args)
        {
            Volume volume = new Volume()
            {
                ServiceId = 1,
                HouseId = 1,
                Month = DateTime.Today,
                Value = 20
            };

            Tariff tariff = new Tariff()
            {
                ServiceId = 1,
                HouseId = 1,
                PeriodBegin = DateTime.Parse("2021-02"),
                PeriodEnd = DateTime.Parse("2021-06"),
                Value = 5
            };

            SubsidiiCalc calc = new SubsidiiCalc();
            calc.OnNotify += Notify;
            calc.OnException += Exception;

            Charge charge = calc.CalculateSubsidy(volume, tariff);

            if (charge != null) Console.WriteLine($"Значение рассчёта: {charge.Value}");

        }
    }
}