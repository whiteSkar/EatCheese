using UnityEngine;

public class CheeseController : MonoBehaviour
{
    private CheeseManager cheeseManager;
    
    void Start()
    {
        cheeseManager = GameObject.Find("CheeseManager").GetComponent<CheeseManager>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            cheeseManager.cheeseIsEaten();
        }
    }
}
