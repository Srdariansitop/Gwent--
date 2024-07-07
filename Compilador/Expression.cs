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
                    SemanticAnalyzer.SemancticError = true;
                    Debug.Log("You have to define the" +(string)actually[posinicial+ 2].Value + "effect beforehand");
                    }
               }
               else
               {
                  SemanticAnalyzer.SemancticError = true;
                  Controller.ExpressionInvalidate(actually[posinicial + 2]);
               }
          }
          else if(actually[posinicial + 1].Type == TypeToken.KeyLeft)
          {
             int posfinal = posinicial + 2;
             SemanticAnalyzer.ExitContext(ref posfinal , actually,0);
             if(actually[posinicial + 2].Type == TypeToken.Name)
             {
               if(actually[posinicial + 3].Type == TypeToken.Equal)
               {
                  if(actually[posinicial + 4].Type == TypeToken.String)
                  {
                       if(FolderEffects((string)actually[posinicial + 4].Value))
                       {
                          Effect vareffect = EffectResult((string)actually[posinicial + 4].Value);
                          ParamsOnEffect(vareffect,actually,posinicial + 5 , posfinal - 2);
                          CompilerCard.OnActivactionEffects.effects.Add(vareffect);
                          ParsingOnActivaction(actually,posfinal);
                       }
                       else
                       {
                        SemanticAnalyzer.SemancticError = true;
                        Debug.Log("You have to define the" +(string)actually[posinicial+ 4].Value + "effect beforehand");
                       }
                  }
                  else
                  {
                   SemanticAnalyzer.SemancticError = true;
                   Controller.ExpressionInvalidate(actually[posinicial + 4]);
                  }
               }
               else
               {
                  SemanticAnalyzer.SemancticError = true;
                  Controller.ErrorExpected('=');
               }
             }
             else
             {
               SemanticAnalyzer.SemancticError = true;
               Controller.ExpressionInvalidate(actually[posinicial + 2]);
             }
          }
          else
          {
            SemanticAnalyzer.SemancticError = true;
            Controller.ExpressionInvalidate(actually[posinicial + 1]);
          }
       }
      else if(actually[posinicial].Type == TypeToken.Selector)
      {
        if(SemanticAnalyzer.Expect(actually[posinicial + 1].Type, TypeToken.KeyLeft))
        {
          int posfinal = posinicial + 2;
          SemanticAnalyzer.ExitContext(ref posfinal , actually,0);
          OnActivaction.Selector(actually,posinicial +2 ,posfinal - 1);
          ParsingOnActivaction(actually,posfinal);
        }
        else
        {

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

   ///<summary>
   ///Este metodo es para darle valor a los parametros del efecto y controlar cualquier tipo de Error de tipo q pueda existir
   ///</summary>
  public static void ParamsOnEffect(Effect effect,List<Token> tokens , int posinit , int posfinal)
  {
    bool search = false;
    if(posinit >= posfinal)
    {
      return;
    }
    else if(tokens[posinit].Type == TypeToken.Var)
    {
      for(int i = 0 ; i < effect.Params.Count;i++)
      {
        if(effect.Params[i].Name == (string)tokens[posinit].Value)
        {
          if(tokens[posinit + 1].Type == TypeToken.Equal)
          {
             if(tokens[posinit+2].Type == TypeToken.Number && effect.Params[i].Type == TypeParam.Number || tokens[posinit+2].Type == TypeToken.Bool && effect.Params[i].Type == TypeParam.Bool || tokens[posinit+2].Type == TypeToken.String && effect.Params[i].Type == TypeParam.String)
             {
               effect.Params[i].ValueString = (string)tokens[posinit + 2].Value;
               //Debug.Log(effect.Params[i].ValueString);
               ParamsOnEffect(effect,tokens,posinit+3,posfinal);
               search = true;
             }
             else
             {
              SemanticAnalyzer.SemancticError = true;
              Debug.Log("The " + effect.Params[i].Name + " parameter does not match the expected type");
             }
          }
          else
          {
            SemanticAnalyzer.SemancticError = true;
            Controller.ErrorExpected('=');
          }
          break;
        }
      }
      //Encontrado o no?
      if(search == false)
      {
      SemanticAnalyzer.SemancticError = true;
      Debug.Log("No Parameter was found with the name  " + (string)tokens[posinit].Value);
      }
    }
    else
    {
      SemanticAnalyzer.SemancticError = true;
      Controller.ExpressionInvalidate(tokens[posinit]);
    }
  }

   ///<summary>
   ///Obtengo el efecto actual sobre el q estoy trabajando
   ///</summary>
  public static Effect EffectResult(string effect)
  {
        Effect result = new Effect();
        string folderPath = "Assets/EffectsResources";
        string[] filePaths = AssetDatabase.FindAssets("", new[] { folderPath });
        foreach (string filePath in filePaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(filePath);
            if (assetPath.EndsWith(".prefab"))
            {
                GameObject prefab = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(assetPath)) as GameObject;
                result = prefab.GetComponent<Effect>();          
            }
        }
        return result;
  }
}
