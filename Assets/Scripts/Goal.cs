using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool wasReached = false;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (wasReached) return;

        if (other.gameObject.CompareTag("Player"))
        {
            wasReached = true;
            Dissolve();
        }
    }

    private void Dissolve()
    {
        LevelManager.Instance.GoalsLeft--;

        if (LevelManager.Instance.GoalsLeft == 0) return;
        wasReached = true;
        StartCoroutine(ChangeAlpha());
    }

    IEnumerator ChangeAlpha()
    {
        float duration = 0.5f;
        
        for (float t = duration; t >= 0f; t -= Time.deltaTime)
        {
            float percent = t / duration;

            Color color = spriteRenderer.color;
            color.a = percent;
            spriteRenderer.color = color;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}