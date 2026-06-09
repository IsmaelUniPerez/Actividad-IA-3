using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    public struct BlackboardData
    {
        public string dataKey;
        public Type type;
        public object value;

        public BlackboardData(string dataKey, object value)
        {
            this.dataKey = dataKey;
            this.type = value.GetType();
            this.value = value;
        }
    }

    private Dictionary<string, BlackboardData> entries;
    private List<Expert> experts;

    public T GetDataByKey<T>(string key)
    {
        if (entries.TryGetValue(key, out BlackboardData data))
        {
            return (T)data.value;
        }
        return default;
    }
    public void AddData(string key, object value)
    {
        entries[key] = new BlackboardData(key, value);
    }
    public void RemoveData(string key)
    {
        entries.Remove(key);
    }

    public Action[] EvaluateExperts()
    {
        Expert bestExpert = null;
        float maxInsistence = float.MinValue;
        foreach (Expert expert in experts)
        {
            float currentInsistence = expert.GetInsistence(this);
            if (bestExpert == null || currentInsistence > maxInsistence)
            {
                maxInsistence = currentInsistence;
                bestExpert = expert;
            }
        }
        if (bestExpert != null)
        {
            return bestExpert.Run(this);
        }
        return null;
    }
    public void RegisterExpert(Expert expert)
    {
        if (!experts.Contains(expert))
        {
            experts.Add(expert);
        }
    }
    private void Awake()
    {
        entries = new Dictionary<string, BlackboardData>();
        experts = new List<Expert>();
        AddData("Example", 42);
        AddData("Example", 100);
        AddData("Vida", 200);
        Debug.Log(GetDataByKey<int>("Example"));
        Debug.Log(GetDataByKey<int>("Vida"));
        var data = GetDataByKey<int>("Example");
        data += 50;
        AddData("Example", data);

        foreach (var entry in entries)
        {
            Debug.Log($"Key: {entry.Key}, Value: {entry.Value.value}");
        }
    }
}