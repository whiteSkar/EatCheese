using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text gameOverText;
    public float gameOverTextOffsetFromBottom;
    public Text restartText;
    public float restartTextOffsetFromBottom;
    
    void Start()
    {
        gameOverText.transform.position = new Vector3(gameOverText.transform.position.x, Screen.height * gameOverTextOffsetFromBottom, gameOverText.transform.position.z);
        restartText.transform.position = new Vector3(restartText.transform.position.x, Screen.height * restartTextOffsetFromBottom, restartText.transform.position.z);
        
        var joystick = GameObject.Find("DPad");
        var cameraOffset = Camera.main.GetComponent<CameraAspectController>().getCameraOffset();
        var cameraOffsetRatio = cameraOffset / (Camera.main.orthographicSize * 2);    // camera height is orthographicSize * 2
        joystick.transform.position = new Vector3(joystick.transform.position.x, Screen.height * cameraOffsetRatio, joystick.transform.position.z);
    }
}
