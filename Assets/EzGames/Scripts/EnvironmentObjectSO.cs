using UnityEngine;





[CreateAssetMenu(fileName = "NewEnvironmentObject", menuName = "Environment Object")]



public class EnvironmentObjectSO : ScriptableObject
{
    public string objectName; // Tên đối tượng môi trường (cây, nhà, etc.)
    public GameObject prefab; // Prefab đối tượng
    
}
