using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Effect : MonoBehaviour
{
   public static List<Param> Params;
   public static string Name;
   public Effect(List<Param> paramss , string name)
   {
    Params = paramss;
    Name = name;
    Controller.NumEffect += 1;
   }
  void Awake()
 {
  PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/EffectsResources/Effect" + Controller.NumEffect + ".prefab"); 
 }
}
