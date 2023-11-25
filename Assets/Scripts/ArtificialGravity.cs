using UnityEditor;
using UnityEngine;

public class ArtificialGravity : MonoBehaviour
{
    [SerializeField]
    private float radius = 5.0f;
    public float Radius
    {
        get => radius;
        set
        {
            radius = value;
            SetRadius(value);
        }
    }

    public float strength = 0.0f;
    public bool attractiveForce = true;

    private ParticleSystem pm;

    private void Awake()
    {
        GetComponent<CircleCollider2D>().radius = Radius;
        pm = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        SetParticles();
    }

    private void SetParticles()
    {
        var m = pm.main;
        m.startLifetime = attractiveForce ? Radius * 0.3f : Radius / 3f;

        if (attractiveForce)
        {
            var s = pm.shape;
            s.radius = Radius * 0.65f;
        }
    }

    private void SetRadius(float value)
    {
        Radius = Mathf.Clamp(value, 2f, 8f);
        SetParticles();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 direction = new(transform.position.x - other.transform.position.x, transform.position.y - other.transform.position.y);

            if (!attractiveForce) direction *= -1;

            float distance = Vector2.Distance(transform.position, other.transform.position);

            float force = (Radius - distance) / Radius;

            other.GetComponent<Rigidbody2D>().AddForce(direction.normalized * strength * force * force, ForceMode2D.Force);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = attractiveForce ? Color.red : Color.green;
        Handles.DrawWireDisc(transform.position + transform.forward, transform.forward, Radius);
    }
}