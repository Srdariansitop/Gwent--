using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public InputField inputField;
    public CardObject cardObject;
    public void ClickBotton()
    {
       cardObject = FindObjectOfType<CardObject>();
       Reset();
       string text = inputField.text;
       Lexer lexer = new Lexer();
       List<Token> tokens = lexer.Tokenizar(text);
       if(Lexer.ErrorLexer == false)
       {
          SemanticAnalyzer.ControllerAnalizerSemantic(tokens,0);
          if(SemanticAnalyzer.SemancticError == false)
          {
          Debug.Log(CompilerCard.Name);
          Debug.Log(CompilerCard.Faction);
          Debug.Log(CompilerCard.Type);
          Debug.Log(CompilerCard.Power);
          if(CompilerCard.Range != null)
          {
            for(int i = 0 ; i < CompilerCard.Range.Length;i++)
            {
              Debug.Log(CompilerCard.Range[i]);
            }
          }
          cardObject.InstanciateNewCard(CompilerCard.Power,CompilerCard.Name,CompilerCard.Faction,CompilerCard.Type,CompilerCard.Range);
          }
       
       }
      
    } 
   
        public static void ErrorToken(string token)
        {
          Debug.Log(" Unidentified " + token + " Token" );
        }
        public static void ExpressionInvalidate(Token token)
        {
         Debug.Log("Invalid expression " + token.Value + " within the context");
        }
        public static void ErrorExpected(char expression)
        { 
          Debug.Log("The program expects a " + expression);
        }
        public static void ErrorExpresionPower(List<Token> tokens)
        {
          string Expresion = "";
           for(int i = 0; i< tokens.Count;i++)
           {
              Expresion = Expresion + " " + tokens[i].Value;
           }
           Debug.Log("Invalid expression "  + Expresion + " within the context Power" );
        }

      ///<summary>
      ///Este metodo es el encargado de limpiar todos los valores estaticos generados por el InputField
      ///</summary>
        public static void Reset()
        {
          Lexer.ErrorLexer = new bool();
          SemanticAnalyzer.SemancticError = new bool();
          CompilerCard.Name = null;
          CompilerCard.Range = null;
          CompilerCard.Type = null;
          CompilerCard.Faction = null;
          CompilerCard.PowerBool = new bool();
        }
}
