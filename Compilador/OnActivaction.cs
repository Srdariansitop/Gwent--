using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class OnActivaction
{
  public List<GameObject> effects = new List<GameObject>();
  //Selector
  public string Source;
  public bool Single;
  public string PredicateParam;
  public TypeToken Signe;
  public string PredicateType;


  public static void Selector(List<Token> tokens , int posinit , int posfinal)
  {
    if(posinit >= posfinal)
    {
        return;
    }
    else if(tokens[posinit].Type == TypeToken.Source)
    {
      if(tokens[posinit + 1].Type == TypeToken.Equal)
      {
        if(tokens[posinit + 2].Type == TypeToken.SourceTemp)
        {
           CompilerCard.OnActivactionEffects.Source = (string)tokens[posinit + 2].Value;
           Selector(tokens,posinit + 3, posfinal);
        }
        else
        {
          SemanticAnalyzer.SemancticError = true;
          Controller.ExpressionInvalidate(tokens[posinit + 2]);
        }
      }
      else
      {
        SemanticAnalyzer.SemancticError = true;
        Controller.ErrorExpected('=');
      }
    }
    else if(tokens[posinit].Type == TypeToken.Single)
    {
        if(tokens[posinit + 1].Type == TypeToken.Equal)
        {
          if(tokens[posinit + 2].Type == TypeToken.Bool)
          {
           CompilerCard.OnActivactionEffects.Single = (bool)tokens[posinit + 2].Value;
           Selector(tokens,posinit + 3, posfinal);
          }
          else
          {
            SemanticAnalyzer.SemancticError = true;
            Controller.ExpressionInvalidate(tokens[posinit + 2]);
          }
        }
        else
        {
          SemanticAnalyzer.SemancticError = true;
          Controller.ErrorExpected('=');
        }
    }
    else if(tokens[posinit].Type == TypeToken.Predicate)
    {
      Predicate(tokens,posinit + 1);
      Selector(tokens,posinit + 8,posfinal);
    }
    else
    {
      SemanticAnalyzer.SemancticError = true;
      Controller.ExpressionInvalidate(tokens[posinit]);
    }
  }

   public static void Predicate(List<Token> tokens , int posinicial)
   {
    
    if(tokens[posinicial].Type == TypeToken.ParenthesisLeft)
    {
        if(tokens[posinicial + 1].Type == TypeToken.unit)
        {
           if(tokens[posinicial + 2].Type == TypeToken.ParenthesisRigth)
           {
               if(tokens[posinicial + 3].Type == TypeToken.Do)
               { 
                 if(tokens[posinicial + 4].Type == TypeToken.unitreferences)
                 {
                  if((string)tokens[posinicial+4].Value == "Power" && (tokens[posinicial + 5].Type == TypeToken.EqualEqual || tokens[posinicial + 5].Type == TypeToken.SmallerThan || tokens[posinicial + 5].Type == TypeToken.LessThan || tokens[posinicial + 5].Type == TypeToken.GreaterThan || tokens[posinicial + 5].Type == TypeToken.GreaterEqualThan))
                  {
                     if(tokens[posinicial + 6].Type == TypeToken.Number)
                     {
                        CompilerCard.OnActivactionEffects.PredicateType = (string)tokens[posinicial + 4].Value;
                        CompilerCard.OnActivactionEffects.PredicateParam = (string)tokens[posinicial + 6].Value;
                        CompilerCard.OnActivactionEffects.Signe = tokens[posinicial + 5].Type;
                     }
                     else
                     {

                     }
                  }
                  else if((string)tokens[posinicial+4].Value == "Faction" && tokens[posinicial + 5].Type == TypeToken.EqualEqual)
                  {
                    if(tokens[posinicial + 6].Type == TypeToken.Red || tokens[posinicial + 6].Type == TypeToken.Legend)
                     {
                        CompilerCard.OnActivactionEffects.PredicateType = (string)tokens[posinicial + 4].Value;
                        CompilerCard.OnActivactionEffects.PredicateParam = (string)tokens[posinicial + 6].Value;
                     }
                     else
                     {
                      //Error
                     }
                  }
                  else if((string)tokens[posinicial+4].Value == "Type" && tokens[posinicial + 5].Type == TypeToken.EqualEqual)
                  {
                     if(tokens[posinicial + 6 ].Type == TypeToken.Meele || tokens[posinicial + 6 ].Type == TypeToken.Siege || tokens[posinicial + 6 ].Type == TypeToken.Distance || tokens[posinicial + 6 ].Type == TypeToken.Leader || tokens[posinicial + 6 ].Type == TypeToken.Clime || tokens[posinicial + 6 ].Type == TypeToken.Increase )
                     {
                        CompilerCard.OnActivactionEffects.PredicateType = (string)tokens[posinicial + 4].Value;
                        CompilerCard.OnActivactionEffects.PredicateParam = (string)tokens[posinicial + 6].Value;
                     }
                     else
                     {

                     }
                  }
                  else if((string)tokens[posinicial+4].Value == "Range" && tokens[posinicial + 5].Type == TypeToken.EqualEqual)
                  {
                     if(tokens[posinicial + 6].Type == TypeToken.Meele || tokens[posinicial + 6].Type == TypeToken.Siege || tokens[posinicial + 6].Type == TypeToken.Distance)
                     {
                        CompilerCard.OnActivactionEffects.PredicateType = (string)tokens[posinicial + 4].Value;
                        CompilerCard.OnActivactionEffects.PredicateParam = (string)tokens[posinicial + 6].Value;
                     }
                     else
                     {
                      //error
                     }
                  }
                  else
                  {
                    //Error
                  }
                 }
                 else
                 {
                  SemanticAnalyzer.SemancticError = true;
                  Controller.ExpressionInvalidate(tokens[posinicial + 4]);
                 }
               }
               else
               {
                  SemanticAnalyzer.SemancticError = true;
                  Controller.ExpressionInvalidate(tokens[posinicial+ 3]);
               }
           }
           else
           {
            SemanticAnalyzer.SemancticError = true;
            Controller.ErrorExpected(')');
           }
        }
        else
        {
         SemanticAnalyzer.SemancticError = true;
         Controller.ExpressionInvalidate(tokens[posinicial + 2]);
        }
    }
    else
    {
      SemanticAnalyzer.SemancticError = true;
      Controller.ErrorExpected('(');
    }
   }

  public static List<GameObject> SourceReturn(string Source , string Faction)
  {
    Deck deck = GameObject.Find("DeckRed").GetComponent<Deck>();
    Deck deck1 = GameObject.Find("DeckLegendarios").GetComponent<Deck>();
    GameObject canvasObject = GameObject.FindGameObjectWithTag("Invocadas");
    if(Source == "Hand" && Faction == "Red" || Source == "OtherHand" && Faction == "Legend")
    {
      return deck.hand;
    }
    else if(Source == "Hand" && Faction == "Legend" || Source == "OtherHand" && Faction == "Red")
    {
      return deck1.hand;
    }
    else if(Source == "Field" && Faction == "Red" || Source == "OtherField" && Faction == "Legend" )
    {
      List<GameObject> list = new List<GameObject>();
      foreach(Transform item in canvasObject.transform)
      {
        CardUnidad actually = item.gameObject.GetComponent<CardUnidad>();
        if(actually != null && actually.Faction == "Red")
        {
          list.Add(actually.gameObject);
        }
      }
      return list;
    }
    else if(Source == "Field" && Faction == "Legend" || Source == "OtherField" && Faction == "Red")
    {
      List<GameObject> list = new List<GameObject>();
      foreach(Transform item in canvasObject.transform)
      {
        CardUnidad actually = item.gameObject.GetComponent<CardUnidad>();
        if(actually != null && actually.Faction == "Legend")
        {
          list.Add(actually.gameObject);
        }
      }
      return list;
    }
    else if(Source == "Deck" && Faction == "Red" || Source == "Deck" && Faction == "Legend")
    {
     return deck.deck;
    }
    else 
    {
     return deck1.deck;
    }
 
  }


}
