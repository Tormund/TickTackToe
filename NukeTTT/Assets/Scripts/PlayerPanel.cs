using UnityEngine;
using System.Collections;

public class PlayerPanel : MonoBehaviour 
{
    public UnityEngine.UI.Text score_label;
    public UnityEngine.UI.Text name_label;
    private int score;
    public string playerName;

    public void IncScore()
    {
        score++;
        score_label.text = "" + score;
    }

    public void SetName(string name)
    {
        playerName = name;
        name_label.text = name;
    }

}
