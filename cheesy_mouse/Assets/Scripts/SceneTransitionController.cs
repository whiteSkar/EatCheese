using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour 
{
    public float splashScreenTime;
    public string mainSceneName;
    
    
    private float startTime;
    private float fadeStartTime;
    private float fadingTime;
    
    void Start()
    {
        startTime = Time.time;
        fadeStartTime = startTime;
    }
    
    void Update()
    {
        if (Time.time - startTime >= splashScreenTime)
        {
            if (fadeStartTime == startTime)
            {
                fadeStartTime = Time.time;
                fadingTime = GetComponent<SceneFader>().BeginFade(1);                
            }
            else if (fadeStartTime + fadingTime <= Time.time)
            {
                SceneManager.LoadScene(mainSceneName);                
            }
        }
    }
}
