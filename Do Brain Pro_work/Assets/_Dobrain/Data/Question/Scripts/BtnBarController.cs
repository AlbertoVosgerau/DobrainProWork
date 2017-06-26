using System.Collections;
using UnityEngine;

public class BtnBarController : MonoBehaviour
{
    GameObject BtnBar;
    Animator btnBarAnim;
    bool isBtnBarIn = true;

    void Start()
    {
        BtnBar = gameObject;
        btnBarAnim = BtnBar.GetComponent<Animator>();
        StartCoroutine(HideBtnBar());
    }
    public void BtnBarToggle()
    {
        if (btnBarAnim.IsInTransition(0) || btnBarAnim.GetBool("isInTransition"))
            return;
        if (isBtnBarIn)
        {
            btnBarAnim.SetTrigger("BtnBarOut");
        }
        else
        {
            btnBarAnim.SetTrigger("BtnBarIn");
            StopAllCoroutines();
            StartCoroutine(HideBtnBar());
        }
        isBtnBarIn = !isBtnBarIn;
    }
    IEnumerator HideBtnBar()
    {
        yield return new WaitForSeconds(3.0f);
        btnBarAnim.SetTrigger("BtnBarOut");
        isBtnBarIn = false;
    }

    void setTransition()
    {
        btnBarAnim.SetBool("isInTransition", true);
    }

    void reSetTransition()
    {
        btnBarAnim.SetBool("isInTransition", false);
    }
}
