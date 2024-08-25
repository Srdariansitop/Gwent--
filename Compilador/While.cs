using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class While 
{
    public string Left;
    public string Rigth;
    public TypeToken Signe;
    public bool Increase;

    public While(string left , string rigth , TypeToken signe , bool increase)
    {
        Left = left;
        Rigth = rigth;
        Signe = signe;
        Increase = increase;
    }
    public static While WhileObject(List<Token> tokens , int posinit)
    {
        string var1 = (string)tokens[posinit].Value;
        //int left = int.Parse(var1);
        TypeToken increases = tokens[posinit + 1].Type;
        bool increase = false;
        if(increases == TypeToken.SumSum)
        {
            increase = true;
        }
        TypeToken signe = tokens[posinit + 2].Type;
        string var2 = (string)tokens[posinit + 3].Value;
        //int rigth = int.Parse(var2);
        While whilereturn = new While(var1, var2, signe, increase);
        return whilereturn;
    }
}
