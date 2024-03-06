using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [Header("Game Vars")]
    public int points;
    public int lives;
    public bool isRunning;
    public float pointMult;

    [Header("Fruit Vars")]
    public GameObject[] fruits;
    public GameObject[] special_fruits;
    public Vector2 spawnMinMax;
    private Vector2 defaultSpawnMinMax;
    private float spawnCooldown;

    [Header("Components")]
    private SwordCursor sword;

    [Header("UI Components")]
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI[] lifeList;
    private List<TextMeshProUGUI> livesObjects;
    public Color grayColor;
    public Color redColor;


    private void Awake()
    {
        sword = FindObjectOfType<SwordCursor>();
        defaultSpawnMinMax = spawnMinMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        newGame();
    }
    void Update()
    {
        if (isRunning)
        {
            if (spawnCooldown > 0)
            {
                spawnCooldown -= Time.deltaTime;
            }
            if (spawnCooldown <= 0)
            {
                spawnFruit();
            }
        }
    }

    public void newGame()
    {
        lives = 3;
        points = 0;
        pointMult = 1f;
        spawnMinMax = defaultSpawnMinMax;
        isRunning = true;

        //UI
        livesObjects = new List<TextMeshProUGUI>();
        foreach (TextMeshProUGUI life in lifeList)
        {
            livesObjects.Add(life);
            life.color = redColor;
        }

    }

    public void uiUpdate()
    {
        pointText.text = points.ToString();
    }

    public void addPoints(int amount)
    {
        points += (int)(amount * pointMult);
        uiUpdate();
    }

    public void looseLife()
    {
        
        lives -= 1;
        TextMeshProUGUI life = livesObjects[livesObjects.Count - 1];
        livesObjects.Remove(life);
        life.color = grayColor;
       
    }

    private void spawnFruit()
    {
        spawnCooldown = Random.Range(spawnMinMax.x, spawnMinMax.y);
        Vector2 spawnPoint = new Vector2(Random.Range(-6f, 6f), -6f);
        Instantiate(fruits[Random.Range(0, fruits.Length)], spawnPoint, Quaternion.identity);

    }


}
