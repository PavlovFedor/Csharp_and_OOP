using System;

class Program
{
    static void Main()
    {
        try
        {
            // Создание объектов
            var r1 = new RationalNumber(1, 2);
            var r2 = new RationalNumber(3, 4);
            var r3 = RationalNumber.Parse("2/3");
            var r4 = RationalNumber.Parse("444:666"); // Сократится до 2/3

            Console.WriteLine($"r1 = {r1}"); // 1/2
            Console.WriteLine($"r2 = {r2}"); // 3/4
            Console.WriteLine($"r3 = {r3}"); // 2/3
            Console.WriteLine($"r4 = {r4}"); // 2/3

            // Арифметические операции
            var sum = r1 + r2;
            var product = r1 * r2;
            var difference = r1 - r2;
            var quotient = r1 / r2;

            Console.WriteLine($"Сумма: {sum}"); // 5/4
            Console.WriteLine($"Произведение: {product}"); // 3/8
            Console.WriteLine($"Разность: {difference}"); // -1/4
            Console.WriteLine($"Частное: {quotient}"); // 2/3

            // Сравнение
            Console.WriteLine($"r1 == r2: {r1 == r2}"); // False
            Console.WriteLine($"r3 == r4: {r3 == r4}"); // True
            Console.WriteLine($"r1 < r2: {r1 < r2}"); // True

            // Генерация случайного числа
            var randomNum = RationalNumber.RandomInRange(
                new RationalNumber(1, 4), 
                new RationalNumber(3, 4));
            Console.WriteLine($"Случайное число: {randomNum}");

            // Обратный элемент
            var reciprocal = r1.Reciprocal;
            Console.WriteLine($"Обратный к {r1}: {reciprocal}"); // 2/1
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}
