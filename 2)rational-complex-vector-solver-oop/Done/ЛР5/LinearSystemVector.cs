using System;
using System.Linq;

// Тип решения системы
public enum SolutionType
{
    NoSolution, // Нет решений
    UniqueSolution, // Единственное решение
    InfiniteSolutions // Бесконечно много решений
}

// Класс системы линейных уравнений с обобщенным типом
public class LinearSystem<T> where T : IField<T>
{
    private Vector<T>[] matrixA; // Матрица коэффициентов (массив векторов-строк)
    private Vector<T> vectorB; // Вектор свободных членов

    public int EquationsCount => matrixA.Length;
    public int VariablesCount => matrixA[0].Dimension;

    public LinearSystem(Vector<T>[] matrixA, Vector<T> vectorB)
    {
        ValidateSystem(matrixA, vectorB);
        this.matrixA = matrixA;
        this.vectorB = vectorB;
    }

    private void ValidateSystem(Vector<T>[] matrixA, Vector<T> vectorB)
    {
        if (matrixA == null || matrixA.Length == 0)
            throw new ArgumentException("Матрица не может быть пустой", nameof(matrixA));

        if (vectorB == null)
            throw new ArgumentException("Вектор свободных членов не может быть null", nameof(vectorB));

        int variables = matrixA[0].Dimension;
        for (int i = 0; i < matrixA.Length; i++)
        {
            if (matrixA[i].Dimension != variables)
                throw new ArgumentException("Все строки матрицы должны иметь одинаковую размерность");
        }

        if (vectorB.Dimension != matrixA.Length)
            throw new ArgumentException("Размерность вектора свободных членов должна совпадать с количеством уравнений");
    }

    // Проверка существования и единственности решения
    public SolutionType CheckSolutionExistence()
    {
        int rankA = GetMatrixRank(matrixA);
        int rankAB = GetAugmentedMatrixRank();

        if (rankA != rankAB)
            return SolutionType.NoSolution;
        else if (rankA == VariablesCount)
            return SolutionType.UniqueSolution;
        else
            return SolutionType.InfiniteSolutions;
    }

    // Решение системы методом Гаусса
    public Vector<T> Solve()
    {
        SolutionType solutionType = CheckSolutionExistence();
        if (solutionType == SolutionType.NoSolution)
            throw new InvalidOperationException("Система не имеет решений");

        if (solutionType == SolutionType.InfiniteSolutions)
            throw new InvalidOperationException("Система имеет бесконечное множество решений");

        // Создаем копии для преобразований
        Vector<T>[] aCopy = matrixA.Select(v => v.Clone()).ToArray();
        Vector<T> bCopy = vectorB.Clone();

        // Прямой ход метода Гаусса
        for (int i = 0; i < Math.Min(VariablesCount, EquationsCount); i++)
        {
            // Поиск ведущего элемента
            int pivotRow = i;
            T maxVal = aCopy[i][i];
            
            for (int j = i + 1; j < EquationsCount; j++)
            {
                T currentVal = aCopy[j][i];
                if (CompareAbsolute(currentVal, maxVal) > 0)
                {
                    maxVal = currentVal;
                    pivotRow = j;
                }
            }

            if (maxVal.IsZero)
                continue; // Пропускаем нулевой столбец

            // Перестановка строк
            if (pivotRow != i)
            {
                (aCopy[i], aCopy[pivotRow]) = (aCopy[pivotRow], aCopy[i]);
                (bCopy[i], bCopy[pivotRow]) = (bCopy[pivotRow], bCopy[i]);
            }

            // Нормализация ведущей строки
            T pivot = aCopy[i][i];
            for (int j = i; j < VariablesCount; j++)
            {
                aCopy[i][j] = aCopy[i][j] / pivot;
            }
            bCopy[i] = bCopy[i] / pivot;

            // Исключение переменной
            for (int j = i + 1; j < EquationsCount; j++)
            {
                T factor = aCopy[j][i];
                for (int k = i; k < VariablesCount; k++)
                {
                    aCopy[j][k] = aCopy[j][k] - factor * aCopy[i][k];
                }
                bCopy[j] = bCopy[j] - factor * bCopy[i];
            }
        }

        // Обратный ход метода Гаусса
        Vector<T> solution = new Vector<T>(VariablesCount);
        for (int i = Math.Min(VariablesCount, EquationsCount) - 1; i >= 0; i--)
        {
            if (aCopy[i][i].IsZero)
                continue;

            solution[i] = bCopy[i];
            for (int j = i + 1; j < VariablesCount; j++)
            {
                solution[i] = solution[i] - aCopy[i][j] * solution[j];
            }
        }

        return solution;
    }

    // Вспомогательный метод для сравнения абсолютных значений
    private int CompareAbsolute(T a, T b)
    {
        // Используем ToDouble() для сравнения абсолютных значений
        double absA = Math.Abs(a.ToDouble());
        double absB = Math.Abs(b.ToDouble());
        return absA.CompareTo(absB);
    }

    // Вычисление ранга матрицы
    private int GetMatrixRank(Vector<T>[] matrix)
    {
        if (matrix.Length == 0) return 0;

        // Создаем копию матрицы для преобразований
        Vector<T>[] tempMatrix = matrix.Select(v => v.Clone()).ToArray();
        int rows = tempMatrix.Length;
        int cols = tempMatrix[0].Dimension;
        int rank = 0;

        for (int col = 0; col < cols && rank < rows; col++)
        {
            // Поиск ненулевого элемента в текущем столбце
            int pivotRow = -1;
            for (int row = rank; row < rows; row++)
            {
                if (!tempMatrix[row][col].IsZero)
                {
                    pivotRow = row;
                    break;
                }
            }

            if (pivotRow == -1) continue;

            // Перестановка строк
            if (pivotRow != rank)
            {
                (tempMatrix[rank], tempMatrix[pivotRow]) = (tempMatrix[pivotRow], tempMatrix[rank]);
            }

            // Нормализация ведущей строки
            T pivot = tempMatrix[rank][col];
            for (int j = col; j < cols; j++)
            {
                tempMatrix[rank][j] = tempMatrix[rank][j] / pivot;
            }

            // Исключение
            for (int i = 0; i < rows; i++)
            {
                if (i != rank && !tempMatrix[i][col].IsZero)
                {
                    T factor = tempMatrix[i][col];
                    for (int j = col; j < cols; j++)
                    {
                        tempMatrix[i][j] = tempMatrix[i][j] - factor * tempMatrix[rank][j];
                    }
                }
            }
            rank++;
        }

        return rank;
    }

    // Вычисление ранга расширенной матрицы
    private int GetAugmentedMatrixRank()
    {
        // Создаем расширенную матрицу
        Vector<T>[] augmented = new Vector<T>[EquationsCount];
        for (int i = 0; i < EquationsCount; i++)
        {
            T[] augmentedRow = new T[VariablesCount + 1];
            for (int j = 0; j < VariablesCount; j++)
            {
                augmentedRow[j] = matrixA[i][j];
            }
            augmentedRow[VariablesCount] = vectorB[i];
            augmented[i] = new Vector<T>(augmentedRow);
        }

        return GetMatrixRank(augmented);
    }

    // Вывод системы на экран
    public void PrintSystem()
    {
        Console.WriteLine("Система уравнений:");
        for (int i = 0; i < EquationsCount; i++)
        {
            string equation = "";
            for (int j = 0; j < VariablesCount; j++)
            {
                if (j > 0) equation += " + ";
                equation += $"{matrixA[i][j]}*x{j + 1}";
            }
            equation += $" = {vectorB[i]}";
            Console.WriteLine(equation);
        }
    }

    // Проверка решения
    public T CheckSolution(Vector<T> solution)
    {
        if (solution.Dimension != VariablesCount)
            throw new ArgumentException("Размерность решения должна совпадать с количеством переменных");

        T maxError = T.Zero;
        for (int i = 0; i < EquationsCount; i++)
        {
            T leftSide = T.Zero;
            for (int j = 0; j < VariablesCount; j++)
            {
                leftSide = leftSide + matrixA[i][j] * solution[j];
            }
            T error = leftSide - vectorB[i];
            
            // Для сравнения ошибок используем абсолютное значение через ToDouble()
            if (Math.Abs(error.ToDouble()) > Math.Abs(maxError.ToDouble()))
            {
                maxError = error;
            }
        }
        return maxError;
    }

    // Создание случайной системы
    public static LinearSystem<T> CreateRandom(int equations, int variables)
    {
        if (equations <= 0 || variables <= 0)
            throw new ArgumentException("Количество уравнений и переменных должно быть положительным");

        Vector<T>[] matrixA = new Vector<T>[equations];
        Vector<T> vectorB = Vector<T>.Random(equations);

        for (int i = 0; i < equations; i++)
        {
            matrixA[i] = Vector<T>.Random(variables);
        }

        return new LinearSystem<T>(matrixA, vectorB);
    }

    // Создание системы с единственным решением (ОБНОВЛЕННЫЙ МЕТОД)
    public static LinearSystem<T> CreateWithUniqueSolution(int size)
    {
        Vector<T>[] matrixA = new Vector<T>[size];
        Vector<T> vectorB = Vector<T>.Random(size);

        // Создаем диагонально-доминантную матрицу для гарантии единственного решения
        for (int i = 0; i < size; i++)
        {
            T[] row = new T[size];
            for (int j = 0; j < size; j++)
            {
                if (i == j)
                {
                    // Диагональный элемент делаем большим
                    row[j] = CreateDiagonalElement();
                }
                else
                {
                    // Недиагональные элементы делаем маленькими
                    row[j] = CreateSmallElement();
                }
            }
            matrixA[i] = new Vector<T>(row);
        }

        return new LinearSystem<T>(matrixA, vectorB);
    }

    // Создание диагонального элемента с использованием обобщенных методов
    private static T CreateDiagonalElement()
    {
        // Используем FromDouble для создания элемента с значением 3.0
        return T.FromDouble(3.0);
    }

    // Создание маленького элемента с использованием обобщенных методов
    private static T CreateSmallElement()
    {
        // Используем FromDouble для создания элемента с маленьким значением 0.5
        return T.FromDouble(0.5);
    }
}
