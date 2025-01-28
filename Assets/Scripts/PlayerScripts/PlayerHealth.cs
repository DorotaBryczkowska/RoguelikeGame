using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 50;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public float damageFlashSpeed = 5f;
    public Color damageFlashColour = new(1f, 0f, 0f, 0.1f);
    public Animator transition;

    Animator anim;
    PlayerMovement playerMovement;
    bool isDead;
    bool damaged;
    private readonly float transitionTime = 1f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = startingHealth;
        healthSlider.maxValue = startingHealth;

        if(PlayerPrefs.GetInt("maxHealth")!=0)
        {
            UpgradedHealth();
        }
    }

    private void UpgradedHealth()
    {
        startingHealth = PlayerPrefs.GetInt("maxHealth");
        currentHealth = PlayerPrefs.GetInt("maxHealth");
        healthSlider.maxValue = PlayerPrefs.GetInt("maxHealth");
        healthSlider.value = PlayerPrefs.GetInt("maxHealth");
    }

    void Update()
    {
        if (damaged) 
        {
            damageImage.color = damageFlashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, damageFlashSpeed * Time.deltaTime);
        }
        damaged = false;
        PlayerPrefs.SetInt("maxHealth", startingHealth);
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }
    void Death()
    {
        isDead = true;
        anim.SetTrigger("Die");
        playerMovement.enabled = false;
        StartCoroutine(Reset());
    }

    public IEnumerator Reset()
    {
        yield return new WaitForSeconds(2);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void IncreaseMaxHealth(int amount)
    {
        startingHealth += amount;
        currentHealth += amount;
        healthSlider.maxValue = startingHealth;
        healthSlider.value = currentHealth;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("maxHealth");
    }
}
