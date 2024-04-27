using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Day : MonoBehaviour
{
    private int dayNum;

    private bool disabled;

    public DateTime DateTime;
    private Button button;
    private Action<Day> onDayClicked;

    [SerializeField] private Color clickedColor;
    [SerializeField] private Color enabledColor;
    [SerializeField] private Color disabledColor;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void Init(int dayNum, DateTime dateTime, Action<Day> action)
    {
        this.dayNum = dayNum;
        DateTime = dateTime;

        onDayClicked = action;
    }

    public void DisableDay()
    {
        disabled = true;

        gameObject.GetComponent<Button>().interactable = false;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
        mainImage.color = disabledColor;
    }
    public void EnableDay(bool today)
    {
        disabled = false;

        gameObject.GetComponent<Button>().interactable = true;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = dayNum.ToString();
        if (today)
        {
            mainImage.color = Color.white;
            todayDay.SetActive(true);
        }
        else
        {
            todayDay.SetActive(false);
            mainImage.color = enabledColor;
        }
    }

    public void SetDayChoosedState()
    {
        if (!disabled)
        {

            mainImage.color = clickedColor;
            todayDay.SetActive(false);

            gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
    }
    public void SetDayUnchoosedState()
    {
        if (!disabled)
        {

            mainImage.color = enabledColor;
            todayDay.SetActive(false);

            gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        }
    }
    private void OnClick()
    {
        onDayClicked.Invoke(this);
        SetDayChoosedState();
    }

    public Image mainImage;
    [SerializeField] private GameObject todayDay;
}
