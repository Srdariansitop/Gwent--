using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInvoke : MonoBehaviour
{
   
public static string MatrixCardField(int i , int j)
{
  //Asedio
 if(i == 2 && j == 0)
 {
   return "Asedio1Espacio1";
 }
 else if(i == 2 && j == 1 )
 {
   return "Asedio1Espacio2";
 }
 else if(i == 2 && j == 2)
 {
   return "Asedio1Espacio3";
 }
 //Distance
 else if(i == 1 && j == 0)
 {
   return "Distancia1Espacio1";
 }
 else if(i == 1 && j == 1)
 {
  return "Distancia1Espacio2";
 }
 else if(i == 1 && j == 2)
 {
  return "Distancia1Espacio3";
 }
 //Distance
 else if(i == 0 && j == 0)
 {
  return "Cuerpo1Espacio3";
 }
 else if(i == 0 && j == 1)
 {
    return "Cuerpo1Espacio1";
 }
 else if(i == 0 && j == 2)
 {
   return "Cuerpo1Espacio2";
 }
 else
 {
  return "";
 }
}
public static string MatrixCardFieldRival(int i , int j)
{
  //Asedio
  if(i == 2 && j == 0)
 {
   return "Asedio2Espacio1";
 }
 else if(i == 2 && j == 1 )
 {
   return "Asedio2Espacio2";
 }
 else if(i == 2 && j == 2)
 {
   return "Asedio2Espacio3";
 }
 //Distance
 else if(i == 1 && j == 0)
 {
   return "Distancia2Espacio1";
 }
 else if(i == 1 && j == 1)
 {
  return "Distancia2Espacio2";
 }
 else if(i == 1 && j == 2)
 {
  return "Distancia2Espacio3";
 }
 //Meele
 else if(i == 0 && j == 0)
 {
  return "Cuerpo2Espacio3";
 }
 else if(i == 0 && j == 1)
 {
    return "Cuerpo2Espacio1";
 }
 else if(i == 0 && j == 2)
 {
   return "Cuerpo2Espacio2";
 }
 else
 {
  return "";
 }
}
public static string MatrixCardMagic(int i , int j)
{
  //Case Clime
  if(i == 0 && j == 1)
  {
    return "Clima1";
  } 
  else if(i == 1 && j == 1)
  {
    return "Clima2";
  } 
  else if(i == 2 && j == 1)
  {
    return "Clima3";
  }
  //Case Increase
  else if(i == 0 && j == 0)
  {
    return "AumentoDistancia1";
  }
  else if(i == 1 && j == 0 )
  {
    return "AumentoAsedio1";
  }
  else if(i == 2 &&  j == 0)
  {
    return "AumentoCuerpo1";
  }
  else
  {
    return "";
  }
}
public static string MatrixCardMagicRival(int i , int j)
{
  //Increase
  if(i == 0 && j == 0)
  {
    return "AumentoDistancia2";
  }
  else if(i == 1 && j == 0)
  {
    return "AumentoCuerpo2";
  }
  else if(i == 2 && j == 0)
  {
    return "AumentoAsedio2";
  }
  //Clime
  else if(i == 0 && j == 1)
  {
    return "Clima4";
  }
  else if(i == 1 &&  j == 1)
  {
    return "Clima5";
  }
  else if(i == 2 && j == 1)
  {
   return "Clima6";
  }
  else
  {
    return "";
  }
}

  public static void DestroyInstance(List<GameObject>gameObjects)
{
  foreach(var card in gameObjects)
  {
    string tag = card.tag;
    GameObject[]instances = GameObject.FindGameObjectsWithTag(tag);
    foreach(GameObject instance in instances)
    {
      Destroy(instance);
    }
  }
}
}
