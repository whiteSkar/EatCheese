using UnityEngine;
using System.Collections;

public class CheeseController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            gameObject.SetActive(false);
    }
}
