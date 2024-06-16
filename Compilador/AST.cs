using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AST 
{
     ///<summary>
     ///Este metodo es el encargado de Generar un Arbol de sintaxis abstracta con un Metodo Recursivo
    ///</summary>
   public static void GenerateAST(List<Token> tokens, ref Node actually)
        {
            if (SumorRest(tokens))
            {
                int Signe = PositionSumorRest(tokens);
                int Left = Signe - 1;
                int Rigth = Signe + 1;
                if (Left >= 0 && Rigth < tokens.Count)
                {
                    Node SumorRest = SumorRestorMultiplicationorDivision(tokens, Signe);
                    if (actually == null)
                    {
                        actually = SumorRest;
                    }
                    else
                    {
                        actually.Children.Add(SumorRest);
                    }
                    List<Token> LeftList = NewExpresion(tokens, Left, 0);
                    List<Token> RightList = NewExpresion(tokens, tokens.Count - 1, Rigth);
                    GenerateAST(LeftList, ref SumorRest);
                    GenerateAST(RightList, ref SumorRest);
                }
                else
                {
                    Controller.ErrorExpresionPower(tokens);
                    Controller.ErrorExpected('#');
                    SemanticAnalyzer.SemancticError = true;
                    return;
                }
            }
            else if (MultiplyorDivision(tokens))
            {
                int Signe = PositionMultiplicationorDivision(tokens);
                int Left = Signe - 1;
                int Rigth = Signe + 1;
                if (Left >= 0 && Rigth < tokens.Count)
                {
                    Node MultiplyorDivision = SumorRestorMultiplicationorDivision(tokens, Signe);
                    if (actually == null)
                    {
                        actually = MultiplyorDivision;
                    }
                    else
                    {
                        actually.Children.Add(MultiplyorDivision);
                    }
                    List<Token> LeftList = NewExpresion(tokens, Left, 0);
                    List<Token> RightList = NewExpresion(tokens, tokens.Count - 1, Rigth);
                    GenerateAST(LeftList,ref MultiplyorDivision);
                    GenerateAST(RightList,ref MultiplyorDivision);
                }
                else
                {
                    Controller.ErrorExpresionPower(tokens);
                    Controller.ErrorExpected('#');
                    SemanticAnalyzer.SemancticError = true;
                    return;
                }
            }
            else if (NumberorNot(tokens))
            {
                
                if (tokens.Count == 1)
                {
                    Node Number = new Node(null, tokens[0].Value);
                    if (actually == null)
                    {
                        actually = Number;
                    }
                    else
                    {
                        actually.Children.Add(Number);
                    }
                    return;
                }
                else
                {
                    Controller.ErrorExpresionPower(tokens);
                    SemanticAnalyzer.SemancticError = true;
                    return;
                }
            }
            else
            {
                Controller.ErrorExpresionPower(tokens);
                SemanticAnalyzer.SemancticError = true;
                return;
            }
        }
      

     public static bool SumorRest(List<Token> tokens)
        {
            for(int i = 0; i < tokens.Count; i++)
            {
                if(tokens[i].Type == TypeToken.Sum || tokens[i].Type == TypeToken.Rest )
                {
                    return true;
                }
            }
            return false;
        }

        public static bool MultiplyorDivision(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TypeToken.Division || tokens[i].Type == TypeToken.Multiplication)
                {
                    return true;
                }
            }
            return false;
        }
          public static bool NumberorNot(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TypeToken.Number)
                {
                    return true;
                }
            }
            return false;
        }

        public static int PositionSumorRest(List<Token> tokens)
        {
            int Final = 0;
            for(int i = 0; i < tokens.Count;i++)
            {
                if(tokens[i].Type == TypeToken.Sum || tokens[i].Type == TypeToken.Rest)
                {
                    Final = i;
                    break;
                }
            }
            return Final;
        }
        public static int PositionMultiplicationorDivision(List<Token> tokens)
        {
            int Final = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TypeToken.Division || tokens[i].Type == TypeToken.Multiplication)
                {
                    Final = i;
                    break;
                }
            }
            return Final;
        }

        public static List<Token> NewExpresion(List<Token> tokens , int final , int inicial)
        {
            List<Token> result = new List<Token>();
            for(int i = inicial; i <= final; i++)
            {
                result.Add(tokens[i]);
            }
            return result;
        }

     public static Node SumorRestorMultiplicationorDivision(List<Token> tokens , int pos)
        {
            if (tokens[pos].Type == TypeToken.Sum)
            {
                return new Node(new List<Node>(), '+');
            }
            else if (tokens[pos].Type == TypeToken.Rest)
            {
                return new Node(new List<Node>(), '-');
            }
            else if (tokens[pos].Type == TypeToken.Multiplication)
            {
                return new Node(new List<Node>(), '*');
            }
            else
            {
                return new Node(new List<Node>(), '/');
            }
        }
}
