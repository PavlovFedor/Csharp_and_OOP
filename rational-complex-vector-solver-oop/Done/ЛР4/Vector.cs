using System;
using System.Linq;
using System.Text;

public class Vector<T> where T : IField<T>
{
    private T[] components;

    // Размерность вектора
    public int Dimension => components.Length;

    // Компоненты вектора (доступ только для чтения)
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= components.Length)
                throw new IndexOutOfRangeException("Индекс выходит за границы вектора");
            return components[index];
        }        
        set
        {
            if (index < 0 || index >= components.Length)
                throw new IndexOutOfRangeException("Индекс выходит за границы вектора");
            components[index] = value;
        }
    }

    // Конструкторы
    public Vector(int dimension)
    {
        if (dimension <= 0)
            throw new ArgumentException("Размерность вектора должна быть положительной", nameof(dimension));
        
        components = new T[dimension];
        for (int i = 0; i < dimension; i++)
        {
            components[i] = T.Zero;
        }
    }

    public Vector(T[] components)
    {
        if (components == null)
            throw new ArgumentNullException(nameof(components));
        if (components.Length == 0)
            throw new ArgumentException("Вектор не может быть пустым", nameof(components));
        
        this.components = (T[])components.Clone();
    }

    // Нулевой вектор
    public static Vector<T> Zero(int dimension)
    {
        return new Vector<T>(dimension);
    }

    // Единичный вектор по заданному направлению
    public static Vector<T> BasisVector(int dimension, int direction)
    {
        if (direction < 0 || direction >= dimension)
            throw new ArgumentException("Направление должно быть в пределах размерности", nameof(direction));
        
        var vector = new Vector<T>(dimension);
        vector.components[direction] = T.One;
        return vector;
    }

    // Оператор сложения векторов
    public static Vector<T> operator +(Vector<T> a, Vector<T> b)
    {
        if (a == null || b == null)
            throw new ArgumentNullException("Векторы не могут быть null");
        if (a.Dimension != b.Dimension)
            throw new ArgumentException("Векторы должны иметь одинаковую размерность");
        
        T[] resultComponents = new T[a.Dimension];
        for (int i = 0; i < a.Dimension; i++)
        {
            resultComponents[i] = a.components[i] + b.components[i];
        }
        return new Vector<T>(resultComponents);
    }

    // Оператор вычитания векторов
    public static Vector<T> operator -(Vector<T> a, Vector<T> b)
    {
        if (a == null || b == null)
            throw new ArgumentNullException("Векторы не могут быть null");
        if (a.Dimension != b.Dimension)
            throw new ArgumentException("Векторы должны иметь одинаковую размерность");
        
        T[] resultComponents = new T[a.Dimension];
        for (int i = 0; i < a.Dimension; i++)
        {
            resultComponents[i] = a.components[i] - b.components[i];
        }
        return new Vector<T>(resultComponents);
    }

    // Оператор умножения на скаляр (справа)
    public static Vector<T> operator *(Vector<T> vector, T scalar)
    {
        if (vector == null)
            throw new ArgumentNullException(nameof(vector));
        
        T[] resultComponents = new T[vector.Dimension];
        for (int i = 0; i < vector.Dimension; i++)
        {
            resultComponents[i] = vector.components[i] * scalar;
        }
        return new Vector<T>(resultComponents);
    }

    // Оператор умножения на скаляр (слева)
    public static Vector<T> operator *(T scalar, Vector<T> vector)
    {
        return vector * scalar;
    }

    // Оператор деления на скаляр
    public static Vector<T> operator /(Vector<T> vector, T scalar)
    {
        if (vector == null)
            throw new ArgumentNullException(nameof(vector));
        if (scalar.IsZero)
            throw new DivideByZeroException("Деление на нулевой скаляр невозможно");
        
        T[] resultComponents = new T[vector.Dimension];
        for (int i = 0; i < vector.Dimension; i++)
        {
            resultComponents[i] = vector.components[i] / scalar;
        }
        return new Vector<T>(resultComponents);
    }

    // Оператор унарного минуса
    public static Vector<T> operator -(Vector<T> vector)
    {
        if (vector == null)
            throw new ArgumentNullException(nameof(vector));
        
        return vector * (-T.One);
    }

    // Векторное произведение (только для 3D)
    public static Vector<T> operator *(Vector<T> a, Vector<T> b)
    {
        if (a == null || b == null)
            throw new ArgumentNullException("Векторы не могут быть null");
        if (a.Dimension != 3 || b.Dimension != 3)
            throw new InvalidOperationException("Векторное произведение определено только для 3D векторов");
        
        T x = a.components[1] * b.components[2] - a.components[2] * b.components[1];
        T y = a.components[2] * b.components[0] - a.components[0] * b.components[2];
        T z = a.components[0] * b.components[1] - a.components[1] * b.components[0];
        
        return new Vector<T>(new T[] { x, y, z });
    }

    // Операторы сравнения
    public static bool operator ==(Vector<T> a, Vector<T> b)
    {
        if (object.ReferenceEquals(a, b)) return true;
        if ((object)a == null || (object)b == null) return false;
        if (a.Dimension != b.Dimension) return false;
        
        for (int i = 0; i < a.Dimension; i++)
        {
            if (a.components[i] != b.components[i])
                return false;
        }
        return true;
    }

    public static bool operator !=(Vector<T> a, Vector<T> b)
    {
        return !(a == b);
    }

    // Скалярное произведение
    public T Dot(Vector<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));
        if (Dimension != other.Dimension)
            throw new ArgumentException("Векторы должны иметь одинаковую размерность");
        
        T result = T.Zero;
        for (int i = 0; i < Dimension; i++)
        {
            result = result + (components[i] * other.components[i]);
        }
        return result;
    }

    // Упрощенный метод Magnitude() - использует ToDouble()
    public double Magnitude()
    {
        double sumOfSquares = 0;
        for (int i = 0; i < Dimension; i++)
        {
            double value = components[i].ToDouble();
            sumOfSquares += value * value;
        }
        return Math.Sqrt(sumOfSquares);
    }

    // Квадрат длины вектора (возвращает тип поля)
    public T MagnitudeSquared()
    {
        T sumOfSquares = T.Zero;
        for (int i = 0; i < Dimension; i++)
        {
            sumOfSquares = sumOfSquares + (components[i] * components[i]);
        }
        return sumOfSquares;
    }

    // Упрощенный метод Normalize()
    public Vector<T> Normalize()
    {
        double magnitude = Magnitude();
        if (magnitude == 0)
            throw new InvalidOperationException("Невозможно нормализовать нулевой вектор");

        // Используем статический метод FromDouble для создания скаляра
        T scalar = T.FromDouble(1.0 / magnitude);
        return this * scalar;
    }
    
    // Создание случайного вектора
    public static Vector<T> Random(int dimension)
    {
        if (dimension <= 0)
            throw new ArgumentException("Размерность должна быть положительной", nameof(dimension));
        
        T[] randomComponents = new T[dimension];
        for (int i = 0; i < dimension; i++)
        {
            randomComponents[i] = T.Random();
        }
        return new Vector<T>(randomComponents);
    }

    // Генерация случайного вектора в диапазоне
    public static Vector<T> RandomInRange(int dimension, T minComponent, T maxComponent)
    {
        if (dimension <= 0)
            throw new ArgumentException("Размерность должна быть положительной", nameof(dimension));
        
        T[] randomComponents = new T[dimension];
        for (int i = 0; i < dimension; i++)
        {
            randomComponents[i] = T.RandomInRange(minComponent, maxComponent);
        }
        return new Vector<T>(randomComponents);
    }

    // Парсинг строки
    public static Vector<T> Parse(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new ArgumentException("Строка не может быть пустой или null", nameof(s));
        
        s = s.Trim();
        
        // Проверяем формат (a1, a2, ..., an)
        if (!s.StartsWith("(") || !s.EndsWith(")"))
            throw new FormatException("Строка должна быть в формате '(a1, a2, ..., an)'");
        
        // Убираем скобки
        string inner = s.Substring(1, s.Length - 2).Trim();
        if (string.IsNullOrEmpty(inner))
            throw new FormatException("Вектор не может быть пустым");
        
        // Разделяем компоненты
        string[] componentStrings = inner.Split(',');
        T[] components = new T[componentStrings.Length];
        
        for (int i = 0; i < componentStrings.Length; i++)
        {
            string componentStr = componentStrings[i].Trim();
            try
            {
                components[i] = T.Parse(componentStr);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Ошибка парсинга компоненты {i + 1}: '{componentStr}'", ex);
            }
        }
        
        return new Vector<T>(components);
    }

    public static bool TryParse(string s, out Vector<T> result)
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

    // Преобразование в строку
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("(");
        for (int i = 0; i < components.Length; i++)
        {
            sb.Append(components[i].ToString());
            if (i < components.Length - 1)
                sb.Append(", ");
        }
        sb.Append(")");
        return sb.ToString();
    }

    // Проверка на нулевой вектор
    public bool IsZero()
    {
        foreach (var component in components)
        {
            if (!component.IsZero)
                return false;
        }
        return true;
    }

    // Реализация интерфейсов
    public override bool Equals(object obj)
    {
        return Equals(obj as Vector<T>);
    }

    public bool Equals(Vector<T> other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            foreach (var component in components)
            {
                hash = hash * 23 + component.GetHashCode();
            }
            return hash;
        }
    }

    // Возвращает клон вектора
    public Vector<T> Clone()
    {
        return new Vector<T>((T[])components.Clone());
    }

    // Проекция вектора на другой вектор
    public Vector<T> ProjectOnto(Vector<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));
        if (Dimension != other.Dimension)
            throw new ArgumentException("Векторы должны иметь одинаковую размерность");
        
        T scalar = this.Dot(other) / other.Dot(other);
        return other * scalar;
    }

    // Проверка ортогональности
    public bool IsOrthogonalTo(Vector<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));
        if (Dimension != other.Dimension)
            throw new ArgumentException("Векторы должны иметь одинаковую размерность");
        
        return this.Dot(other).IsZero;
    }
}
