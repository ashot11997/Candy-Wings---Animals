using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AnimalPrefab : MonoBehaviour
{
    public Button Btn;
    public RawImage Thumbnail;
    public Text Name;

    public Texture CatTxt;
    public Texture DogTxt;
    public Texture HorseTxt;

    public Action<Animal> Clicked;

    private Animal CurrentAnimal;

    void Awake()
    {
        Btn.onClick.AddListener(Click);
    }

    void Click() {
        Clicked.Invoke(CurrentAnimal);
    }

    public void Setup(Animal animal)
    {
        CurrentAnimal = animal;


        if (animal.Type == "cat")
            Thumbnail.texture = CatTxt;
        else if (animal.Type == "dog")
            Thumbnail.texture = DogTxt;
        else if (animal.Type == "horse")
            Thumbnail.texture = HorseTxt;

        Thumbnail.color = new Color32(255,255,255,255);

        Name.text = char.ToUpper(animal.Type[0]) + animal.Type.Substring(1);

    }
}
