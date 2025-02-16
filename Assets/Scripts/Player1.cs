using UnityEngine;
using UnityEngine.InputSystem;

public class Player1 : MonoBehaviour {

    [SerializeField] private float movementSpeed=5f, offSetLimits=0.5f;
    [SerializeField] private InputActionReference inputActionMovement;
    private Rigidbody2D rb;
    private Vector2 movementDirection;

    private float minY, maxY;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        //Define os limites baseado no tamanho do sprite do jogador:
        float limitBottomY = CameraController.bottomLeft.y + offSetLimits, limitTopY = CameraController.topRight.y - offSetLimits;
        float playerHalfHeight = transform.localScale.y / 2;

        minY = limitBottomY + playerHalfHeight;
        maxY = limitTopY - playerHalfHeight;
    }

    void FixedUpdate() {
        movementDirection = inputActionMovement.action.ReadValue<Vector2>();
        if(movementDirection != Vector2.zero) {
            Vector2 newPosition = (Vector2)transform.position + movementDirection * movementSpeed * Time.fixedDeltaTime;
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            rb.MovePosition(newPosition);
        }
    }
}
