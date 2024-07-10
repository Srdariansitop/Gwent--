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

}
