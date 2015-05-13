using UnityEngine;
using System.Collections;



public class GameField : MonoBehaviour
{
    public AllertLabel allert;
    public PlayerPanel player1;
    public PlayerPanel player2;

    public Button_State current_state = Button_State.kCross; // крестик первый ходит
    private Field field;

	// Use this for initialization
	void OnEnable () 
    {

        allert.gameObject.SetActive(false);
        allert.OnAnimationEnd += OnAllertEnd;

        int mode = PlayerPrefs.GetInt("mode");

        player1.SetName( Constants.PLAYER_1_NAME );
        player2.SetName( mode > 0 ? Constants.PLAYER_2_NAME : Constants.BOT_NAME );
        field = new Field();

        if (mode == 0) // SINGLEPLAYER
        {
            field.bot = new Bot(transform, 2, field);

        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            UnityEngine.UI.Button btn = child.GetComponent<UnityEngine.UI.Button>();
            btn.onClick.AddListener(
                delegate
                {
                    ButtonPressed(btn, current_state);
                }
            );

            if (mode == 0)
            {
                child.GetComponent<GameButton>().BotHandler += ButtonPressed;
            }
        }
	}

    void OnAllertEnd()
    {
        allert.gameObject.SetActive(false);
        ResetGame();
    }

    void ButtonPressed(UnityEngine.UI.Button button, Button_State state)
    {
        GameButton game_btn = button.GetComponent<GameButton>();

        if (game_btn.State == Button_State.kEmpty)
        {
            game_btn.State = current_state;

            switch (current_state)
            {
                case Button_State.kCross:
                    current_state = Button_State.kZero;
                    break;
                case Button_State.kZero:
                    current_state = Button_State.kCross;
                    break;
            }
        }

        field.Step(game_btn.pos, (int)game_btn.State);

        if (field.CheckWinO() != 0 )
        {
            allert.gameObject.SetActive(true);
            allert.ShowPlayerWin(player2.playerName);
            player2.IncScore();
        }
        else if (field.CheckWinX() != 0)
        {
            allert.gameObject.SetActive(true);
            allert.ShowPlayerWin(player1.playerName);
            player1.IncScore();
        }
        else if (field.CheckGameOver())
        {
            allert.gameObject.SetActive(true);
            allert.ShowGameOver();
        }
    }

    public void ResetGame()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetComponent<GameButton>().State = Button_State.kEmpty;
        }

        field.Reset();
        current_state = Button_State.kCross;
    }

    public void ToMenu()
    {
        Application.LoadLevel("menu_scene");
    }
}
