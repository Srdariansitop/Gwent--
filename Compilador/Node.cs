using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
