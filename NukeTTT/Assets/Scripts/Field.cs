using System;
using UnityEngine; // for Debug.Log(...);
using System.Collections.Generic;

public class Bot
{

    public Transform buttons_panel;
    public int bot_value ; // 1 is O, 2 is X
    List<int> empty_btns_list;
    Field field;

    public Bot(Transform panel, int value, Field f)
    {
        buttons_panel = panel;
        bot_value = value;
        field = f;
        empty_btns_list = new List<int>();
    }

    public void BotStep()
    {
        empty_btns_list.Clear();

        for (int i = 0; i < buttons_panel.childCount; i++)
        {
            GameButton btn = buttons_panel.GetChild(i).GetComponent<GameButton>();
            if (btn.State == Button_State.kEmpty)
                empty_btns_list.Add(btn.pos);
        }

       // Debug.Log(empty_btns_list.Count);

        if (!TryAttack())
        {
            if (!TryDeffence())
            {
                DoAnything();
            }
        }

        
    }

    bool TryAttack()
    {
      //  Debug.Log("TryAttack");
        foreach (int i in empty_btns_list)
        {
            if (field.CheckPos(i, bot_value))
            {
                MarkCell(i);
                return true;
            }
        }
        return false;
    }

    bool TryDeffence()
    {
       // Debug.Log("TryDeffence");
        int player_value = 2;
        if (bot_value == 2)
            player_value = 1;
        foreach (int i in empty_btns_list)
        {
            if (field.CheckPos(i, player_value))
            {
                MarkCell(i);
                return true;
            }
        }
        return false;
    }

    void DoAnything()
    {
        //Debug.Log("DoAnything");
        
        int mid_cell = 4;
        if (empty_btns_list.Contains(mid_cell))
        {
            MarkCell(mid_cell);
            return;
        }

        UnityEngine.Random.seed = (int)Time.time;

        int rnd_index = UnityEngine.Random.Range(0, empty_btns_list.Count);

       // Debug.Log(empty_btns_list[rnd_index]);
        MarkCell(empty_btns_list[rnd_index]);
      
    }

    void MarkCell(int index)
    {
        buttons_panel.GetChild(index).GetComponent<GameButton>().BotMark();
    }
}

public class Field
{
    enum WinLines
    { 
        kTop_g = 0x15000,
        kMid_g = 0x00540,
        kBot_g = 0x00015, //0x00015
        kLef_v = 0x10410,
        kMid_v = 0x04104,
        kRig_v = 0x01041,
        kLef_d = 0x10101,
        kRig_d = 0x01110
    }
    private int[] lines ;
    private int field;
    private int step;

    public Bot bot;
    int Insert(int obj, int pos)
    {
        return obj << (pos * 2);
    }

    public Field()
    {
        WinLines[] enumArray = Enum.GetValues(typeof(WinLines)) as WinLines[];
        lines = Array.ConvertAll<WinLines, int> (enumArray, delegate(WinLines value) { return (int)value; });

        Reset();
    }

    public void Reset()
    {
        field = 0x00000;
        step = 0;
    }

    public bool CheckPos(int pos, int value)
    {
        bool result = false;

        int check_enemy, curr_step, tmp;

        if (value == 2)
            check_enemy = 1;
        else
            check_enemy = 2;

        curr_step = Insert(value, pos);
        check_enemy = Insert(check_enemy, pos);

        tmp = curr_step & field;
        check_enemy = check_enemy & field;

        if (tmp == 0 && check_enemy == 0)
        {
            int tmp_field = field + curr_step;
            for(int i = 0; i < lines.Length; i++)
            {
                result = CheckLine(i , value, tmp_field);
                if (result) return result;
            }
  
        }

        return result;
    }

    private bool CheckLine(int index, int value, int c_field)
    {
        bool result = false;

        int line = value == 1 ? lines[index] : lines[index] << 1;

        int res = c_field & line;
        if (res == line)
            result = true;

        return result;
    }

    private bool CheckLine(int index, int value)
    {
        return CheckLine(index, value, field);
    }

    public int CheckWinO()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (CheckLine(i, 1))
                return i + 1;
        }
        return 0;
    }

    public int CheckWinX()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if(CheckLine(i, 2))
                return i + 1;
        }
        return 0;
    }

    public bool CheckGameOver()
    {
        if (step >= 9)
            return true;
        return false;
    }

    public void Step(int pos, int value)
    {

        int check_enemy, curr_step, tmp;

        if (value == 2)
            check_enemy = 1;
        else
            check_enemy = 2; 

        curr_step = Insert(value, pos);
        check_enemy = Insert(check_enemy, pos);

        tmp = curr_step & field;
        check_enemy = check_enemy & field;
        
        if (tmp == 0 && check_enemy == 0)
        {
            field += curr_step;
            step++;
        }



        if (bot != null && value == bot.bot_value && step < 9)
            bot.BotStep();
    }
}
