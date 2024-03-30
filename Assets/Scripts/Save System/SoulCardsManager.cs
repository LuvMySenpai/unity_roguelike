using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoulCardsManager : MonoBehaviour
{
    [Header("Block Card Buttons")]
    public List<Button> blockButtons = new List<Button>(6);

    [Header("Gold Earned Display")]
    public TMP_Text goldEarnedDisplay;

    [Header("Start Button")]
    [SerializeField]
    Button startButton;

    [Header("Character Selector")]
    [SerializeField]
    CharacterSelector characterSelector;

    List<XMLManager.SoulTemplate> soulCards = new List<XMLManager.SoulTemplate>();

    public void Awake()
    {
        XMLManager.Souls souls = XMLManager.instance.LoadSoulCards();

        soulCards = souls.soulCards;

        CheckForBoughtSouls();

        goldEarnedDisplay.text = XMLManager.instance.LoadGoldAmount().ToString();
    }

    public void Update()
    {
        if (characterSelector.characterData)
            startButton.gameObject.SetActive(true);
    }

    public void CheckForBoughtSouls()
    {
        for (int i = 0; i < soulCards.Count; i++)
        {
            if (soulCards[i].isBought == true)
                blockButtons[i].gameObject.SetActive(false);
        }
    }

    public void BuySoul(int id)
    {
        for (int i = 0; i < soulCards.Count;i++)
        {
            if(i == id)
            {
                if (soulCards[i].price <= XMLManager.instance.LoadGoldAmount())
                {
                    XMLManager.instance.RemoveGold(soulCards[i].price);
                    goldEarnedDisplay.text = XMLManager.instance.LoadGoldAmount().ToString();

                    XMLManager.instance.SaveSoulCards(id);
                    CheckForBoughtSouls();
                }
            }
        }
    }
}
