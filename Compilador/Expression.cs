using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
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
             ParsingOnActivaction(actually,2);
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

    ///<summary>
    ///Este metodo es para parsear el OnActivaction de la Carta 
    ///</summary>
    public static void ParsingOnActivaction(List<Token> actually , int posinicial)
    {
       if(posinicial >= actually.Count)
       {
          return;
       }
      else if(actually[posinicial].Type == TypeToken.Effect)
       {
          if(actually[posinicial + 1].Type == TypeToken.Equal)
          {
               if(SemanticAnalyzer.Expect(actually[posinicial + 2].Type,TypeToken.String))
               {
                    if(FolderEffects((string)actually[posinicial + 2].Value))
                    {
                        ParsingOnActivaction(actually , posinicial + 3);
                    }
                    else
                    {
                      //No esta en la lista
                    }
               }
               else
               {
                //ERROR
               }
          }
          else if(actually[posinicial + 1].Type == TypeToken.KeyLeft)
          {
             int posfinal = posinicial + 2;
             SemanticAnalyzer.ExitContext(ref posfinal , actually,0);
             for(int i  = posinicial + 2 ;  i < posfinal -1; i++)
             {
                if(actually[i].Type == TypeToken.Var)
                {
                  if(i + 1 < posfinal - 1 && actually[i + 1].Type == TypeToken.Equal)
                  {

                  }
                  else
                  {

                  }
                }
                else if(actually[i].Type == TypeToken.Name)
                {
                    if(i + 1 < posfinal - 1 && actually[i + 1].Type == TypeToken.Equal)
                  {

                  }
                  else
                  {

                  }
                }
                else
                {
                  //Error
                }
                i +=2;
             }
          }
          else
          {
            //Error
          }
       }
    }

   ///<summary>
   ///Este metodo es el encargado de iterar sobre la carpeta de los efectos prefabs
   ///</summary>
  public static bool FolderEffects(string effect)
  {
     string folderPath = "Assets/EffectsResources";
        string[] filePaths = AssetDatabase.FindAssets("", new[] { folderPath });
        foreach (string filePath in filePaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(filePath);
            if (assetPath.EndsWith(".prefab"))
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                Effect efffectObject = prefab.GetComponent<Effect>();
                if(efffectObject.Name == effect)
                {
                 return true;
                }
              
            }
        }
     return false;
  }

}
