using UnityEngine;
using System.Collections;

public class CameraAspectController : MonoBehaviour
{
    public float targetAspectWidth;
    public float targetAspectHeight;
    public float gameUnitOffsetFromTop;
    
    private float cameraOffset;
    
    
    public float getCameraOffset()
    {
        return cameraOffset;
    }
    
    void Awake()
    {
        float targetaspect = targetAspectWidth / targetAspectHeight;

        // determine the game window's current aspect ratio
        float windowaspect = (float) Screen.width / (float) Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize /= scaleheight;
        
        // Move the game to the top
        var mapSprite = GameObject.Find("Map").GetComponent<SpriteRenderer>().sprite;
        var mapHeightInPixels = mapSprite.rect.size.y;
        var mapHeightInUnits = mapHeightInPixels / mapSprite.pixelsPerUnit;
        var mapHeightInScreen = mapHeightInUnits * 0.5f / Camera.main.orthographicSize;

        var screenHeightRatioInScreen = Screen.height * 0.5f / Camera.main.orthographicSize;
        var screenHeightInScreen = Screen.height / screenHeightRatioInScreen;
        
        var camPos = camera.transform.position;
        cameraOffset = screenHeightInScreen * ((1 - mapHeightInScreen) / 2.0f) - gameUnitOffsetFromTop;
        camera.transform.position = new Vector3(camPos.x, camPos.y - cameraOffset, camPos.z);

/*
        if (scaleheight < 1.0f) // if scaled height is less than current height, add letterbox
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            //camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
*/
    }
}