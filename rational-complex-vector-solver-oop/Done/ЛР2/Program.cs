using System;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("=== СОЗДАНИЕ ОБЪЕКТОВ ===");
            var c1 = new ComplexNumber(2, 3);
            var c2 = new ComplexNumber(1, -1);
            var c3 = ComplexNumber.Parse("4+2i");
            var c4 = ComplexNumber.Parse("-3i");
            var c5 = ComplexNumber.Parse("5");
            var c6 = ComplexNumber.Parse("5+i");
            var c7 = ComplexNumber.Parse("i");

            Console.WriteLine($"c1 = {c1}"); // 2.00+3.00i
            Console.WriteLine($"c2 = {c2}"); // 1.00-1.00i
            Console.WriteLine($"c3 = {c3}"); // 4.00+2.00i
            Console.WriteLine($"c4 = {c4}"); // -3.00i
            Console.WriteLine($"c5 = {c5}"); // 5.00
            Console.WriteLine($"c6 = {c6}"); // 5.00+1.00i
            Console.WriteLine($"c7 = {c7}"); // 1.00i
            Console.WriteLine();

            Console.WriteLine("=== АРИФМЕТИЧЕСКИЕ ОПЕРАЦИИ ===");
            var sum = c1 + c2;
            var product = c1 * c2;
            var difference = c1 - c2;
            var quotient = c1 / c2;

            Console.WriteLine($"({c1}) + ({c2}) = {sum}"); // 3.00+2.00i
            Console.WriteLine($"({c1}) * ({c2}) = {product}"); // 5.00+1.00i
            Console.WriteLine($"({c1}) - ({c2}) = {difference}"); // 1.00+4.00i
            Console.WriteLine($"({c1}) / ({c2}) = {quotient}"); // -0.50+2.50i
            Console.WriteLine();

            Console.WriteLine("=== ОПЕРАЦИИ СРАВНЕНИЯ ===");
            Console.WriteLine($"c1 == c2: {c1 == c2}"); // False
            Console.WriteLine($"c1 == c1: {c1 == c1}"); // True
            Console.WriteLine($"c3 != c4: {c3 != c4}"); // True
            Console.WriteLine();

            Console.WriteLine("=== СПЕЦИАЛЬНЫЕ ЭЛЕМЕНТЫ ===");
            Console.WriteLine($"Нулевой элемент: {ComplexNumber.Zero}"); // 0.00
            Console.WriteLine($"Единичный элемент: {ComplexNumber.One}"); // 1.00
            Console.WriteLine($"Мнимая единица: {ComplexNumber.ImaginaryOne}"); // 1.00i
            Console.WriteLine($"Сопряженное к {c1}: {c1.Conjugate}"); // 2.00-3.00i
            Console.WriteLine($"Обратный к {c1}: {c1.Reciprocal}"); // 0.15-0.23i
            Console.WriteLine();

            Console.WriteLine("=== СВОЙСТВА КОМПЛЕКСНЫХ ЧИСЕЛ ===");
            Console.WriteLine($"Модуль {c1}: {c1.Magnitude:F2}"); // 3.61
            Console.WriteLine($"Аргумент {c1}: {c1.Argument:F2} радиан"); // 0.98 радиан
            Console.WriteLine($"Модуль {c2}: {c2.Magnitude:F2}"); // 1.41
            Console.WriteLine($"Аргумент {c2}: {c2.Argument:F2} радиан"); // -0.79 радиан
            Console.WriteLine();

            Console.WriteLine("=== ГЕНЕРАЦИЯ СЛУЧАЙНЫХ ЧИСЕЛ ===");
            var randomNum1 = ComplexNumber.Random();
            var randomNum2 = ComplexNumber.RandomInRange(-10, 10, -5, 5);
            Console.WriteLine($"Случайное число 1: {randomNum1}");
            Console.WriteLine($"Случайное число 2 (в диапазоне): {randomNum2}");
            Console.WriteLine();

            Console.WriteLine("=== ВОЗВЕДЕНИЕ В СТЕПЕНЬ ===");
            var powered2 = c1.Power(2);
            var powered3 = c1.Power(3);
            Console.WriteLine($"({c1})^2 = {powered2}"); // -5.00+12.00i
            Console.WriteLine($"({c1})^3 = {powered3}"); // -46.00+9.00i
            Console.WriteLine();

            Console.WriteLine("=== ИЗВЛЕЧЕНИЕ КОРНЕЙ ===");
            var roots = ComplexNumber.Roots(new ComplexNumber(16, 0), 4); // Корни 4-й степени из 16
            Console.WriteLine("Корни 4-й степени из 16:");
            for (int i = 0; i < roots.Length; i++)
            {
                Console.WriteLine($"  Корень {i + 1}: {roots[i]}");
            }
            Console.WriteLine();

            Console.WriteLine("=== ОБРАБОТКА ИСКЛЮЧЕНИЙ ===");
            try
            {
                var invalid = new ComplexNumber(0, 0).Reciprocal;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка 1: {ex.Message}");
            }

            try
            {
                var divisionByZero = c1 / ComplexNumber.Zero;
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Ошибка 2: {ex.Message}");
            }

            try
            {
                var invalidParse = ComplexNumber.Parse("invalid");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Ошибка 3: {ex.Message}");
            }
            Console.WriteLine();

            Console.WriteLine("=== ПРЕОБРАЗОВАНИЕ ТИПОВ ===");
            double realNumber = 7.5;
            ComplexNumber complexFromDouble = (ComplexNumber)realNumber;
            Console.WriteLine($"double {realNumber} -> ComplexNumber: {complexFromDouble}"); // 7.50

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
        }
    }
}

