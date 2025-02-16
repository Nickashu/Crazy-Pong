using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {

    [SerializeField] private float movementSpeed=5f, offSetLimits=0.05f;
    [SerializeField] private GameObject ball, player1;
    private Rigidbody2D rb;
    private Vector2 movementDirection;

    private float minY, maxY;
    private bool enableRandomMovementCoroutine = false, enableNormalMovementCoroutine = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        //Define os limites baseado no tamanho do sprite do jogador:
        float limitBottomY = CameraController.bottomLeft.y + offSetLimits, limitTopY = CameraController.topRight.y - offSetLimits;
        float playerHalfHeight = transform.localScale.y / 2;

        minY = limitBottomY + playerHalfHeight;
        maxY = limitTopY - playerHalfHeight;
    }

    void FixedUpdate() {
        if(Math.Abs(transform.position.x-ball.transform.position.x) < Math.Abs(player1.transform.position.x-ball.transform.position.x)) {
            enableRandomMovementCoroutine = true;
            StopCoroutine(RandomMovement());
            if(enableNormalMovementCoroutine){
                StartCoroutine(NormalMovement());
                enableNormalMovementCoroutine = false;
            }
        }
        else {
            enableNormalMovementCoroutine = true;
            StopCoroutine(NormalMovement());
            if(enableRandomMovementCoroutine){
                StartCoroutine(RandomMovement());
                enableRandomMovementCoroutine = false;
            }
        }

        if(movementDirection != Vector2.zero) {
            Vector2 newPosition = (Vector2)transform.position + movementDirection * movementSpeed * Time.fixedDeltaTime;
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            rb.MovePosition(newPosition);
        }
    }

    private IEnumerator NormalMovement() {
        if(ball.transform.position.y > transform.position.y)
            movementDirection = Vector2.up;
        else if(ball.transform.position.y < transform.position.y)
            movementDirection = Vector2.down;
        else
            movementDirection = Vector2.zero;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.25f, 0.7f));
        enableNormalMovementCoroutine = true;
    }

    private IEnumerator RandomMovement() {
        List<Vector2> directions = new List<Vector2> { Vector2.up, Vector2.down, Vector2.zero };
        int randomIndex = UnityEngine.Random.Range(0, directions.Count);
        movementDirection = directions[randomIndex];
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.75f, 1.5f));
        enableRandomMovementCoroutine = true;
    }
}
