using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    [SerializeField] private float movementSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float minX, maxX, minY, maxY;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        List<float> angles = new List<float> { Random.Range(0f, 30f), Random.Range(150f, 210f), Random.Range(330f, 360f) };
        int randomIndex = Random.Range(0, angles.Count);
        float angle = angles[randomIndex];  //Definindo uma direção inicial aleatória
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        Debug.Log("Angle: " + angle);
        Debug.Log("Move direction (sin cos): " + moveDirection);

        float objectHalfWidth = transform.localScale.x / 2;
        float objectHalfHeight = transform.localScale.y / 2;
        minX = CameraController.bottomLeft.x + objectHalfWidth;
        maxX = CameraController.topRight.x - objectHalfWidth;
        minY = CameraController.bottomLeft.y + objectHalfHeight;
        maxY = CameraController.topRight.y - objectHalfHeight;

        rb.linearVelocity = moveDirection * movementSpeed;
    }

    void FixedUpdate() {
        Vector2 newPosition = (Vector2)transform.position + moveDirection * movementSpeed * Time.fixedDeltaTime;

        //Verifica se o objeto está saindo pelos lados
        if (newPosition.x <= minX || newPosition.x >= maxX) {
            moveDirection.x = -moveDirection.x; // Reflete horizontalmente
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            movementSpeed = 0f;
            //Debug.Log("Game Over!");
        }

        //Verifica se o objeto está saindo pelo topo ou base
        if (newPosition.y <= minY || newPosition.y >= maxY) {
            moveDirection.y = -moveDirection.y; //Reflete verticalmente
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        }

        rb.linearVelocity = moveDirection * movementSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Vector2 normal = collision.contacts[0].normal;   //Obtendo a normal da colisão (a direção perpendicular à superfície)
        moveDirection = Vector2.Reflect(moveDirection, normal).normalized;  //Refletindo a direção do movimento
        rb.linearVelocity = moveDirection * movementSpeed;
    }
}
