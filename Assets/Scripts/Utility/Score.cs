using UnityEngine;
using System.Collections;

public class Score : System.IComparable<Score>
{
    public string name;
    public int score;
    public Score(string n, int s) 
    { 
        name = n; score = s; 
    }
    public int CompareTo(Score other)
    {
        if (other == null)
            return 0;
        int value = other.score - this.score;
        return value;
    }
    public override string ToString()
    {
        return name + " : " + score.ToString();
    }
}
