using UnityEngine;
using System.Collections.Generic;

public class PizzaVision : MonoBehaviour
{
    public float visionDistance = 5f;  // Khoảng cách tầm nhìn
    public float visionAngle = 45f;    // Góc mở tầm nhìn (miếng pizza)

    public Transform pickupPosition;   // Vị trí đặt vật thể trên đầu player
    public float pickupTime = 1f;      // Thời gian cần để pick vật thể

    private List<GameObject> pickedObjects = new List<GameObject>();  // Danh sách các vật đã pick
    private float timer = 0f;

    void Update()
    {
        CheckForObjectsInPizzaVision();
    }

    void CheckForObjectsInPizzaVision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionDistance); // Kiểm tra các vật thể trong tầm xa
        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float angleBetween = Vector3.Angle(transform.forward, directionToTarget);

            // Kiểm tra nếu vật thể trong góc miếng pizza và có tag "PickAble"
            if (angleBetween < visionAngle && hitCollider.CompareTag("PickAble"))
            {
                if (!pickedObjects.Contains(hitCollider.gameObject))  // Chỉ pick những vật chưa được pick
                {
                    timer += Time.deltaTime;

                    if (timer >= pickupTime)
                    {
                        PickupObject(hitCollider.gameObject);  // Pick vật thể
                        timer = 0f;  // Reset lại timer sau khi pick
                    }
                }
            }
        }
    }

    void PickupObject(GameObject obj)
    {
        pickedObjects.Add(obj);  // Thêm vật thể vào danh sách

        obj.transform.SetParent(pickupPosition);  // Đặt vật thể làm con của vị trí pickup
        
        float currentStackHeight = 2f;  // Tính tổng chiều cao của các vật đã được pick

        // Tính chiều cao của các vật thể đã pick
        foreach (GameObject pickedObj in pickedObjects)
        {
            currentStackHeight += pickedObj.GetComponent<BoxCollider>().size.y *2;  // Tính chiều cao của từng vật thể
        }

        // Đặt vật thể mới lên trên cùng
        obj.transform.localPosition = new Vector3(0, currentStackHeight, 0);  // Điều chỉnh vị trí theo chiều cao
    }

    
  
}
