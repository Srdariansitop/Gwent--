using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using System;
using System.IO;

public class CompilerEffect: MonoBehaviour
{
   public static string NameEffect;
   public static List<Param> Params = new List<Param>();
   public static List<Token> ActionTokens = new List<Token>();
   
   ///<summary>
   ///Este metodo es el encargado de Parsear el efecto declarado en la carta actual
   ///</summary>
   public static void ExpressionEffect(List<Token> tokens, int pos , int posfinal ,Token ultimate , List<Token> actuallyToken)
   {
      if(pos >= posfinal && actuallyToken.Count == 0)
      {
        return;
      }
      else if(ultimate == null)
      {
          actuallyToken.Add(tokens[pos]);
          ultimate = tokens[pos];
          ExpressionEffect(tokens, pos + 1 , posfinal , ultimate, actuallyToken );
      }
      else if(ultimate.Type == TypeToken.Name)
      {
         if (SemanticAnalyzer.Expect(tokens[pos].Type, TypeToken.Equal))
         {
          actuallyToken.Add(tokens[pos]);
          ultimate = tokens[pos];
          ExpressionEffect(tokens, pos + 1 , posfinal , ultimate, actuallyToken );
         }
         else
         {
         SemanticAnalyzer.SemancticError = true;
         Controller.ErrorExpected('=');
         }
      }
      else if(ultimate.Type == TypeToken.Equal)
      {
         if(NameEffect != null)
         {
            //Error
         }
        else if(actuallyToken[0].Type == TypeToken.Name)
         {
            if(tokens[pos].Type == TypeToken.String)
            {
               NameEffect = (string)tokens[pos].Value;
               actuallyToken = new List<Token>();
               ultimate = null;
               ExpressionEffect(tokens,pos + 1,posfinal,ultimate,actuallyToken);
            }
            else
            {
            SemanticAnalyzer.SemancticError = true;
            Controller.ExpressionInvalidate(tokens[pos]);
            }
         }
         else
         {
         SemanticAnalyzer.SemancticError = true;
         Controller.ExpressionInvalidate(tokens[pos]);
         }
      }
      else if(ultimate.Type == TypeToken.Params)
      {
         if(SemanticAnalyzer.Expect(TypeToken.KeyLeft,tokens[pos].Type))
         {
           int posexample = pos+1;
           SemanticAnalyzer.ExitContext(ref posexample,tokens,0);
           ParamsAnalyzer(pos + 1 , posexample,tokens);
           actuallyToken = new List<Token>();
           ultimate = null;
           ExpressionEffect(tokens,posexample,posfinal,ultimate,actuallyToken);
         }
         else
         {
            SemanticAnalyzer.SemancticError = true;
            Controller.ErrorExpected('{');
         }
      }
      else if(ultimate.Type == TypeToken.Action)
      {
         if(ParserAction(tokens,pos))
         {
            pos = pos + 6;
            if(SemanticAnalyzer.Expect(tokens[pos].Type,TypeToken.KeyLeft))
            {
               int posexample = pos+1;
               SemanticAnalyzer.ExitContext(ref posexample,tokens,0);
               ActionList(tokens,pos + 1,posexample - 1);
               ActionParsing.ParsingActionEffects(ActionTokens,0,ActionTokens.Count);
               actuallyToken = new List<Token>();
               ultimate = null;
               ExpressionEffect(tokens,posexample + 1,posfinal,ultimate,actuallyToken);
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
         }
      }
      else
      {
         SemanticAnalyzer.SemancticError = true;
         Controller.ExpressionInvalidate(tokens[pos]);
      }

   }
  
   ///<summary>
   ///Este metodo es el encargado analizar y evaluar los params del efecto y guardarlo en la carta actual
   ///</summary>
   public static void ParamsAnalyzer(int pos , int posfinal , List<Token> tokens)
   {
     if(pos >= posfinal)
     {
      return;
     }
     if(tokens[pos].Type == TypeToken.Var)
     {
      if(SemanticAnalyzer.Expect(TypeToken.Equal,tokens[pos + 1].Type))
      {
         if(SemanticAnalyzer.Expect(TypeToken.NumberWord,tokens[pos + 2].Type)|| SemanticAnalyzer.Expect(TypeToken.BoolWord,tokens[pos + 2].Type) ||SemanticAnalyzer.Expect(TypeToken.StringWord,tokens[pos + 2].Type))
         {
            Param param ;
            if(tokens[pos + 2].Type == TypeToken.BoolWord)
            {
              param = new Param(TypeParam.Bool,(string)tokens[pos].Value);
            }
            else if(tokens[pos + 2].Type == TypeToken.NumberWord)
            {
              param = new Param(TypeParam.Number,(string)tokens[pos].Value);
            }
            else
            {
              param = new Param(TypeParam.String,(string)tokens[pos].Value);
            }
           Params.Add(param);
           ParamsAnalyzer(pos + 3,posfinal,tokens);
         }
         else
         {
          Controller.ExpressionInvalidate(tokens[pos + 2]);
         }
      }
      else
      {
        Controller.ErrorExpected('=');
      }
     }
   }


   ///<summary>
   ///Este metodo es el encargado de analizar Semanticamente el Action del effect 
   ///</summary>
  public static bool ParserAction(List<Token> tokens,int pos)
  {
    if(tokens[pos].Type == TypeToken.ParenthesisLeft)
    {
      if(tokens[pos + 1].Type == TypeToken.targets)
      {
         if(tokens[pos + 2].Type == TypeToken.Coma)
         {
             if(tokens[pos + 3].Type == TypeToken.context)
             {
               if(tokens[pos + 4].Type == TypeToken.ParenthesisRigth)
               {
                  if(tokens[pos + 5].Type == TypeToken.Do)
                  {
                     return true;
                  }
                  else
                  {
                  Controller.ExpressionInvalidate(tokens[pos + 5]);
                  return false;
                  }
               }
               else
               {
                  Controller.ErrorExpected(')');
                  return false;
               }
             }
             else
             {
                Controller.ExpressionInvalidate(tokens[pos + 3]);
               return false;
             }
         }
         else
         {
              Controller.ErrorExpected(',');
              return false;
         }
      }
      else
      {
         Controller.ExpressionInvalidate(tokens[pos + 1]);
         return false;
      }
    }
    else
    {
      Controller.ErrorExpected('(');
      return false;
    }
  }


   ///<summary>
   ///Este metodo es el encargado de rellenar el Action List
   ///</summary>
   public static void ActionList(List<Token> tokens , int pos , int posfinal)
   {
      for(int i = pos ; i < posfinal ; i++)
      {
         ActionTokens.Add(tokens[i]);
      }

   }
  public static void ResetEffect()
  {
   NameEffect = null;
   Params = new List<Param>();
   ActionTokens= new List<Token>(); 
  }
 

 
}
