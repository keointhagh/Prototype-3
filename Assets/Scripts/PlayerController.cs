using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRb;

    public InputAction jumpAction;

    public float jumpForce = 10f;
    public float gravityModifier = 1.5f;
    private bool isOnGround = true;
    public bool isGameOver { get; private set; }
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound;
    public AudioClip crashSound;
    public AudioSource playerAudio;

    private Animator playerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        jumpAction.Enable();

        Physics.gravity *= gravityModifier;
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpAction.triggered && isOnGround && !isGameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnimator.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGameOver)
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;
            Debug.Log("Game Over!");
            playerAnimator.SetBool("Death_b", true);
            if(isOnGround) playerAnimator.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }
}
