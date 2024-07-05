using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;

public class Effect : MonoBehaviour
{
   public  List<Param> Params;
   public  string Name;
   public  List<Token> ActionToken;
   public void EffectInstanciate(List<Param> paramss , string name , List<Token> actiontoken)
   {
    Params = paramss;
    Name = name;
    ActionToken = actiontoken;   
    Controller.NumEffect += 1;
    PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/EffectsResources/Effect" + Controller.NumEffect + ".prefab");
   }

}
