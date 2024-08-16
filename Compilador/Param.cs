using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
[System.Serializable]
public class Param
{
   public TypeParam Type;
   public string Name;
   public string ValueString;
   public Param(TypeParam type , string name)
   {
     Type = type;
     Name = name;
   }

  public static void InstanceParamofCard(List<Param> @params)
  {
   foreach(var param in @params)
   {
    EvaluateExpressionAction.keyValuePairs.Add(param.Name,param.ValueString);
   }
  }

}
public enum TypeParam
{
 Number,
 Bool,
 String,
}
