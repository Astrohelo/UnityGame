using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    
    //private Collider2D coll;
    private CapsuleCollider2D coll;
    [SerializeField] private LayerMask ground;
    private enum State { idle, running, jumping, falling, hurt };
    private State state = State.idle;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 1.5f;
    [SerializeField] private float speed = 7;
    [SerializeField] private float jumpForce = 12;
    [SerializeField] private int lives = 3;
    [SerializeField] private Text livesAmount;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;
    [SerializeField] private float hurtForce = 7f;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource cherrySound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        livesAmount.text = lives.ToString();
    }


    private void Update()
    {
        if (state != State.hurt)
        {
            Movement();
        }

        AnimSwitch();
        anim.SetInteger("state", (int)state);

        //jobb ugras ha tovabb nyomod magasabb lesz
        if (rb.velocity.y < 0)
        {
            rb.velocity += UnityEngine.Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += UnityEngine.Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherrySound.Play();
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {   
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                lives -= 1;
                livesAmount.text = lives.ToString();
                if (lives == 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                //Enemy on player's right
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new UnityEngine.Vector2(-hurtForce, rb.velocity.y);
                }
                else //Enemy on player's left
                {
                    rb.velocity = new UnityEngine.Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void Movement()
    {
        float hdirection = Input.GetAxis("Horizontal");

        //sebesseg es forgatas balra
        if (hdirection < 0)
        {
            rb.velocity = new UnityEngine.Vector2(-speed, rb.velocity.y);
            transform.localScale = new UnityEngine.Vector2(-1, 1);

        }
        //sebesseg es forgatas jobbra
        else if (hdirection > 0)
        {
            rb.velocity = new UnityEngine.Vector2(speed, rb.velocity.y);
            transform.localScale = new UnityEngine.Vector2(1, 1);


        }
        else
        {
            rb.velocity = new UnityEngine.Vector2(0, rb.velocity.y);
        }

        //ugrani tud ha a földön van
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers())
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }
    private void AnimSwitch()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Going 
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
        
    }

    private void FootStep()
    {
        footstep.Play();
    }

}
