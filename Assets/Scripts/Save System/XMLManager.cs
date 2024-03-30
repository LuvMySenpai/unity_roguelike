using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;
    public Gold gold;
    public Souls souls;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"EXTRA {this} DELETED");
            Destroy(gameObject);
        }

        if(!Directory.Exists(Application.persistentDataPath + "/GoldAmount/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/GoldAmount/");
        }

        if(!Directory.Exists(Application.persistentDataPath + "/Souls/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Souls/");

            List<SoulTemplate> tempCards = new List<SoulTemplate>()
            {
                new SoulTemplate(0, "Второй", false, 2),
                new SoulTemplate(1, "Третий", false, 3),
                new SoulTemplate(2, "Четвёртый", false, 4),
                new SoulTemplate(3, "Пятый", false, 5),
                new SoulTemplate(4, "Шестой", false, 6),
                new SoulTemplate(5, "Седьмой", false, 7),
            };

            souls.soulCards = tempCards;

            XmlSerializer serializer = new XmlSerializer(typeof(Souls));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Souls/soulCards.xml", FileMode.Create);
            serializer.Serialize(stream, souls);
            stream.Close();
        }
    }

    [System.Serializable]
    public class Gold
    {
        public int goldAmount;
    }

    [System.Serializable]
    public class Souls
    {
        public List<SoulTemplate> soulCards = new List<SoulTemplate>(6);

    }

    public class SoulTemplate
    {
        public int Id;
        public string Name;
        public bool isBought;
        public int price;

        public SoulTemplate()
        {
        }

        public SoulTemplate(int id, string name, bool isBought, int price)
        {
            Id = id;
            Name = name;
            this.isBought = isBought;
            this.price = price;
        }
    }

    public void RemoveGold(int gldAmount)
    {
        gold.goldAmount -= gldAmount;
        SaveGoldAmount(0);
    }

    public void SaveGoldAmount(int gldAmount)
    {
        gold.goldAmount += gldAmount;
        XmlSerializer serializer = new XmlSerializer(typeof(Gold));
        FileStream stream = new FileStream(Application.persistentDataPath + "/GoldAmount/goldAmount.xml", FileMode.Create);
        serializer.Serialize(stream, gold);
        stream.Close();
    }

    public int LoadGoldAmount()
    {
        if(File.Exists(Application.persistentDataPath + "/GoldAmount/goldAmount.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Gold));
            FileStream stream = new FileStream(Application.persistentDataPath + "/GoldAmount/goldAmount.xml", FileMode.Open);
            gold = serializer.Deserialize(stream) as Gold;
            stream.Close();
        }

        return gold.goldAmount;
    }

    public void SaveSoulCards(int id)
    {
        for (int i = 0; i < souls.soulCards.Count; i++)
        {
            if (i == id)
                souls.soulCards[i].isBought = true;
        }
        XmlSerializer serializer = new XmlSerializer(typeof(Souls));
        FileStream stream = new FileStream(Application.persistentDataPath + "/Souls/soulCards.xml", FileMode.Create);
        serializer.Serialize(stream, souls);
        stream.Close();
    }

    public Souls LoadSoulCards()
    {
        if (File.Exists(Application.persistentDataPath + "/Souls/soulCards.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Souls));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Souls/soulCards.xml", FileMode.Open);
            souls = serializer.Deserialize(stream) as Souls;
            stream.Close();
        }

        return souls;
    }
}
