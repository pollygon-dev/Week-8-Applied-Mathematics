using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Vector3 playerPosition;
    public Animator animator;

    [Header("Movement")]
    public Transform[] lanes;
    private int currentLane = 0;
    [SerializeField] private float travelTime = 0.3f;
    private float lerpTimer;
    private Vector3 startPos;
    private bool isMoving;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float jumpDuration = 0.5f;
    public bool isJumping;
    private float jumpTimer;
    private float groundY;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private float healthRegenRate = 1f; 
    private float regenTimer;
    [SerializeField] private TextMeshProUGUI healthText;



    private void Start()
    {
        if (lanes.Length > 0)
        {
            currentLane = 1;
            playerPosition = lanes[currentLane].position;
            groundY = playerPosition.y;
        }

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleJump();
        HandleHealthRegen();
        UpdateVisual();
    }

    private void HandleInput()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentLane < lanes.Length - 1)
                {
                    currentLane++;
                    StartMovement();
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentLane > 0)
                {
                    currentLane--;
                    StartMovement();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            animator.SetBool("isJumping", true);
            isJumping = true;
            jumpTimer = 0f;
        }
    }

    private void StartMovement()
    {
        startPos = playerPosition;
        lerpTimer = 0f;
        isMoving = true;
    }

    private void HandleMovement()
    {
        if (!isMoving) return;

        lerpTimer += Time.deltaTime;
        float t = Mathf.Clamp01(lerpTimer / travelTime);

        Vector3 targetPos = lanes[currentLane].position;
        playerPosition.x = startPos.x + (targetPos.x - startPos.x) * t;
        playerPosition.z = startPos.z + (targetPos.z - startPos.z) * t;

        if (t >= 1f)
        {
            isMoving = false;
            groundY = lanes[currentLane].position.y;
        }
    }

    private void HandleJump()
    {
        if (!isJumping)
        {
            playerPosition.y = groundY;
            return;
        }

        jumpTimer += Time.deltaTime;
        float t = jumpTimer / jumpDuration;

        if (t >= 1f)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
            playerPosition.y = groundY;
        }
        else
        {
            float jumpProgress = 1f - ((t - 0.5f) * (t - 0.5f) * 4f);
            playerPosition.y = groundY + jumpHeight * jumpProgress;
        }
    }

    private void HandleHealthRegen()
    {
        if (currentHealth >= maxHealth) return;

        regenTimer += Time.deltaTime;
        if (regenTimer >= 1f)
        {
            currentHealth = currentHealth + 1;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            UpdateHealthUI();
            regenTimer = 0f;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died!");
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }
    }

    private void UpdateVisual()
    {
        float perspective = CameraComponent.GetPerspective(playerPosition.z);
        transform.position = new Vector2(playerPosition.x, playerPosition.y) * perspective;
        transform.localScale = Vector3.one * perspective;
    }
}