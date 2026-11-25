using System;
using System.Collections.Generic;
using System.Text;

namespace OpamLab07
{
    internal class Program
    {
        static int CountVowels(string s)
        {
            const string vowels = "аеєиіїоуюяАЕЄИІЇОУЮЯaeiouyAEIOUY";
            int count = 0;
            foreach (char ch in s)
                if (vowels.IndexOf(ch) >= 0) count++;
            return count;
        }

        static bool StartsWithVowel(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            const string vowels = "аеєиіїоуюяАЕЄИІЇОУЮЯaeiouyAEIOUY";
            return vowels.IndexOf(s[0]) >= 0;
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // ВКАЖІТЬ ВАШІ ПАРАМЕТРИ ВАРІАНТА:
            int N = 7;   // Номер у списку групи
            int K = 19;  // Номер першої літери імені в укр. абетці

            int a = N % 3, b = N % 4, c = N % 5;
            Console.WriteLine($"N = {N}, K = {K}, a = {a}, b = {b}, c = {c}\n");

            var rng = new Random(N);
            var numbers = new List<int>(20);
            for (int i = 0; i < 20; i++) numbers.Add(rng.Next(-K, K + 1));

            var words = new List<string>
            {
                "код", "алгоритм", "делегат-функція", "осінь", "ітератор",
                "єнот", "лямбда-вираз", "шум", "об'єкт", "масив-даних",
                "індекс", "юніт-тест", "екран", "яблуко"
            };

            var students = new List<Student>
            {
                new Student("Андрієнко", "КН-3/1", 92),
                new Student("Бойко", "КН-3/2", 58),
                new Student("Василенко", "КН-3/1", 76),
                new Student("Гриценко", "КН-3/1", 64),
                new Student("Дмитренко", "КН-3/2", 85),
                new Student("Євтушенко", "КН-3/1", 49),
                new Student("Захарченко", "КН-3/2", 95),
                new Student("Іваненко", "КН-3/1", 75),
                new Student("Коваленко", "КН-3/2", 88),
                new Student("Лисенко", "КН-3/1", 60)
            };

            // Завдання 1: Filter
            Func<int, bool> numPred = a == 0 ? x => x > K : a == 1 ? x => x % 3 == 0 : x => x <= 0;
            Func<string, bool> strPred = a == 0 ? s => s.Length > 5 : a == 1 ? StartsWithVowel : s => s.Contains('-');
            Func<Student, bool> studPred = a == 0 ? st => st.Score >= 75 : a == 1 ? st => st.Group == "КН-3/1" : st => st.Score >= 60 && st.Score <= 89;

            Console.WriteLine("--- Завдання 1: Filter ---");
            var fNums = Algorithms.Filter(numbers, numPred);
            var fWords = Algorithms.Filter(words, strPred);
            var fStuds = Algorithms.Filter(students, studPred);
            Console.WriteLine($"Числа ({fNums.Count}): {string.Join(", ", fNums)}");
            Console.WriteLine($"Рядки ({fWords.Count}): {string.Join(", ", fWords)}");
            Console.WriteLine($"Студенти ({fStuds.Count}): {string.Join("; ", fStuds)}");

            // Завдання 2: Map
            Console.WriteLine("\n--- Завдання 2: Map ---");
            if (b == 0)
            {
                Console.WriteLine("Числа: " + string.Join(", ", Algorithms.Map(numbers, x => x * x)));
                Console.WriteLine("Рядки: " + string.Join(", ", Algorithms.Map(words, s => s.Length)));
                Console.WriteLine("Студенти: " + string.Join(", ", Algorithms.Map(students, st => st.Name)));
            }
            else if (b == 1)
            {
                int mod = (K % 7) + 2;
                Console.WriteLine("Числа: " + string.Join(", ", Algorithms.Map(numbers, x => x % mod)));
                Console.WriteLine("Рядки: " + string.Join(", ", Algorithms.Map(words, s => s.ToUpper())));
                Console.WriteLine("Студенти: " + string.Join("; ", Algorithms.Map(students, st => $"{st.Name} ({st.Score})")));
            }
            else if (b == 2)
            {
                Console.WriteLine("Числа: " + string.Join(", ", Algorithms.Map(numbers, x => Math.Sign(x))));
                Console.WriteLine("Рядки: " + string.Join(", ", Algorithms.Map(words, s => s[0])));
                Console.WriteLine("Студенти: " + string.Join(", ", Algorithms.Map(students, st => st.Score)));
            }
            else
            {
                Console.WriteLine("Числа: " + string.Join(", ", Algorithms.Map(numbers, x => $"({x})")));
                Console.WriteLine("Рядки: " + string.Join(", ", Algorithms.Map(words, CountVowels)));
                Console.WriteLine("Студенти: " + string.Join(", ", Algorithms.Map(students, st => st.Score >= 60)));
            }

            // Завдання 3: Aggregate
            Console.WriteLine("\n--- Завдання 3: Aggregate ---");
            Console.WriteLine("Сума: " + Algorithms.Aggregate(numbers, 0, (acc, x) => acc + x));
            Console.WriteLine("Добуток: " + Algorithms.Aggregate(numbers, 1L, (acc, x) => acc * x));
            Console.WriteLine("Максимум: " + Algorithms.Aggregate(numbers, int.MinValue, (acc, x) => x > acc ? x : acc));

            if (c == 0) Console.WriteLine("4-та (c=0): " + Algorithms.Aggregate(numbers, 0, (acc, x) => numPred(x) ? acc + 1 : acc));
            else if (c == 1) Console.WriteLine("4-та (c=1): " + Algorithms.Aggregate(numbers, 0, (acc, x) => Math.Abs(x) > Math.Abs(acc) ? x : acc));
            else if (c == 2) Console.WriteLine("4-та (c=2): " + Algorithms.Aggregate(words, "", (acc, s) => acc.Length == 0 ? s : acc + ", " + s));
            else if (c == 3) Console.WriteLine("4-та (c=3): " + Algorithms.Aggregate(numbers, int.MaxValue, (acc, x) => x < acc ? x : acc));
            else
            {
                var (s, cnt) = Algorithms.Aggregate(numbers, (sum: 0, count: 0), (acc, x) => (acc.sum + x, acc.count + 1));
                Console.WriteLine($"4-та (c=4): {(double)s / cnt:F2}");
            }

            // Завдання 4: GroupBy
            Console.WriteLine("\n--- Завдання 4: GroupBy ---");
            PrintGroups("За групою", Algorithms.GroupBy(students, st => st.Group));
            PrintGroups("За першою літерою", Algorithms.GroupBy(students, st => st.Name[0]));
            PrintGroups("За діапазоном балу", Algorithms.GroupBy(students, st => st.Score < 60 ? "0-59" : st.Score < 75 ? "60-74" : st.Score < 90 ? "75-89" : "90-100"));
            PrintGroups("Рядки за довжиною", Algorithms.GroupBy(words, s => s.Length));

            // Завдання 5: Захоплення змінних
            Console.WriteLine("\n--- Завдання 5: Захоплення змінних ---");
            int threshold = K;
            Func<int, bool> isAbove = x => x > threshold;
            threshold = 1000;
            Console.WriteLine("1) Змінна після лямбди: " + isAbove(50));

            var forFuncs = new List<Func<int>>();
            for (int i = 0; i < 3; i++) forFuncs.Add(() => i);
            Console.Write("2) Цикл for: ");
            foreach (var f in forFuncs) Console.Write(f() + " ");
            Console.WriteLine();

            var foreachFuncs = new List<Func<int>>();
            foreach (int item in new[] { 0, 1, 2 }) foreachFuncs.Add(() => item);
            Console.Write("3) Цикл foreach: ");
            foreach (var f in foreachFuncs) Console.Write(f() + " ");
            Console.WriteLine();
        }

        static void PrintGroups<TKey, T>(string title, Dictionary<TKey, List<T>> groups) where TKey : notnull
        {
            var parts = new List<string>();
            foreach (var kv in groups) parts.Add($"[{kv.Key}: {kv.Value.Count}]");
            Console.WriteLine($"{title}: {string.Join(", ", parts)}");
        }
    }
}