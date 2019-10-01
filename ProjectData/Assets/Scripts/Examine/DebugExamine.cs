using UnityEngine;

/// <summary>
/// Debug用の調べるクラス
/// </summary>
public class DebugExamine : ExamineBase
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
    }

    public override void HitUpdate()
    {
    }

    public override void HitExit()
    {
        outline.enabled = false;
    }

    public override void MainProcess()
    {
        Destroy(Instantiate(mainProcessObject, transform.position, Quaternion.identity), 1.0f);
        Destroy(gameObject);
    }
}
