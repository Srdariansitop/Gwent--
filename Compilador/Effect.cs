using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;
using System.Linq;

public class Effect : MonoBehaviour
{
   public  List<Param> Params;
   public  string Name;
   public string Acction;
  // public Node Parent = new Node(new List<Node>(), "Parent");

     ///<summary>
     ///Encargado de Instanciar los valores del efecto para despues guardalo como un prefab
    ///</summary>
   public void EffectInstanciate(List<Param> paramss , string name , List<Token> actiontoken)
   {
   Params = paramss;
   Name = name;
   Acction = StringAction(actiontoken);
   //TreeAction(Parent, actiontoken, 0, actiontoken.Count);
   Controller.NumEffect += 1;
   PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/EffectsResources/Effect" + Controller.NumEffect + ".prefab");

   }
   
      ///<summary>
     ///Este metodo es el encargado de convertir la lista del Action a String para q los datos se pueden pasar de una escena a otra correctamente
    ///</summary>
   public string StringAction(List<Token> actionToken)
   {
      string result ="";
      foreach(var a in actionToken)
      {
         if(a.Type == TypeToken.String)
         {
            result += " " +  '"' + (string)a.Value + '"' ;
            continue;
         }
        result += " " + (string)a.Value;
      }
      return result;
   }
  
      ///<summary>
     ///Este metodo es el encargado de generar un arbol a partir de los tokens del Action
    ///</summary>
   public static void TreeAction(Node parent , List<Token> tokens , int posinit, int posfinal)
        {
            for(int i = posinit; i < posfinal; i++)
            {
                if (tokens[i].Type == TypeToken.For)
                {
                    int pos = i + 5;
                    SemanticAnalyzer.ExitContext(ref pos, tokens, 0);
                    List<Token> newtokens = ListComplete(tokens, pos - 2, i + 5);
                    Node node = new Node(new List<Node>(), "For");
                    parent.Children.Add(node);
                    TreeAction(node, newtokens, 0, newtokens.Count);
                    i = pos - 1;
                }
                else if (tokens[i].Type == TypeToken.While)
                {
                    int posaux = i + 2;
                    SemanticAnalyzer.ExitParenthesis(ref posaux, tokens, 0);
                    List<Token> newtokens = ListComplete(tokens, posfinal-1, posaux);
                    Node node = new Node(new List<Node>(), "While");
                    parent.Children.Add(node);
                    TreeAction(node, newtokens, 0, newtokens.Count);
                    break;
                }
                else
                {
                    int posfinalaux = PositionPuntCom(tokens, i + 1);
                    List<Token> nodevalue = ListComplete(tokens, posfinalaux-1, i);
                    Node Node = new Node(new List<Node>(), nodevalue);
                    parent.Children.Add(Node);
                    i = posfinalaux;
                }
            }
        }

   public static int PositionPuntCom(List<Token> tokens, int posinit)
        {
            int x = 0;
            for(int i = posinit; i < tokens.Count; i++)
            {
                if(tokens[i].Type == TypeToken.PuntComa)
                {
                    x = i;
                    break;
                }
            }
            return x;
        }
   public static List<Token> ListComplete(List<Token> tokens , int posfinal , int posinit)
        {
            List<Token> result = new List<Token>();
            for(int i = posinit; i <= posfinal; i++)
            {
                result.Add(tokens[i]);
            }
            return result;
        }

}
