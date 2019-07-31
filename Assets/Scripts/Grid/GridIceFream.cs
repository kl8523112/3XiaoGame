using UnityEngine;
using System.Collections;

public class GridIceFream : MonoBehaviour {
    public int hp=2;
	void Start ()
    {
	
	}

    public void MinusHp()
    {
        hp--;
    }
    public bool isDead()
    {
        return hp <= 0 ? true : false;
    }
}
