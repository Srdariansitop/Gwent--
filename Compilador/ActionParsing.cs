using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionParsing
{
    ///<summary>
   ///Este metodo es el encargado de analizar Semanticamente el Action del effect profundamente
   ///</summary>
 public static void ParsingActionEffects(List<Token> tokens , int pos , int posfinal)
 {
    for(int k = pos ; k < posfinal ; k++)
    {
      Debug.Log(tokens[k].Value);
    }
   if(pos >= posfinal)
   {
      return;
   }
   else if(tokens[pos].Type == TypeToken.TargetProps)
   {
      if(tokens[pos + 1].Type == TypeToken.Equal)
      {
         if(ExtractToProp((string)tokens[pos].Value) == "Power" && (tokens[pos + 2].Type == TypeToken.Number ||tokens[pos + 2].Type == TypeToken.Var && CompareType(tokens[pos + 2],tokens,TypeToken.Number,pos) ||tokens[pos + 2].Type == TypeToken.Var &&  CompareTypeOfParams(tokens[pos + 2],TypeToken.Number)) )
         {
           Debug.Log("Mataste pa");
         }
         else if(ExtractToProp((string)tokens[pos].Value) == "Name" && (tokens[pos + 2].Type == TypeToken.String || tokens[pos + 2].Type == TypeToken.Var && CompareType(tokens[pos +2],tokens,TypeToken.String,pos) || tokens[pos + 2].Type == TypeToken.Var && CompareTypeOfParams(tokens[pos + 2] , TypeToken.String)) )
         {

         }
         else if(ExtractToProp((string)tokens[pos].Value) == "Faction" )
         { 
           if(tokens[pos + 2].Type == TypeToken.String && ((string)tokens[pos + 2].Value == "Red" || (string)tokens[pos + 2].Value == "Legend"))
           {

           }
           else if(tokens[pos + 2].Type == TypeToken.Var && CompareTypeOfParams(tokens[pos + 2],TypeToken.String) && (ValueStringParams((string)tokens[pos + 2].Value) == "Red" || ValueStringParams((string)tokens[pos + 2].Value) == "Legend"))
           {

           }
           else
           {

           }
         }
      }
      else
      {

      }
   }
   else if(tokens[pos].Type == TypeToken.Var)
   {
      if(tokens[pos + 1].Type == TypeToken.Equal)
      {
        if(tokens[pos + 2].Type == TypeToken.Number || tokens[pos + 2].Type == TypeToken.Bool || tokens[pos + 2].Type == TypeToken.String || tokens[pos + 2].Type == TypeToken.ContextPropBoard || tokens[pos + 2].Type == TypeToken.TargetProps)
        {
            if(tokens[pos + 3].Type == TypeToken.PuntComa)
            {
               ParsingActionEffects(tokens,pos + 4,posfinal);
            }
            else
            {

            }
        }
        else if(tokens[pos + 2].Type == TypeToken.Var)
        {
          if(CompareTypeOfParams(tokens[pos + 2],TypeToken.Bool) ||CompareTypeOfParams(tokens[pos + 2],TypeToken.Number) || CompareTypeOfParams(tokens[pos + 2],TypeToken.String) || ExistsOfContext(tokens,pos,tokens[pos + 2]))
          {
             if(tokens[pos + 3].Type == TypeToken.PuntComa)
             {
               ParsingActionEffects(tokens,pos + 4,posfinal);
             }
             else
             {
               
             }
          }
          else
          {

          }
        }
        else if(tokens[pos + 2].Type == TypeToken.ContextProp)
        {
         if(tokens[pos + 3].Type == TypeToken.ParenthesisLeft)
         {
           if(tokens[pos + 4].Type == TypeToken.ContextTrigger || tokens[pos + 4].Type == TypeToken.TargetProps && ExtractToProp((string)tokens[pos + 4].Value) == "Owner" || tokens[pos + 4].Type == TypeToken.Var && TargetPropValue(tokens,pos,(string)tokens[pos + 4].Value,"Owner") )
           {
             if(tokens[pos + 5].Type == TypeToken.ParenthesisRigth)
             {
               if(tokens[pos + 6].Type == TypeToken.PuntComa)
               {
                 ParsingActionEffects(tokens,pos + 7, posfinal);
               }
               else
               {

               }
               
             }
             else
             {
               
             }
           }
           else
           {

           }
         }
         else
         {

         }
        }
        else
        {

        }
      }
      else if(tokens[pos + 1].Type == TypeToken.SumSum || tokens[pos + 1].Type == TypeToken.RestRest )
      {
         if(CompareTypeOfParams(tokens[pos],TypeToken.Number) || CompareType(tokens[pos],tokens,TypeToken.Number,pos))
         {
            if(tokens[pos + 2].Type == TypeToken.PuntComa)
            {
               ParsingActionEffects(tokens,pos + 3,posfinal);
            }
            else
            {

            }
         }
         else
         {

         }
      }
      else
      {

      }
   }
   else if(tokens[pos].Type == TypeToken.For)
   {
      if(tokens[pos + 1].Type == TypeToken.target )
      {
       if(tokens[pos + 2].Type == TypeToken.In)
       {
         if(tokens[pos + 3].Type == TypeToken.targets )
         {
            if(tokens[pos + 4].Type == TypeToken.KeyLeft)
              {
               
               int posfinalaux = pos + 5;
               SemanticAnalyzer.ExitContext(ref posfinalaux,tokens,0);
               //Analizo el for
               ParsingActionEffects(tokens,pos + 5,posfinalaux - 1);
               //Analizo lo q queda de la funcion
               ParsingActionEffects(tokens,posfinalaux,posfinal);
               }
              else
              {

              } 
         }
         else
         {

         }
       }
       else
       {

       }
      }
      else
      {

      }     
   }
   else if(tokens[pos].Type == TypeToken.While)
   {
      if(tokens[pos + 1].Type == TypeToken.ParenthesisLeft)
      {
        int posfinalaux = pos + 2;
        SemanticAnalyzer.ExitParenthesis(ref posfinalaux , tokens , 0);
        ParsingExpressionBoolean(tokens,pos + 2,posfinalaux - 1);
        ParsingActionEffects(tokens,posfinalaux,posfinal);
      }
      else
      {
         SemanticAnalyzer.SemancticError = true;
         Controller.ErrorExpected('(');
         return;
      }
   }
   else
   {

   }
 }
  

   ///<summary>
   ///Este metodo es el encargado de analizar la expresion Booleana dentro del While osea un predicate
   ///</summary>
 public static void ParsingExpressionBoolean(List<Token> tokens , int pos , int posfinal)
 {
   if(pos == posfinal)
   {
      return;
   }
   if(tokens[pos].Type == TypeToken.Number)
   {
     if(tokens[pos + 1].Type == TypeToken.EqualEqual || tokens[pos + 1].Type == TypeToken.GreaterEqualThan||tokens[pos + 1].Type == TypeToken.GreaterThan || tokens[pos + 1].Type == TypeToken.LessThan || tokens[pos + 1].Type == TypeToken.SmallerThan  )
     {
        if(tokens[pos + 2].Type == TypeToken.Number || tokens[pos + 2].Type == TypeToken.Var && (CompareType(tokens[pos + 2],tokens,TypeToken.Number,pos) || CompareTypeOfParams(tokens[pos + 2],TypeToken.Number) ))
        {
          ParsingExpressionBoolean(tokens,pos + 3,posfinal);
        }
        else 
        {
            Controller.ErrorOfType(tokens[pos],tokens[pos+2]);
            SemanticAnalyzer.SemancticError = true;
            return;
        }
     }
     else if(tokens[pos + 1].Type == TypeToken.SumSum || tokens[pos + 1].Type == TypeToken.RestRest)
     {
      if(tokens[pos + 2].Type == TypeToken.EqualEqual || tokens[pos + 2].Type == TypeToken.GreaterEqualThan||tokens[pos + 2].Type == TypeToken.GreaterThan || tokens[pos + 2].Type == TypeToken.LessThan || tokens[pos + 2].Type == TypeToken.SmallerThan  )
      {
        if(tokens[pos + 3].Type == TypeToken.Number || tokens[pos + 3].Type == TypeToken.Var && (CompareType(tokens[pos + 3],tokens,TypeToken.Number,pos) || CompareTypeOfParams(tokens[pos + 3],TypeToken.Number) ))
        {
          ParsingExpressionBoolean(tokens,pos + 4,posfinal);
        }
        else 
        {
            Controller.ErrorOfType(tokens[pos],tokens[pos+3]);
            SemanticAnalyzer.SemancticError = true;
            return;
        }
      }
      else
      {
         SemanticAnalyzer.SemancticError = true;
         Controller.ErrorExpected('=');
         return;
      }
     }
     else
     {
         SemanticAnalyzer.SemancticError = true;
         Controller.ExpressionInvalidate(tokens[pos + 1]);
         return;
     }
   }
   else if(tokens[pos].Type == TypeToken.Bool || tokens[pos].Type == TypeToken.String)
   {
      if(tokens[pos + 1].Type == TypeToken.EqualEqual)
      {
         if(tokens[pos + 2].Type == tokens[pos].Type || tokens[pos + 2].Type == TypeToken.Var && (CompareType(tokens[pos + 2],tokens,tokens[pos].Type,pos) || CompareTypeOfParams(tokens[pos + 2],tokens[pos].Type) ))
         {
            ParsingExpressionBoolean(tokens,pos + 3,posfinal);
         }
         else 
         {
            Controller.ErrorOfType(tokens[pos],tokens[pos+2]);
            SemanticAnalyzer.SemancticError = true;
            return;
         }
      }
      else 
      {
         SemanticAnalyzer.SemancticError = true;
         Controller.ErrorExpected('=');
         return;
      }
   }
   else if(tokens[pos].Type == TypeToken.Var)
   {
     TypeToken typeToken = TypeofVariable(tokens[pos] , tokens , pos);
     if(typeToken == TypeToken.Action )
     {
       SemanticAnalyzer.SemancticError = true;
       Debug.Log("Variable " + (string)tokens[pos].Value + " has not been declared correctly"); 
       return;
     }
     else if(typeToken == TypeToken.Number && (tokens[pos + 1].Type == TypeToken.EqualEqual || tokens[pos + 1].Type == TypeToken.GreaterEqualThan||tokens[pos + 1].Type == TypeToken.GreaterThan || tokens[pos + 1].Type == TypeToken.LessThan || tokens[pos + 1].Type == TypeToken.SmallerThan ))
     {
      if(tokens[pos + 2 ].Type == TypeToken.Number || tokens[pos + 2].Type == TypeToken.Var && TypeofVariable(tokens[pos + 2] , tokens , pos + 2) == TypeToken.Number)
            {
             ParsingExpressionBoolean(tokens,pos + 3 , posfinal); 
            }
            else 
            {
            Controller.ErrorOfType(tokens[pos],tokens[pos+2]);
            SemanticAnalyzer.SemancticError = true;
            return;
            }
     }
     else if(typeToken == TypeToken.Number && (tokens[pos + 1].Type == TypeToken.SumSum || tokens[pos + 1].Type == TypeToken.RestRest) )
     {
         if(tokens[pos + 2].Type == TypeToken.EqualEqual || tokens[pos + 2].Type == TypeToken.GreaterEqualThan||tokens[pos + 2].Type == TypeToken.GreaterThan || tokens[pos + 2].Type == TypeToken.LessThan || tokens[pos + 2].Type == TypeToken.SmallerThan)
         {
            if(tokens[pos + 3 ].Type == TypeToken.Number || tokens[pos + 3].Type == TypeToken.Var && TypeofVariable(tokens[pos + 3] , tokens , pos + 3) == TypeToken.Number)
            {
             ParsingExpressionBoolean(tokens,pos + 4 , posfinal); 
            }
            else 
            {
            Controller.ErrorOfType(tokens[pos],tokens[pos+3]);
            SemanticAnalyzer.SemancticError = true;
            return;
            }
         }
     }
     else if((typeToken == TypeToken.String || typeToken == TypeToken.Bool)&& tokens[pos + 1].Type == TypeToken.EqualEqual)
     {
         if(tokens[pos + 2 ].Type == typeToken || tokens[pos + 2].Type == TypeToken.Var && TypeofVariable(tokens[pos + 2] , tokens , pos + 2) == typeToken)
         {
            ParsingExpressionBoolean(tokens,pos + 3 , posfinal); 
         }
         else 
         {
            Controller.ErrorOfType(tokens[pos],tokens[pos+2]);
            SemanticAnalyzer.SemancticError = true;
            return;
         }
     }
     else 
     {
      SemanticAnalyzer.SemancticError = true;
      Controller.ExpressionInvalidate(tokens[pos + 1]);
      return;
     }
   }
   else
   {
      SemanticAnalyzer.SemancticError = true;
      Controller.ExpressionInvalidate(tokens[pos]);
      return;
   }

 }

   ///<summary>
   ///Este metodo es el encargado de Comparar y buscar si fue declarado en el Action la variable y buscar su valor
   ///</summary>
  public static bool CompareType(Token token , List<Token> tokens , TypeToken typeToken , int pos)
  {

   for(int i = 0 ; i < pos ; i++)
   {
      if(tokens[i].Type == TypeToken.Var && (string)tokens[i].Value == (string)token.Value)
      {
         if(tokens[i + 1].Type == TypeToken.Equal && typeToken == tokens[i + 2].Type)
         {
           return true;
         }
         else
         {
            return false;
         }
      }
   }
   return false;
  }

   ///<summary>
   ///Este metodo es el encargado de Comparar y buscar en el Params si la variable fue declarada ahi y cual es su valor
   ///</summary>
  public static bool CompareTypeOfParams(Token token , TypeToken typeToken)
  {
     foreach(var param in CompilerEffect.Params)
     {
        if(param.Name == (string)token.Value)
        {
          if(typeToken == TypeToken.String && param.Type == TypeParam.String || typeToken == TypeToken.Number && param.Type == TypeParam.Number || typeToken == TypeToken.Bool && param.Type == TypeParam.Bool )
          {
            return true;
          }
        }
     }
     return false;
  }


   ///<summary>
   ///Este metodo es el utilizado para saber el tipo de una variable 
   ///</summary>
 public static TypeToken TypeofVariable(Token token , List<Token> tokens , int pos)
 {
   
   TypeToken typeToken = TypeToken.Action;
   //Buscar en el params
   foreach(var param in CompilerEffect.Params)
   {
      if(param.Name == (string)token.Value)
      {
         if(param.Type == TypeParam.Bool)
         {
            typeToken = TypeToken.Bool;
         }
         else if(param.Type == TypeParam.Number)
         {
            typeToken = TypeToken.Number;
         }
         else
         {
            typeToken = TypeToken.String;
         }
      }
   }
   //Buscar en el propio metodo
   for(int i = 0 ; i < pos ; i++)
   {
      if(tokens[i].Type == TypeToken.Var && (string)tokens[i].Value == (string)token.Value && tokens[i + 1].Type == TypeToken.Equal)
      {
         if(tokens[i + 2].Type == TypeToken.String || tokens[i + 2].Type == TypeToken.Number || tokens[i + 2].Type == TypeToken.Bool )
         {
            typeToken = tokens[i + 2].Type;
         }
         else
         {
            typeToken = TypeofVariable(tokens[i + 2] , tokens  , i + 2);
         }
      }
   }
    return typeToken;
 }
   
   ///<summary>
   ///Este metodo es el utilizado para saber q propiedad se le pide al target
   ///</summary>
public static string ExtractToProp(string word)
{
   string result = "";
   for(int i = 0 ; i < word.Length;i++)
   {
      if(word[i] == '.')
      {
        for(int j = i + 1 ; j < word.Length;j++)
        {
         result+=word[j];
        }
        break;
      }
   }
   return result;
}

   ///<summary>
   ///Este metodo es el utilizado para saber si una variable existe en el contexto definido
   ///</summary>
public static bool ExistsOfContext(List<Token>tokens , int pos , Token token)
{
   for(int i = 0 ; i < pos; i++)
   {
     if(tokens[i].Type == TypeToken.Var && (string)tokens[i].Value == (string)token.Value)
     {
      return true;
     } 
   }
   return false;
}

   ///<summary>
   ///Este metodo es ver la propiedad q pide el target
   ///</summary>
public static bool TargetPropValue(List<Token> tokens , int pos , string name , string propieties)
{
   for(int i = 0 ; i < pos ; i++)
   {
    if(tokens[i].Type == TypeToken.Var && (string)tokens[i].Value == name && tokens[i+1].Type == TypeToken.Equal)
    {
      if(tokens[i + 2].Type == TypeToken.TargetProps && ExtractToProp((string)tokens[i+2].Value) == propieties)
      {
         return true;
      }
    }
   }
   return false;
}

   ///<summary>
   ///Este metodo es ver si en las propiedades del Target de Faction y Type los valores de las variables se ajustan al contexto del juego
   ///</summary>
public static string ValueStringParams(string name)
{
   string result = "";
   for(int i = 0 ; i < CompilerEffect.Params.Count ; i++ )
   {
      if(name == CompilerEffect.Params[i].Name)
      {
         result = CompilerEffect.Params[i].ValueString;
      }
   }
   return result;
}

}
