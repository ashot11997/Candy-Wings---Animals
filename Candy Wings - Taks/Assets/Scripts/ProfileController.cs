using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    public Text Name;
    public Text Autor;
    public Text CreatedTime;
    public Text UpdatedTime;
    public Text Verified;
    public Text Description;

    public RawImage Thumbnail;

    public Texture CatTxt;
    public Texture DogTxt;
    public Texture HorseTxt;

    public Button BackButton;

    public Animator Anim;

    void Awake()
    {
        BackButton.onClick.AddListener(()=> { StartCoroutine(Close()); });
    }

    public void Open()
    {
        Anim.Play("ContainerOpeningAnim");
    }

    IEnumerator Close()
    {
        Anim.Play("ContainerClosingAnim");
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    public void Setup(Animal animal)
    {
        if (animal.Type == "cat")
            Thumbnail.texture = CatTxt;
        else if (animal.Type == "dog")
            Thumbnail.texture = DogTxt;
        else if (animal.Type == "horse")
            Thumbnail.texture = HorseTxt;

        Thumbnail.color = new Color32(255, 255, 255, 255);

        Name.text = char.ToUpper(animal.Type[0]) + animal.Type.Substring(1);
        Autor.text = "Created by - " + animal.Source;

        CreatedTime.text = animal.CreatedTime;
        UpdatedTime.text = animal.UpdatedTime;
        Verified.text = animal.Status;

        Description.text = animal.Text;
    }
}
