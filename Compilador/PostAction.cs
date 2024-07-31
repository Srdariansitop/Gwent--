using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PostAction 
{
  public string Type = "";
  //Selector
  public string Source;
  public bool Single;
  public string PredicateParam;
  public TypeToken Signe;
  public string PredicateType;

    public static void PostActionParsing(List<Token> tokens , int posinit , int posfinal)
  {
    if(posinit >= posfinal)
    {
      return;
    }
    if(tokens[posinit].Type == TypeToken.Type)
    {
        if(tokens[posinit + 1].Type == TypeToken.Equal)
        {
            if(tokens[posinit + 2].Type == TypeToken.String && Expression.FolderEffects((string)tokens[posinit + 2].Value))
            {
                CompilerCard.PostAction.Type = (string)tokens[posinit + 2].Value;
                PostActionParsing(tokens,posinit + 3,posfinal);
            }
            else
            {
              SemanticAnalyzer.SemancticError = true;
              Debug.Log("You have to define the " +(string)tokens[posinit+ 2].Value + " effect beforehand");
            }
        }
        else
        {
          SemanticAnalyzer.SemancticError = true;
          Controller.ErrorExpected('=');
        }
    }
    else if(tokens[posinit].Type == TypeToken.Selector)
    {
        if(SemanticAnalyzer.Expect(tokens[posinit + 1].Type, TypeToken.KeyLeft))
        {
          int posfinal2 = posinit + 2;
          SemanticAnalyzer.ExitContext(ref posfinal2 , tokens,0);
          OnActivaction.Selector(tokens,posinit +2 ,posfinal - 1,"PostAction");
          PostActionParsing(tokens,posfinal2,posfinal);
        }
        else
        {
          SemanticAnalyzer.SemancticError = true;
          Controller.ErrorExpected('{'); 
        } 
    }
    else
    {
      SemanticAnalyzer.SemancticError = true;
      Controller.ExpressionInvalidate(tokens[posinit]);
    }
  }
}
