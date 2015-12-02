using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class PathFindingMovement : MovementBase
{
    public GameObject map;
    public Transform player;
    
    
    private static readonly List<Vector2> DIRS = new List<Vector2>() {new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1)};
    // static possibly
    private IDictionary<Vector2, AStarNode> validNodes = new Dictionary<Vector2, AStarNode>();
    private Vector2 dest;
    private GameManager gameManager;
    
    [System.Serializable]
    private class AStarNode
    {
        public int cost;
        public int h;
        public Vector2 parentNodePos;
        
        public AStarNode(int cost, int h, Vector2 parentNodePos)
        {
            this.cost = cost;
            this.h = h;
            this.parentNodePos = parentNodePos;
        }
    }
    
    public override void Reset()
    {
        base.Reset();
        
        dest = transform.position;
    }

    void FixedUpdate()
    {
        if (gameManager.state != GameManager.GameState.Playing) return;

        Vector2 curPos = transform.position;

        if (curPos == dest)
        {
            // starting pos is maybe a float
            Vector2 curPosAdjusted = new Vector2(Mathf.Round(curPos.x), Mathf.Round(curPos.y));
            Vector2 playerPosAdjusted = new Vector2(Mathf.Round(player.position.x), Mathf.Round(player.position.y));
             
            ComputeAstarH();
            
            var visitedPoses = new HashSet<Vector2>();
            var willVisitPoses = new HashSet<Vector2>();
            willVisitPoses.Add(curPosAdjusted);
            
            AStarNode playerAStarNode = null;
            
            // Uncomment if I want to limit how far the cat can detect the player
            //int AStarSensitivityLevel = 20;
            //for (int m = 0; m < AStarSensitivityLevel && willVisitPoses.Count > 0; m++)
            while (willVisitPoses.Count > 0)
            {
                var pos = willVisitPoses.OrderBy(p => validNodes[p].cost + validNodes[p].h).First();
                var aStarNode = validNodes[pos];
                
                playerAStarNode = aStarNode;                
                if (pos == playerPosAdjusted)
                    break;
                
                visitedPoses.Add(pos);
                willVisitPoses.Remove(pos);
                
                var adjacentNodesPos = GetAdjacentValidNodes(pos);
                for (int i = 0; i < adjacentNodesPos.Count; i++)
                {
                    var adjacentNodePos = adjacentNodesPos[i];
                    if (visitedPoses.Contains(adjacentNodePos))
                        continue;
                    
                    int newCost = aStarNode.cost + 1;
                    if (!willVisitPoses.Contains(adjacentNodePos))
                    {
                        validNodes[adjacentNodePos].cost = newCost;
                        validNodes[adjacentNodePos].parentNodePos = pos;
                        willVisitPoses.Add(adjacentNodePos);
                    }
                    else
                    {
                        var adjacentNode = validNodes[adjacentNodePos];
                        if (newCost < adjacentNode.cost)
                        {
                            adjacentNode.cost = newCost;
                            adjacentNode.parentNodePos = pos;
                        }
                    }
                }
            }
            
            // When the parentNodePos of currentAStarNode is curPosAdjusted, 
            //  the currentAStarNode is the first node in the shortest path
            while (playerAStarNode != null && playerAStarNode.parentNodePos != curPosAdjusted)
            {
                dest = playerAStarNode.parentNodePos;
                playerAStarNode = validNodes[dest];
            }
        }

        Move(dest - curPos);
    }
    
    void ComputeAstarH()
    {
        Vector2 targetPos = player.position;
        
        foreach (var nodePos in validNodes.Keys)
        {
            var deltaDist = targetPos - nodePos;
            validNodes[nodePos].cost = 0;
            validNodes[nodePos].h = (int) Mathf.Abs(deltaDist.x) + (int) Mathf.Abs(deltaDist.y);
            validNodes[nodePos].parentNodePos = Vector2.zero;
        }
    }
    
    IList<Vector2> GetAdjacentValidNodes(Vector2 pos)
    {
        List<Vector2> nodes = new List<Vector2>();
        
        for (int i = 0; i < DIRS.Count; i++)
        {
            var adjacentNode = pos + DIRS[i];
            if (validNodes.ContainsKey(adjacentNode))
            {
                nodes.Add(adjacentNode);
            }
        }
        
        return nodes;
    }
    
    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("Blocking"))
        {
            dest = transform.position;
        }
    }
    
    // Refactor with CheeseManager code.
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
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
                    if (IsIntersect(mapBlocks[k], point))
                    {
                        isOnBlock = true;
                        break;
                    }
                }

                if (!isOnBlock)
                    validNodes.Add(new Vector2(j, i), new AStarNode(0, 0, Vector2.zero));
            }
        }
        
        Reset();
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
