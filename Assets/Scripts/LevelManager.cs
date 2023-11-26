using Assets.Scripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject playerPrefab;
    public GameObject attractivePrefab;
    public GameObject repulsivePrefab;

    private Transform spawnPoint;

    private static GameObject player;
    private ArtificialGravity lastArtificialGravity;
    private List<ArtificialGravity> artificialGravityList = new();

    public static bool IsPlaying => player != null;

    public TMP_Text PlayStopButtonText { get; set; }

    [SerializeField] private int attractiveAvailable;
    [SerializeField] private int repulsiveAvailable;

    [SerializeField] private Texture2D cursorTexture;
    private GravityLeft attractiveText;
    private GravityLeft repulsiveText;

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

    public int originalGoalsLeft;

    private int goalsLeft;
    public int GoalsLeft
    {
        get { return goalsLeft; }
        set
        {
            goalsLeft = value;
            if (goalsLeft <= 0) LevelCompleted();
        }
    }

    public GameObject levelCompleted;

    public GameObject[] goals;

    protected override void Awake()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        attractiveText = GameObject.FindGameObjectWithTag("AttractiveLeft").GetComponent<GravityLeft>();
        repulsiveText = GameObject.FindGameObjectWithTag("RepulsiveLeft").GetComponent<GravityLeft>();
        PlayStopButtonText = GameObject.FindGameObjectWithTag("PlayStopButton").GetComponent<TMP_Text>();
        goals = GameObject.FindGameObjectsWithTag("Goal");

        base.Awake();
    }

    private void Start()
    {
        Vector2 cursorHotspot = new(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        AttractiveLeft = attractiveAvailable;
        RepulsiveLeft = repulsiveAvailable;

        GoalsLeft = GameObject.FindGameObjectsWithTag("Goal").Length;
        originalGoalsLeft = GoalsLeft;

        PlayStopButtonText.text = "Play";
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

    public static void PlayStop()
    {
        if (Instance.levelCompleted.activeInHierarchy) return;

        if (IsPlaying)
        {
            Destroy(player);
            player = null;
            Instance.PlayStopButtonText.text = "Play";

            foreach (var g in Instance.goals)
            {
                g.SetActive(true);
                g.GetComponent<Goal>().wasReached = false;
            }

            Instance.GoalsLeft = Instance.originalGoalsLeft;
        }
        else
        {
            player = Instantiate(Instance.playerPrefab, Instance.spawnPoint.position, Quaternion.identity);
            if (Instance.lastArtificialGravity != null) Instance.lastArtificialGravity.radiusGraphics.GetComponent<SpriteRenderer>().enabled = false;
            Instance.PlayStopButtonText.text = "Stop";
        }
    }

    public static void Restart()
    {
        Time.timeScale = 1f;
        Instance.levelCompleted.SetActive(false);

        Destroy(player);
        player = null;

        foreach (var item in Instance.artificialGravityList)
        {
            if (item != null) Destroy(item.gameObject);
        }

        foreach (var g in Instance.goals)
        {
            g.SetActive(true);
            g.GetComponent<Goal>().wasReached = false;
        }

        Instance.GoalsLeft = Instance.originalGoalsLeft;

        Instance.artificialGravityList = new List<ArtificialGravity>();

        Instance.AttractiveLeft = Instance.attractiveAvailable;
        Instance.RepulsiveLeft = Instance.repulsiveAvailable;

        Instance.PlayStopButtonText.text = "Play";
    }

    public void LevelCompleted()
    {
        Time.timeScale = 0f;
        levelCompleted.SetActive(true);
    }

    public static void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}