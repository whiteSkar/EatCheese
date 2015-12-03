using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text gameOverText;
    public float gameOverTextOffsetFromBottom;
    public float gameOverTextMinWidthPercent;
    public Text restartText;
    public float restartTextOffsetFromBottom;
    public float restartTextMinWidthPercent;
    
    
    // Use this for initialization
    void Start()
    {
        gameOverText.transform.position = new Vector3(gameOverText.transform.position.x, Screen.height * gameOverTextOffsetFromBottom, gameOverText.transform.position.z);
        restartText.transform.position = new Vector3(restartText.transform.position.x, Screen.height * restartTextOffsetFromBottom, restartText.transform.position.z);
        
        gameOverText.resizeTextMinSize = (int) (Screen.width * gameOverTextMinWidthPercent);
        restartText.resizeTextMinSize = (int) (Screen.width * restartTextMinWidthPercent);
    }
}
