using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float movement_speed;
    private Vector2 position;

    void Start() {
        
    }

    void Update() {
        transform.Translate(movement_speed * position * Time.deltaTime);
    }
}
