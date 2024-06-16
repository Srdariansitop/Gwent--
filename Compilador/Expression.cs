using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expression : MonoBehaviour
{
    ///<summary>
    ///Este metodo es el encargado de Evaluar todas las expresiones creadas en el contexto de Card
    ///</summary>
    public static void Evaluate (List<Token> actually)
    {
        
          if(CompilerCard.Name == null && actually[0].Type == TypeToken.Name)
          {
              CompilerCard.Name = (string)actually[2].Value;
          }
          else if(CompilerCard.Faction == null && actually[0].Type == TypeToken.Faction)
          {
            CompilerCard.Faction = (string)actually[2].Value;
          }
          else if(CompilerCard.Type == null && actually[0].Type == TypeToken.Type)
          {
            CompilerCard.Type = (string)actually[2].Value;
          }
          else if(CompilerCard.Range == null && actually[0].Type == TypeToken.Range)
          {
            List<string> Ranges = new List<string>();
            for(int i = 2 ; i < actually.Count;i++)
            {
              if(actually[i].Type == TypeToken.Siege )
              {
                  Ranges.Add("Siege");
                  continue;
              }
              else if(actually[i].Type == TypeToken.Meele)
              {
                Ranges.Add("Meele");
                continue;
              }
              else if(actually[i].Type == TypeToken.Distance)
              {
                Ranges.Add("Distance");
                continue;
              }
              else if(actually[i].Type == TypeToken.Coma)
              {
                   continue;
              }
              else
              {
                Controller.ExpressionInvalidate(actually[i]);
                SemanticAnalyzer.SemancticError = true;
                break;
              }
            }
             CompilerCard.Range = Ranges.ToArray();
          }
          else if(CompilerCard.OnActivation == false && actually[0].Type == TypeToken.OnActivation)
          {
             CompilerCard.OnActivation = true;
          }
    }
  

    ///<summary>
    ///Este metodo es el encargado de Evaluar el Arbol de Sintaxis abstracta generado por el Parser
    ///</summary>
     public static int EvaluateTree(Node node)
        {
            if(node.Value is char)
            {
                if ((char)node.Value == '+')
                {
                    return EvaluateTree(node.Children[0]) + EvaluateTree(node.Children[1]);
                }
                else if ((char)node.Value == '-')
                {
                    return EvaluateTree(node.Children[0]) - EvaluateTree(node.Children[1]);
                }
                else if ((char)node.Value == '*')
                {
                    return EvaluateTree(node.Children[0]) * EvaluateTree(node.Children[1]);
                }
                else 
                {
                    return EvaluateTree(node.Children[0]) / EvaluateTree(node.Children[1]);
                }
            }
            else
            {
              return Convert.ToInt32(node.Value);
            }
        }

    



}
