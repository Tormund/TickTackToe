using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void StartSingle()
    {
        PlayerPrefs.SetInt("mode", 0);
        Application.LoadLevel("game_scene");
    }

    public void StartMulti()
    {
        PlayerPrefs.SetInt("mode", 1);
        Application.LoadLevel("game_scene");
    }
}
