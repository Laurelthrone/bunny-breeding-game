using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chance
{
    public static bool Percent(float p)
    {
        return (Random.value <= p / 100);
    }

    static bool Fraction(string p)
    {
        return false;
    }
}
