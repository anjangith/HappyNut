using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float distanceToBeGrounded = 1.5f;
    public float gravityModifier = 1f;

    protected bool grounded;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);

    [Tooltip("Gets added to position every frame.")]
    protected Vector2 MoveDirection = Vector2.zero;
    
    protected const float shellRadius = 2.0f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    void Start () 
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        contactFilter.useLayerMask = true;
    }
    
    //void Update () 
    //{
    //    targetVelocity = Vector2.zero;
    //    ComputeVelocity (); 
    //}

    protected virtual void ComputeVelocity()
    {
    
    }

    //void FixedUpdate()
    //{
    //    velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
    //    velocity.x = targetVelocity.x;

    //    grounded = false;

    //    Vector2 deltaPosition = velocity * Time.deltaTime;

    //    Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);

    //    Vector2 move = moveAlongGround * deltaPosition.x;

    //    Movement (move, false);

    //    move = Vector2.up * deltaPosition.y;

    //    Movement (move, true);
    //}


    /// <summary>
    /// Set the varaible grounded to true or false.
    /// </summary>
    protected void SetGrounded()
    {
        grounded = false;

        int count = rb2d.Cast(Vector2.down, contactFilter, hitBuffer, shellRadius);
        hitBufferList.Clear();
        for (int i = 0; i < count; i++)
        {
            hitBufferList.Add(hitBuffer[i]);
        }
          

        foreach (RaycastHit2D t in hitBufferList)
        {
            if (t.distance < distanceToBeGrounded)
            {
                grounded = true;
            }
        }


        
        
    }

}
