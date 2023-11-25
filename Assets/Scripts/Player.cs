using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector2 direction;

    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawLine(transform.position, transform.position + (new Vector3(direction.normalized.x, direction.normalized.y, 0) * speed));
    }
}