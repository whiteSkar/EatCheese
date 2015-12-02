using UnityEngine;
using System.Collections;

public class CatManager : MonoBehaviour
{
    public GameObject[] cats;
        
    
    public void ResetCats()
    {
        for (int i = 0; i < cats.Length; i++)
            cats[i].GetComponent<MovementBase>().Reset();
    }
}
