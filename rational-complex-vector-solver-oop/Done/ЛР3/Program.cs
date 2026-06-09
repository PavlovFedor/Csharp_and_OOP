using System;

// Пример использования
class Program
{
    static void Main()
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ИНТЕРФЕЙСА ПОЛЯ ===");
        Console.WriteLine();
        
        // Демонстрация для рациональных чисел
        FieldOperations.DemonstrateFieldOperations<RationalNumber>();
        
        // Демонстрация для комплексных чисел
        FieldOperations.DemonstrateFieldOperations<ComplexNumber>();
        
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ПАРСИНГА ===");
        
        // Парсинг рациональных чисел
        FieldOperations.DemonstrateParsing<RationalNumber>("3/4");
        FieldOperations.DemonstrateParsing<RationalNumber>("2:3");
        FieldOperations.DemonstrateParsing<RationalNumber>("5");
        
        // Парсинг комплексных чисел
        FieldOperations.DemonstrateParsing<ComplexNumber>("2+3i");
        FieldOperations.DemonstrateParsing<ComplexNumber>("5+i");
        FieldOperations.DemonstrateParsing<ComplexNumber>("1.5-2.5i");
                
        Console.WriteLine();
        Console.WriteLine("=== ГЕНЕРАЦИЯ В ДИАПАЗОНЕ ===");
        
        // генерация в диапазоне
        DemonstrateRangeGeneration();
        
        Console.WriteLine();
        Console.WriteLine("=== ОБОБЩЕННЫЕ ВЫЧИСЛЕНИЯ ===");
        
        // Обобщенные вычисления
        PerformGenericCalculations<RationalNumber>();
        PerformGenericCalculations<ComplexNumber>();
    }
    
    static void PerformGenericCalculations<T>() where T : IField<T>
    {
        Console.WriteLine($"Вычисления для {typeof(T).Name}:");
        
        T a = T.Random();
        T b = T.Random();
        
        // (a + b) * (a - b) = a² - b²
        T leftSide = (a + b) * (a - b);
        T rightSide = (a * a) - (b * b);
        
        Console.WriteLine($"a = {a}, b = {b}");
        Console.WriteLine($"(a + b) * (a - b) = {leftSide}");
        Console.WriteLine($"a² - b² = {rightSide}");
        Console.WriteLine($"Результаты равны: {leftSide == rightSide}");
        Console.WriteLine();
    }
    
     // Демонстрация генерации в диапазоне для обоих типов чисел
    static void DemonstrateRangeGeneration()
    {
        Console.WriteLine("Рациональные числа в диапазоне [1/4, 3/2]:");
        var rationalMin = new RationalNumber(1, 4);
        var rationalMax = new RationalNumber(3, 2);
        
        for (int i = 0; i < 3; i++)
        {
            var randomRational = RationalNumber.RandomInRange(rationalMin, rationalMax);
            Console.WriteLine($"  {randomRational} ({randomRational.ToDouble():F2})");
        }

        Console.WriteLine("Комплексные числа в диапазоне [-2-2i, 2+2i]:");
        var complexMin = new ComplexNumber(-2, -2);
        var complexMax = new ComplexNumber(2, 2);
        
        for (int i = 0; i < 3; i++)
        {
            var randomComplex = ComplexNumber.RandomInRange(complexMin, complexMax);
            Console.WriteLine($"  {randomComplex}");
        }
    }
}

// Пример использования с обобщенными методами
public class FieldOperations
{
    // Обобщенный метод для демонстрации операций поля
    public static void DemonstrateFieldOperations<T>() where T : IField<T>
    {
        Console.WriteLine($"=== ОПЕРАЦИИ ПОЛЯ {typeof(T).Name} ===");
        
        // Создание элементов
        T zero = T.Zero;
        T one = T.One;
        
        Console.WriteLine($"Нулевой элемент: {zero}");
        Console.WriteLine($"Единичный элемент: {one}");
        
        // Генерация случайных элементов
        T a = T.Random();
        T b = T.Random();
        
        Console.WriteLine($"Случайный элемент a: {a}");
        Console.WriteLine($"Случайный элемент b: {b}");
        
        // Арифметические операции
        T sum = a + b;
        T difference = a - b;
        T product = a * b;
        T quotient = a / b;
        
        Console.WriteLine($"a + b = {sum}");
        Console.WriteLine($"a - b = {difference}");
        Console.WriteLine($"a * b = {product}");
        Console.WriteLine($"a / b = {quotient}");
        
        // Обратный элемент
        try
        {
            T reciprocalA = a.Reciprocal;
            Console.WriteLine($"Обратный к a: {reciprocalA}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Не удалось вычислить обратный элемент: {ex.Message}");
        }
        
        // Проверка на нуль
        Console.WriteLine($"a является нулем: {a.IsZero}");
        Console.WriteLine($"zero является нулем: {zero.IsZero}");
        
        Console.WriteLine();
    }
    
    // Обобщенный метод парсинга
    public static void DemonstrateParsing<T>(string input) where T : IField<T>
    {
        try
        {
            T parsed = T.Parse(input);
            Console.WriteLine($"Успешный парсинг '{input}': {parsed}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка парсинга '{input}': {ex.Message}");
        }
    }
}
