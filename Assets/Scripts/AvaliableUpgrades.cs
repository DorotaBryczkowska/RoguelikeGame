using UnityEngine;

public class AvaliableUpgrades : MonoBehaviour
{
    public int upgradeNumber;
    public GameObject descriptionText;

    private PlayerUpgrades playerUpgrades;
    private bool isPlayerInRange = false;

    private void Start()
    {
        if (descriptionText != null)
        {
            descriptionText.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (descriptionText != null)
            {
                descriptionText.SetActive(true);
            }

            isPlayerInRange = true;
            if (!other.TryGetComponent<PlayerUpgrades>(out playerUpgrades))
            {
                Debug.LogError("PlayerUpgrades component not found on Player!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (descriptionText != null)
            {
                descriptionText.SetActive(false);
            }
            playerUpgrades = null;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Y))
        {
            TryToUpgradePlayer();
        }
    }

    void TryToUpgradePlayer()
    {
        if (playerUpgrades != null && playerUpgrades.upgradesAvaliable > 0)
        {
            playerUpgrades.UpgradePlayer(upgradeNumber);
            playerUpgrades.upgradesAvaliable--;
        }
    }
}
