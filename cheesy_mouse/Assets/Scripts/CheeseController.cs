using UnityEngine;

public class CheeseController : MonoBehaviour
{
    public AudioClip eatSound;

    private CheeseManager cheeseManager;    
    
    void Start()
    {
        cheeseManager = GameObject.Find("CheeseManager").GetComponent<CheeseManager>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //audioManager.PlaySound(eatSound);
            
            gameObject.SetActive(false);
            cheeseManager.cheeseIsEaten();
        }
    }
}
