using UnityEngine;
using System.Collections;

public class PrepareField : MonoBehaviour 
{
    public Transform button_pref;
    public float x_offset;
    public float y_offset;
    public Rect mustInRect;

    private bool need_update_pos = false;
    private int btn_count = 9;

	// Use this for initialization
	void Start () 
    {

        for (int i = 0; i < btn_count; i++)
        {
            Transform btn = Instantiate(button_pref) as Transform;
            btn.parent = transform;
            btn.GetComponent<GameButton>().pos = i;
        }

        UpdateButtonsPossition();

        GetComponent<GameField>().enabled = true;
	}

    void UpdateButtonsPossition()
    {
        float x = mustInRect.x;
        float y = mustInRect.yMax;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);

            RectTransform rect = t.gameObject.GetComponent<RectTransform>();
            rect.localScale = new Vector3(1, 1, 1);
            rect.anchoredPosition = new Vector2(x, y);

            x += x_offset;
            if (x > mustInRect.xMax - x_offset / 2)
            {
                x = mustInRect.x;
                y -= y_offset;
            }
        }
        need_update_pos = false;
    }
}
