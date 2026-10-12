/*
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Vector3 playerPos = new Vector3(1f, 4f, 0.5f);
    private Rigidbody playerRb;

    public InputAction jumpAction;

    public float jumpForce = 10f;

    // Changing this to a base gravity vector ensures it resets properly every reload
    private readonly Vector3 normalGravity = new Vector3(0, -9.81f, 0);
    public float gravityModifier = 1.5f;

    private bool isOnGround = true;
    public bool isGameOver { get; private set; }
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound;
    public AudioClip crashSound;
    public AudioSource playerAudio;

    private Animator playerAnimator;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        jumpAction.Enable();

        // FIX: Hard-set the gravity relative to Earth normal instead of compounding it (*=)
        Physics.gravity = normalGravity * gravityModifier;

        isGameOver = false;
        isOnGround = true;

        // Safely force-reset the Animator values at the start of the clean scene
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("Death_b", false);
            playerAnimator.SetInteger("DeathType_int", 0);
            playerAnimator.SetFloat("speed_f", 1f); // Set back to running speed if applicable

            playerAnimator.Play("Alive", 0, 0f);
            playerAnimator.Update(0f);
        }
    }

    void Update()
    {
        if (jumpAction.triggered && isOnGround && !isGameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnimator.SetTrigger("Jump_trig");
            dirtParticle.Stop();

            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        if (jumpAction.triggered && isGameOver)
        {
            // Cancel any accidental double invokes and queue up the scene refresh
            CancelInvoke("RestartScene");
            Invoke("RestartScene", 1f);
        }
    }

    void RestartScene()
    {
        // Prep physics for the reload frame so they don't clip through the floor
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.Sleep();
        }

        // Just focus on loading the scene. The Start() function will cleanly handle the rest!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGameOver)
        {
            isOnGround = true;
            dirtParticle.Play();
        }

        if (collision.gameObject.CompareTag("Obstacle") && !isGameOver)
        {
            isGameOver = true;
            Debug.Log("Game Over!");

            playerAnimator.SetBool("Death_b", true);
            if (isOnGround) playerAnimator.SetInteger("DeathType_int", 1);

            explosionParticle.Play();
            dirtParticle.Stop();

            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }
}
*/

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Vector3 playerPos = new Vector3(1f, 4f, 0.5f);
    private Quaternion playerRot;
    private readonly Vector3 normalGravity = new Vector3(0, -9.81f, 0);
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

        Physics.gravity = normalGravity * gravityModifier;
        isGameOver = false;
        playerRot = transform.rotation;

        isOnGround = true; // Assume true if they start on the ground

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

        //Below is to reset the game
        if(jumpAction.triggered && isGameOver)
        {
            Invoke("RestartScene", 1f);
        }

    }

    void RestartScene()
    {
        playerAnimator.SetBool("Death_b", false);
        playerAnimator.SetInteger("DeathType_int", 0);

        transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameOver = false;
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
