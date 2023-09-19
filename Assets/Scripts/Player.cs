using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Entity entity;

    [Header("Player Regen System")]
    public bool regenHPEnabled = true;
    public float regenHPTime = 5f;
    public int regenHPValue = 5;
    public bool regenMPEnabled = true;
    public float regenMPTime = 10f;
    public int regenMPValue = 5;

    [Header("Game Manager")]
    public GameManager manager;

    [Header("Player Shortcuts")]
    public KeyCode attributesKey = KeyCode.C;

    [Header("Player UI Panels")]
    public GameObject attributesPanel;

    [Header("Player UI")]
    public Slider health;
    public Slider mana;
    public Slider stamina;
    public Slider exp;
    public Text expText;
    public Text levelText;
    public Text strTxt;
    public Text resTxt;
    public Text intTxt;
    public Text wilTxt;
    public Button strPositiveBtn;
    public Button resPositiveBtn;
    public Button intPositiveBtn;
    public Button wilPositiveBtn;
    public Button strNegativeBtn;
    public Button resNegativeBtn;
    public Button intNegativeBtn;
    public Button wilNegativeBtn;
    public Text pointsTxt;

    [Header("Exp")]
    public int currentExp;
    public int expBase;
    public int expLeft;
    public float expMode;
    public GameObject levelUpFx;
    public AudioClip levelUpSound;
    public int givePoints = 5;

    [Header("Respawn")]
    public float respawnTime = 5;
    public GameObject prefab;


    void Start()
    {
        if (manager == null)
        {
            Debug.LogError("Você precisa anexar o game manager aqui no player");
            return;
        }

        entity.maxHealth = manager.CalculateHealth(entity);
        entity.maxMana = manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);

        entity.currentHealth = entity.maxHealth;
        entity.currentMana = entity.maxMana;
        entity.currentStamina = entity.maxStamina;

        health.maxValue = entity.maxHealth;
        health.value = health.maxValue;

        mana.maxValue = entity.maxMana;
        mana.value = mana.maxValue;

        stamina.maxValue = entity.maxStamina;
        stamina.value = stamina.maxValue;

        exp.value = currentExp;
        exp.maxValue = expLeft;

        expText.text = String.Format("Exp: {0}/{1}", currentExp, expLeft);
        levelText.text = entity.level.ToString();

        // iniciar o regenhealth
        StartCoroutine(RegenHealth());
        StartCoroutine(RegenMana());

        UpdatePoints();
        SetupUIButtons();
    }

    private void Update()
    {
        if (entity.dead)
            return;

        if (entity.currentHealth <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(attributesKey))
        {
            attributesPanel.SetActive(!attributesPanel.activeSelf);
        }
           
        health.value = entity.currentHealth;
        mana.value = entity.currentMana;
        stamina.value = entity.currentStamina;

        exp.value = currentExp;
        exp.maxValue = expLeft;
        expText.text = String.Format("Exp: {0}/{1}", currentExp, expLeft);
        levelText.text = entity.level.ToString();
    }

    IEnumerator RegenHealth()
    {
        while (true) // loop infinito
        {
            if (regenHPEnabled)
            {
                if (entity.currentHealth < entity.maxHealth)
                {
                    Debug.LogFormat("Recuperando HP do jogador");
                    entity.currentHealth += regenHPValue;
                    yield return new WaitForSeconds(regenHPTime);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator RegenMana()
    {
        while (true) // loop infinito
        {
            if (regenHPEnabled)
            {
                if (entity.currentMana < entity.maxMana)
                {
                    Debug.LogFormat("Recuperando MP do jogador");
                    entity.currentMana += regenMPValue;
                    yield return new WaitForSeconds(regenMPTime);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    void Die()
    {
        entity.currentHealth = 0;
        entity.dead = true;
        entity.target = null;

        StopAllCoroutines();
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        GetComponent<PlayerController>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        GameObject newPlayer = Instantiate(prefab, transform.position, transform.rotation, null);
        newPlayer.name = prefab.name;
        newPlayer.GetComponent<Player>().entity.dead = false;
        newPlayer.GetComponent<Player>().entity.combatCoroutine = false;
        newPlayer.GetComponent<PlayerController>().enabled = true;

        Destroy(this.gameObject);
    }
    
    public void GainExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= expLeft)
        {
            levelUp();
        }
    }
    public void levelUp()
    {
        currentExp = -expLeft;
        entity.level++;
        entity.points += givePoints;
        UpdatePoints();

        entity.currentHealth = entity.maxHealth;

        float newExp = Mathf.Pow((float) expMode, entity.level);
        expLeft = (int)Mathf.Floor((float)expBase * newExp);

        entity.entityAudio.PlayOneShot(levelUpSound);
        Instantiate(levelUpFx, this.gameObject.transform);
    }
    public void UpdatePoints()
    {
        strTxt.text = entity.strength.ToString();
        resTxt.text = entity.resistence.ToString();
        intTxt.text = entity.intelligence.ToString();
        wilTxt.text = entity.willpower.ToString();
        pointsTxt.text = entity.points.ToString();
    }

    public void SetupUIButtons()
    {
        strPositiveBtn.onClick.AddListener(() => AddPoints(1));
        resPositiveBtn.onClick.AddListener(() => AddPoints(2));
        intPositiveBtn.onClick.AddListener(() => AddPoints(3));
        wilPositiveBtn.onClick.AddListener(() => AddPoints(4));

        strNegativeBtn.onClick.AddListener(() => RemovePoints(1));
        resNegativeBtn.onClick.AddListener(() => RemovePoints(2));
        intNegativeBtn.onClick.AddListener(() => RemovePoints(3));
        wilNegativeBtn.onClick.AddListener(() => RemovePoints(4));
    }

    public void AddPoints(int index)
    {
        if (entity.points > 0)
        {
            if (index == 1) // str
                entity.strength++;
            else if (index == 2)
                entity.resistence++;
            else if (index == 3)
                entity.intelligence++;
            else if (index == 4)
                entity.willpower++;

            entity.points--;
            UpdatePoints();
        }
    }
    public void RemovePoints(int index)
    {
        if (entity.points > 0)
        {
            if (index == 1 && entity.strength > 0)
                entity.strength--;
            else if (index == 2 && entity.resistence > 0)
                entity.resistence--;
            else if (index == 3 && entity.intelligence > 0)
                entity.intelligence--;
            else if (index == 4 && entity.willpower > 0)
                entity.willpower--;

            entity.points++;
            UpdatePoints();
        }
    }

}