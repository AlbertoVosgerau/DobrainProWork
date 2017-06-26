using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINewContentAlwaysNotificationPopup : MonoBehaviour {

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Show(bool visible)
    {
        if(visible)
            animator.Play("Show");
        else
            animator.Play("Hide");
    }
     
     

}
