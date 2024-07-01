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
                if(char.IsLetter(text[i]))
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

                        case "amount":
                            return(new Token("amount",TypeToken.amount));

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
