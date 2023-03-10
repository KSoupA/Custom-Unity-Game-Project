using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyScript : MonoBehaviour
{
    int timer;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        timer = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > -1) {
            timer++;
            if (timer > 550) {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player") ){
            timer = 0;
            anim.Play("Base Layer.Collect");
        }
    }
}
