using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSave : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;

    PlayerAttack playerAttack;
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    PlayerUpgrades playerUpgrades;

    private void Start()
    {
        playerAttack = player.GetComponent<PlayerAttack>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerUpgrades = player.GetComponent<PlayerUpgrades>();
    }
    public void SaveGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        { 
            SaveSystem.SavePlayer(playerHealth, playerMovement, playerAttack, playerUpgrades);
        }
        else
        {
            Debug.Log("You cannot save in dungeons!");
        }
    }

    public void LoadGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerData data = SaveSystem.LoadPlayer();

            playerHealth.currentHealth = data.currentHealth;
            playerHealth.startingHealth = data.startingHealth;
            playerMovement.movementSpeed = data.movementSpeed;
            playerAttack.damagePerAttack = data.damage;
            playerUpgrades.upgradesAvaliable = data.upgradesAvaliable;
            playerUpgrades.experience = data.currentExp;
            playerUpgrades.expToNextUpgrade = data.expToNextUpgrade;

            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            transform.position = position;
            position.z = -10;
            mainCamera.transform.position = position;
        }
        else
        {
            Debug.Log("You cannot load in dungeons!");
        }
    }
}
