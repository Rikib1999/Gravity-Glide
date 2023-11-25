using UnityEditor;
using UnityEngine;

public class ArtificialGravity : MonoBehaviour
{
    public float strength = 0.0f;
    public float radius = 5.0f;
    public bool attractiveForce = true;

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = radius;
        var pm = GetComponentInChildren<ParticleSystem>();
        
        var m = pm.main;
        m.startLifetime = attractiveForce ? radius * 0.3f : radius / 3f;

        if (attractiveForce)
        {
            var s = pm.shape;
            s.radius = radius * 0.65f;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 direction = new(transform.position.x - other.transform.position.x, transform.position.y - other.transform.position.y);

            if (!attractiveForce) direction *= -1;

            float distance = Vector2.Distance(transform.position, other.transform.position);

            float force = (radius - distance) / radius;

            other.GetComponent<Rigidbody2D>().AddForce(direction.normalized * strength * force * force, ForceMode2D.Force);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = attractiveForce ? Color.red : Color.green;
        Handles.DrawWireDisc(transform.position + transform.forward, transform.forward, radius);
    }
}