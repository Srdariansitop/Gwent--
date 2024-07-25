using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CompilerCard : MonoBehaviour
{
   public static string Name ;
   public static string Type;
   public static string Faction;
   public static string []Range;
   public static int Power;
   public static bool PowerBool;
   public static bool OnActivation;
   public static OnActivaction OnActivactionEffects = new OnActivaction();

           ///<summary>
          ///Maquina de Estados Recursiva utilizada para Parsear y dar valor a las propiedades de la Carta
          ///</summary>
      public static void ExpresionCard(List<Token> tokens, int pos , Token ultimate , int posfinal,List<Token> actuallyToken)
        {
            if(pos == posfinal)
            {
                return;
            }
            else if(ultimate == null)
            {
               actuallyToken.Add(tokens[pos]);
               ultimate = tokens[pos];
               pos += 1;
               ExpresionCard(tokens, pos, ultimate,posfinal,actuallyToken);   
            }
            else if (ultimate.Type == TypeToken.Name || ultimate.Type == TypeToken.Type|| ultimate.Type == TypeToken.Range || ultimate.Type == TypeToken.Power || ultimate.Type == TypeToken.Faction|| ultimate.Type == TypeToken.OnActivation)
            {
                if (SemanticAnalyzer.Expect(tokens[pos].Type, TypeToken.Equal))
                {
                    actuallyToken.Add(tokens[pos]);
                    ultimate = tokens[pos];
                    pos += 1;
                    ExpresionCard(tokens, pos, ultimate,posfinal,actuallyToken);
                }
                else
                {
                    SemanticAnalyzer.SemancticError = true;
                    Controller.ErrorExpected('=');
                }
            }
            else if (ultimate.Type == TypeToken.Equal)
            {
                if (actuallyToken[0].Type == TypeToken.Name)
                {
                    if (SemanticAnalyzer.Expect(tokens[pos].Type, TypeToken.String))
                    {
                        actuallyToken.Add(tokens[pos]);
                        pos+=1;
                        Expression.Evaluate(actuallyToken);
                        actuallyToken = new List<Token>();
                        ultimate = null;
                        ExpresionCard(tokens,pos,ultimate,posfinal,actuallyToken);
                    }
                    else
                    {
                        SemanticAnalyzer.SemancticError = true;
                        Controller.ExpressionInvalidate(tokens[pos]);
                    }
                }
                else if(actuallyToken[0].Type == TypeToken.Type)
                {
                    if(SemanticAnalyzer.Expect(tokens[pos].Type,TypeToken.Clime) || SemanticAnalyzer.Expect(tokens[pos].Type,TypeToken.Gold) || SemanticAnalyzer.Expect(tokens[pos].Type,TypeToken.Silver) || SemanticAnalyzer.Expect(tokens[pos].Type,TypeToken.Increase) || SemanticAnalyzer.Expect(tokens[pos].Type,TypeToken.Leader))
                    {
                        actuallyToken.Add(tokens[pos]);
                        pos+=1;
                        Expression.Evaluate(actuallyToken);
                        actuallyToken = new List<Token>();
                        ultimate = null;
                        ExpresionCard(tokens,pos,ultimate,posfinal,actuallyToken);
                    }
                    else
                    {
                        SemanticAnalyzer.SemancticError = true;
                      Controller.ExpressionInvalidate(tokens[pos]); 
                    }
                }
                else if(actuallyToken[0].Type == TypeToken.Faction)
                {
                    if(SemanticAnalyzer.Expect(tokens[pos].Type,TypeToken.Red) || SemanticAnalyzer.Expect(tokens[pos].Type,TypeToken.Legend))
                    {
                       actuallyToken.Add(tokens[pos]);
                        pos+=1;
                        Expression.Evaluate(actuallyToken);
                        actuallyToken = new List<Token>();
                        ultimate = null;
                        ExpresionCard(tokens,pos,ultimate,posfinal,actuallyToken);
                    }
                    else
                    {
                        SemanticAnalyzer.SemancticError = true;
                        Controller.ExpressionInvalidate(tokens[pos]);
                    }
                }
                else if(actuallyToken[0].Type == TypeToken.Range|| actuallyToken[0].Type == TypeToken.OnActivation)
                {
                  if(SemanticAnalyzer.Expect(tokens[pos].Type , TypeToken.SquareBracketLeft))
                   {
                      int contextSquareFinal = pos + 1;
                      SemanticAnalyzer.SquareExit(tokens,ref contextSquareFinal,posfinal-1,0);
                      for(int i = pos+1 ; i < contextSquareFinal;i++)
                      {
                        actuallyToken.Add(tokens[i]);
                      }
                      pos = contextSquareFinal + 1;
                      Expression.Evaluate(actuallyToken);
                      actuallyToken = new List<Token>();
                      ultimate = null;
                      ExpresionCard(tokens,pos,ultimate,posfinal,actuallyToken);
                   }
                   else
                   {
                    SemanticAnalyzer.SemancticError = true;
                    Controller.ExpressionInvalidate(tokens[pos]);
                   }
                }
                else if(actuallyToken[0].Type == TypeToken.Power)
                {
                    if (SemanticAnalyzer.Expect(tokens[pos].Type, TypeToken.Number))
                    {
                       List<Token> ExpressionMath = SemanticAnalyzer.ContextOfPower(tokens,pos,posfinal);
                       if(PowerBool == false)
                       {
                       PowerBool = true;
                       Node Tree = null;
                       AST.GenerateAST(ExpressionMath,ref Tree);
                       if(SemanticAnalyzer.SemancticError == false)
                       {
                        Power = Expression.EvaluateTree(Tree);
                       }
                     pos = SemanticAnalyzer.PosFinalOfPower(tokens,pos,posfinal - 1 ) + 1;
                     actuallyToken = new List<Token>();
                     ultimate = null;
                     ExpresionCard(tokens,pos,ultimate,posfinal,actuallyToken);
                       }
                    
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
                    Controller.ExpressionInvalidate(actuallyToken[0]);
                }
            }
            else
            {
                SemanticAnalyzer.SemancticError = true;
                Controller.ExpressionInvalidate(tokens[pos]);
            }
            
        }
       



}
