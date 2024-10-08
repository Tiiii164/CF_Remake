using UnityEngine;

public class TsunamiController : MonoBehaviour
{
    //public Transform hero;         
    [SerializeField] private float phase1Speed = 10f; 
    [SerializeField] private float phase2Speed = 40f; 
    [SerializeField] private float phase1Distance = 400f; 
    [SerializeField] private float phase2Distance = 1000f; 
    [SerializeField] private float startPosition = 0f;
    private GameOverController gameOverController;
    private Vector3 tsunamiPosition;  // Vị trí hiện tại của sóng thần
    private float currentSpeed;       // Tốc độ hiện tại của sóng thần
    private float distanceTravelled;  // Khoảng cách đã đi

    void Start()
    {
        tsunamiPosition = transform.position;
        currentSpeed = phase1Speed;   // Sóng thần bắt đầu với tốc độ phase 1
        
        gameOverController = FindObjectOfType<GameOverController>();
        
    }

    void Update()
    {
        
        //S = V.t
        distanceTravelled += currentSpeed * Time.deltaTime;
        tsunamiPosition.x = startPosition + distanceTravelled;
        transform.position = tsunamiPosition;

        // hết phase 1 thì qua 2
        if (distanceTravelled >= phase1Distance)
        {
            currentSpeed = phase2Speed;
        }

        
        if (distanceTravelled >= phase1Distance + phase2Distance)
        {
            Debug.Log("Tsunami reached Finish Line 2!");
            // đi hết rồi đó
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Nếu đối tượng va chạm có tag "Player"
        {
            Debug.Log("Hit player");
            gameOverController.StopGame(); // Gọi function StopGame khi chạm vào player
        }
    }
}
