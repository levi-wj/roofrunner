using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpPower = 4f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] Shot bullet = null;
    [SerializeField] GameObject respawnUI = null;
    [SerializeField] RespawnTimer slide = null;
    [SerializeField] DeathScore endgameUI = null;
    [SerializeField] BoxCollider2D standingCollider = null;
    [SerializeField] BoxCollider2D diveCollider = null;

    private Rigidbody2D rb = null;
    private Animator anim;
    private BoxCollider2D[] boxes = null;
    private Spawner level = null;
    private ParticleSystem particle = null;

    private SoundManager sound = null;
    private GameManager game = null;
    private CameraShake cam = null;

    private LayerMask groundLayer;
    private Vector3 respawnPos = new Vector3(-.471f, -1.5f, 0f);
    private Vector3 restartPos = new Vector3(-.471f, -.408f, 0f);
    public bool grounded = false;
    public bool jumping = false;
    public bool gameGoing = false;
    public bool tutorialGoing = false;
    private bool shooting = false;
    public bool sliding = false;
    public bool hasRespawned = false;
    public int jumpFrameCount = 0;
    private float groundLevel = 0f;
    private float shots = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxes = GetComponents<BoxCollider2D>();
        anim = GetComponent<Animator>();
        particle = GetComponent<ParticleSystem>();

        game = FindObjectOfType<GameManager>();
        cam = FindObjectOfType<CameraShake>();
        sound = FindObjectOfType<SoundManager>();

        groundLayer = LayerMask.GetMask("Ground");
        level = FindObjectOfType<Spawner>();

        diveCollider.enabled = false;
    }

    void Update()
    {
        if (gameGoing || tutorialGoing) {
            checkGrounded();

            // Better jumping
            if (rb.velocity.y < 0) {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            } else if (rb.velocity.y > 0 && !jumping) {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            // Fix if player is too far left
            if (transform.position.x < respawnPos.x) {
                transform.position = new Vector3(transform.position.x + (.1f * Time.deltaTime), transform.position.y);
            }

            // TODO: For desktop, remove later
            if (Input.GetButtonDown("Shoot")) {
                Shoot(false);
            }

            if (shots < 3) {
                shots += Time.deltaTime;
            }
        }
    }

    private void checkGrounded()
    {
        if (jumpFrameCount > 0) {
            jumpFrameCount--;
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, .27f, groundLayer);
        // Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - .27f));

        grounded = (hit.collider != null);
        if (grounded && !sliding && !shooting) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run")) {
                anim.Play(tutorialGoing ? "Idle" : "Run");
            }
        }
    }

    public void setGameGoing(bool game)
    {
        if (game) {
            resumeAnim();
            rb.simulated = true;
        } else {
            pauseAnim();
            rb.simulated = false;
        }

        gameGoing = game;
    }

    public void setTutorial(bool tut)
    {
        tutorialGoing = tut;
        if (tut) {
            rb.simulated = true;
        }
    }

    public void Jump(bool tutorial)
    {
        if (gameGoing || tutorial) {
            if (grounded) {
                grounded = false;
                rb.velocity = Vector2.up * jumpPower;
                anim.SetTrigger("Jump");
                sound.PlaySound("hop");
                jumping = true;

                // Jumpframe institutes delay in checkground()'s raycast, which ensures animation looks right
                jumpFrameCount = 7;
            }
        }
    }

    public void Fall()
    {
        standingCollider.enabled = false;
        diveCollider.enabled = false;

        Die();
    }

    public void Dive(bool tutorial)
    {
        if ((gameGoing || tutorial) && !sliding) {
            sliding = true;
            diveCollider.enabled = true;
            standingCollider.enabled = false;
            particle.Play();

            anim.SetTrigger("Slide");
            rb.gravityScale = 2.5f;
        }
    }

    public void StopDive(bool overrideGameGoing)
    {
        if (overrideGameGoing || (gameGoing && sliding)) {
            sliding = false;
            standingCollider.enabled = true;
            diveCollider.enabled = false;
            particle.Stop();

            rb.gravityScale = 1f;
            anim.Play("Run");
        }
    }

    public void PointerUp()
    {
        if (gameGoing) {
            if (jumping) {
                jumping = false;
                anim.Play("Run");
            } else if (sliding) {
                StopDive(false);
            }
        }
    }

    public void Shoot(bool tutorial)
    {
        if (gameGoing || tutorial) {
            if (shots >= 1) {
                // Instantiate at different positions depending on if the player is sliding
                Instantiate(bullet, new Vector2(transform.position.x + .3f, transform.position.y + (!sliding ? .1f : -.03f)), transform.rotation);
                cam.SetCameraShake(.06f, .01f);
                sound.PlaySound("shot");

                if (!sliding && !tutorial) {
                    shooting = true;
                    anim.Play("Run_shoot", 0, anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    StartCoroutine("finishShootAnimation");
                }
                shots--;
            }
        }
    }

    IEnumerator finishShootAnimation ()
    {
        yield return new WaitForSeconds(.6f);
        shooting = false;
        anim.Play("Run", 0, anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    public void Die()
    {
        game.pauseGame();
        pauseAnim();
        boxes[0].enabled = false;
        boxes[1].enabled = false;
        StopDive(true);

        if (hasRespawned) {
            endgameUI.Show();
        } else {
            respawnUI.SetActive(true);
            slide.StartCounting();
        }

    }

    public void pauseAnim()
    {
        anim.speed = 0;
    }

    public void resumeAnim()
    {
        anim.speed = 1;
    }

    public void Respawn()
    {
        hasRespawned = true;
        transform.position = respawnPos;
        boxes[1].enabled = true;
        rb.bodyType = RigidbodyType2D.Static;

        // Get rid of all the drones
        Drone[] drones = FindObjectsOfType<Drone>();
        for (int i = 0; i < drones.Length; i++) {
            Destroy(drones[i].gameObject);
        }

        StartCoroutine(waitForGround());
    }

    private IEnumerator waitForGround()
    {
        level.setMoving(true);
        yield return new WaitWhile(isGroundMovedYet);
        yield return new WaitUntil(isGroundMovedYet);
        level.setMoving(false);

        StartCoroutine(finishRespawn());
    }

    private bool isGroundMovedYet()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 2f, groundLayer);
        
        if (hit.collider == null) {
            return false;
        } else {
            groundLevel = hit.point.y;
            return true;
        }
    }

    private IEnumerator finishRespawn()
    {
        float i = 0f;
        Vector3 startPos = new Vector3(respawnPos.x, -2);
        transform.position = startPos;
        Vector3 endPos = new Vector3(respawnPos.x, groundLevel + .5f);

        while (i < 1f) {
            i += Time.deltaTime * 1.5f;
            transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
        resumeAnim();
        game.resumeGame();
        jumpFrameCount = 5;
        grounded = true;
    }

    public void Restart()
    {
        rb.velocity = Vector2.zero;
        transform.position = restartPos;

        anim.speed = 1;
        boxes[1].enabled = true;
        hasRespawned = false;

        setGameGoing(true);
    }
}