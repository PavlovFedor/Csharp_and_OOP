using System;

// Класс рациональных чисел с реализацией интерфейса IField
public class RationalNumber : IField<RationalNumber>, IEquatable<RationalNumber>, IComparable<RationalNumber>
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

    // Реализация интерфейса IField
    public static RationalNumber Zero
    {
        get { return new RationalNumber(0, 1); }
    }
    
    public static RationalNumber One
    {
        get { return new RationalNumber(1, 1); }
    }
    
    public RationalNumber Reciprocal
    {
        get
        {
            if (Numerator == 0)
                throw new InvalidOperationException("Невозможно вычислить обратный элемент для нуля");
            return new RationalNumber(Denominator, Numerator);
        }
    }
    
    public bool IsZero
    {
        get { return Numerator == 0; }
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

        if (Denominator < 0)
        {
            Numerator = -Numerator;
            Denominator = -Denominator;
        }
    }

    // Наибольший общий делитель
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

        if (s.Contains("/"))
        {
            string[] parts = s.Split('/');
            if (parts.Length != 2)
                throw new FormatException("Неверный формат строки. Ожидается формат 'a/b'");

            if (int.TryParse(parts[0].Trim(), out int numerator) && 
                int.TryParse(parts[1].Trim(), out int denominator))
            {
                return new RationalNumber(numerator, denominator);
            }
        }
        else if (s.Contains(":"))
        {
            string[] parts = s.Split(':');
            if (parts.Length != 2)
                throw new FormatException("Неверный формат строки. Ожидается формат 'a:b'");

            if (int.TryParse(parts[0].Trim(), out int numerator) && 
                int.TryParse(parts[1].Trim(), out int denominator))
            {
                return new RationalNumber(numerator, denominator);
            }
        }
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

    // Генерация случайного числа
    public static RationalNumber Random()
    {
        var random = new Random();
        int numerator = random.Next(-10, 11);
        int denominator = random.Next(1, 11); // Знаменатель всегда положительный
        return new RationalNumber(numerator, denominator);
    }
    
    // Генерация в диапазоне
    public static RationalNumber RandomInRange(RationalNumber min, RationalNumber max)
    {
        if (min.CompareTo(max) > 0)
            throw new ArgumentException("Минимальное значение должно быть меньше или равно максимальному");

        var random = new Random();
        
        // Генерируем случайный знаменатель
        int denominator = random.Next(1, 21); // от 1 до 20
        
        // Вычисляем диапазоны для числителя
        int minNumerator = min.Numerator * denominator / min.Denominator;
        int maxNumerator = max.Numerator * denominator / max.Denominator;
        
        // Корректируем границы с учетом знаменателя
        minNumerator = (int)Math.Ceiling(min.ToDouble() * denominator);
        maxNumerator = (int)Math.Floor(max.ToDouble() * denominator);
        
        if (minNumerator >= maxNumerator)
        {
            // Если не удалось подобрать подходящий числитель, генерируем новый знаменатель
            return RandomInRange(min, max);
        }
        
        int numerator = random.Next(minNumerator, maxNumerator + 1);
        return new RationalNumber(numerator, denominator);
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
        if (object.ReferenceEquals(a, b)) return true;
        if ((object)a == null || (object)b == null) return false;
        return a.Numerator == b.Numerator && a.Denominator == b.Denominator;
    }

    public static bool operator !=(RationalNumber a, RationalNumber b)
    {
        return !(a == b);
    }

    // Преобразование в double
    public double ToDouble()
    {
        return (double)Numerator / Denominator;
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

    public override int GetHashCode()
    {
        unchecked
        {
            return (Numerator * 397) ^ Denominator;
        }
    }

    public int CompareTo(RationalNumber other)
    {
        if ((object)other == null) return 1;
        long crossProduct1 = (long)Numerator * other.Denominator;
        long crossProduct2 = (long)other.Numerator * Denominator;
        return crossProduct1.CompareTo(crossProduct2);
    }
}
