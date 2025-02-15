using UnityEngine;

public class CameraController : MonoBehaviour {

    private Camera mainCamera;
    [SerializeField] private GameObject player1, player2, ball;
    [SerializeField] private float playerHeightMult = 0.21f, playerWidthMult = 0.03f, playerOffsetMult = 0.04f, ballScaleMult=0.3f;

    void Start() {
        mainCamera = Camera.main;

        player1.SetActive(true);
        player2.SetActive(true);
    }

    void Update() {
        float screenHeightUnits = mainCamera.orthographicSize * 2; //Altura da câmera em unidades de mundo
        float screenWidthUnits = screenHeightUnits * mainCamera.aspect; //Largura baseada no aspecto

        //Largura e altura dos jogadores em relação à tela:
        Vector3 playerScale = new Vector3(screenWidthUnits * playerWidthMult, screenHeightUnits * playerHeightMult, 1);
        player1.transform.localScale = playerScale;
        player2.transform.localScale = playerScale;
        Vector3 ballScale = new Vector3(mainCamera.aspect * ballScaleMult, mainCamera.aspect * ballScaleMult, 1);
        ball.transform.localScale = ballScale;

        //Posição dos jogadores em relação à tela:
        float playerOffset = screenWidthUnits * playerOffsetMult;
        float halfScreenWidth = screenWidthUnits / 2;
        player1.transform.position = new Vector2(-halfScreenWidth + playerOffset, 0);
        player2.transform.position = new Vector2(halfScreenWidth - playerOffset, 0);
    }
}

