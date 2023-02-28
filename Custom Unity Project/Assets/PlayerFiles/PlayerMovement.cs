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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = new Vector3(0, 0, 1);
        cameraTransform = camera.GetComponent<Transform>();
        cameraRotation = 0;
        canJump = false;
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector3(Mathf.Sin(cameraRotation * 0.0174533f), 0, Mathf.Cos(cameraRotation * 0.0174533f)) * Time.deltaTime * Input.GetAxis("Vertical") * speed, ForceMode.Impulse);
        rb.AddForce(new Vector3(Mathf.Sin((cameraRotation+90) * 0.0174533f), 0, Mathf.Cos((cameraRotation + 90) * 0.0174533f)) * Time.deltaTime * Input.GetAxis("Horizontal") * speed, ForceMode.Impulse);

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
        Debug.Log(cameraRotation);
        cameraTransform.rotation = Quaternion.Euler(25, cameraRotation, 0);
        transform.rotation = Quaternion.Euler(0, cameraRotation, 0);

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