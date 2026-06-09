using System;
using System.Globalization;
using System.Text.RegularExpressions;

public class ComplexNumber : IEquatable<ComplexNumber>
{
    public double Real { get; private set; }
    public double Imaginary { get; private set; }

    // Конструкторы
    public ComplexNumber(double real, double imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    public ComplexNumber(double real) : this(real, 0) { }

    public ComplexNumber() : this(0, 0) { }

    // Нулевой элемент
    public static ComplexNumber Zero
    {
        get { return new ComplexNumber(0, 0); }
    }
    
    // Единичный элемент
    public static ComplexNumber One
    {
        get { return new ComplexNumber(1, 0); }
    }
    
    // Мнимая единица
    public static ComplexNumber ImaginaryOne
    {
        get { return new ComplexNumber(0, 1); }
    }
    
    // Обратный элемент (мультипликативно обратный)
    public ComplexNumber Reciprocal
    {
        get
        {
            if (Real == 0 && Imaginary == 0)
                throw new InvalidOperationException("Невозможно вычислить обратный элемент для нуля");
                
            double denominator = Real * Real + Imaginary * Imaginary;
            return new ComplexNumber(Real / denominator, -Imaginary / denominator);
        }
    }

    // Модуль комплексного числа
    public double Magnitude
    {
        get { return Math.Sqrt(Real * Real + Imaginary * Imaginary); }
    }

    // Аргумент комплексного числа (в радианах)
    public double Argument
    {
        get
        {
            if (Real == 0 && Imaginary == 0)
                throw new InvalidOperationException("Аргумент не определен для нулевого комплексного числа");
                
            return Math.Atan2(Imaginary, Real);
        }
    }

    // Сопряженное комплексное число
    public ComplexNumber Conjugate
    {
        get { return new ComplexNumber(Real, -Imaginary); }
    }

    // Парсинг строк
    public static ComplexNumber Parse(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new ArgumentException("Строка не может быть пустой или null", nameof(s));
    
        s = s.Trim().ToLower().Replace(" ", "");
    
        // Обработка случая "i" или "-i"
        if (s == "i") return new ComplexNumber(0, 1);
        if (s == "-i") return new ComplexNumber(0, -1);
    
        // Обработка случая только мнимой части (например, "4i", "-2i")
        if (s.EndsWith("i") && !s.Contains("+") && !s.Substring(0, s.Length - 1).Contains("-"))
        {
            string imaginaryPart = s.Substring(0, s.Length - 1);
            // Обработка случая "+i" или "-i"
            if (imaginaryPart == "+") return new ComplexNumber(0, 1);
            if (imaginaryPart == "-") return new ComplexNumber(0, -1);
            if (double.TryParse(imaginaryPart, NumberStyles.Any, CultureInfo.InvariantCulture, out double imaginary))
            {
                return new ComplexNumber(0, imaginary);
            }
        }
    
        // Обработка общего случая "a+bi" или "a-bi"
        // Ищем разделитель, но не в начале строки (для отрицательных чисел)
        int plusIndex = s.IndexOf('+', 1); // начинаем поиск с позиции 1
        int minusIndex = s.IndexOf('-', 1); // начинаем поиск с позиции 1
        
        int separatorIndex = -1;
        if (plusIndex > 0) separatorIndex = plusIndex;
        else if (minusIndex > 0) separatorIndex = minusIndex;
    
        if (separatorIndex > 0 && s.EndsWith("i")) // Разделитель найден и строка заканчивается на i
        {
            string realPartStr = s.Substring(0, separatorIndex);
            string imaginaryPartStr = s.Substring(separatorIndex, s.Length - separatorIndex - 1); // убираем "i" в конце
            
            // Обработка случаев "+i" и "-i"
            if (imaginaryPartStr == "+") return new ComplexNumber(double.Parse(realPartStr, CultureInfo.InvariantCulture), 1);
            if (imaginaryPartStr == "-") return new ComplexNumber(double.Parse(realPartStr, CultureInfo.InvariantCulture), -1);
    
            if (double.TryParse(realPartStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double real) &&
                double.TryParse(imaginaryPartStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double imaginary))
            {
                return new ComplexNumber(real, imaginary);
            }
        }
        else if (s.EndsWith("i")) // Только мнимая часть с возможным знаком в начале
        {
            string imaginaryPartStr = s.Substring(0, s.Length - 1);
            if (imaginaryPartStr == "+") return new ComplexNumber(0, 1);
            if (imaginaryPartStr == "-") return new ComplexNumber(0, -1);
            if (double.TryParse(imaginaryPartStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double imaginary))
            {
                return new ComplexNumber(0, imaginary);
            }
        }
        else
        {
            // Пробуем распарсить как вещественное число
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double real))
            {
                return new ComplexNumber(real, 0);
            }
        }
    
        throw new FormatException("Неверный формат строки. Ожидается формат 'a+bi', 'a-bi', 'bi' или вещественное число");
    }
/*
double.TryParse(realPartStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double real)
Параметры:
    realPartStr - строка, которую нужно преобразовать в число
    NumberStyles.Any - разрешает любые числовые форматы (целые, дробные, научная нотация и т.д.)
    CultureInfo.InvariantCulture - использует инвариантную культуру (точка как разделитель дробной части)
    out double real - выходной параметр для результата преобразования
Логика работы:
    Пытается преобразовать строку realPartStr в число типа double
    Возвращает true если преобразование успешно, и false если нет
    При успехе записывает результат в переменную real
    Использует инвариантную культуру, что гарантирует:
        -Разделитель дробной части: . (точка)
        -Разделитель тысяч: не используется
        -Формат: 123.45, -67.89, 1.23e+5
*/
    // Арифметические операторы
    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
    {
        return new ComplexNumber(a.Real + b.Real, a.Imaginary + b.Imaginary);
    }

    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
    {
        return new ComplexNumber(a.Real - b.Real, a.Imaginary - b.Imaginary);
    }

    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
    {
        double real = a.Real * b.Real - a.Imaginary * b.Imaginary;
        double imaginary = a.Real * b.Imaginary + a.Imaginary * b.Real;
        return new ComplexNumber(real, imaginary);
    }

    public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
    {
        if (b.Real == 0 && b.Imaginary == 0)
            throw new DivideByZeroException("Деление на ноль невозможно");

        double denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
        double real = (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator;
        double imaginary = (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator;
        return new ComplexNumber(real, imaginary);
    }

    public static ComplexNumber operator -(ComplexNumber a)
    {
        return new ComplexNumber(-a.Real, -a.Imaginary);
    }

    // Операторы сравнения
    public static bool operator ==(ComplexNumber a, ComplexNumber b)
    {
        if (object.ReferenceEquals(a, b)) return true;
        if ((object)a == null || (object)b == null) return false;
        return a.Real == b.Real && a.Imaginary == b.Imaginary;
    }

    public static bool operator !=(ComplexNumber a, ComplexNumber b)
    {
        return !(a == b);
    }

    // Возведение в степень
    public ComplexNumber Power(int exponent)
    {
        if (exponent == 0) return One;
        if (exponent == 1) return this;
        
        ComplexNumber result = this;
        for (int i = 1; i < Math.Abs(exponent); i++)
        {
            result = result * this;
        }
        
        return exponent > 0 ? result : result.Reciprocal;
    }

    // Извлечение корня
    public static ComplexNumber[] Roots(ComplexNumber number, int n)
    {
        if (n <= 0)
            throw new ArgumentException("Степень корня должна быть положительным числом", nameof(n));

        double magnitude = Math.Pow(number.Magnitude, 1.0 / n);
        double argument = number.Argument;
        
        ComplexNumber[] roots = new ComplexNumber[n];
        for (int k = 0; k < n; k++)
        {
            double real = magnitude * Math.Cos((argument + 2 * Math.PI * k) / n);
            double imaginary = magnitude * Math.Sin((argument + 2 * Math.PI * k) / n);
            roots[k] = new ComplexNumber(real, imaginary);
        }
        
        return roots;
    }

    // Генерация случайного комплексного числа
    public static ComplexNumber Random(Random random = null)
    {
        Random rnd = random ?? new Random();
        double real = rnd.NextDouble() * 10 - 5; // от -5 до 5
        double imaginary = rnd.NextDouble() * 10 - 5; // от -5 до 5
        return new ComplexNumber(real, imaginary);
    }

    // Генерация случайного комплексного числа в заданном диапазоне
    public static ComplexNumber RandomInRange(double minReal, double maxReal, double minImaginary, double maxImaginary, Random random = null)
    {
        if (minReal > maxReal || minImaginary > maxImaginary)
            throw new ArgumentException("Минимальные значения должны быть меньше максимальных");

        Random rnd = random ?? new Random();
        double real = minReal + rnd.NextDouble() * (maxReal - minReal);
        double imaginary = minImaginary + rnd.NextDouble() * (maxImaginary - minImaginary);
        return new ComplexNumber(real, imaginary);
    }

    // Преобразование в строку
    public override string ToString()
    {
        if (Imaginary == 0) return Real.ToString("F2", CultureInfo.InvariantCulture);
        if (Real == 0) return Imaginary.ToString("F2", CultureInfo.InvariantCulture) + "i";
        
        string sign = Imaginary > 0 ? "+" : "-";
        double absImaginary = Math.Abs(Imaginary);
        
        return $"{Real:F2}{sign}{absImaginary:F2}i";
    }

    // Реализация интерфейсов
    public bool Equals(ComplexNumber other)
    {
        if ((object)other == null) return false;
        return Real == other.Real && Imaginary == other.Imaginary;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as ComplexNumber);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Real.GetHashCode();
            hash = hash * 23 + Imaginary.GetHashCode();
            return hash;
        }
    }

    // Явные преобразования
    public static explicit operator ComplexNumber(double real)
    {
        return new ComplexNumber(real);
    }
}

