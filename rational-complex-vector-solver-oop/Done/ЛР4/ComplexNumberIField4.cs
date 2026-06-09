using System;

// Класс комплексных чисел с реализацией интерфейса IField
public class ComplexNumber : IField<ComplexNumber>, IEquatable<ComplexNumber>
{
    public double Real { get; private set; }
    public double Imaginary { get; private set; }

    // Конструкторы
    public ComplexNumber(double real, double imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    // Реализация нового метода ToDouble()
    public double ToDouble()
    {
        return Magnitude;
    }

    public ComplexNumber(double real) : this(real, 0) { }

    public ComplexNumber() : this(0, 0) { }

    // Реализация интерфейса IField
    public static ComplexNumber Zero
    {
        get { return new ComplexNumber(0, 0); }
    }
    
    public static ComplexNumber One
    {
        get { return new ComplexNumber(1, 0); }
    }
    
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
    
    public bool IsZero
    {
        get { return Real == 0 && Imaginary == 0; }
    }

    // Мнимая единица
    public static ComplexNumber ImaginaryOne
    {
        get { return new ComplexNumber(0, 1); }
    }

    // Реализация нового метода FromDouble
    public static ComplexNumber FromDouble(double value)
    {
        return new ComplexNumber(value, 0);
    }


    public double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);


    public double Argument
    {
        get
        {
            if (Real == 0 && Imaginary == 0)
                throw new InvalidOperationException("Аргумент не определен для нуля");
            return Math.Atan2(Imaginary, Real);
        }
    }

    // Сопряженное число
    public ComplexNumber Conjugate
    {
        get { return new ComplexNumber(Real, -Imaginary); }
    }

    public static ComplexNumber Parse(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new ArgumentException("Строка не может быть пустой или null", nameof(s));
    
        s = s.Trim().ToLower().Replace(" ", "");
    
        if (s == "i") return new ComplexNumber(0, 1);
        if (s == "-i") return new ComplexNumber(0, -1);
    
        // Обработка случая только мнимой части (например, "4i", "-2i")
        if (s.EndsWith("i") && !s.Contains("+") && !s.Substring(0, s.Length - 1).Contains("-"))
        {
            string imaginaryPart = s.Substring(0, s.Length - 1);
            if (imaginaryPart == "+") return new ComplexNumber(0, 1);
            if (imaginaryPart == "-") return new ComplexNumber(0, -1);
            if (double.TryParse(imaginaryPart, out double imaginary))
            {
                return new ComplexNumber(0, imaginary);
            }
        }
    
        // Обработка общего случая "a+bi" или "a-bi"
        int plusIndex = s.IndexOf('+', 1);
        int minusIndex = s.IndexOf('-', 1);
        
        int separatorIndex = -1;
        if (plusIndex > 0) separatorIndex = plusIndex;
        else if (minusIndex > 0) separatorIndex = minusIndex;
    
        if (separatorIndex > 0 && s.EndsWith("i"))
        {
            string realPartStr = s.Substring(0, separatorIndex);
            string imaginaryPartStr = s.Substring(separatorIndex, s.Length - separatorIndex - 1);
            
            if (imaginaryPartStr == "+") return new ComplexNumber(double.Parse(realPartStr), 1);
            if (imaginaryPartStr == "-") return new ComplexNumber(double.Parse(realPartStr), -1);
    
            if (double.TryParse(realPartStr, out double real) &&
                double.TryParse(imaginaryPartStr, out double imaginary))
            {
                return new ComplexNumber(real, imaginary);
            }
        }
        else if (s.EndsWith("i"))
        {
            string imaginaryPartStr = s.Substring(0, s.Length - 1);
            if (imaginaryPartStr == "+") return new ComplexNumber(0, 1);
            if (imaginaryPartStr == "-") return new ComplexNumber(0, -1);
            if (double.TryParse(imaginaryPartStr, out double imaginary))
            {
                return new ComplexNumber(0, imaginary);
            }
        }
        else
        {
            if (double.TryParse(s, out double real))
            {
                return new ComplexNumber(real, 0);
            }
        }
    
        throw new FormatException("Неверный формат строки. Ожидается формат 'a+bi', 'a-bi', 'bi' или вещественное число");
    }

    public static bool TryParse(string s, out ComplexNumber result)
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
    public static ComplexNumber Random()
    {
        var random = new Random();
        double real = random.NextDouble() * 10 - 5;
        double imaginary = random.NextDouble() * 10 - 5;
        return new ComplexNumber(real, imaginary);
    }
    
    // НОВЫЙ МЕТОД: генерация в диапазоне
    public static ComplexNumber RandomInRange(ComplexNumber min, ComplexNumber max)
    {
        var random = new Random();
        
        // Генерируем действительную и мнимую части в соответствующих диапазонах
        double real = min.Real + random.NextDouble() * (max.Real - min.Real);
        double imaginary = min.Imaginary + random.NextDouble() * (max.Imaginary - min.Imaginary);
        
        return new ComplexNumber(real, imaginary);
    }

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
    
    // Добавляем константу для точности сравнения
    private const double Tolerance = 1e-10;
    // Операторы сравнения
    public static bool operator ==(ComplexNumber a, ComplexNumber b)
    {
        if (object.ReferenceEquals(a, b)) return true;
        if ((object)a == null || (object)b == null) return false;
        
        return Math.Abs(a.Real - b.Real) < Tolerance && 
               Math.Abs(a.Imaginary - b.Imaginary) < Tolerance;
    }

    public static bool operator !=(ComplexNumber a, ComplexNumber b)
    {
        return !(a == b);
    }

    // Преобразование в строку
    public override string ToString()
    {
        if (Imaginary == 0) return Real.ToString("F2");
        if (Real == 0) return Imaginary.ToString("F2") + "i";
        
        string sign = Imaginary > 0 ? "+" : "-";
        double absImaginary = Math.Abs(Imaginary);
        return $"{Real:F2}{sign}{absImaginary:F2}i";
    }

    // Реализация интерфейсов
    public bool Equals(ComplexNumber other)
    {
        if ((object)other == null) return false;
        return Math.Abs(Real - other.Real) < Tolerance && 
               Math.Abs(Imaginary - other.Imaginary) < Tolerance;
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
}
