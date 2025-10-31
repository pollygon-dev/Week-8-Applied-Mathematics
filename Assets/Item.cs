using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 itemPosition;
    public Vector3 targetPosition;
    
    private SpriteRenderer spriteRenderer;
    private PlayerController player;

    [Header("Collision")]
    [SerializeField] private float collisionDistance = 1.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindFirstObjectByType<PlayerController>();

        //if (spriteRenderer != null)
        //{
        //    spriteRenderer.color = GetRandomColor();
        //}
    }

    private void Update()
    {
        UpdateFake3DVisual();
        CheckCollision();
    }

    private void UpdateFake3DVisual()
    {
        float perspective = CameraComponent.GetPerspective(itemPosition.z);
        transform.position = new Vector2(itemPosition.x, itemPosition.y) * perspective;
        transform.localScale = Vector3.one * perspective;
    }

    private void CheckCollision()
    {
        if (player == null) return;

        if (player.isJumping) return;

        float dz = itemPosition.z - player.playerPosition.z;
        if (Mathf.Abs(dz) > 2f) return;

        float dx = itemPosition.x - player.playerPosition.x;
        if (Mathf.Abs(dx) > 1f) return;

        player.TakeDamage(5);
        Destroy(gameObject);
    }

    //private Color GetRandomColor()
    //{
    //    var rRand = Random.Range(0f, 1f);
    //    var gRand = Random.Range(0f, 1f);
    //    var bRand = Random.Range(0f, 1f);
    //    return new Color(rRand, gRand, bRand);
    //}
}