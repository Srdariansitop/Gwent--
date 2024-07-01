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

   }
  void Awake()
 {
  PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/EffectsResources/Effect" + 1 + ".prefab"); 
 }
}
