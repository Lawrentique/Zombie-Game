using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [Header("Game Settings")]
    
    public GameObject selectedZombie;
    public GameObject[] zombies;
    public Vector3 selectedSize;
    private InputAction left, right, jump;
    private int  selectedZombieIndex = 0;
    public Vector3 pushForce;

    public GameObject collectablePrefab;
    public Transform[] spawnPoints;
    private GameObject currentCollectable;
    private int lastSpawnIndex = -999;
    
    public TMP_Text timerText;
    private float time = 0;
    
    public TMP_Text scoreText;
    [SerializeField] private int score = 0;

    public GameObject gameOverCanvas;
    [SerializeField] private float deathY = -1f;
    private bool isGameOver = false;
    
    public AudioSource pickupAudioSource;
    public AudioClip pickupSound;
    
    void Start()
    {
        Time.timeScale = 1;
        
        SelectZombie(0);
        
        left = InputSystem.actions.FindAction("Previous Zombie");
        right = InputSystem.actions.FindAction("Next Zombie");
        jump = InputSystem.actions.FindAction("Jump");
        
        SpawnCollectable();
        UpdateScoreUI();
        
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);
    }

    void SelectZombie(int index)
    {
        if (selectedZombie != null)
            selectedZombie.transform.localScale = Vector3.one;
        selectedZombie = zombies[index];
        selectedZombie.transform.localScale = selectedSize;
    }

    void Update()
    {
        if (isGameOver) return;

        for (int i = 0; i < zombies.Length; i++)
        {
            if (zombies[i].transform.position.y <= deathY)
            {
                GameOver();
            }
        }
        
        if (jump.triggered)
        {
            Rigidbody rb = selectedZombie.GetComponent<Rigidbody>();
            rb.AddForce(pushForce);
        }
        
        if (left.triggered)
        {
            selectedZombieIndex--;
            if (selectedZombieIndex < 0)
                selectedZombieIndex = zombies.Length - 1;
            SelectZombie(selectedZombieIndex);
        }
        
        if (right.triggered)
        {
            selectedZombieIndex++;
            if (selectedZombieIndex >= zombies.Length)
                selectedZombieIndex = 0;
            SelectZombie(selectedZombieIndex);
        }
        
        time+=Time.deltaTime;
        timerText.text = "Time: " + time.ToString("F1") + "s";
    }

    public void AddScore()
    {
        score++;
        UpdateScoreUI();
        
        pickupAudioSource.PlayOneShot(pickupSound);
        
        Destroy(currentCollectable);
        StartCoroutine(SpawnWithDelay());
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    void SpawnCollectable()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        }
        while (randomIndex == lastSpawnIndex);

        lastSpawnIndex = randomIndex;

        currentCollectable = Instantiate(collectablePrefab, spawnPoints[randomIndex].position, Quaternion.identity);
        currentCollectable.GetComponent<Collectible>().gameManager = this;
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        gameOverCanvas.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(1f);
        SpawnCollectable();
    }
}
