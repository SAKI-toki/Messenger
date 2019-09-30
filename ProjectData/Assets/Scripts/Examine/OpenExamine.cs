using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenExamine : ExamineBase
{
    [SerializeField]
    Material HitColor = null;
    Material defaultColor = null;
    MeshRenderer meshRenderer = null;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material;
    }
    public override void HitInitialize()
    {
        Debug.Log("HitInitialize");
        meshRenderer.material = HitColor;
    }

    public override void HitUpdate()
    {
        Debug.Log("HitUpdate");
    }

    public override void HitExit()
    {
        Debug.Log("HitExit");
        meshRenderer.material = defaultColor;
    }

    public override void MainProcess()
    {
        Debug.Log("MainProcess");
    }
}
