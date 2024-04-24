using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class HelpScreenController : MonoBehaviour
{
    [Header("Buttons")]
    public Button HowToPlayButton;
    public Button HowToGoldButton;
    public Button HowToCharactersButton;
    public Button HowToProfilesButton;
    public Button HowToRecordsButton;

    [Header("Scroll Holder")]
    public ScrollRect scrollRect;

    [Header("HowTo Info")]
    public GameObject HowToPlayInfo;
    public GameObject HowToGoldInfo;
    public GameObject HowToCharactersInfo;

    public void HideAllInfo()
    {
        scrollRect.content = null;

        HowToPlayInfo.SetActive(false);
        HowToGoldInfo.SetActive(false);
        HowToCharactersInfo.SetActive(false);
    }

    public void HowToPlayButtonClick()
    {
        HideAllInfo();
        HowToPlayInfo.SetActive(true);
        scrollRect.content = HowToPlayInfo.GetComponent<RectTransform>();
    }

    public void HowToGoldButtonClick()
    {
        HideAllInfo();
        HowToGoldInfo.SetActive(true);
        scrollRect.content = HowToGoldInfo.GetComponent<RectTransform>();
    }

    public void HowToCharactersButtonClick()
    {
        HideAllInfo();
        HowToCharactersInfo.SetActive(true);
        scrollRect.content = HowToCharactersInfo.GetComponent<RectTransform>();
    }
}
