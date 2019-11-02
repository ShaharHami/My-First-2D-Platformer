using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text speedText;
    [SerializeField]
    private int healthMultiplyer = 10;
    [SerializeField]
    private int movementSpeedMultiplyer = 1;

    [SerializeField]
    private int upgradeCost = 50;
    private PlayerStats stats;
    string pressedButtonSound = "ButtonPress";
    string hoverOverSound = "ButtonHover";
    void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }
    void UpdateValues()
    {
        healthText.text = "Health: " + stats.maxHealth.ToString();
        speedText.text = "Speed: " + stats.movementSpeed.ToString();
    }
    public void UpgradeHealth()
    {
        if (GameMaster.Money < upgradeCost)
        {
            AudioManager.instance.PlaySound("No Money");
            return;
        }
        stats.maxHealth = stats.maxHealth + healthMultiplyer;
        GameMaster.Money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues();
    }
    public void UpgradeSpeed()
    {
        if (GameMaster.Money < upgradeCost)
        {
            AudioManager.instance.PlaySound("No Money");
            return;
        }
        stats.movementSpeed = stats.movementSpeed + movementSpeedMultiplyer;
        GameMaster.Money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues();
    }
    public void QuitGame()
    {
        AudioManager.instance.PlaySound(pressedButtonSound);
        Application.Quit();
    }
}
