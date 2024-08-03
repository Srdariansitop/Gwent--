using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Node 
{
     public List<Node> Children;
     public object Value;
     public Node(List<Node> children,object value)
        {
            Children = children;
            Value = value;
        }
}
