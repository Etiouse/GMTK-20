using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHandler : MonoBehaviour
{
    [SerializeField]
    private BallMazeManager ball;

    [SerializeField]
    private GameObject spawnParent;

    [SerializeField]
    private GameObject obstaclesParent;

    [SerializeField]
    private GameObject firstPopupProtectionWall;

    [SerializeField]
    private GameObject wrapper;

    [SerializeField]
    private GameObject notActiveUI;

    private static int currentSpawnIndex;

    private const int ACTIVATE_FIRST_PROTECTION_WALL_SPAWN_INDEX = 1;
    private const int ACTIVATE_OBSTACLES_SPAWN_INDEX = 3;

    private List<GameObject> obstacles;

    private bool isMazeStarted = false;

    public static int CurrentSpawnIndex
    {
        get
        {
            return currentSpawnIndex;
        }

        set
        {
            if (value >= ACTIVATE_FIRST_PROTECTION_WALL_SPAWN_INDEX)
            {
                activateFirstProtectionWall = true;
            }

            currentSpawnIndex = value;
        }
    }

    private static bool activateFirstProtectionWall = false;

    private static List<GameObject> spawns;

    private void OnEnable()
    {
        ProgressWindow.OnProgressFinishedAction += StartMaze;
        BallMazeManager.OnEndMazeEvent += EndMaze;
    }

    private void OnDisable()
    {
        ProgressWindow.OnProgressFinishedAction -= StartMaze;
        BallMazeManager.OnEndMazeEvent -= EndMaze;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartMaze();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMazeStarted)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ball.SetBallPosition(Vector3.zero);
        }

        if (currentSpawnIndex >= ACTIVATE_OBSTACLES_SPAWN_INDEX)
        {
            foreach (GameObject item in obstacles)
            {
                item.SetActive(true);
            }
        }

        firstPopupProtectionWall.SetActive(activateFirstProtectionWall);
    }

    public static GameObject GetCurrentSpawn()
    {
        return spawns[currentSpawnIndex];
    }

    public static int GetMaxSpawnNumber()
    {
        return spawns.Count;
    }

    public static void HighlightNextSpawn()
    {
        foreach (GameObject item in spawns)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }

        spawns[currentSpawnIndex + 1].GetComponent<SpriteRenderer>().enabled = true;
    }

    private void StartMaze()
    {
        currentSpawnIndex = 0;

        spawns = new List<GameObject>();
        for (int i = 0; i < spawnParent.transform.childCount; i++)
        {
            GameObject spawn = spawnParent.transform.GetChild(i).gameObject;
            spawns.Add(spawn);
        }

        obstacles = new List<GameObject>();
        for (int i = 0; i < obstaclesParent.transform.childCount; i++)
        {
            GameObject obstacle = obstaclesParent.transform.GetChild(i).gameObject;
            obstacles.Add(obstacle);
            obstacle.SetActive(false);
        }

        HighlightNextSpawn();

        isMazeStarted = true;
        wrapper.SetActive(true);
        notActiveUI.SetActive(false);
    }

    private void EndMaze()
    {
        notActiveUI.SetActive(true);
        wrapper.SetActive(false);
    }
}
