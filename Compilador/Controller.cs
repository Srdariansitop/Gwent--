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
          Debug.Log(CompilerEffect.NameEffect);
          foreach(var x in CompilerEffect.Params)
          {
            Debug.Log(x.Name);
          }
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
           effect.EffectInstanciate(CompilerEffect.Params,CompilerEffect.NameEffect,CompilerEffect.ActionTokens);
          //  GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/EffectsResources/Effect" + Controller.NumEffect + ".prefab");
          //  Debug.Log(prefab);
          //  Debug.Log(PrefabUtility.IsPartOfPrefabInstance(prefab));
          //  Effect effecth = prefab.GetComponent<Effect>();
          //  effecth.ActionToken.Add(new Token("a",TypeToken.StringWord));
          //  PrefabUtility.ApplyPrefabInstance(prefab, InteractionMode.AutomatedAction);
          }
          else
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
