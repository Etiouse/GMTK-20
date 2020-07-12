using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHandler : MonoBehaviour
{
    public enum Button
    {
        NONE,
        ADMIN_CONFIRM,
        PROGRESS_DONE
    }

    public delegate void ButtonAdminConfirmDownEvent();
    public static event ButtonAdminConfirmDownEvent OnButtonAdminConfirmDownEvent;

    public delegate void ButtonProgressDoneDownEvent();
    public static event ButtonProgressDoneDownEvent OnButtonProgressDoneDownEvent;

    [SerializeField] private BallMazeManager ball;

    [SerializeField] private GameObject notActiveUI;

    [SerializeField] private GameObject spawnParent;
    [SerializeField] private GameObject obstaclesParent;
    [SerializeField] private GameObject wrapper;

    [SerializeField] private GameObject firstPopupProtectionWall;
    [SerializeField] private GameObject wallProtectionPart2;
    [SerializeField] private GameObject wallProtectionPartObstacles;
    [SerializeField] private GameObject wallEnd;
    [SerializeField] private GameObject wallEndFinishProtection1;
    [SerializeField] private GameObject wallEndFinishProtection2;

    [SerializeField] private GameObject buttonAdminConfirm;
    [SerializeField] private GameObject buttonProgressDone;

    [SerializeField] private GameObject adminConfirmPart1;
    [SerializeField] private GameObject adminConfirmPart2;
    [SerializeField] private GameObject progressDonePart1;
    [SerializeField] private GameObject progressDonePart2;

    [SerializeField] private Masker masksPart2;

    private static int currentSpawnIndex;

    private const int ACTIVATE_FIRST_PROTECTION_WALL_SPAWN_INDEX = 1;
    private const int ACTIVATE_OBSTACLES_SPAWN_INDEX = 3;

    private List<GameObject> obstacles;

    private bool isMazeStarted = false;
    private bool clippyCalled = false;

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

    public static Button CurrentButton { get; set; }

    private static bool activateFirstProtectionWall = false;

    private static List<GameObject> spawns;

    private void OnEnable()
    {
        ProgressWindow.OnProgressFinishedAction += StartMaze;
        OnButtonAdminConfirmDownEvent += ButtonAdminConfirm;
        OnButtonProgressDoneDownEvent += EndMaze;
    }

    private void OnDisable()
    {
        ProgressWindow.OnProgressFinishedAction -= StartMaze;
        OnButtonAdminConfirmDownEvent -= ButtonAdminConfirm;
        OnButtonProgressDoneDownEvent -= EndMaze;
    }

    // Start is called before the first frame update
    void Start()
    {
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

        if (Input.GetMouseButtonDown(0))
        {
            switch (CurrentButton)
            {
                case Button.ADMIN_CONFIRM:
                    OnButtonAdminConfirmDownEvent();
                    break;
                case Button.PROGRESS_DONE:
                    OnButtonProgressDoneDownEvent();
                    break;
                case Button.NONE:
                    break;
            }
        }

        if (currentSpawnIndex >= ACTIVATE_OBSTACLES_SPAWN_INDEX)
        {
            foreach (GameObject item in obstacles)
            {
                item.SetActive(true);
            }

            wallProtectionPartObstacles.SetActive(false);
            wallEnd.SetActive(false);

            wallEndFinishProtection1.SetActive(true);
            wallEndFinishProtection2.SetActive(true);

            progressDonePart1.SetActive(false);
            progressDonePart2.SetActive(true);
           
            if (!clippyCalled)
            {
                clippyCalled = true;

                Clippy.Instance.ChangeState(Clippy.State.EVIL);
                Clippy.Instance.Show(15);
                Clippy.Instance.ChangeText("It seems to be too easy for you. I’ll increase the difficulty mwahahaha", false);
                Clippy.Instance.ChangePos(new Vector3(200, -270, 0));

                GlitchEffect.instance.SetGlitch(1, 3, 0.1f);
                AudioManager.instance.PlayGlitch();
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

    private void ButtonAdminConfirm()
    {
        wallProtectionPart2.SetActive(false);
        buttonAdminConfirm.SetActive(false);

        masksPart2.enabled = true;

        adminConfirmPart1.SetActive(false);
        adminConfirmPart2.SetActive(true);

        Clippy.Instance.ChangeState(Clippy.State.EVIL);
        Clippy.Instance.Show(15);
        Clippy.Instance.ChangeText("I took the control of your PC Mwahahahaha! If you don’t want your work to be erased, you’ll have to prove your worth by escaping the maze", false);
        Clippy.Instance.ChangePos(new Vector3(200, -270, 0));

        GlitchEffect.instance.SetGlitch(1, 5, 0.1f);
        AudioManager.instance.PlayGlitch();
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
        Cursor.lockState = CursorLockMode.None;
        notActiveUI.GetComponent<UIHandler>().EndSavingProcess();
    }
}
