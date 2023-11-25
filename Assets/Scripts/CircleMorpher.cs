using UnityEngine;

public class CircleMorpher : MonoBehaviour
{
    private float max;
    private float min;

    public float variance = 0.1f;
    public float differnece = 0.01f;

    private void Start()
    {
        max = transform.localScale.x + variance;
        min = transform.localScale.x - variance;
    }

    void Update()
    {
        float x = Mathf.Clamp(transform.localScale.x + Random.Range(-differnece, differnece), min, max);

        transform.localScale = new Vector3(x, x, 1);
    }
}