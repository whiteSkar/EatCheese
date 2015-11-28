using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text gameOverText;
    
    public enum GameState
    {
        Playing,
        Won,
        Lost,
        Over,
    };
    public GameState state;
    
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        
        state = GameState.Playing;
        
        StartCoroutine(FSM());
    }
    
    IEnumerator FSM()
    {
        while (true)
        {
            switch (state)
            {
                case GameState.Playing:
                    break;
                case GameState.Won:
                    gameOverText.text = "YOU WON!";
                    gameOverText.gameObject.SetActive(true);
                    state = GameState.Over;
                    break;
                case GameState.Lost:
                    gameOverText.text = "YOU LOST!";
                    gameOverText.gameObject.SetActive(true);
                    state = GameState.Over;
                    break;
                case GameState.Over:
                    break;
                default:
                    break;
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }


    void Update()
    {
        
    }
}
