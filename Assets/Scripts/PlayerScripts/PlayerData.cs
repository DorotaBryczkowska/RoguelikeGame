[System.Serializable]
public class PlayerData
{
    public int currentHealth;
    public int startingHealth;
    public int damage;
    public int upgradesAvaliable;
    public int currentExp;
    public int expToNextUpgrade;
    public float movementSpeed;
    public float[] position;

    public PlayerData(PlayerHealth playerHealth, PlayerMovement playerMovement, PlayerAttack playerAttack, PlayerUpgrades playerUpgrades) 
    {
        currentHealth = playerHealth.currentHealth;
        startingHealth = playerHealth.startingHealth;
        damage = playerAttack.damagePerAttack;
        upgradesAvaliable = playerUpgrades.upgradesAvaliable;
        currentExp = playerUpgrades.experience;
        expToNextUpgrade= playerUpgrades.expToNextUpgrade;
        movementSpeed = playerMovement.movementSpeed;

        position = new float[3];
        position[0] = playerMovement.transform.position.x;
        position[1] = playerMovement.transform.position.y;
        position[2] = playerMovement.transform.position.z;
    }
}
