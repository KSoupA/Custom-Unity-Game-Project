using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody rb;
    public float speed;
    Vector3 direction;
    new public GameObject camera;
    public float jumpHeight;
    public LayerMask layerMask;
    Transform cameraTransform;
    new Transform transform;
    float cameraRotation;
    bool canJump;
    bool walkAnimRepeat;
    bool idleAnimRepeat;
    public float maxSpeed;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = new Vector3(0, 0, 1);
        cameraTransform = camera.GetComponent<Transform>();
        cameraRotation = 0;
        canJump = false;
        transform = GetComponent<Transform>();
        walkAnimRepeat = false;
        idleAnimRepeat = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            /*
            if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical")==0)
                rb.velocity = new Vector3(Mathf.Sin((cameraRotation + 90) * 0.0174533f) * speed * 25, rb.velocity.y, Mathf.Cos((cameraRotation + 90) * 0.0174533f) * speed * 25);
            if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") == 0)
                rb.velocity = new Vector3(Mathf.Sin((cameraRotation + 90) * 0.0174533f) * speed * -25, rb.velocity.y, Mathf.Cos((cameraRotation + 90) * 0.0174533f) * speed * -25);

            if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal")==0)
                rb.velocity = new Vector3(Mathf.Sin(cameraRotation * 0.0174533f) * speed * 25, rb.velocity.y, Mathf.Cos(cameraRotation * 0.0174533f) * speed * 25);
            if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") == 0)
                rb.velocity = new Vector3(Mathf.Sin(cameraRotation * 0.0174533f) * speed * -25, rb.velocity.y, Mathf.Cos(cameraRotation * 0.0174533f) * speed * -25);

            if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
                rb.velocity = new Vector3(Mathf.Sin((cameraRotation + 45) * 0.0174533f) * speed * 25, rb.velocity.y, Mathf.Cos((cameraRotation + 45) * 0.0174533f) * speed * 25);
            if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
                rb.velocity = new Vector3(Mathf.Sin((cameraRotation + 225) * 0.0174533f) * speed * 25, rb.velocity.y, Mathf.Cos((cameraRotation + 225) * 0.0174533f) * speed * 25);
            if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
                rb.velocity = new Vector3(Mathf.Sin((cameraRotation + 315) * 0.0174533f) * speed * 25, rb.velocity.y, Mathf.Cos((cameraRotation + 315) * 0.0174533f) * speed * 25);
            if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
                rb.velocity = new Vector3(Mathf.Sin((cameraRotation + 135) * 0.0174533f) * speed * 25, rb.velocity.y, Mathf.Cos((cameraRotation + 135) * 0.0174533f) * speed * 25);
        */
            if (rb.velocity.x + rb.velocity.z > -maxSpeed|| rb.velocity.x + rb.velocity.z < maxSpeed) { 
                rb.AddForce(new Vector3(Mathf.Sin(cameraRotation * 0.0174533f), 0, Mathf.Cos(cameraRotation * 0.0174533f)) * Time.deltaTime * Input.GetAxis("Vertical") * speed, ForceMode.Impulse);
                rb.AddForce(new Vector3(Mathf.Sin((cameraRotation + 90) * 0.0174533f), 0, Mathf.Cos((cameraRotation + 90) * 0.0174533f)) * Time.deltaTime * Input.GetAxis("Horizontal") * speed, ForceMode.Impulse);
            }
            //if(!animator.isPlaying)
            animator.speed = (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z))/10;
            idleAnimRepeat = false;
            if (!walkAnimRepeat)
            {
                animator.CrossFade("Base Layer.Run", 30, -1, 0,1);
                walkAnimRepeat = true;
                animator.speed = (Mathf.Abs(rb.velocity.x)+ Mathf.Abs(rb.velocity.z));
            }

        }
        else {
            walkAnimRepeat = false;
            if (!idleAnimRepeat)
            {
                animator.CrossFade("Base Layer.End Run", 200, -1, 0, 1);
                idleAnimRepeat = true;
                //animator.speed = (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z)) / 10;
            }
            animator.speed = 1;
        }
        if (Input.GetAxis("Jump") > .2 && canJump) {
            rb.AddForce(Vector3.up*jumpHeight, ForceMode.Impulse);
            canJump = false;
        }
        
        cameraRotation += Input.GetAxis("Mouse X");
        if (cameraRotation >= 360)
        {
            cameraRotation -= 360;
        }
        else if (cameraRotation < 0)
        {
            cameraRotation += 360;
        }
        
        
        cameraTransform.rotation = Quaternion.Euler(25, cameraRotation, 0);

        if (rb.velocity.x + rb.velocity.y < -.03 || rb.velocity.x + rb.velocity.y > .03)
        {
            transform.rotation = Quaternion.Euler(0, cameraRotation+ Mathf.Atan(Mathf.Sin(cameraRotation * 0.0174533f) / Mathf.Cos(cameraRotation * 0.0174533f)), 0);
        }

        cameraTransform.position = transform.position + new Vector3(Mathf.Sin(cameraRotation * 0.0174533f) * -8, 4, Mathf.Cos(cameraRotation * 0.0174533f) * -8);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraTransform.position-transform.position, out hit, 10, layerMask)){
            cameraTransform.position = hit.point- (cameraTransform.position - transform.position)*.05f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        canJump = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            canJump = false;
    }
}