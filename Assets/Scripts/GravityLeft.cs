using TMPro;
using UnityEngine;

public class GravityLeft : MonoBehaviour
{
    private TMP_Text text;

    private int number;
    public int Number
    {
        private get { return number; }
        set
        {
            number = value;
            SetText(value);
        }
    }


    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void SetText(int number)
    {
        text.text = number.ToString();
    }
}