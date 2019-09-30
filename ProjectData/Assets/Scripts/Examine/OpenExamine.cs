using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenExamine : ExamineBase
{
    [SerializeField]
    GameObject mainProcessObject = null;
    Outline outline = null;
    void Start()
    {
        outline = GetComponent<Outline>();
    }

    public override void HitInitialize()
    {
        outline.enabled = true;
        Debug.Log("HitInitialize");
    }

    public override void HitUpdate()
    {
        Debug.Log("HitUpdate");
    }

    public override void HitExit()
    {
        outline.enabled = false;
        Debug.Log("HitExit");
    }

    public override void MainProcess()
    {
        Destroy(Instantiate(mainProcessObject, transform.position, Quaternion.identity), 1.0f);
        Destroy(gameObject);
    }
}
