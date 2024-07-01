using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CompilerEffect
{
   public static string NameEffect;
   public static List<Param> Params = new List<Param>();
   
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
           ExpressionEffect(tokens,posexample + 1,posfinal,ultimate,actuallyToken);
         }
         else
         {
            Controller.ErrorExpected('{');
         }
      }
      else
      {
         SemanticAnalyzer.SemancticError = true;
         Controller.ExpressionInvalidate(tokens[pos]);
      }

   }
  
   //Metodo Params
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

  public static void ResetEffect()
  {
   NameEffect = null;
  }
 
  



}
