using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text gameOverText;
    public Text restartText;
    public CatManager catManager;
    public PlayerMovement playerMovement;
    public CheeseManager cheeseManager;
    public CameraAspectController cameraAspectController;
    
    public enum GameState
    {
        Initialize,
        Playing,
        Won,
        Lost,
        Over,
        Restarting,
    };
    public GameState state;
    
    
    void Start()
    {
        state = GameState.Initialize;
        
        StartCoroutine(FSM());
    }
    
    IEnumerator FSM()
    {
        while (true)
        {
            switch (state)
            {
                case GameState.Initialize:
                    gameOverText.gameObject.SetActive(false);
                    restartText.gameObject.SetActive(false);
                    
                    catManager.ResetCats();
                    playerMovement.Reset();
                    cheeseManager.Reset();
                    
                    state = GameState.Playing;
                    break;
                case GameState.Playing:

                    break;
                case GameState.Won:
                    gameOverText.text = "YOU WON!";
                    gameOverText.gameObject.SetActive(true);
                    
                    StartCoroutine(GameOver());
                    state = GameState.Over;
                    break;
                case GameState.Lost:
                    gameOverText.text = "YOU LOST!";
                    gameOverText.gameObject.SetActive(true);
                    
                    StartCoroutine(GameOver());
                    state = GameState.Over;
                    break;
                case GameState.Over:
                    break;
                case GameState.Restarting:
                    break;
                default:
                    break;
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        restartText.gameObject.SetActive(true);
        state = GameState.Restarting;
        
    }
    
    void Update()
    {
        // change to touch input later
        if (state == GameState.Restarting && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            state = GameState.Initialize;
        }
    }
}
