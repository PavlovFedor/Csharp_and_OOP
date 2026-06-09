using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== СИСТЕМЫ ЛИНЕЙНЫХ УРАВНЕНИЙ ===");
        Console.WriteLine();

        Console.WriteLine("=== С РАЦИОНАЛЬНЫМИ ЧИСЛАМИ ===");
        DemonstrateLinearSystem<RationalNumber>();

        Console.WriteLine("=== С КОМПЛЕКСНЫМИ ЧИСЛАМИ ===");
        DemonstrateLinearSystem<ComplexNumber>();

        //Console.WriteLine("=== ТЕСТИРОВАНИЕ РЕШЕНИЙ ===");
        //TestLinearSystemSolutions();
    }

    static void DemonstrateLinearSystem<T>() where T : IField<T>
    {
        try
        {
            // Создаем систему 2x2 или 3х3
            var system = LinearSystem<T>.CreateWithUniqueSolution(3);
            
            Console.WriteLine("Исходная система:");
            system.PrintSystem();
            Console.WriteLine();

            // Проверяем существование решения
            var solutionType = system.CheckSolutionExistence();
            Console.WriteLine($"Тип решения: {solutionType}");
            Console.WriteLine();

            if (solutionType == SolutionType.UniqueSolution)
            {
                // Решаем систему
                var solution = system.Solve();
                Console.WriteLine($"Решение: {solution}");
                
                // Проверяем точность
                var error = system.CheckSolution(solution);
                Console.WriteLine($"Максимальная ошибка: {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        Console.WriteLine();
    }
}
