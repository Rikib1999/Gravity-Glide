using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject playerPrefab;
    public GameObject attractivePrefab;
    public GameObject repulsivePrefab;

    public Transform spawnPoint;

    private static GameObject player;
    private ArtificialGravity lastArtificialGravity;
    private List<ArtificialGravity> artificialGravityList = new List<ArtificialGravity>();

    public static bool IsPlaying => player != null;

    [SerializeField] private int attractiveAvailable;
    [SerializeField] private int repulsiveAvailable;

    [SerializeField] private Texture2D cursorTexture;
    public GravityLeft attractiveText;
    public GravityLeft repulsiveText;

    private int attractiveLeft;
    public int AttractiveLeft
    {
        get { return attractiveLeft; }
        set
        {
            attractiveLeft = value;
            attractiveText.Number = value;
        }
    }

    private int repulsiveLeft;
    public int RepulsiveLeft
    {
        get { return repulsiveLeft; }
        set
        {
            repulsiveLeft = value;
            repulsiveText.Number = value;
        }
    }

    private void Start()
    {
        Vector2 cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        AttractiveLeft = attractiveAvailable;
        RepulsiveLeft = repulsiveAvailable;
    }

    private void Update()
    {
        if (IsPlaying) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (AttractiveLeft <= 0) return;

            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100.0f);

            if (!hit || hit.collider.transform.gameObject.layer != 6) return;

            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (lastArtificialGravity != null) lastArtificialGravity.radiusGraphics.GetComponent<SpriteRenderer>().enabled = false;
            lastArtificialGravity = Instantiate(attractivePrefab, pos, Quaternion.identity).GetComponent<ArtificialGravity>();
            AttractiveLeft--;
            artificialGravityList.Add(lastArtificialGravity);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (RepulsiveLeft <= 0) return;

            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100.0f);

            if (!hit || hit.collider.transform.gameObject.layer != 6) return;

            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if (lastArtificialGravity != null) lastArtificialGravity.radiusGraphics.GetComponent<SpriteRenderer>().enabled = false;
            lastArtificialGravity = Instantiate(repulsivePrefab, pos, Quaternion.identity).GetComponent<ArtificialGravity>();
            RepulsiveLeft--;
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

        AttractiveLeft = attractiveAvailable;
        RepulsiveLeft = repulsiveAvailable;
    }
}