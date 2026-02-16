using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    [Header("Zombie Settings")]
    
    public GameObject selectedZombie;
    public GameObject[] zombies;
    public Vector3 selectedSize;
    private InputAction left, right, jump;
    private int  selectedZombieIndex = 0;
    public Vector3 pushForce;
    
    public TMP_Text timerText;
    private float time = 0;
    void Start()
    {
        SelectZombie(0);
        
        left = InputSystem.actions.FindAction("Previous Zombie");
        right = InputSystem.actions.FindAction("Next Zombie");
        jump = InputSystem.actions.FindAction("Jump");
        
        
    }

    void SelectZombie(int index)
    {
        if (selectedZombie != null)
            selectedZombie.transform.localScale = Vector3.one;
        selectedZombie = zombies[index];
        selectedZombie.transform.localScale = selectedSize;
        Debug.Log("Selected: " + selectedZombie.name);
    }

    void Update()
    {
        if (jump.triggered)
        {
            Rigidbody rb = selectedZombie.GetComponent<Rigidbody>();
            rb.AddForce(pushForce);
        }
        
        if (left.triggered)
        {
            selectedZombieIndex--;
            if (selectedZombieIndex <= 0)
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
}
