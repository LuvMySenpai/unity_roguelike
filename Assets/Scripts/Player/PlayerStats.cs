using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    CharacterData characterData;
    public CharacterData.Stats baseStats;
    [SerializeField]
    CharacterData.Stats actualStats;

    float health;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return health; }
        set
        {
            //Check if the value has changed 
            if (health != value)
            {                
                health = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format(
                        "Health: {0} / {1}",
                        health, actualStats.maxHealth
                    );
                }
            }
        }
    }

    public float MaxHealth
    {
        get { return actualStats.maxHealth; }

        // If we try and set the max health, the UI interface
        // on the pause screen will also be updated.
        set
        {
            //Check if the value has changed
            if (actualStats.maxHealth != value)
            {
                actualStats.maxHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format(
                        "Health: {0} / {1}",
                        health, actualStats.maxHealth
                    );
                }
                //Update the real time value of the stat
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentRecovery
    {
        get { return Recovery; }
        set { Recovery = value; }
    }

    public float Recovery
    {
        get { return actualStats.recovery; }
        set
        {
            //Check if the value has changed
            if (actualStats.recovery != value)
            {
                actualStats.recovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + actualStats.recovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return MoveSpeed; }
        set { MoveSpeed = value; }
    }

    public float MoveSpeed
    {
        get { return actualStats.moveSpeed; }
        set
        {
            //Check if the value has changed
            if (actualStats.moveSpeed != value)
            {
                actualStats.moveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + actualStats.moveSpeed;
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return Might; }
        set { Might = value; }
    }

    public float Might
    {
        get { return actualStats.might; }
        set
        {
            //Check if the value has changed
            if (actualStats.might != value)
            {
                actualStats.might = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + actualStats.might;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return Speed; }
        set { Speed = value; }
    }

    public float Speed
    {
        get { return actualStats.speed; }
        set
        {
            //Check if the value has changed
            if (actualStats.speed != value)
            {
                actualStats.speed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + actualStats.speed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get { return Magnet; }
        set { Magnet = value; }
    }

    public float Magnet
    {
        get { return actualStats.magnet; }
        set
        {
            //Check if the value has changed
            if (actualStats.magnet != value)
            {
                actualStats.magnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + actualStats.magnet;
                }
            }
        }
    }
    #endregion

    public ParticleSystem damageEffect;
    public AudioClip damageSound;

    //Level-up and experience
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //Class to work with level range
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    //I-frames
    [Header("I-frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;


    public List<LevelRange> levelRanges;

    PlayerInventory inventory;
    PlayerCollector collector;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TMP_Text levelText;

    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;

    AudioSource audioSource;

    void Awake()
    {
        characterData = CharacterSelector.GetData();

        if(CharacterSelector.instance)
            CharacterSelector.instance.DestroySingleton();

        health = characterData.stats.maxHealth;

        inventory = GetComponent<PlayerInventory>();
        collector = GetComponentInChildren<PlayerCollector>();
        audioSource = GetComponent<AudioSource>();

        //Assign the values
        baseStats = actualStats = characterData.stats;
        collector.SetRadius(actualStats.magnet);
    }

    void Start()
    {
        //Spawn the starting weapon
        inventory.Add(characterData.StartingWeapon);

        experienceCap = levelRanges[0].experienceCapIncrease;

        //Set the current stats display
        GameManager.instance.currentHealthDisplay.text = "Здоровье: " + Convert.ToInt32(CurrentHealth) + " / " + MaxHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Регенерация: " + CurrentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Скорость перемещения: " + CurrentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Сила: " + CurrentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Скорость снарядов: " + CurrentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Магнит: " + CurrentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }

    void Update()
    {
        //Invincibility time checker
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }    
        else if(isInvincible)
        {
            isInvincible = false;
        }

        if(GameManager.instance.pauseScreen)
        {
            GameManager.instance.currentHealthDisplay.text = "Здоровье: " + Convert.ToInt32(CurrentHealth) + " / " + MaxHealth;
            GameManager.instance.currentRecoveryDisplay.text = "Регенерация: " + CurrentRecovery;
            GameManager.instance.currentMoveSpeedDisplay.text = "Скорость перемещения: " + CurrentMoveSpeed;
            GameManager.instance.currentMightDisplay.text = "Сила: " + CurrentMight;
            GameManager.instance.currentProjectileSpeedDisplay.text = "Скорость снарядов: " + CurrentProjectileSpeed;
            GameManager.instance.currentMagnetDisplay.text = "Магнит: " + CurrentMagnet;
        }

        Recover();
    }

    public void RecalculateStats()
    {
        actualStats = baseStats;
        foreach (PlayerInventory.Slot s in inventory.passiveSlots)
        {
            Passive p = s.item as Passive;
            if (p)
            {
                actualStats += p.GetBoosts();
            }
        }

        // Update the PlayerCollector's radius.
        collector.SetRadius(actualStats.magnet);
    }

    public void IncreaseExperience(int amount) 
    {
        experience += amount;
        LevelUpChecker();

        UpdateExpBar();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;

            foreach(LevelRange range in levelRanges) 
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
        }
    }

    void UpdateExpBar()
    {
        //Update the experience bar
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        //Update level text
        levelText.text = "УР. " + level.ToString();
    }

    public void TakeDamage(float dmg)
    {
        //Check for I-frames
        if(!isInvincible)
        {
            CurrentHealth -= dmg;

            //If damage effect is assigned - play it
            if (damageEffect) Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity), 5f);

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }

            audioSource.PlayOneShot(damageSound);

            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        //Update the health bar
        healthBar.fillAmount = CurrentHealth / actualStats.maxHealth;
    }

    public void Kill()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponSlots, inventory.passiveSlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        if(CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += amount;

            //prevent surpassing Max health
            if(CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }

            UpdateHealthBar();
        }
    }

    void Recover()
    {
        if(CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            //prevent surpassing Max health
            if (CurrentHealth > actualStats.maxHealth) 
            {
                CurrentHealth = actualStats.maxHealth;
            }

            UpdateHealthBar();
        }
    }

    [System.Obsolete("Old function that is kept to maintain compatibility with the InventoryManager. Will remove someday (maybe not)")]
    public void SpawnWeapon(GameObject weapon)
    {
        //Check if inventory full
        if(weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory already full");
            return;
        }
        //Spawn the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); //Weapon becomes the child element of a Player
        //inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>()); //Add a weapon to it's inventory slot

        weaponIndex++;
    }

    [System.Obsolete("No need to spawn passive items directly now. No idea why I'm keeping it btw")]
    public void SpawnPassiveItem(GameObject passiveItem)
    {
        //Check if inventory full
        if (passiveItemIndex >= inventory.passiveSlots.Count - 1)
        {
            Debug.LogError("Inventory already full");
            return;
        }
        //Spawn the starting item
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); //Item becomes the child element of a Player
        //inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); //Add an item to it's inventory slot

        passiveItemIndex++;
    }
}
