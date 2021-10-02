using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource dieSound;

    // Start is called before the first frame update
    protected virtual void Start()
    { 
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dieSound = GetComponent<AudioSource>();
    }

    public void JumpedOn()
    {
        anim.SetTrigger("Death");
        dieSound.Play();
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;


    }
    public void Death()
    {
        Destroy(this.gameObject);
    }
}
