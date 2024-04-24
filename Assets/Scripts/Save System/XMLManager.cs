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
    public Profiles profiles;

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

        if (!Directory.Exists(Application.persistentDataPath + "/GoldAmount/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/GoldAmount/");
        }

        if (!Directory.Exists(Application.persistentDataPath + "/Souls/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Souls/");

            List<SoulTemplate> tempCards = new List<SoulTemplate>()
            {
                new SoulTemplate(0, "Knight", false, 100),
                new SoulTemplate(1, "Guard", false, 150),
                new SoulTemplate(2, "Wizard", false, 200),
                new SoulTemplate(3, "Hunter", false, 250),
                new SoulTemplate(4, "Pyromancer", false, 300),
                new SoulTemplate(5, "Electromancer", false, 350),
            };

            souls.soulCards = tempCards;

            XmlSerializer serializer = new XmlSerializer(typeof(Souls));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Souls/soulCards.xml", FileMode.Create);
            serializer.Serialize(stream, souls);
            stream.Close();
        }

        if (!Directory.Exists(Application.persistentDataPath + "/Profiles/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles/");
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

    [System.Serializable]
    public class Profiles
    {
        public string currentUser;

        public List<ProfileTemplate> profilesList = new List<ProfileTemplate>(3);
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

    public class ProfileTemplate
    {
        public string Username;
        public int goldAmount;
        public List<SoulTemplate> soulCards = new List<SoulTemplate>(6);
    }

    public void AddProfile(string username)
    {
        List<SoulTemplate> tempCards = new List<SoulTemplate>()
        {
            new SoulTemplate(0, "Knight", false, 100),
            new SoulTemplate(1, "Guard", false, 150),
            new SoulTemplate(2, "Wizard", false, 200),
            new SoulTemplate(3, "Hunter", false, 250),
            new SoulTemplate(4, "Pyromancer", false, 300),
            new SoulTemplate(5, "Electromancer", false, 350),
        };

        ProfileTemplate newProfile = new ProfileTemplate()
        {
            Username = username,
            goldAmount = 0,
            soulCards = tempCards,
        };


        profiles.currentUser = username;
        profiles.profilesList.Add(newProfile);
    }

    public void RemoveGold(int gldAmount)
    {
        gold.goldAmount -= gldAmount;
        SaveGoldAmount(0);
    }

    //public void SaveProfile()
    //{

    //}

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
        if (File.Exists(Application.persistentDataPath + "/GoldAmount/goldAmount.xml"))
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
