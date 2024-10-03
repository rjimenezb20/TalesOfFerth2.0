using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonHover : MonoBehaviour
{
    [Header("Buttons Images")]
    public Sprite normalBt;
    public Sprite hoverBt;

    public List<Button> buttons;

    private void Start()
    {
        foreach (Button button in buttons)
        {
            AddHoverEffect(button);
        }
    }

    private void AddHoverEffect(Button button)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => { OnButtonHoverEnter(button); });
        trigger.triggers.Add(pointerEnter);

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => { OnButtonHoverExit(button); });
        trigger.triggers.Add(pointerExit);
    }

    private void OnButtonHoverEnter(Button button)
    {
        button.GetComponent<Image>().sprite = hoverBt;
    }

    private void OnButtonHoverExit(Button button)
    {
        button.GetComponent<Image>().sprite = normalBt;
    }
}