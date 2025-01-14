using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public int upgradesAvaliable = 1;
    public int experience = 0;
    public int expToNextUpgrade = 100;

    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        if (experience >= expToNextUpgrade)
        {
            upgradesAvaliable++;
            experience -= expToNextUpgrade;
            expToNextUpgrade = Mathf.RoundToInt(expToNextUpgrade * 1.5f); 
        }
    }

    public void UpgradePlayer(int upgradeType)
    {
        switch (upgradeType)
        {
            case 0: // Szybszy ruch
                playerMovement.IncreaseSpeed(0.5f);
                Debug.Log("Movement speed upgraded!");
                break;
            case 1: // Większy atak
                playerAttack.IncreaseDamage(5);
                Debug.Log("Attack damage upgraded!");
                break;
            case 2: // Więcej życia
                playerHealth.IncreaseMaxHealth(10);
                Debug.Log("Max health upgraded!");
                break;
        }
    }
}
