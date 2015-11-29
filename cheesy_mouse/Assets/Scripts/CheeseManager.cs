using UnityEngine;

public class CheeseManager : MonoBehaviour
{
    public GameObject cheese;
    public GameObject map;
    public CircleCollider2D player;
    
    private int cheeseCount = 0;
    private GameManager gameManager;
    private AudioSource audioSrc;
    
    
    public void cheeseIsEaten()
    {
        audioSrc.Play();
        
        cheeseCount--;
        if (cheeseCount <= 0)
        {
            gameManager.state = GameManager.GameState.Won;
        }
    }
    
    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSrc = GetComponent<AudioSource>();
        
        var mapBlocks = map.GetComponents<BoxCollider2D>();
        
        // 2, 31, 28 are all hacky way for getting map size and offset.
        int mapOffset = 2;
        for (int i = (int) map.transform.position.x + mapOffset; i < 31; i++)
        {
            for (int j = (int) map.transform.position.y + mapOffset; j < 28; j++)
            {
                var point = new Vector2(j, i);       
                         
                bool isOnBlock = false;
                for (int k = 0; k < mapBlocks.Length; k++)
                {
                    //if (mapBlocks[k].OverlapPoint(point))   // why does this not work?
                    if (IsIntersect(mapBlocks[k], point))
                    {
                        isOnBlock = true;
                        break;
                    }
                }
                
                if (IsIntersect(player, point))
                    isOnBlock = true;

                if (!isOnBlock)
                {
                    Instantiate(cheese, new Vector3(j, i, 0f), Quaternion.identity);
                    cheeseCount++;
                }
            }
        }
    }
    
    bool IsIntersect(Collider2D collider, Vector2 point)
    {
        var offset = collider.offset + (Vector2) collider.transform.position;
        float halfX;
        float halfY;
        
        if (collider is BoxCollider2D)
        {
            halfX = (collider as BoxCollider2D).size.x / 2.0f;
            halfY = (collider as BoxCollider2D).size.y / 2.0f;
        }
        else if (collider is CircleCollider2D)
        {
            halfX = (collider as CircleCollider2D).radius;
            halfY = (collider as CircleCollider2D).radius;
        }
        else
        {
            return false; // should not happen
        }
        
        if (point.x >= offset.x - halfX && 
            point.x <= offset.x + halfX &&
            point.y >= offset.y - halfY && 
            point.y <= offset.y + halfY)
        {
            return true;
        }
        return false;
    }
}
