using UnityEngine;

public class CheeseManager : MonoBehaviour
{
    public GameObject cheese;
    public GameObject map;
    
    
    // Use this for initialization
    void Start()
    {
        var mapBlocks = map.GetComponents<BoxCollider2D>();
        
        // 2, 31, 28 are all hacky way for getting map size and offset.
        int mapOffset = 2;
        for (int i = (int) map.transform.position.x + mapOffset; i < 31; i++)
        {
            for (int j = (int) map.transform.position.y + mapOffset; j < 28; j++)
            {
                bool isOnBlock = false;
                for (int k = 0; k < mapBlocks.Length; k++)
                {
                    var point = new Vector2(j, i);
                    //if (mapBlocks[k].OverlapPoint(point))   // why does this not work?
                    if (IsIntersect(mapBlocks[k], point))
                    {
                        isOnBlock = true;
                        break;
                    }
                }

                if (!isOnBlock)
                    Instantiate(cheese, new Vector3(j, i, 0f), Quaternion.identity);
            }
        }
    }
    
    bool IsIntersect(BoxCollider2D collider, Vector2 point)
    {
        var offset = collider.offset;
        var halfX = collider.size.x / 2.0f;
        var halfY = collider.size.y / 2.0f;
        
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
