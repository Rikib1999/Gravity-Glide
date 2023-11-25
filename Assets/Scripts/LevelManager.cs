using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject attractivePrefab;
    public GameObject repulsivePrefab;

    public Transform spawnPoint;

    private static GameObject player;
    private ArtificialGravity lastArtificialGravity;
    private List<ArtificialGravity> artificialGravityList = new List<ArtificialGravity>();

    public static bool IsPlaying => player != null;

    private void Update()
    {
        if (IsPlaying) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100.0f);

            if (!hit || hit.collider.transform.gameObject.layer != 6) return;

            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (lastArtificialGravity != null) lastArtificialGravity.radiusGraphics.GetComponent<SpriteRenderer>().enabled = false;
            lastArtificialGravity = Instantiate(attractivePrefab, pos, Quaternion.identity).GetComponent<ArtificialGravity>();
            artificialGravityList.Add(lastArtificialGravity);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100.0f);

            if (!hit || hit.collider.transform.gameObject.layer != 6) return;

            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (lastArtificialGravity != null) lastArtificialGravity.radiusGraphics.GetComponent<SpriteRenderer>().enabled = false;
            lastArtificialGravity = Instantiate(repulsivePrefab, pos, Quaternion.identity).GetComponent<ArtificialGravity>();
            artificialGravityList.Add(lastArtificialGravity);
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

    public void Restart()
    {
        Destroy(player);
        player = null;

        foreach (var item in artificialGravityList)
        {
            if (item != null) Destroy(item.gameObject);
        }

        artificialGravityList = new List<ArtificialGravity>();
    }
}