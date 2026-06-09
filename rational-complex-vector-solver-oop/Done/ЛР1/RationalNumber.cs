using System;
using System.Globalization;

public class RationalNumber : IEquatable<RationalNumber>, IComparable<RationalNumber>
{
    public int Numerator { get; private set; }
    public int Denominator { get; private set; }

    // Конструкторы
    public RationalNumber(int numerator, int denominator)
    {
        if (denominator == 0)
            throw new ArgumentException("Знаменатель не может быть равен нулю", nameof(denominator));

        Numerator = numerator;
        Denominator = denominator;
        Simplify();
    }

    public RationalNumber(int wholeNumber) : this(wholeNumber, 1) { }

    public RationalNumber() : this(0, 1) { }

    // Нулевой элемент
    public static RationalNumber Zero
    {
        get { return new RationalNumber(0, 1); }
    }
    
    // Единичный элемент
    public static RationalNumber One
    {
        get { return new RationalNumber(1, 1); }
    }
    
    // Обратный элемент
    public RationalNumber Reciprocal
    {
        get { return new RationalNumber(Denominator, Numerator); }
    }

    // Сокращение дроби
    private void Simplify()
    {
        if (Numerator == 0)
        {
            Denominator = 1;
            return;
        }

        int gcd = GCD(Math.Abs(Numerator), Math.Abs(Denominator));
        Numerator /= gcd;
        Denominator /= gcd;

        // Обеспечиваем положительный знаменатель
        if (Denominator < 0)
        {
            Numerator = -Numerator;
            Denominator = -Denominator;
        }
    }

    // Наибольший общий делитель (алгоритм Евклида)
    private static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // Парсинг строк
    public static RationalNumber Parse(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new ArgumentException("Строка не может быть пустой или null", nameof(s));

        // Парсинг формата "a/b"
        if (s.Contains("/"))
        {
            string[] parts = s.Split('/');
            if (parts.Length != 2)// Числитель и знаменатель
                throw new FormatException("Неверный формат строки. Ожидается формат 'a/b' или 'a:b'");

            if (int.TryParse(parts[0].Trim(), out int numerator) &&//Trim удаляет пробелы 
                int.TryParse(parts[1].Trim(), out int denominator))
            {
                return new RationalNumber(numerator, denominator);
            }
        }
        // Парсинг формата "a:b"
        else if (s.Contains(":"))
        {
            string[] parts = s.Split(':');
            if (parts.Length != 2)
                throw new FormatException("Неверный формат строки. Ожидается формат 'a/b' или 'a:b'");

            if (int.TryParse(parts[0].Trim(), out int numerator) && 
                int.TryParse(parts[1].Trim(), out int denominator))
            {
                return new RationalNumber(numerator, denominator);
            }
        }
        // Парсинг целого числа
        else
        {
            if (int.TryParse(s.Trim(), out int wholeNumber))
            {
                return new RationalNumber(wholeNumber);
            }
        }

        throw new FormatException("Неверный формат строки. Ожидается формат 'a/b', 'a:b' или целое число");
    }

    public static bool TryParse(string s, out RationalNumber result)
    {
        result = null;
        try
        {
            result = Parse(s);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Арифметические операторы
    public static RationalNumber operator +(RationalNumber a, RationalNumber b)
    {
        int commonDenominator = a.Denominator * b.Denominator;
        int numerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
        return new RationalNumber(numerator, commonDenominator);
    }

    public static RationalNumber operator -(RationalNumber a, RationalNumber b)
    {
        int commonDenominator = a.Denominator * b.Denominator;
        int numerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
        return new RationalNumber(numerator, commonDenominator);
    }

    public static RationalNumber operator *(RationalNumber a, RationalNumber b)
    {
        return new RationalNumber(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    }

    public static RationalNumber operator /(RationalNumber a, RationalNumber b)
    {
        if (b.Numerator == 0)
            throw new DivideByZeroException("Деление на ноль невозможно");

        return new RationalNumber(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
    }

    public static RationalNumber operator -(RationalNumber a)
    {
        return new RationalNumber(-a.Numerator, a.Denominator);
    }

    // Операторы сравнения
    public static bool operator ==(RationalNumber a, RationalNumber b)
    {
        if (object.ReferenceEquals(a, b)) return true;//Проверка: ссылаются ли на один и тот же объект
        if ((object)a == null || (object)b == null) return false;
        return a.Numerator == b.Numerator && a.Denominator == b.Denominator;
    }

    public static bool operator !=(RationalNumber a, RationalNumber b)
    {
        return !(a == b);
    }

    public static bool operator <(RationalNumber a, RationalNumber b)
    {
        return a.CompareTo(b) < 0;
    }

    public static bool operator >(RationalNumber a, RationalNumber b)
    {
        return a.CompareTo(b) > 0;
    }

    public static bool operator <=(RationalNumber a, RationalNumber b)
    {
        return a.CompareTo(b) <= 0;
    }

    public static bool operator >=(RationalNumber a, RationalNumber b)
    {
        return a.CompareTo(b) >= 0;
    }

    // Преобразование в double
    public double ToDouble()
    {
        return (double)Numerator / Denominator;
    }

    // Генерация случайного числа на интервале
    public static RationalNumber RandomInRange(RationalNumber min, RationalNumber max, Random random = null)
    {
        if (min >= max)
            throw new ArgumentException("min должен быть меньше max");

        Random rnd = random ?? new Random();
        
        // Преобразуем в double для генерации
        double minValue = min.ToDouble();
        double maxValue = max.ToDouble();
        double randomValue = minValue + rnd.NextDouble() * (maxValue - minValue);

        // Преобразуем обратно в RationalNumber с некоторой точностью
        return FromDouble(randomValue, 1000000);
    }

    // Преобразование double в RationalNumber с заданной точностью
    private static RationalNumber FromDouble(double value, int maxDenominator)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Значение должно быть конечным числом");

        int sign = Math.Sign(value);
        value = Math.Abs(value);

        int bestNumerator = 0;
        int bestDenominator = 1;
        double bestError = double.MaxValue;

        for (int denominator = 1; denominator <= maxDenominator; denominator++)
        {
            int numerator = (int)Math.Round(value * denominator);
            double error = Math.Abs(value - (double)numerator / denominator);

            if (error < bestError)
            {
                bestError = error;
                bestNumerator = numerator;
                bestDenominator = denominator;
            }
        }

        return new RationalNumber(sign * bestNumerator, bestDenominator);
    }

    // Преобразование в строку
    public override string ToString()
    {
        if (Denominator == 1)
            return Numerator.ToString();
        
        return $"{Numerator}/{Denominator}";
    }

    // Реализация интерфейсов
    public bool Equals(RationalNumber other)
    {
        if ((object)other == null) return false;
        return Numerator == other.Numerator && Denominator == other.Denominator;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as RationalNumber);
    }

    public int CompareTo(RationalNumber other)
    {
        if ((object)other == null) return 1;
        
        long crossProduct1 = (long)Numerator * other.Denominator;
        long crossProduct2 = (long)other.Numerator * Denominator;
        
        return crossProduct1.CompareTo(crossProduct2);
    }

    // Явные преобразования
    public static explicit operator double(RationalNumber r)
    {
        return r.ToDouble();
    }
    
    public static explicit operator RationalNumber(int i)
    {
        return new RationalNumber(i);
    }

    //Хеш не нужен по условию, но с ним прятней глазу
    //Объяснение его необходимости от нейронки:
    //Без GetHashCode() ваши объекты будут некорректно работать в хэш-коллекциях, 
    //что приведет к трудноотлаживаемым ошибкам и значительному падению производительности.
    public override int GetHashCode()
    {
        unchecked// вычисления без проверки переполнения
        {//Умножение на простое число лучше распределяет хэши(магическое 397)
            return (Numerator * 397) ^ Denominator;
        }
    }
}
