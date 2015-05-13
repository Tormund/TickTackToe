using UnityEngine;
using System.Collections;


public class AllertLabel : MonoBehaviour 
{

    public delegate void OnEnd();
    public OnEnd OnAnimationEnd;

    public float max_alfa;
    public float speed;
    public float alfa;

    bool needHide;
    bool needShow;

    UnityEngine.UI.Text allert_label;
	
    // Use this for initialization
	void Start () 
    {
        allert_label = GetComponent<UnityEngine.UI.Text>();

        setOpacity(0);
	}

    void ShowText()
    {
        alfa = 0;
        needShow = true;
    }

    public void ShowGameOver()
    {
        allert_label.text = Constants.GAME_OVER_TEXT;
        ShowText();
    }

    public void ShowPlayerWin(string name)
    {
        allert_label.text = name + Constants.PLAYER_WIN_TEXT;
        ShowText();
    }

    void setOpacity(float o)
    {
        Color c = allert_label.color;
        c.a = o / 255;
        allert_label.color = c;
    }

	// Update is called once per frame
	void Update () 
    {
        if (needShow)
        {
            if (alfa >= max_alfa)
            {
                needShow = false;
                needHide = true;
                return;
            }
            alfa += Time.deltaTime * speed;
            setOpacity(alfa);
            
        }
        else if (needHide)
        {
            if (alfa <= 0)
            {
                if (OnAnimationEnd != null)
                    OnAnimationEnd();
            }
            alfa -= Time.deltaTime * speed;
            setOpacity(alfa);
        }
	}
}
