using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHandler : MonoBehaviour
{
    [SerializeField]
    private BallMazeManager ball = null;

    [SerializeField]
    private GameObject spawnParent = null;

    [SerializeField]
    private GameObject obstaclesParent = null;

    [SerializeField]
    private GameObject firstPopupProtectionWall = null;

    private static int currentSpawnIndex;

    private const int ACTIVATE_FIRST_PROTECTION_WALL_SPAWN_INDEX = 1;
    private const int ACTIVATE_OBSTACLES_SPAWN_INDEX = 3;

    private List<GameObject> obstacles;

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

    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
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
}
