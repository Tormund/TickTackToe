using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Button_State
{ 
    kEmpty,
    kZero,
    kCross
}

public class GameButton : MonoBehaviour
{
    public int pos;

    public Button_State State
    {
        set
        {
            state = value;
            UpdateBtnText();
        }
        get
        {
            return state;
        }
    }

    private Button_State state;

    public delegate void OnBotMark(Button btn, Button_State st);
    public OnBotMark BotHandler;

    public void BotMark()
    {
        Debug.Log("BotMark");
        if (BotHandler != null)
            BotHandler(GetComponent<Button>(), state);
    }

	// Use this for initialization
	void Start () 
    {
        State = Button_State.kEmpty;
	}

    void UpdateBtnText()
    {
        Text t = transform.GetComponentInChildren<Text>();
        string str = Constants.EMPTY_TEXT;

        switch (state)
        { 
            case Button_State.kCross:
                str = Constants.CROSS_TEXT;
                break;

            case Button_State.kZero:
                str = Constants.ZERO_TEXT;
                break;
        }

        t.text = str;
        
    }
}
