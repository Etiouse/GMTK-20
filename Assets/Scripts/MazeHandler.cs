using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHandler : MonoBehaviour
{
    [SerializeField]
    private BallMazeManager ball;

    [SerializeField]
    private GameObject spawnParent;

    public static int CurrentSpawnIndex { get; set; }

    private static List<GameObject> spawns;

    // Start is called before the first frame update
    void Start()
    {
        CurrentSpawnIndex = 0;

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
    }

    public static GameObject GetCurrentSpawn()
    {
        return spawns[CurrentSpawnIndex];
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

        spawns[CurrentSpawnIndex + 1].GetComponent<SpriteRenderer>().enabled = true;
    }
}
