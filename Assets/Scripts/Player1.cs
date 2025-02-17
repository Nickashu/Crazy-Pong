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
        if(!GameController.gameOver){
            movementDirection = inputActionMovement.action.ReadValue<Vector2>();
            if(movementDirection != Vector2.zero) {
                Vector2 newPosition = (Vector2)transform.position + movementDirection * movementSpeed * Time.fixedDeltaTime;
                newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
                rb.MovePosition(newPosition);
            }
        }
    }
    /*
    void OnCollisionEnter2D(Collision2D collision) {
        ContactPoint2D contact = collision.contacts[0];
        Vector2 collisionPoint = contact.point;

        if(collision.collider.gameObject.tag == "Ball") {
            Vector2 center = GetComponent<BoxCollider2D>().bounds.center;

            float deltaX = Mathf.Abs(contact.point.x - center.x);
            float deltaY = Mathf.Abs(contact.point.y - center.y);

            if (deltaY > deltaX) {
                Debug.Log("Colidiu pelo topo ou base!");
            } 
            else {
                if (contact.point.x > center.x) {
                    Debug.Log("Colidiu pelo lado direito!");
                } 
                else {
                    Debug.Log("Colidiu pelo lado esquerdo!");
                }
            }
        }
    }
    */
}
