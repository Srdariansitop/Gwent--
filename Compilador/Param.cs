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
}
public enum TypeParam
{
 Number,
 Bool,
 String,
}
