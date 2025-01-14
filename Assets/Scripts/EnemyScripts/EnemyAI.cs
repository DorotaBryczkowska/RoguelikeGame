using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 5f;
    public float speed = 2f;
    public float roamRadius = 3f;
    public Transform player;

    private Animator enemyAnim;
    private Vector2 roamPosition;
    private bool isDetectingPlayer = false;
    private bool isWaiting = false;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastPosition;
    EnemyHealth enemyHealth;

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealth= GetComponent<EnemyHealth>();
        lastPosition = transform.position;
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    void Update()
    {
        if (!enemyHealth.isDead)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                isDetectingPlayer = true;
            }
            else if (isDetectingPlayer && distanceToPlayer > detectionRange)
            {
                isDetectingPlayer = false;
            }

            if (isDetectingPlayer)
            {
                FollowPlayer();
            }
            else
            {
                Roam();
            }
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(speed * Time.deltaTime * direction);
        FaceMovementDirection();
        RunAnim(true);
    }

    void Roam()
    {
        if (isWaiting)
        {
            RunAnim(false);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, roamPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, roamPosition) < 0.1f)
        {
            RunAnim(false);
            StartCoroutine(WaitToSetNewRoamPosition());
        }
        else
        {
            FaceMovementDirection();
            RunAnim(true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WaitToSetNewRoamPosition());
        }
    }

    private IEnumerator WaitToSetNewRoamPosition()
    {
        isWaiting = true;
        RunAnim(false);
        yield return new WaitForSeconds(2f);
        SetNewRoamPosition();
        isWaiting = false;
    }

    private void SetNewRoamPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle * roamRadius;
        roamPosition = (Vector2)transform.position + randomDirection;
    }

    void RunAnim(bool isRunning)
    {
        enemyAnim.SetBool("Run", isRunning);
    }

    void FaceMovementDirection()
    {
        Vector2 currentPosition = transform.position;
        Vector2 movementDirection = currentPosition - lastPosition;
        if (movementDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        lastPosition = currentPosition;
    }
}
