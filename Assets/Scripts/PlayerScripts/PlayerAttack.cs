using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damagePerAttack = 15;
    public float timeBetweenAttacks = 1f;
    public float attackRange = 0.7f;
    public Transform attackPoint;
    public LayerMask attackableLayer;

    Animator anim;
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    Rigidbody2D playerRigidbody;
    float timer;

    void Awake()
    {
        attackableLayer = LayerMask.GetMask("Attackable");
        anim = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement= GetComponent<PlayerMovement>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timer >= timeBetweenAttacks && playerHealth.currentHealth > 0)
        {
            Attack();
        }
    }

    void Attack()
    {
        timer = 0f;
        playerMovement.enabled = false;
        anim.SetTrigger("Attack");
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero;
        }

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackableLayer);

        foreach(Collider2D objectHit in hitObjects)
        {
            objectHit.GetComponent<EnemyHealth>().TakeDamage(damagePerAttack);
        }

        if(playerHealth.currentHealth > 0)
        {
            StartCoroutine(EnableMovementAfterAttack());
        }
    }

    IEnumerator EnableMovementAfterAttack()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length+0.5f);
        playerMovement.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void IncreaseDamage(int amount)
    {
        damagePerAttack += amount;
    }
}
