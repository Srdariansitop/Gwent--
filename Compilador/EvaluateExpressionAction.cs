using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EvaluateExpressionAction 
{
 public static List<(string , int)> VarInt = new List<(string, int)>();
 public static List<(string , string)> VarString = new List<(string, string)>();
 public static List<(string , bool)> VarBoolean = new List<(string, bool)>();
 public static List<(string , GameObject)> VarCard = new List<(string, GameObject)>();

 public static void ResetList()
 {
   VarInt = new List<(string, int)>();
   VarBoolean = new List<(string, bool)>();
   VarString = new List<(string, string)>();
   VarCard =  new List<(string, GameObject)>();
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
      Console.WriteLine("Es un variable aun no");
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
if(Method == "Add")
{

}
else if(Method == "Shuffle")
{

}
else if(Method == "Remove")
{

}
else if(Method == "Push")
{

}
else if(Method == "Find")
{

}
else if(Method == "SendBootom")
{

}

}

}
