using UnityEngine;
using System.Collections.Generic;
public class ArrayExercise : MonoBehaviour
{
    int[] numbers = new int[5];
    int[] otherNumbers = new int[5] { 10, 20, 30, 40, 50 };

    void Start()
    {
        numbers[0] = 10;
        numbers[1] = 20;
        numbers[2] = 30;
        numbers[3] = 40;
        numbers[4] = 50;

        for (int i = 0; i < numbers.Length; i++) { Debug.Log(numbers[i]); }

        Debug.Log("The arrays are " + (numbers.SequenceEqual(otherNumbers) ? "equal" : "not equal"));
    }
}
public class ListExercise : MonoBehaviour
{
    List<int> number = new();

    void Start()
    {
        number.Add(10);
        number.Add(20);
        number.Add(30);
        foreach (int number in number)
        {
            Debug.Log(number);
        }
    }
}
public class HashSetExercise : MonoBehaviour
{
    HashSet<string> words = new() { "this", "these", "those", "them" };

    void Start()
    {
        words.Add("Apple");
        words.Add("banana");
        words.Add("orange");

        if (words.Contains("banana")) { Debug.Log("Found banana!"); }

        foreach (string word in words)
        {
            Debug.Log(word);
        }
    }
}
public class DictionaryExercise : MonoBehaviour
{
    Dictionary<string, int> ages = new();

    void Start()
    {
        ages.Add("Alexander", 25);
        ages.Add("Darius", 30);
        ages.Add("Arthur", 35);
        int a = ages["Alexander"];

        foreach (KeyValuePair<string, int> pair in ages)
        {
            Debug.Log(pair.Key + ": " + pair.Value);
        }
    }
}
public class QueueExercise : MonoBehaviour
{
    Queue<string> actions = new();

    void Start()
    {
        actions.Enqueue("Jump");
        actions.Enqueue("Shoot");
        actions.Enqueue("Dodge");

        while (actions.Count > 0)
        {
            string action = actions.Dequeue();
            Debug.Log("Performing action: " + action);
        }
    }
}
public class StackExercise : MonoBehaviour
{
    Stack<string> history = new();

    void Start()
    {
        history.Push("1");
        history.Push("2");
        history.Push("3");

        while (history.Count > 0)
        {
            string page = history.Pop();
            Debug.Log("Viewing Page: " + page);
        }
    }
}
public class TestYourLimits : MonoBehaviour
{
    List<int> numbers1 = new() { 8, 10, 54, 70 };
    List<int> numbers2 = new() { 15, 2, 30, 1 };
    List<int> numbers3 = new();

    void Start()
    {
        numbers3.Clear();
        for (int i = 0; numbers1.Count == numbers2.Count && i < numbers1.Count; i++)
        {
            switch (i)
            {
                case 0: { numbers3.Add(numbers1[0] + numbers2[0]); break; }
                case 1: { numbers3.Add(numbers1[1] - numbers2[1]); break; }
                case 2: { numbers3.Add(numbers1[2] * numbers2[2]); break; }
                case 3: { numbers3.Add(numbers1[3] / numbers2[3]); break; }
                default: { break; }
            }
        }
    }
}
public class Overkill : MonoBehaviour
{
    List<int> numbers1 = new() { 8, 10, 54, 70 };
    List<int> numbers2 = new() { 15, 2, 30, 1 };
    [SerializeField] List<int> numbers3 = new();
    Dictionary<int, Func<int, int, int>> functions = new()
    {
        { 0, (a, b) => a + b },
        { 1, (a, b) => a - b },
        { 2, (a, b) => a * b },
        { 3, (a, b) => a / b }
    };

    void Start()
    {
        for (int i = 0; numbers1.Count == numbers2.Count && i < numbers1.Count; i++)
        {
            if (functions.TryGetValue(i, out Func<int, int, int> function))
            {
                numbers3.Add(function(numbers1[i], numbers2[i]));
            }
        }
    }
}
