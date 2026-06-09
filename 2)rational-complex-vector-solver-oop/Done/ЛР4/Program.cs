using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ КЛАССА ВЕКТОРА ===");
        Console.WriteLine();
        
        Console.WriteLine("=== ВЕКТОРЫ С РАЦИОНАЛЬНЫМИ ЧИСЛАМИ ===");
        DemonstrateVectorOperations<RationalNumber>();
        
        Console.WriteLine("=== ВЕКТОРЫ С КОМПЛЕКСНЫМИ ЧИСЛАМИ ===");
        DemonstrateVectorOperations<ComplexNumber>();
        
        Console.WriteLine("=== ВЕКТОРНОЕ ПРОИЗВЕДЕНИЕ (3D) ===");
        DemonstrateCrossProduct();
        
        Console.WriteLine("=== ГЕНЕРАЦИЯ ВЕКТОРОВ В ДИАПАЗОНЕ ===");
        DemonstrateVectorRangeGeneration();
        
        Console.WriteLine("=== ПАРСИНГ ВЕКТОРОВ ===");
        DemonstrateVectorParsing();
    }

    static void DemonstrateVectorOperations<T>() where T : IField<T>
    {
        var v1 = Vector<T>.Random(3);
        var v2 = Vector<T>.Random(3);
        var scalar = T.Random();
        
        Console.WriteLine($"v1 = {v1}");
        Console.WriteLine($"v2 = {v2}");
        Console.WriteLine($"scalar = {scalar}");
        Console.WriteLine();
        
        Console.WriteLine($"v1 + v2 = {v1 + v2}");
        Console.WriteLine($"v1 - v2 = {v1 - v2}");
        Console.WriteLine($"v1 * scalar = {v1 * scalar}");
        Console.WriteLine($"scalar * v2 = {scalar * v2}");
        Console.WriteLine($"-v1 = {-v1}");
        Console.WriteLine();
        
        Console.WriteLine($"v1 ⋅ v2 = {v1.Dot(v2)}");
        Console.WriteLine($"||v1||² = {v1.MagnitudeSquared()}");
        Console.WriteLine($"||v1|| = {v1.Magnitude():F4}");
        Console.WriteLine($"||v2|| = {v2.Magnitude():F4}");
        Console.WriteLine();
        
        Console.WriteLine($"v1 == v2: {v1 == v2}");
        Console.WriteLine($"v1 != v2: {v1 != v2}");
        Console.WriteLine($"v1 == v1: {v1 == v1.Clone()}");
        Console.WriteLine();
        
        var zero = Vector<T>.Zero(3);
        var basis = Vector<T>.BasisVector(3, 0);
        Console.WriteLine($"Нулевой вектор: {zero}");
        Console.WriteLine($"Базисный вектор: {basis}");
        Console.WriteLine($"v1 является нулевым: {v1.IsZero()}");
        Console.WriteLine($"zero является нулевым: {zero.IsZero()}");
        Console.WriteLine();
    }

    static void DemonstrateCrossProduct()
    {
        var v1 = new Vector<RationalNumber>(new RationalNumber[] {
            new RationalNumber(1, 1), new RationalNumber(2, 1), new RationalNumber(3, 1)
        });
        var v2 = new Vector<RationalNumber>(new RationalNumber[] {
            new RationalNumber(4, 1), new RationalNumber(5, 1), new RationalNumber(6, 1)
        });
        
        Console.WriteLine($"v1 = {v1}");
        Console.WriteLine($"v2 = {v2}");
        Console.WriteLine($"v1 × v2 = {v1 * v2}");
        Console.WriteLine();
    }

    static void DemonstrateVectorRangeGeneration()
    {
        Console.WriteLine("--- РАЦИОНАЛЬНЫЕ ВЕКТОРЫ В ДИАПАЗОНЕ ---");
        var rationalMin = new RationalNumber(1, 4);
        var rationalMax = new RationalNumber(3, 2);
        Console.WriteLine($"Диапазон компонент: [{rationalMin}, {rationalMax}]");
        
        for (int i = 0; i < 3; i++)
        {
            var randomVector = Vector<RationalNumber>.RandomInRange(3, rationalMin, rationalMax);
            Console.WriteLine($"  Вектор {i + 1}: {randomVector}");
            Console.WriteLine($"         Длина: {randomVector.Magnitude():F4}");
        }
        Console.WriteLine();

        Console.WriteLine("--- КОМПЛЕКСНЫЕ ВЕКТОРЫ В ДИАПАЗОНЕ ---");
        var complexMin = new ComplexNumber(-2, -1);
        var complexMax = new ComplexNumber(2, 1);
        Console.WriteLine($"Диапазон компонент: [{complexMin}, {complexMax}]");
        
        for (int i = 0; i < 3; i++)
        {
            var randomVector = Vector<ComplexNumber>.RandomInRange(3, complexMin, complexMax);
            Console.WriteLine($"  Вектор {i + 1}: {randomVector}");
            Console.WriteLine($"         Длина: {randomVector.Magnitude():F4}");
        }
        Console.WriteLine();

        Console.WriteLine("=== ВЕКТОРЫ РАЗНЫХ РАЗМЕРНОСТЕЙ ===");
        var smallMin = new RationalNumber(1, 10);
        var smallMax = new RationalNumber(1, 1);
        
        Console.WriteLine($"2D вектор в диапазоне [{smallMin}, {smallMax}]:");
        var vector2D = Vector<RationalNumber>.RandomInRange(2, smallMin, smallMax);
        Console.WriteLine($"  {vector2D}, длина: {vector2D.Magnitude():F4}");
        
        Console.WriteLine($"4D вектор в диапазоне [{smallMin}, {smallMax}]:");
        var vector4D = Vector<RationalNumber>.RandomInRange(4, smallMin, smallMax);
        Console.WriteLine($"  {vector4D}, длина: {vector4D.Magnitude():F4}");
        Console.WriteLine();
    }

    static void DemonstrateVectorParsing()
    {
        try
        {
            var rationalVec = Vector<RationalNumber>.Parse("(1/2, 3/4, 5/6)");
            Console.WriteLine($"Рациональный вектор: {rationalVec}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка парсинга рационального вектора: {ex.Message}");
        }

        try
        {
            var complexVec = Vector<ComplexNumber>.Parse("(2+3i, -4i, 5)");
            Console.WriteLine($"Комплексный вектор: {complexVec}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка парсинга комплексного вектора: {ex.Message}");
        }

        try
        {
            var invalid = Vector<RationalNumber>.Parse("(1/2, invalid, 3/4)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ожидаемая ошибка парсинга: {ex.Message}");
        }
        Console.WriteLine();
    }
}
