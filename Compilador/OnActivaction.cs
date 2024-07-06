using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OnActivaction
{
  public List<Effect> effects = new List<Effect>();
  //Selector
  public string Source;
  public bool Single;



  public static void Selector(List<Token> tokens , int posinit , int posfinal)
  {
    if(posinit >= posfinal)
    {
        return;
    }
    else if(tokens[posinit].Type == TypeToken.Source)
    {
      if(tokens[posinit + 1].Type == TypeToken.Equal)
      {
        if(tokens[posinit + 2].Type == TypeToken.SourceTemp)
        {
           CompilerCard.OnActivactionEffects.Source = (string)tokens[posinit + 2].Value;
           Selector(tokens,posinit + 3, posfinal);
        }
        else
        {

        }
      }
      else
      {

      }
    }
    else if(tokens[posinit].Type == TypeToken.Single)
    {
        if(tokens[posinit + 1].Type == TypeToken.Equal)
        {
          if(tokens[posinit + 2].Type == TypeToken.Bool)
          {
           CompilerCard.OnActivactionEffects.Single = (bool)tokens[posinit + 2].Value;
           Selector(tokens,posinit + 3, posfinal);
          }
          else
          {

          }
        }
        else
        {

        }
    }
  }
}
