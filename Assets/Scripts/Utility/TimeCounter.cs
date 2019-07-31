using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimeCounter : MonoBehaviour {
    private int timer;
    private int combo;
    private float averCombo=0;
    public Text comboText;

    List<int> comboList;

    private int timeLimit=4;

	void Awake () 
    {
        combo =timer = 0;
        comboText.text = "0";
        comboList = new List<int>();
	}
    IEnumerator CountTime()
    {
        yield return new WaitForSeconds(1f);
        timer += 1;
        yield return CountTime();
    }
    public void ResetTimerAndCombo()
    {
        averCombo=combo = timer = 0;
    }
    public void AddCombo()
    {
        if (timer <= timeLimit) 
        {
            timer = 0;
            combo++;
        }
    }
    public void UpdateCombo()
    {
        if(timer>timeLimit)
        {
            if (combo != 0) 
            {
                comboList.Add(combo); 
            }
            ResetTimerAndCombo();
        }
        comboText.text = combo.ToString();
        if (combo <= 5) { comboText.color = Color.white; Global.instance.GridAnimation.HighLightText(comboText, 70, 0.01f); }
        else { comboText.color = Color.red; Global.instance.GridAnimation.HighLightText(comboText, 70, 0.01f); }
    }
    public void StartCount()
    {
        StartCoroutine("CountTime");
    }
    public void StopCount()
    { 
        StopAllCoroutines();
    }
    public float CalculateAverCombo()
    {
        averCombo = 0;
        foreach (int i in comboList)
            averCombo += i;
        if (comboList.Count != 0) averCombo = (float)averCombo / (float)comboList.Count;
        else averCombo = 0;
        return averCombo;
    }
    public void InitTimeCounter()
    {
        StopAllCoroutines();
        ResetTimerAndCombo();
        comboList.Clear();
    }
    public int GetComboMultiple()
    {
        return combo;
    }
    public string GetComboCommit()
    {
        if (averCombo == 0) return "Loser";
        else if (averCombo <= 2) return "C";
        else if (averCombo <= 4) return "B";
        else if (averCombo <= 6) return "A";
        else if (averCombo <= 7) return "AAA";
        else if (averCombo <= 8) return "S";
        else if (averCombo <= 10) return "SS";
        else return "SSS";
    }
}
