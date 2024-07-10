using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;
using System.Linq;

public class Effect : MonoBehaviour
{
   public  List<Param> Params;
   public  string Name;
   public string Acction;

   public void EffectInstanciate(List<Param> paramss , string name , List<Token> actiontoken)
   {
    Params = paramss;
    Name = name;
    Acction = StringAction(actiontoken);
    Controller.NumEffect += 1;
    PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/EffectsResources/Effect" + Controller.NumEffect + ".prefab");

   }
   

   public string StringAction(List<Token> actionToken)
   {
      string result ="";
      foreach(var a in actionToken)
      {
        result += " " + (string)a.Value;
      }
      return result;
   }

}
