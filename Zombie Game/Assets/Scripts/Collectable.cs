using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameManager gameManager;


    public float amplitude = 0.25f;
    public float frequency = 1f;
    public float rotateSpeed = 90f;
    
    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + Vector3.up * yOffset;
        
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.AddScore();
        }
    }
}