using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
  public int Power;
  public string Name;
  public string Faction;
  public string Type;
  public string[] Range;
 
 public Sprite Sprite;
 public ListImage listImage;
 public UnityEngine.UI.Image Image;

  public void InstanciateNewCard(int power,string name , string faction,string type,string[]range)
  {
   Debug.Log("Hola");
   Power = power;
   Name = name;
   Faction = faction;
   Type = type;
   Range = range;
   listImage = FindObjectOfType<ListImage>();
   Sprite = listImage.sprites[Random.Range(0,listImage.sprites.Count)];
   PrefabUtility.SaveAsPrefabAsset(gameObject,"Assets/Resources/Card.prefab");
   GameObject ImageWindscript = GameObject.Find("Image");
    if (ImageWindscript != null)
    {
        Image = ImageWindscript.GetComponent<UnityEngine.UI.Image>();
        Image.sprite = Sprite;
    }
   
   
   
   
   
  }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
