using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject attractivePrefab;
    public GameObject repulsivePrefab;

    public Transform spawnPoint;

    private GameObject player;
    private ArtificialGravity lastArtificialGravity;

    public bool IsPlaying => player != null;

    private void Update()
    {
        if (IsPlaying) return;

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

    public void PlayStop()
    {
        if (IsPlaying)
        {
            Destroy(player);
            player = null;
        }
        else
        {
            player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            if (lastArtificialGravity != null) lastArtificialGravity.radiusGraphics.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}