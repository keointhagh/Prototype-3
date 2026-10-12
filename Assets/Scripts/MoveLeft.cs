using UnityEngine;
using UnityEngine.UIElements;

public class MoveLeft : MonoBehaviour
{
    public float speed = 20f;
    private PlayerController playerController;
    private float leftBound = -15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController != null && !playerController.isGameOver)
            transform.Translate(Vector3.left * Time.deltaTime * speed);

        if(transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
