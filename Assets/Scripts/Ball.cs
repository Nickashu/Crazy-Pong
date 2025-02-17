using UnityEngine;

public class Ball : MonoBehaviour {
    [SerializeField] private GameObject particleEffectExplosion;
    [SerializeField] private float movementSpeed = 5f;
    [HideInInspector] public bool gameStarted = false;
    [HideInInspector] public Vector2 moveDirection;
    private Rigidbody2D rb;
    private float minX, maxX, minY, maxY, objectHalfHeight, objectHalfWidth;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        objectHalfWidth = transform.localScale.x / 2;
        objectHalfHeight = transform.localScale.y / 2;
        minX = CameraController.bottomLeft.x + objectHalfWidth;
        maxX = CameraController.topRight.x - objectHalfWidth;
        minY = CameraController.bottomLeft.y + objectHalfHeight;
        maxY = CameraController.topRight.y - objectHalfHeight;

        GameController.GetInstance().StartGame();
    }

    void FixedUpdate() {
        if(gameStarted){
            if(!GameController.gameOver){
                Vector2 newPosition = (Vector2)transform.position + moveDirection * movementSpeed * Time.fixedDeltaTime;

                //Verifica se o objeto está saindo pelos lados (Fim do jogo):
                if (newPosition.x <= minX || newPosition.x >= maxX) {
                    newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                    moveDirection = Vector2.zero;
                    bool playerWin = newPosition.x >= maxX;
                    gameObject.SetActive(false);
                    gameStarted = false;
                    GameController.GetInstance().GameOver(playerWin);

                    StartParticleEffect(playerWin);   //Efeito de partículas
                }

                //Verifica se o objeto está saindo pelo topo ou base
                if (newPosition.y <= minY || newPosition.y >= maxY) {
                    moveDirection.y = -moveDirection.y; //Reflete verticalmente
                    newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
                }
            }

            rb.linearVelocity = moveDirection * movementSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Vector2 normal = collision.contacts[0].normal;   //Obtendo a normal da colisão (a direção perpendicular à superfície)
        moveDirection = Vector2.Reflect(moveDirection, normal).normalized;  //Refletindo a direção do movimento
        rb.linearVelocity = moveDirection * movementSpeed;
    }

    private void StartParticleEffect(bool playerWin){
        Vector2 particlesPosition = new Vector2(transform.position.x, transform.position.y);
        Quaternion particlesRotation = Quaternion.Euler(0, -90, -90);
        if(playerWin){
            particlesPosition.x += objectHalfWidth;
        }
        else{
            particlesPosition.x -= objectHalfWidth;
            particlesRotation = Quaternion.Euler(180, -90, -90);
        }
        GameObject particleEffect = Instantiate(particleEffectExplosion, particlesPosition, Quaternion.identity);
        particleEffect.transform.rotation = particlesRotation;
        ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
        float psDuration = ps.main.duration + ps.main.startLifetime.constant;
        Destroy(particleEffect, psDuration);
        particleEffect.SetActive(true);
        particleEffect.GetComponent<ParticleSystem>().Play();
    }
}
