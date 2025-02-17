using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] private GameObject player1, player2, ball, canvasMenuGameOver;
    public static bool gameOver = false;
    private static GameController instance;
    private bool isRestarting=false;

    public static GameController GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void GameOver(bool player1Win){
        gameOver = true;
        canvasMenuGameOver.SetActive(true);
        Debug.Log("Game Over! Player " + (player1Win ? "1" : "2") + " wins!");
    }

    public void RestartGame(){
        PositionPlayersAndBall();
        ball.SetActive(true);
        canvasMenuGameOver.SetActive(false);
        if(!isRestarting){
            StartCoroutine(StartGameCoroutine());
            isRestarting = true;
        }
    }

    public void PositionPlayersAndBall(){
        player1.transform.position = new Vector2(player1.transform.position.x, 0);
        player2.transform.position = new Vector2(player2.transform.position.x, 0);
        ball.transform.position = new Vector2(0, 0);
    }

    //TODO: consertar interação entre player e bola (player não deve conseguir mover a bola)
    public void StartGame(){
        isRestarting = false;

        List<float> possibleDirectionsX = new List<float> { Random.Range(-1f, -0.7f), Random.Range(0.7f, 1f) };
        List<float> possibleDirectionsY = new List<float> { Random.Range(-0.3f, 0f), Random.Range(0.3f, 0f) };
        int randomIndex = Random.Range(0, 2);
        Vector2 moveDirection = new Vector2(possibleDirectionsX[randomIndex], possibleDirectionsY[randomIndex]).normalized;

        ball.GetComponent<Ball>().moveDirection = moveDirection;
        ball.GetComponent<Ball>().gameStarted = true;
        gameOver = false;
    }

    private IEnumerator StartGameCoroutine(){
        yield return new WaitForSeconds(1.5f);
        StartGame();
    }
}
