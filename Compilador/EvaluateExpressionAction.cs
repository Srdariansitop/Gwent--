using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine.AI;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.Search;
public class EvaluateExpressionAction : MonoBehaviour
{
public static Dictionary<string,object> keyValuePairs = new Dictionary<string, object>();
 public static void ResetList()
 {
 keyValuePairs = new Dictionary<string, object>();
 }

public static void EvaluateNode(Node nodeactual,List<GameObject> source,string faction,int index)
{
  if (NotTokenList(nodeactual) && (string)nodeactual.Value == "Parent")
  {
    foreach(var a in nodeactual.Children)
    {
      EvaluateNode(a,source,faction,0);
    }
  }
  else if (NotTokenList(nodeactual) && (string)nodeactual.Value == "For")
  {
    for(int i = 0; i < source.Count; i++)
    {
      foreach (var a in nodeactual.Children)
      {
        EvaluateNode(a,source,faction,i);
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
      VarSave(tokens,source,faction,index);
    }
    else if(tokens[0].Type == TypeToken.TargetProps)
    {
      TargetsPropsEvaluate(tokens,source,index);
    }
    else
    {
     ContextMethodAnalyzer(tokens,faction,source,index);
    }
  }
}

 public static bool NotTokenList(Node node)
{
  try{ string a = (string)node.Value; return true;}
  catch { return false;}   
}

public static void ContextMethodAnalyzer(List<Token> tokens,string Faction,List<GameObject> SourceGlobal , int index)
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
    InstanceHand(Source,Faction);
  }
 }
 else
 {
   Source.Add(SourceGlobal[index]);
  if(SourceString == "Hand")
  {
    InstanceHand(Source,Faction);
  }
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
 }
 else
 {
  Source.Remove(SourceGlobal[index]);
 }
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
else if(Method == "Push")
{
 if(keyValuePairs.ContainsKey((string)tokens[2].Value))
 {
  Source.Insert(0,(GameObject)keyValuePairs[(string)tokens[2].Value]);
  if(SourceString == "Hand")
  {
    InstanceHand(Source,Faction);
  }
 }
 else
 {
  Source.Insert(0,SourceGlobal[index]);
  if(SourceString == "Hand")
  {
    InstanceHand(Source,Faction);
  }
 }
}


}

public static void TargetsPropsEvaluate(List<Token> expression , List<GameObject>SourceGlobal , int index)
{
 string prop = ActionParsing.ExtractToProp((string)expression[0].Value);
 CardUnidad cardUnidad = SourceGlobal[index].GetComponent<CardUnidad>();
 if(prop == "Power")
 {
    if(expression[1].Type == TypeToken.Sum)
    {
      string temp = (string)expression[3].Value;
      int temp2 = int.Parse(temp);
      cardUnidad.Attack += temp2;
    }
    else if(expression[1].Type == TypeToken.Rest)
    {
      string temp = (string)expression[3].Value;
      int temp2 = int.Parse(temp);
      cardUnidad.Attack -= temp2;
    }
    else
    {
      string temp = (string)expression[2].Value;
      int temp2 = int.Parse(temp);
      cardUnidad.Attack = temp2;
    }
 }
 else if(prop == "Faction")
 {
  cardUnidad.Faction = (string)expression[2].Value;
 }
 else if(prop == "Type")
 {
  cardUnidad.Tipo = (string)expression[2].Value;
 }
 else
 {
  cardUnidad.Name = (string)expression[2].Value;
 }
}
public static void VarSave(List<Token> tokens, List<GameObject> Source,string Faction,int index)
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
    else if(tokens[2].Type == TypeToken.target)
    {
      keyValuePairs.Add((string)tokens[0].Value,Source[index]);
    }
   }
   else
   {
    if(tokens[2].Type == TypeToken.Var)
    {
      keyValuePairs.Add((string)tokens[0].Value,keyValuePairs[(string)tokens[2].Value]);
    }
    else if(tokens[2].Type == TypeToken.ContextProp)
    {
      string sourcestring = ActionParsing.ExtractToProp((string)tokens[2].Value);
     List<GameObject> sourcenew = OnActivaction.SourceReturn(sourcestring,Faction);
     keyValuePairs.Add((string)tokens[0].Value , sourcenew);
    }
    else if(tokens[2].Type == TypeToken.ContextTrigger)
    {
      keyValuePairs.Add((string)tokens[0].Value,Faction);
    }
    else if(tokens[2].Type == TypeToken.ContextPseudoMethod)
    {
      if(tokens[4].Type == TypeToken.TargetProps )
      {
       string factionnn = Source[index].GetComponent<CardUnidad>().Faction;
       string sourcestring = ActionParsing.ExtractToProp((string)tokens[2].Value);
       string sourcestring2 = ActionParsing.ExtractToProp(sourcestring);
       List<GameObject> sourcenew = OnActivaction.SourceReturn(sourcestring2,factionnn);
       keyValuePairs.Add((string)tokens[0].Value , sourcenew);     
      }
      else if(tokens[4].Type == TypeToken.ContextTrigger)
      {
     string sourcestring = ActionParsing.ExtractToProp((string)tokens[2].Value);
     string sourcestring2 = ActionParsing.ExtractToProp(sourcestring);
     List<GameObject> sourcenew = OnActivaction.SourceReturn(sourcestring2,Faction);
     keyValuePairs.Add((string)tokens[0].Value , sourcenew);
      }
      else
      {
      string Faction2 = (string)keyValuePairs[(string)tokens[4].Value];
      string sourcestring = ActionParsing.ExtractToProp((string)tokens[2].Value);
      List<GameObject> sourcenew = OnActivaction.SourceReturn(sourcestring,Faction2);
      keyValuePairs.Add((string)tokens[0].Value , sourcenew);
      }
    }
    else if(tokens[2].Type == TypeToken.ContextMethod)
    {
      string Method = ActionParsing.WhichMethodContext((string)tokens[2].Value);
      if(Method == "Find")
      { 
        string SourceString = ActionParsing.WichSourceContext((string)tokens[2].Value);
        List<GameObject> Sourcetemp = OnActivaction.SourceReturn(SourceString,Faction);
        List<GameObject> newList = FindCondition(Sourcetemp,tokens);
        keyValuePairs.Add((string)tokens[2].Value,newList);
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

public static void InstanceHand(List<GameObject> Source, string Faction)
{
 PositionInvoke.DestroyInstance(Source);
 Deck deck ;
 if(Faction == "Red")
 {
  deck = GameObject.Find("DeckRed").GetComponent<Deck>();
 }
 else
 {
 deck = GameObject.Find("DeckLegendarios").GetComponent<Deck>();
 } 
  Transform handposi = deck.transform.Find("HandPosition");
    //Mostrar tablero
    for(int i = 0 ; i < 10 ; i++)
    {
        GameObject card = Source[i];
        Transform pos = handposi.GetChild(i);
        GameObject nuevainstancia = Instantiate(card,pos.position,Quaternion.identity);
        //Debug.Log(card);
        float scale = 0.02590f;
        nuevainstancia.transform.localScale = new Vector3(scale,scale,scale);       
    }  
}

public static List<GameObject> FindCondition(List<GameObject> Source , List<Token> tokens)
{
  string prop = "";
  TypeToken Signe = TypeToken.Action;
  string SecondCondition = "";
  for(int i = 0 ; i < tokens.Count ;i ++)
  {
    if(tokens[i].Type == TypeToken.GreaterThan)
    {
     prop = (string)tokens[i + 1].Value;
     if(prop == "Power")
     {
      if(tokens[i + 2].Type == TypeToken.Equal)
      {
      Signe = TypeToken.EqualEqual;
      SecondCondition = (string)tokens[i + 4].Value;
      }
      else
      {
      Signe = tokens[i + 2].Type;
      SecondCondition = (string)tokens[i + 3].Value;
      }
     }
     else
     {
      Signe = TypeToken.EqualEqual;
      SecondCondition = (string)tokens[i + 4].Value;
     }
     break;
    }
  }
  List<GameObject> result = new List<GameObject>();
  //Iterar sobre Source
  for(int i = 0 ; i  < Source.Count ; i++)
  {
    CardUnidad cardUnidad = Source[i].GetComponent<CardUnidad>();
    if(prop == "Range")
    {
      if(SecondCondition == "Siege")
      {
       if(cardUnidad.Tipo == "Asedio" || cardUnidad.Tipo == "Silver" || cardUnidad.Tipo == "Oro")
       {
          result.Add(Source[i]);
       }
      }
      else if(SecondCondition == "Distance")
      {
        if(cardUnidad.Tipo == "Distancia" || cardUnidad.Tipo == "Silver" || cardUnidad.Tipo == "Oro")
       {
        result.Add(Source[i]);
       }
      }
      else
      {
        if(cardUnidad.Tipo == "Cuerpo a Cuerpo" || cardUnidad.Tipo == "Silver" || cardUnidad.Tipo == "Oro")
       {
       result.Add(Source[i]);
       }
      }
    }
    else if(prop ==  "Type")
    {
     if(cardUnidad.Tipo == "Cuerpo a Cuerpo" && SecondCondition == "Meele" || cardUnidad.Tipo == "Asedio" && SecondCondition == "Siege" || cardUnidad.Tipo == "Distancia" && SecondCondition == "Distance" || cardUnidad.Tipo == "Aumento" && SecondCondition == "Increase" || cardUnidad.Tipo == "Clima" && SecondCondition == "Clime" || cardUnidad.Tipo == SecondCondition)
     {
      result.Add(Source[i]);
     }
    }
    else if(prop == "Faction")
    {
      if(SecondCondition == "Red" )
      {
       if(cardUnidad.Faction == "Red")
       {
        result.Add(Source[i]);
       }  
      }
      else 
      {
       if(cardUnidad.Faction == "Legend")
       {
        result.Add(Source[i]);
       }
      }
    }
    else if(prop == "Power")
    {
      if(Signe == TypeToken.GreaterEqualThan)
      {     
        if(cardUnidad.Attack >= int.Parse(SecondCondition))
        {
          result.Add(Source[i]);
        }
      }
      else if(Signe == TypeToken.GreaterThan)
      {
        if(cardUnidad.Attack > int.Parse(SecondCondition))
        {
          result.Add(Source[i]);
        }
      }
      else if(Signe == TypeToken.LessThan)
      {
        if(cardUnidad.Attack <= int.Parse(SecondCondition))
        {
          result.Add(Source[i]);
        }
      }
      else if(Signe == TypeToken.SmallerThan)
      {
        if(cardUnidad.Attack < int.Parse(SecondCondition))
        {
          result.Add(Source[i]);
        }
      }
      else
      {
        if(cardUnidad.Attack == int.Parse(SecondCondition))
        {
          result.Add(Source[i]);
        }
      }
    }
  }
 return result;
}
}
