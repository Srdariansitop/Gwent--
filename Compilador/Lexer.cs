using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Lexer 
{
    public static bool ErrorLexer;
       public  List<Token> Tokenizar (string text)
        {
            List<Token> result = new List<Token>();
            string word = "";
           for(int i = 0 ; i <  text.Length ; i++)
           {
                if(char.IsWhiteSpace(text[i]))
                  {
                    if(word.Length > 0)
                    {
                       if(IsNumber(word))
                       {
                         result.Add(new Token(word,TypeToken.Number));
                         word = "";
                         continue;
                       }
                       Token temp = WordToken(word);
                       result.Add(temp);
                       word = "";
                    }
                    continue;
                  }
                if(char.IsLetter(text[i]) || text[i] == '.')
                  {
                     if(IsNumber(word))
                     {
                      Controller.ErrorToken(word + text[i]);
                      ErrorLexer = true;
                       break;
                     }
                     word += text[i];
                     continue;
                  }
                if(char.IsNumber(text[i]))
                  {
                      word += text[i];
                      continue;
                  }
                if(word.Length > 0)
                  {
                    if(IsNumber(word))
                    {
                      result.Add(new Token(word,TypeToken.Number));
                      word = "";
                    }
                    else
                    {
                        Token temp = WordToken(word);
                        result.Add(temp);
                        word = "";
                    } 
                  }
                  if(text[i] == '=')
                  {
                    if(text[i + 1] < text.Length && text[i + 1] == '>')
                     {
                        result.Add(new Token("=>",TypeToken.Do));
                        i++;
                        continue;
                     }
                     if(text[i + 1] < text.Length && text[i + 1] == '=')
                     {
                        result.Add(new Token("==",TypeToken.EqualEqual));
                        i++;
                        continue;
                     }
                     result.Add(new Token("=",TypeToken.Equal));
                     continue;
                  }
                if(text[i] == '+')
                  {
                     if(text[i + 1] < text.Length && text[i + 1] == '=')
                     {
                        result.Add(new Token("+=",TypeToken.EqualSum));
                        i++;
                        continue;
                     }
                     if(text[i + 1] < text.Length && text[i + 1] == '+')
                     {
                        result.Add(new Token("++",TypeToken.SumSum));
                        i++;
                        continue;
                     }
                     result.Add(new Token("+",TypeToken.Sum));
                     continue;
                  }
                if(text[i] == '-')
                  {
                     if(text[i + 1] < text.Length && text[i + 1] == '=')
                     {
                        result.Add(new Token("-=",TypeToken.EqualRest));
                        i++;
                        continue;
                     }
                      if(text[i + 1] < text.Length && text[i + 1] == '-')
                     {
                        result.Add(new Token("--",TypeToken.RestRest));
                        i++;
                        continue;
                     }
                     result.Add(new Token("-",TypeToken.Rest));
                     continue;
                  }
                if(text[i] == '>')
                {
                  if(text[i + 1] == '=')
                  {
                    result.Add(new Token(">=" , TypeToken.GreaterEqualThan));
                    i++;
                    continue;
                  }
                  else
                  {
                    result.Add(new Token(">", TypeToken.GreaterThan));
                    continue;
                  }
                }
                if(text[i] == '<')
                {
                  if(text[i + 1] == '=')
                  {
                    result.Add(new Token("<=" , TypeToken.LessThan));
                    i++;
                    continue;
                  }
                  else
                  {
                    result.Add(new Token("<" , TypeToken.SmallerThan));
                    continue;
                  }
                }
                if(text[i] == '*')
                  {
                    result.Add(new Token("*",TypeToken.Multiplication));
                     continue;
                  }
                if(text[i] == '/')
                  {
                    result.Add(new Token("/",TypeToken.Division));
                     continue;
                  }
                if(text[i] == '{')
                {
                  result.Add(new Token("{" , TypeToken.KeyLeft));
                  continue;
                }
                if(text[i] == '}')
                {
                  result.Add(new Token("}" , TypeToken.KeyRigth));
                  continue;
                }
                if(text[i] == '[')
                {
                  result.Add(new Token("[" , TypeToken.SquareBracketLeft));
                  continue;
                }
                  if(text[i] == ']')
                {
                  result.Add(new Token("]" , TypeToken.SquareBracketRigth));
                  continue;
                }
                if(text[i] == '(')
                {
                  result.Add(new Token("(" , TypeToken.ParenthesisLeft));
                  continue;
                }
                  if(text[i] == ')')
                {
                  result.Add(new Token(")" , TypeToken.ParenthesisRigth));
                  continue;
                }
                if(text[i] == ',')
                {
                  result.Add(new Token(",",TypeToken.Coma));
                  continue;
                }
                if(text[i] == ';')
                {
                  result.Add(new Token(";",TypeToken.PuntComa));
                  continue;
                }
                  if(text[i] == '@')
                {
                  if(text[i + 1] < text.Length && text[i + 1] == '@')
                  {
                    result.Add(new Token("@@" , TypeToken.ConcatSpace));
                    i++;
                    continue;
                  }
                  result.Add(new Token("@" , TypeToken.Concat));
                  continue;
                }
                if(text[i] == '|')
                {
                  if(text[i + 1] < text.Length && text[i + 1] == '|')
                  {
                      result.Add(new Token("||" ,TypeToken.Or));
                       i++;
                       continue;
                  }
                  else
                  {
                     ErrorLexer = true;
                    Controller.ErrorExpected('|');
                    break;
                  }
                }
                if(text[i] == '&')
                {
                  if(text[i + 1] < text.Length && text[i + 1] == '&')
                  {
                      result.Add(new Token("&&" ,TypeToken.And));
                       i++;
                       continue;
                  }
                  else
                  {
                    ErrorLexer = true;
                    Controller.ErrorExpected('&');
                    break;
                  }
                }
                if(text[i] == '"')
                {
                  if(i + 1 < text.Length && ValidateMarks(text,i+1))
                  {
                    int tempi= PosMarks(text,i+1);
                    result.Add(new Token(text.Substring(i+1,tempi-(i+1)),TypeToken.String));
                    i = tempi;
                  }
                  else
                  {
                    ErrorLexer = true;
                    Controller.ErrorExpected('"');
                    break;
                  }
                }
                else
                {
                  ErrorLexer = true;
                  Controller.ErrorToken(word + text[i]);
                  break;
                }
           }
            return result;
        }



        public static Token WordToken(string word)
        {
             switch (word)
                    {
                        case "Card":
                            return (new Token("Card", TypeToken.Card));

                        case "Params":
                            return (new Token("Params",TypeToken.Params));    

                        case "Effect":
                            return (new Token("Effect", TypeToken.Effect));
                        
                         case "OnActivation":
                            return (new Token("OnActivation",TypeToken.OnActivation));    

                        case "Type":
                            return (new Token("Type", TypeToken.Type));
                        
                        case "True":
                             return(new Token(true,TypeToken.Bool));

                        case "False":
                             return (new Token(false,TypeToken.Bool));

                        case "Targets":
                             return(new Token("Targets",TypeToken.targets));
                        
                        case "Target":
                              return(new Token("Target",TypeToken.target));
                        
                        case "Context":
                             return(new Token("Context",TypeToken.context));

                        case "Number":
                              return(new Token("Number",TypeToken.NumberWord));
                  
                        case "String":
                             return (new Token("String",TypeToken.StringWord));

                        case "Bool":
                             return (new Token("Bool",TypeToken.BoolWord));
                             
                        case "Gold":
                             return (new Token("Gold",TypeToken.Gold));

                        case "Silver":
                             return (new Token("Silver",TypeToken.Silver));

                        case "Clime":
                             return (new Token("Clime",TypeToken.Clime));

                         case "Leader":
                             return (new Token("Leader",TypeToken.Leader));

                        case "Increase":
                             return (new Token("Increase",TypeToken.Increase));
                        
                        case "Red":
                             return (new Token("Red",TypeToken.Red));

                        case "Legend":
                             return (new Token("Legend",TypeToken.Legend));

                        case "Action":
                            return (new Token("Action", TypeToken.Action));

                        case "Name":
                            return (new Token("Name", TypeToken.Name));
                            
                        case "Faction":
                            return (new Token("Faction", TypeToken.Faction));
                            
                        case "Power":
                            return (new Token("Power", TypeToken.Power));
                            
                        case "Range":
                            return(new Token("Range", TypeToken.Range));

                        case "Meele":
                             return (new Token("Meele",TypeToken.Meele));

                        case "Distance":
                             return (new Token("Distance",TypeToken.Distance));
                        
                        case "Siege":
                             return (new Token("Siege",TypeToken.Siege));

                        case "Selector":
                              return (new Token("Selector",TypeToken.Selector));

                        case "Source":
                              return(new Token("Source",TypeToken.Source));

                        case "Deck":
                             return(new Token("Deck", TypeToken.SourceTemp));

                       case "OtherDeck":
                              return(new Token("OtherDeck",TypeToken.SourceTemp));

                        case "Field":
                             return(new Token("Field", TypeToken.SourceTemp));

                        case "OtherField":
                              return(new Token("OtherField",TypeToken.SourceTemp));

                        case "Hand":
                             return(new Token("Hand", TypeToken.SourceTemp));

                        case "OtherHand":
                              return(new Token("OtherHand",TypeToken.SourceTemp));    

                        case "Parent":
                              return(new Token("Parent",TypeToken.SourceTemp));

                        case "Single":
                             return(new Token("Single",TypeToken.Single)); 
                        
                        case "Predicate":
                              return(new Token("Predicate",TypeToken.Predicate));
 
                        case "unit":
                              return(new Token("unit",TypeToken.unit));

                        case "unit.power":
                              return(new Token("Power",TypeToken.unitreferences));      

                        case "unit.faction":
                              return(new Token("Faction",TypeToken.unitreferences));      

                        case "unit.type":
                              return(new Token("Type",TypeToken.unitreferences));      

                        case "unit.range":
                              return(new Token("Range",TypeToken.unitreferences));      
                        
                        case "for":
                              return(new Token("for",TypeToken.For));      

                        case "while":
                              return(new Token("while",TypeToken.While));      

                        case "in":
                              return(new Token("in",TypeToken.In));      
                        
                        case "context.Hand":
                              return(new Token("context.Hand",TypeToken.ContextProp));

                        case "context.Deck":
                              return(new Token("context.Deck",TypeToken.ContextProp));

                        case "context.Graveyard":
                               return(new Token("context.Graveyard",TypeToken.ContextProp));

                        case "context.Board":
                               return(new Token("context.Board",TypeToken.ContextPropBoard));

                        case "context.TriggerPlayer":
                               return(new Token("context.TriggerPlayer",TypeToken.ContextTrigger));

                        case "context.HandOfPlayer":
                              return(new Token("context.HandOfPlayer",TypeToken.ContextPseudoMethod));

                        case "context.FieldOfPlayer":
                              return(new Token("context.FieldOfPlayer",TypeToken.ContextPseudoMethod));

                        case "context.DeckOfPlayer":
                               return(new Token("context.DeckOfPlayer",TypeToken.ContextPseudoMethod));
                            
                        case "context.GraveyardOfPlayer":
                               return(new Token("context.GraveyardOfPlayer",TypeToken.ContextPseudoMethod));    
                        
                        case "context.Hand.Add":
                              return(new Token("context.Hand.Add", TypeToken.ContextMethod)); 

                        case "context.Hand.Find":
                              return(new Token("context.Hand.Find", TypeToken.ContextMethod));  

                        case "context.Hand.Push":
                              return(new Token("context.Hand.Push", TypeToken.ContextMethod)); 

                        case "context.Hand.SendBootom":
                              return(new Token("context.Hand.SendBootom", TypeToken.ContextMethod)); 

                        case "context.Hand.Pop":
                              return(new Token("context.Hand.Pop", TypeToken.ContextMethod)); 

                        case "context.Hand.Remove":
                              return(new Token("context.Hand.Remove", TypeToken.ContextMethod)); 
                        
                        case "context.Hand.Shuffle":
                              return(new Token("context.Hand.Shuffle", TypeToken.ContextMethod));                         

                        case "context.Deck.Add":
                              return(new Token("context.Deck.Add", TypeToken.ContextMethod)); 

                        case "context.Deck.Find":
                              return(new Token("context.Deck.Find", TypeToken.ContextMethod));  

                        case "context.Deck.Push":
                              return(new Token("context.Deck.Push", TypeToken.ContextMethod)); 

                        case "context.Deck.SendBootom":
                              return(new Token("context.Deck.SendBootom", TypeToken.ContextMethod)); 

                        case "context.Deck.Pop":
                              return(new Token("context.Deck.Pop", TypeToken.ContextMethod)); 

                        case "context.Deck.Remove":
                              return(new Token("context.Deck.Remove", TypeToken.ContextMethod)); 
                        
                        case "context.Deck.Shuffle":
                              return(new Token("context.Deck.Shuffle", TypeToken.ContextMethod));                         

                        case "context.Graveyard.Add":
                              return(new Token("context.Graveyard.Add", TypeToken.ContextMethod)); 

                        case "context.Graveyard.Find":
                              return(new Token("context.Graveyard.Find", TypeToken.ContextMethod));  

                        case "context.Graveyard.Push":
                              return(new Token("context.Graveyard.Push", TypeToken.ContextMethod)); 

                        case "context.Graveyard.SendBootom":
                              return(new Token("context.Graveyard.SendBootom", TypeToken.ContextMethod)); 

                        case "context.Graveyard.Pop":
                              return(new Token("context.Graveyard.Pop", TypeToken.ContextMethod)); 

                        case "context.Graveyard.Remove":
                              return(new Token("context.Graveyard.Remove", TypeToken.ContextMethod)); 
                        
                        case "context.Graveyard.Shuffle":
                              return(new Token("context.Graveyard.Shuffle", TypeToken.ContextMethod));   

                        case "target.Power":
                              return(new Token("target.Power",TypeToken.TargetProps));

                        case "target.Owner":
                              return(new Token("target.Owner",TypeToken.TargetProps));

                        case "target.Faction":
                              return(new Token("target.Faction",TypeToken.TargetProps));

                        case "target.Type":
                              return(new Token("target.Type",TypeToken.TargetProps));

                        case "target.Name":
                              return(new Token("target.Name",TypeToken.TargetProps));

                        default:
                            return(new Token(word,TypeToken.Var));
                            
                    }
        } 
        public static bool ValidateMarks(string text , int indice)
        {
          for(int i = indice ; i<text.Length ; i++)
          {
            
             if(text[i] == '"')
             {
              return true;
             }
          }
          return false;
        }
        public static int PosMarks(string text, int indice)
        {
          int indiceComillas = 0;
          for(int i = indice ; i<text.Length ; i++)
          {
             if(text[i] == '"')
             {
              indiceComillas = i;
              break;
             }
          }
          return indiceComillas;

        }
         public static bool IsNumber(string word)
        {
             if(word.Length == 0)
            {
                return false;
            }
            foreach (char character in word)
            {
                if (!char.IsNumber(character))
                {
                    return false;
                }
            }
            return true;
        }
}
