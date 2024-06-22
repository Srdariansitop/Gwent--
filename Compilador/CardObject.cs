using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardObject : MonoBehaviour
{
  public int Power;
  public string Name;
  public string Faction;
  public string Type;
  public string[] Range;
  public Deck Deck;
 public ListImage listImage;
 public UnityEngine.UI.Image Image;
 private GameObject spriteGigante;
 public SpriteRenderer S;

  public void InstanciateNewCard(int power,string name , string faction,string type,string[]range)
  {
   Power = power;
   Name = name;
   Faction = faction;
   Type = type;
   Range = range;
   listImage = FindObjectOfType<ListImage>();
   S = gameObject.GetComponent<SpriteRenderer>();
   S.sprite =  listImage.sprites[Random.Range(0,listImage.sprites.Count)];
   PrefabUtility.SaveAsPrefabAsset(gameObject,"Assets/Resources/Card"+ Controller.NumCard + ".prefab");
   GameObject ImageWindscript = GameObject.Find("Image");
    if (ImageWindscript != null)
    {
        Image = ImageWindscript.GetComponent<UnityEngine.UI.Image>();
        Image.sprite = S.sprite;
    }
    Controller.NumCard +=1;
   
  }
   


    void OnMouseEnter()
  {
    Debug.Log("Hola");
    spriteGigante.transform.localScale = new Vector3(0.3f,0.3f,0);
    S.sprite = GetComponent<SpriteRenderer>().sprite;
  }
  void OnMouseExit()
  {
     spriteGigante.transform.localScale = Vector3.zero;
  }

    // Update is called once per frame
    void Update()
    {
      Debug.Log(spriteGigante);
      spriteGigante = GameObject.FindGameObjectWithTag("Show Card");
    }
}
