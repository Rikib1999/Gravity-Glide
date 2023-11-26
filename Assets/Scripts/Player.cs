using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector2 direction;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawLine(transform.position, transform.position + (new Vector3(direction.normalized.x, direction.normalized.y, 0) * speed));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            LevelManager.Instance.PlayStopButtonText.text = "Play";
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<ParticleSystem>().Play();
        }
    }
}