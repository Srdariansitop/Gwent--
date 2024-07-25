using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;
using UnityEditor;
public class Controller : MonoBehaviour
{
    public InputField inputField;
    public CardObject cardObject;
    public Effect effect;
    public static int NumCard;
    public static int NumEffect;

    ///<summary>
    ///Este metodo es el encargado de Controllar el flujo del programa entero tanto para la creacion de un efecto como para la de una carta
    ///</summary>
    public void ClickBotton()
    {
       cardObject = FindObjectOfType<CardObject>();
       effect = FindObjectOfType<Effect>();
       Reset();
       CompilerEffect.ResetEffect();
       string text = inputField.text;
       Lexer lexer = new Lexer();
       List<Token> tokens = lexer.Tokenizar(text);
       if(Lexer.ErrorLexer == false)
       {
          SemanticAnalyzer.ControllerAnalizerSemantic(tokens,0);
        if(SemanticAnalyzer.SemancticError == false)
        {
          if(CompleteCard())
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
          if(CompilerEffect.ActionTokens.Count > 0 && CompilerEffect.NameEffect != null )
          {
          Debug.Log(CompilerEffect.NameEffect);
          foreach(var x in CompilerEffect.Params)
          {
            Debug.Log(x.Name);
          }
           effect.EffectInstanciate(CompilerEffect.Params,CompilerEffect.NameEffect,CompilerEffect.ActionTokens);
          }
          
          else if(SemanticAnalyzer.EffectIns == true)
          {
            Debug.Log("Lack of parameters to declare an effect");
          }
        }
       
       }
      
    } 
    ///<summary>
    ///Este metodo es para saber si la Carta introducida por el usuario esta completa
    ///</summary>
     public static bool CompleteCard()
        {
            if(CompilerCard.Name != null && CompilerCard.Range != null && CompilerCard.Faction != null && CompilerCard.Type != null)
            {
                if(CompilerCard.Type == "Silver" ||CompilerCard.Type == "Gold" || CompilerCard.Type == "Meele" ||CompilerCard.Type == "Distance"||CompilerCard.Type ==  "Siege" )
                {
                   if(CompilerCard.PowerBool == true)
                   {
                       return true;
                   }
                   else
                   {
                      return false;
                   }
                }
                else
                {
                  return true;
                }
            }
            return false;
        }

   
   #region Errores
    
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
        public static void ErrorOfType(Token token , Token token1)
        {
          Debug.Log("You cannot compare the " + token.Value + " variable of Type " + token.Type + " with the variable " + token1.Value + " of type " + token1.Type);
        }

   #endregion
      
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
          CompilerCard.OnActivation = new bool();
          CompilerCard.OnActivactionEffects = new OnActivaction();
          CompilerEffect.ActionTokens = new List<Token>();
          CompilerEffect.Params = new List<Param>();
          CompilerEffect.NameEffect = null;
          SemanticAnalyzer.EffectIns = new bool();
        }
}
