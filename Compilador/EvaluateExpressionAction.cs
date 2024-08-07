using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.ComponentModel;
public class EvaluateExpressionAction 
{
public static Dictionary<string,object> keyValuePairs = new Dictionary<string, object>();
 public static void ResetList()
 {
 keyValuePairs = new Dictionary<string, object>();
 }

public static void EvaluateNode(Node nodeactual,List<GameObject> source,string faction)
{
  if (NotTokenList(nodeactual) && (string)nodeactual.Value == "Parent")
  {
    foreach(var a in nodeactual.Children)
    {
      EvaluateNode(a,source,faction);
    }
  }
  else if (NotTokenList(nodeactual) && (string)nodeactual.Value == "For")
  {
    for(int i = 0; i < source.Count; i++)
    {
      foreach (var a in nodeactual.Children)
      {
        EvaluateNode(a,source,faction);
      }
    }
  }
  else if (NotTokenList(nodeactual) && (string)nodeactual.Value == "While")
  {

  }
  else
  {
    List<Token> tokens = (List<Token>)nodeactual.Value;
    if (tokens[0].Type == TypeToken.Var)
    {
      VarSave(tokens,source,faction);
    }
    else
    {
     ContextMethodAnalyzer(tokens,faction);
    }
  }
}

 public static bool NotTokenList(Node node)
{
  try{ string a = (string)node.Value; return true;}
  catch { return false;}   
}

public static void ContextMethodAnalyzer(List<Token> tokens,string Faction)
{
string Method = ActionParsing.WhichMethodContext((string)tokens[0].Value);
string SourceString = ActionParsing.WichSourceContext((string)tokens[0].Value);
List<GameObject> Source = OnActivaction.SourceReturn(SourceString,Faction);
if(Method == "Add" || Method == "SendBootom")
{
 //Variable o indexado de lista
 if(keyValuePairs.ContainsKey((string)tokens[2].Value))
 {
  Source.Add((GameObject)keyValuePairs[(string)tokens[2].Value]);
  if(SourceString == "Hand")
  {

  }
 }
 else
 {

 }
}
else if(Method == "Shuffle")
{
 System.Random random = new System.Random();
 for(int i = 0 ; i < Source.Count ; i++)
 {
   int j = random.Next(Source.Count - 1);
   var temp = Source[i];
   Source[i] = Source[j];
   Source[j] = temp; 
 }
}
else if(Method == "Remove")
{
 if(keyValuePairs.ContainsKey((string)tokens[2].Value))
 {
  Source.Remove(((GameObject)keyValuePairs[(string)tokens[2].Value]));
  if(SourceString == "Hand")
  {
  GameObject CardTemp = (GameObject)keyValuePairs[(string)tokens[2].Value];
  string tag = CardTemp.tag;
  GameObject [] clones = GameObject.FindGameObjectsWithTag(tag);
  foreach(var x in clones)
  {
  x.transform.position = new Vector3(200f,200f,200f);        
  }
  }
 }
 else
 {
  
 }
}
else if(Method == "Push")
{
 if(keyValuePairs.ContainsKey((string)tokens[2].Value))
 {
   Source.Insert(0,(GameObject)keyValuePairs[(string)tokens[2].Value]);
 }
 else
 {
  
 }
}


}


public static void VarSave(List<Token> tokens, List<GameObject> Source,string Faction)
{
  //Existe la variable
   if(keyValuePairs.ContainsKey((string)tokens[0].Value))
   {
    if(tokens[1].Type == TypeToken.SumSum)
    {
      ModVar((string)tokens[0].Value, TypeToken.SumSum);
    }
    else if(tokens[1].Type == TypeToken.RestRest)
    {
      ModVar((string)tokens[0].Value, TypeToken.RestRest);
    }
    else if(tokens[2].Type == TypeToken.Var)
    {
     keyValuePairs[(string)tokens[0].Value] = keyValuePairs[(string)tokens[2].Value];
    }
   }
   else
   {
    if(tokens[2].Type == TypeToken.Var)
    {
      keyValuePairs.Add((string)tokens[0].Value,keyValuePairs[(string)tokens[2].Value]);
    }
    else if(tokens[2].Type == TypeToken.ContextMethod)
    {
      string Method = ActionParsing.WhichMethodContext((string)tokens[2].Value);
      if(Method == "Find")
      {

      }
      else if(Method == "Pop")
      {
        string SourceString = ActionParsing.WichSourceContext((string)tokens[2].Value);
        List<GameObject> Sourcetemp = OnActivaction.SourceReturn(SourceString,Faction);
        keyValuePairs.Add((string)tokens[0].Value,Sourcetemp[0]);
        Sourcetemp.RemoveAt(0);
        GameObject CardTemp = (GameObject)keyValuePairs[(string)tokens[0].Value];
        string tag = CardTemp.tag;
        GameObject [] clones = GameObject.FindGameObjectsWithTag(tag);
        foreach(var x in clones)
        {
          x.transform.position = new Vector3(200f,200f,200f);
          
        }
      }
    }
    else
    {
     keyValuePairs.Add((string)tokens[0].Value,tokens[2].Value);
    } 
   }
}


public static void ModVar(string name , TypeToken operation)
{
int number = int.Parse((string)keyValuePairs[name]);
if (operation == TypeToken.SumSum)
{
  number += 1;
}
else if(operation == TypeToken.RestRest)
{
number -= 1;
}
keyValuePairs[name] = number;
}



}
