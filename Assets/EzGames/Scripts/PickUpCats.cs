using UnityEngine;
using System.Collections.Generic;

public class PizzaVision : MonoBehaviour
{
    public float visionDistance = 5f;  
    public float visionAngle = 45f;    

    public Transform pickupPosition;   
    public float pickupTime = 1f;     

    private List<GameObject> pickedObjects = new List<GameObject>();  
    private float timer = 0f;

    void Update()
    {
        CheckForObjectsInPizzaVision();
    }

    void CheckForObjectsInPizzaVision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionDistance); 
        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;
            float angleBetween = Vector3.Angle(transform.forward, directionToTarget);

            if (angleBetween < visionAngle && hitCollider.CompareTag("PickAble"))
            {
                if (!pickedObjects.Contains(hitCollider.gameObject))  
                {
                    timer += Time.deltaTime;

                    if (timer >= pickupTime)
                    {
                        PickupObject(hitCollider.gameObject);  
                        timer = 0f;  
                    }
                }
            }
        }
    }

    void PickupObject(GameObject obj)
    {
        pickedObjects.Add(obj);  

        obj.transform.SetParent(pickupPosition);  
        
        float currentStackHeight = 2f; 

        
        foreach (GameObject pickedObj in pickedObjects)
        {
            currentStackHeight += pickedObj.GetComponent<BoxCollider>().size.y *2;  
        }

     
        obj.transform.localPosition = new Vector3(0, currentStackHeight, 0);  
    }

    
  
}
