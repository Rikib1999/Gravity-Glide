using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject attractivePrefab;
    public GameObject repulsivePrefab;

    private ArtificialGravity lastArtificialGravity;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (lastArtificialGravity != null) lastArtificialGravity.radiusGraphics.GetComponent<SpriteRenderer>().enabled = false;
            lastArtificialGravity = Instantiate(attractivePrefab, pos, Quaternion.identity).GetComponent<ArtificialGravity>();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (lastArtificialGravity != null) lastArtificialGravity.radiusGraphics.GetComponent<SpriteRenderer>().enabled = false;
            lastArtificialGravity = Instantiate(repulsivePrefab, pos, Quaternion.identity).GetComponent<ArtificialGravity>();
        }

        if (lastArtificialGravity != null && Input.mouseScrollDelta.y != 0) lastArtificialGravity.Radius += Input.mouseScrollDelta.y;
    }
}