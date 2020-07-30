using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DDRManager : MonoBehaviour
{

    // length of the ddr match
    public float maxTime = 15f;
    // how fast the keys should drop
    public float dropSpeed = 100f;
    // how often the keys should drop
    public float spawnFrequency = .33f;
    
    // up-down tolerance for pressing a key
    public float pressTolerance = 30f;
    // target position for when a key should be pressed
    public GameObject targetLocation;
    // holds the possible keys to spawn
    public GameObject[] keys;

    // used for displaying the score
    public Text scoreText;
    //used for displaying end game info
    public Text endText;

    // point values for various conditions
    public int pointsForMiss = 2;
    public int badKeyPressPoints = 1;
    public int defaultPointGain = 1;
    public int goodTimingPointGain = 3;
    public int greatTimingPointGain = 5;

    // not needed to be public but so that it is visible from the inspector
    public int score;

    List<GameObject> movingArrows;
    float timer;
    float spawnTimer;
    int maxScore;
    float targetY;
    bool unloaded = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = maxTime;
        spawnTimer = 0;
        score = 0;
        maxScore = 0;
        movingArrows = new List<GameObject>();
        UpdateScoreText();
        targetY = targetLocation.transform.position.y + pressTolerance / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                SpawnKey();
                spawnTimer = spawnFrequency;
            }
            else
            {
                spawnTimer -= Time.deltaTime;
            }
        }
        UpdateKeyPositions();
        UpdateScoreText();
        if (!unloaded && movingArrows.Count == 0 && timer <= 0)
        {
            // Do whatever should happen when the game is over - could be calling a level manager script,
            //  a call back to the sting script, something
            UpdateEndText();
            unloaded = true;
            Invoke("Unload", 3f);
        }
    }

    public void startDDR()
    {
        score = 0;
        spawnTimer = 0;
        timer = maxTime;
        maxScore = 0;
        movingArrows = new List<GameObject>();
        UpdateScoreText();
        targetY = targetLocation.transform.position.y + pressTolerance / 2;
        unloaded = false;
    }

    private void SpawnKey()
    {
        int index = Random.Range(0, keys.Length);
        GameObject key = Instantiate(keys[index], GameObject.FindGameObjectWithTag(keys[index].tag).transform);
        key.transform.SetParent(GameObject.FindGameObjectWithTag("KeyCatcher").transform);
        movingArrows.Add(key);
        maxScore += greatTimingPointGain;
    }

    public void HandleKeyPress(string keyPressed)
    {
        GameObject[] matchingTag = GameObject.FindGameObjectsWithTag(keyPressed);
        matchingTag.Reverse();
        float maxY = targetY + pressTolerance;
        float minY = targetY - pressTolerance;
        foreach (GameObject g in matchingTag)
        {
            float yPos = g.transform.position.y;
            if (yPos <= maxY && yPos >= minY)
            {
                if (yPos <= targetY + pressTolerance / 5 && yPos >= targetY - pressTolerance / 5)
                {
                    score += greatTimingPointGain;
                } 
                else if (yPos <= targetY + pressTolerance / 3 && yPos >= targetY - pressTolerance / 3)
                {
                    score += goodTimingPointGain;
                }
                else
                {
                    score += defaultPointGain;
                }
                Destroy(g);
                return;
            }
        }
        score -= badKeyPressPoints;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateKeyPositions()
    {
        for (int i = 0; i < movingArrows.Count; i++)
        {
            if (movingArrows[i] == null)
            {
                movingArrows.RemoveAt(i);
                i--;
            }
            else
            {
                GameObject key = movingArrows[i];
                Vector3 position = key.transform.position;
                position.y -= dropSpeed * Time.deltaTime;
                key.transform.position = position;
                if (position.y <= targetY - pressTolerance)
                {
                    Destroy(key);
                    movingArrows.RemoveAt(i);
                    i--;
                    score -= pointsForMiss;
                }
            }

        }
    }

    private void UpdateEndText()
    {
        endText.gameObject.SetActive(true);
        if (endText != null)
        {
            string prec = (score / maxScore * 100).ToString("f2");
            endText.text = $"Max Score: {maxScore} ({prec}%)" + maxScore;
        }
    }

    private void Unload()
    {
        ///SceneManager.UnloadSceneAsync("BrockDDR");
        foreach (Transform child in GameObject.FindGameObjectWithTag("KeyCatcher").transform)
        {
            Destroy(child.gameObject);
        }
        endText.gameObject.SetActive(false);
        FindObjectOfType<LevelManager>().EndDDR(score, maxScore);
    }
}
