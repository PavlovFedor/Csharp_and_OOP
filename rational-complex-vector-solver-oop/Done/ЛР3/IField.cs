using System;

// Интерфейс поля
public interface IField<T> where T : IField<T>
{
    // Парсинг строки
    static abstract T Parse(string s);
    
    // Попытка парсинга строки
    static abstract bool TryParse(string s, out T result);
    
    // Генерация случайного элемента
    static abstract T Random();
    
    // Нулевой элемент
    static abstract T Zero { get; }
    
    // Единичный элемент
    static abstract T One { get; }
    
    // генерация в диапазоне
    static abstract T RandomInRange(T min, T max);
    
    // Обратный элемент (мультипликативно обратный)
    T Reciprocal { get; }
    
    // Арифметические операторы
    static abstract T operator +(T a, T b);
    static abstract T operator -(T a, T b);
    static abstract T operator *(T a, T b);
    static abstract T operator /(T a, T b);
    static abstract T operator -(T a);
    
    // Операторы сравнения
    static abstract bool operator ==(T a, T b);
    static abstract bool operator !=(T a, T b);
    
    // Проверка на нуль
    bool IsZero { get; }
}
