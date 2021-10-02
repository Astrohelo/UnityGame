using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    // Start is called before the first frame update
    [SerializeField] private float LeftEnd;
    [SerializeField] private float RightEnd;
    [SerializeField] private float JumpLength = 5;
    [SerializeField] private float JumpHeight = 7;
    [SerializeField] private LayerMask ground;
    private bool FacingLeft = true;
    private Collider2D coll;


    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (anim.GetBool("Jumping") == true)
        {
            if (rb.velocity.y < .1)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }
    }
    private void FrogMove()
    {
        if (FacingLeft)
        {
            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector3(1, 1);
            }

            if (transform.position.x > LeftEnd)
            {
                if (coll.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(-JumpLength, JumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                FacingLeft = false;
            }
        }
        else
        {
            if (transform.localScale.x == 1)
            {
                transform.localScale = new Vector3(-1, 1);
            }

            if (transform.position.x < RightEnd)
            {
                if (coll.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(JumpLength, JumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                FacingLeft = true;
            }
        }
    }

    
}
