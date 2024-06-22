using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SemanticAnalyzer 
{
   public static bool SemancticError;

          ///<summary>
          ///Este metodo es el que controla el flujo del programa en el analizador
          ///</summary>
        public static void ControllerAnalizerSemantic(List<Token> tokens , int index )
        {
            if(index == tokens.Count || SemancticError == true)
            {
                return;
            }
            int posfinal = 0;
            DefContext(index, tokens, ref posfinal);
            ControllerAnalizerSemantic(tokens, posfinal);
        }

           ///<summary>
          ///Este metodo es el encargado de dirigir el programa dado el contexto actual
         ///</summary>
        public static void DefContext(int pos , List<Token> tokens , ref int posfinal)
        {
            if(tokens[pos].Type == TypeToken.Card|| tokens[pos].Type == TypeToken.Effect)
            {
                if(Expect(tokens[pos + 1].Type , TypeToken.KeyLeft))
                {
                    posfinal = pos + 2;
                    ExitContext(ref posfinal, tokens, 0);
                    if(tokens[pos].Type == TypeToken.Card)
                    {
                        CompilerCard.ExpresionCard(tokens,pos + 2,null,posfinal - 1,new List<Token>());
                    }
                    else
                    {
                        //Metodo Effecto
                    }
                }
                else
                {
                    SemancticError = true;
                    Controller.ErrorExpected('{');
                }
            }
            else
            {
                SemancticError = true;
                Controller.ExpressionInvalidate(tokens[pos]);
            }
        }
 

          ///<summary>
          ///Metodo para cerrar las llaves en el contexto actual , utilizo su puntero para saber cual es el indice final de la expresion
          ///</summary>
        public static void ExitContext(ref int pos, List<Token> tokens, int contador)
        {
            if (contador == -1)
            {
                return;
            }
            if(pos == tokens.Count)
            {
               SemancticError = true;
               Controller.ErrorExpected('}');
                return;
            }
            if(tokens[pos].Type == TypeToken.KeyLeft)
            {
                pos += 1;
                contador += 1;
                ExitContext(ref pos, tokens, contador);
            }
            else if(tokens[pos].Type == TypeToken.KeyRigth)
            {
                pos += 1;
                contador -= 1;
                ExitContext(ref pos, tokens, contador);
            }
            else
            {
                pos += 1;
                ExitContext(ref pos, tokens, contador);
            }
        }
        ///<summary>
        ///Metodo utlizado para devolver un booleano ,dado si el token actual coincide con el esperado
        ///</summary>
        public static bool Expect(TypeToken Token , TypeToken TokenExpect)
        {
            if(Token == TokenExpect)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void SquareExit(List<Token> list ,ref int posinit ,int posfinal,int contador)
        {
            
            for(int i = posinit;i <= posfinal;i++)
            {
              
               if(list[i].Type == TypeToken.SquareBracketLeft)
                {
                    posinit = i;
                    contador++;
                
                }
               if(list[i].Type == TypeToken.SquareBracketRigth)
                {
                    posinit = i;
                    contador--;
                }
                   if(contador == -1)
                {
                    posinit = i;
                    break;
                }
               
            }
            if(contador != -1)
            {
              SemancticError = true;
               Controller.ErrorExpected(']');
            }
            
        }
  
        ///<summary>
        ///Metodo utlizado para generar la lista de Tokens q posteriormente analizara AST
        ///</summary>
        public static List<Token> ContextOfPower(List<Token> actually , int init , int final)
        {
            List<Token> Power = new List<Token>();
              for(int i = init ; i < final ; i++)
              {
                if(actually[i].Type != TypeToken.Number && actually[i].Type != TypeToken.Sum && actually[i].Type != TypeToken.Rest && actually[i].Type != TypeToken.Division && actually[i].Type != TypeToken.Multiplication)
                {
                    break;
                }
                Power.Add(actually[i]);
              }
              return Power;
        }
          public static int PosFinalOfPower(List<Token> actually , int init , int final)
        {
              int PowerFinal =  0;
              for(int i = init ; i < final ; i++)
              {
                if(actually[i].Type != TypeToken.Number && actually[i].Type != TypeToken.Sum && actually[i].Type != TypeToken.Rest && actually[i].Type != TypeToken.Division && actually[i].Type != TypeToken.Multiplication)
                {
                    PowerFinal = i;
                    break;
                }
                
              }
              if(PowerFinal == 0)
              {
                PowerFinal = final ;
              }
              else
              {
                PowerFinal -=1;
              }
              
              return PowerFinal;
        }
       
}
