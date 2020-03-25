using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;


public class Animal
{
    public string Source;
    public string Type;
    public string CreatedTime;
    public string UpdatedTime;
    public string Status;
    public string Text;
}

public class JsonLoader : MonoBehaviour
{
    private string JsonUrl = "https://cat-fact.herokuapp.com/facts/random?animal_type=cat,dog,horse&amount=10";

    private List<Animal> AnimalsList = new List<Animal>();

    public IEnumerator Load(Action<List<Animal>> List)
    {
         WWW www = new WWW(JsonUrl);
         yield return www;
         if (www.error == null)
         {
             Processjson(www.text, List);
         }
         else
         {
             Debug.Log("Wrong Url");
         }
    }

    private void Processjson(string jsonString, Action<List<Animal>> List)
    {
        AnimalsList.Clear();
        JSONNode data = JSON.Parse(jsonString);
        for (int i = 0; i < data.Count; i++)
        {
            Animal animal = new Animal();
            animal.Source = data[i]["source"].Value.ToString();
            animal.Text = data[i]["text"].Value.ToString();
            string[] createdTime = data[i]["createdAt"].Value.ToString().Split('T');
            animal.CreatedTime = createdTime[0];
            string[] updatedTime = data[i]["updatedAt"].Value.ToString().Split('T');
            animal.UpdatedTime = updatedTime[0];
            animal.Type = data[i]["type"].Value.ToString();
            animal.Status = data[i]["status"]["verified"].Value.ToString();

            AnimalsList.Add(animal);
        }

        List.Invoke(AnimalsList);
    }

    
}
