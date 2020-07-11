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
    private GameObject firstPopupProtectionWall;

    private static int currentSpawnIndex;

    public static int CurrentSpawnIndex
    {
        get
        {
            return currentSpawnIndex;
        }

        set
        {
            if (value >= 1)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ball.SetBallPosition(Vector3.zero);
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
