using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token 
{
      public object Value { get; }
      public TypeToken Type { get; }
        public Token(object value , TypeToken type)
        {
            Value = value;
            Type = type;
        }

}
 public enum TypeToken
    {
      
      Effect,
      Card,
      Bool,
      Number,
      Name,
      String,
      Var,
      //Effects
      Params,
      Action,
      amount,
      //Cards
      Type,
      Faction,
      Power,
      Range,
      OnActivation,
      //Operator
      Equal,
      EqualEqual,
      EqualSum,
      EqualRest,
      SumSum,
      Sum,
      RestRest,
      Rest,
      Division,
      Multiplication,
      ParenthesisLeft,
      ParenthesisRigth,
      SquareBracketLeft,
      SquareBracketRigth,
      Concat,
      ConcatSpace,
      Do,
      KeyLeft,
      KeyRigth,
      Or,
      And,
      Coma,
      //Dentro de las Cartas

      //Selector
      Selector,
      //PostAction
      PostAction,

        //Type 
      Gold,
      Silver,
      Clime,
      Leader,
      Increase,

      //Faction
      Red,
      Legend,
      
      //Range
      Siege,
      Meele,
      Distance,
      
      NumberWord,
      BoolWord,
      StringWord,
      
    }
