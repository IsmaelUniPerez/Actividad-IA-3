using System;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    public struct BlackboardData
    {
        //Aquí indicamos como se estructurará cada entrada con sus datos
        public string dataKey;
        public Type type;
        public object value;

        public BlackboardData(string dataKey, object value) //Hacemos un constructor
        {
            this.dataKey = dataKey;
            this.type = value.GetType();
            this.value = value;
        }
    }

    private Dictionary<string, BlackboardData> entries;//Aquí se almacenarán las entradas de la blackboard, cada una con su clave y su valor
    private List<Expert> experts;//Guardaremos una lista de expertos que se registren en la blackboard, para que el árbitro pueda evaluarlos y elegir cuál ejecutar

    public T GetDataByKey<T>(string key)//Éste método nos permite acceder a los datos almacenados en la blackboard a través de su clave, y devuelve el valor del tipo especificado
    {
        if (entries.TryGetValue(key, out BlackboardData data))
        {
            return (T)data.value;
        }
        return default;
    }
    public void AddData(string key, object value)//tenemos un método para agregar o actualizar los datos
    {
        entries[key] = new BlackboardData(key, value);
    }
    public void RemoveData(string key)//y otro para eliminar datos de la blackboard
    {
        entries.Remove(key);
    }

    public Action[] EvaluateExperts()//Éste método es el árbitro de la blackboard, evaluará la insistencia de cada experto y ejecutará el que tenga la mayor prioridad
    {
        Expert bestExpert = null;
        float maxInsistence = 0f;

        foreach (Expert expert in experts)
        {
            float currentInsistence = expert.GetInsistence(this);
            if (currentInsistence > maxInsistence)
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
    public void RegisterExpert(Expert expert)//aquí podemos registrar a los expertos que quieran participar en la evaluación, asegurándonos de no agregar duplicados
    {
        if (!experts.Contains(expert))
        {
            experts.Add(expert);
        }
    }
    private void Awake()
    {
        //éstas entradas son solo para probar el funcionamiento del blackboard, se pueden eliminar o modificar según las necesidades del proyecto
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