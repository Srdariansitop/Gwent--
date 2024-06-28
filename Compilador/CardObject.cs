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
   if(Type == "Clime" || Type == "Leader" || Type == "Increase")
   {
    Power = 0;
    S.sprite =  listImage.spritesMagic[Random.Range(0,listImage.spritesMagic.Count)];
   }
   else
   {
     S.sprite =  listImage.sprites[Random.Range(0,listImage.sprites.Count)];
   }
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
    Debug.Log("Name : " + Name);
    Debug.Log("Power : " + Power);
    Debug.Log("Faction : " + Faction);
    Debug.Log("Type : " + Type);
    spriteGigante.transform.localScale = new Vector3(0.3f,0.3f,0);
    spriteGigante.GetComponent<SpriteRenderer>().sprite = S.sprite;
  }
  void OnMouseExit()
  {
     spriteGigante.transform.localScale = Vector3.zero;
  }
  void OnMouseDown()
  {
    Invokee();
  }

  public void Invokee()
  {
     if(CardUnidad.Invocaste == false)
     {
      CardUnidad.Invocaste = true;
      //Canvas Searching
      GameObject canvasObject = GameObject.FindGameObjectWithTag("Invocadas");
        if (canvasObject != null)
        {
            Transform canvasTransform = canvasObject.transform;
            gameObject.transform.SetParent(canvasTransform, false); 
        } 
      gameObject.transform.localScale = new Vector3(2f,2f,0);
      if(Type == "Increase")
      {
        if(Faction == "Red")
        {
          for(int i = 0 ; i < 3 ; i++)
          {
            if(CardUnidad.posicionescartasmagicas[i,0] == false)
            {
              InvokkeAux(PositionInvoke.MatrixCardMagic(i,0));
              CardUnidad.posicionescartasmagicas[i,0] = true;
              return;
            }
          }
          ErrorInvocacion("Increase");
        }
        else
        {
          for(int i = 0 ; i < 3 ; i++)
          {
            if(CardUnidad.posicionescartasmagicasrival[i,0] == false)
            {
              InvokkeAux(PositionInvoke.MatrixCardMagicRival(i,0));
              CardUnidad.posicionescartasmagicasrival[i,0] = true;
              return;
            }
          }
        }
      }
      else if(Type == "Leader")
      {
         if(Faction == "Red" && CardUnidad.LeaderPos == false)
         {
            InvokkeAux("CartaLider1");
            CardUnidad.LeaderPos = true; 
         }
         else if(Faction == "Legend" && CardUnidad.LeaderRivalPos == false)
         {
            InvokkeAux("CartaLider2");
            CardUnidad.LeaderRivalPos = true; 
         }
         else
         {
          ErrorInvocacion(Type);
         }
      }
      else if(Type == "Clime")
      {
        if(Faction == "Red")
        {
          for(int i = 0 ; i < 3 ; i++)
          {
            if(CardUnidad.posicionescartasmagicas[i,1] == false)
            {
              InvokkeAux(PositionInvoke.MatrixCardMagic(i,1));
              CardUnidad.posicionescartasmagicas[i,1] = true;
              return;
            }
          }
        ErrorInvocacion("Clime");
        }
        else
        {
          for(int i = 0 ; i < 3 ; i++)
          {
            if(CardUnidad.posicionescartasmagicasrival[i,1] == false)
            {
              InvokkeAux(PositionInvoke.MatrixCardMagicRival(i,1));
              CardUnidad.posicionescartasmagicasrival[i,1] = true;
              return;
            }
          }
          ErrorInvocacion("Clime");
        }
      }
      else
      {
        if(Content("Meele"))
        {
          if(Faction == "Red")
          {
              for(int i = 0 ; i < 3 ; i++)
             {
              if(CardUnidad.posicionescampo[0,i] == false)
              {
                InvokkeAux(PositionInvoke.MatrixCardField(0,i));
                CardUnidad.posicionescampo[0,i] = true;
                return;
              }
             }
          }
          else
          {
             for(int i = 0 ; i < 3 ; i++)
             {
              if(CardUnidad.posicionescamporival[0,i] == false)
              {
                InvokkeAux(PositionInvoke.MatrixCardFieldRival(0,i));
                CardUnidad.posicionescamporival[0,i] = true;
                return;
              }
             }
          }
        }
        if(Content("Siege"))
        {
          if(Faction == "Red")
          {
             for(int i = 0 ; i < 3 ; i++)
             {
              if(CardUnidad.posicionescampo[2,i] == false)
              {
                InvokkeAux(PositionInvoke.MatrixCardField(2,i));
                CardUnidad.posicionescampo[2,i] = true;
                return;
              }
             }
          }
          else
          {
            for(int i = 0 ; i < 3 ; i++)
             {
              if(CardUnidad.posicionescamporival[2,i] == false)
              {
                InvokkeAux(PositionInvoke.MatrixCardFieldRival(2,i));
                CardUnidad.posicionescamporival[2,i] = true;
                return;
              }
             }
          }
        }
        if(Content("Distance"))
        {
           if(Faction == "Red")
          {
            for(int i = 0 ; i < 3 ; i++)
             {
              if(CardUnidad.posicionescampo[1,i] == false)
              {
                InvokkeAux(PositionInvoke.MatrixCardField(1,i));
                CardUnidad.posicionescampo[1,i] = true;
                return;
              }
             }
          }
          else
          {
            for(int i = 0 ; i < 3 ; i++)
             {
              if(CardUnidad.posicionescamporival[1,i] == false)
              {
                InvokkeAux(PositionInvoke.MatrixCardFieldRival(1,i));
                CardUnidad.posicionescamporival[1,i] = true;
                return;
              }
             }
          }
        }
        ErrorInvocacion(Type);
        
      }
     }
  }

public void InvokkeAux(string PosCardField)
{
  //Invocadas.Add(positionhand);
  GameObject posx = GameObject.Find(PosCardField);
  Vector3 posy = posx.transform.position;
  gameObject.transform.position = new Vector3(posy.x , posy.y , 2f);
  if(Faction == "Red")
  {
   CardUnidad.Contador += Power;
  }
  else
  {
    CardUnidad.ContadorRival += Power;
  }
}
public bool Content(string RangeString)
{
  for(int i = 0 ; i < Range.Length ; i++)
  {
    if(Range[i] == RangeString)
    {
      return true;
    }
  }
  return false;
}
  public void ErrorInvocacion(string position)
  {
    Debug.Log(position + " positions are busy in the field");
  }


    // Update is called once per frame
    void Update()
    {
      
      spriteGigante = GameObject.FindGameObjectWithTag("Show Card");
    }
}
