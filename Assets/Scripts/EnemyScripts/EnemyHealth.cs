using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100; 
    public int currentHealth; 
    public bool isDead;
    public int experienceReward = 20;

    Animator enemyAnim;

    void Awake()
    {
        enemyAnim = GetComponent<Animator>();
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return; 
        currentHealth -= amount;
        enemyAnim.SetTrigger("Hit");
        if (currentHealth <= 0)
        {
            Death();
        }
    }
    void Death()
    {
        isDead = true;
        enemyAnim.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;

        PlayerUpgrades playerUpgrades = FindObjectOfType<PlayerUpgrades>();
        if (playerUpgrades != null)
        {
            playerUpgrades.AddExperience(experienceReward);
        }
        Destroy(gameObject, 3f);
    }
}
