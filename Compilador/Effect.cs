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

     ///<summary>
     ///Encargado de Instanciar los valores del efecto para despues guardalo como un prefab
    ///</summary>
   public void EffectInstanciate(List<Param> paramss , string name , List<Token> actiontoken)
   {
    Params = paramss;
    Name = name;
    Acction = StringAction(actiontoken);
    Controller.NumEffect += 1;
    PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/EffectsResources/Effect" + Controller.NumEffect + ".prefab");

   }
   
      ///<summary>
     ///Este metodo es el encargado de convertir la lista del Action a String para q los datos se pueden pasar de una escena a otra correctamente
    ///</summary>
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
