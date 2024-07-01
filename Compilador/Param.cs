using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Param
{
   public TypeParam Type;
   public string Name;
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
