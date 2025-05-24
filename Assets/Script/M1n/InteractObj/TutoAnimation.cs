using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoAnimation 
{
    public GameObject Singer;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = Singer.GetComponent<Animator>();
        animator.SetBool("isTuto", true);
        animator.SetBool("isTuto2", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
