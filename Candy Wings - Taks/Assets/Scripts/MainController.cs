using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public GameObject ListContainer;
    public GameObject PageContainer;

    public JsonLoader JsonLoader;

    [Header("Animal List Part")]
    public AnimalPrefab Prefab;
    public Transform Content;
    public GameObject LoaderBar;
    public GameObject NotInternetWindow;
    public Button LoadBtn;

    [Header("Profile Part")]
    public ProfileController ProfileController;

    void Awake()
    {
        LoadBtn.onClick.AddListener(LoadAnimals);
    }

    void Start()
    {
        ListContainer.SetActive(true);
        PageContainer.SetActive(false);
    }
    
    void LoadAnimals()
    {
        foreach (Transform item in Content)
        {
            Destroy(item.gameObject);
        }

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            NotInternetWindow.SetActive(false);
            LoaderBar.SetActive(true);
            StartCoroutine(JsonLoader.Load(List => {
                AddAnimals(List);
            }));
        }
        else
        {
            NotInternetWindow.SetActive(true);
        }

    }

    void AddAnimals(List<Animal> list)
    {
        LoaderBar.SetActive(false);

        foreach (var animal in list)
        {
            var newAnimal = Instantiate(Prefab, Content);
            newAnimal.Setup(animal);
            newAnimal.Clicked += ClickedAnimal;
        }
    }

    void ClickedAnimal(Animal animal) {
        PageContainer.SetActive(true);
        ProfileController.Open();
        ProfileController.Setup(animal);
    }
}
