using UnityEngine;

/// <summary>
/// 調べるクラスの抽象クラス
/// </summary>
[RequireComponent(typeof(Outline))]
public abstract class ExamineBase : MonoBehaviour
{
    Outline outline = null;
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        outline.OutlineColor = new Color(0, 1, 1, 1);
    }

    /// <summary>
    /// 当たった瞬間に実行
    /// </summary>
    public void HitEnter()
    {
        outline.enabled = true;
        HitEnterImpl();
    }

    /// <summary>
    /// 当たっている間に実行
    /// </summary>
    public void HitStay()
    {
        HitStayImpl();
    }

    /// <summary>
    /// 当たらなくなった瞬間に実行
    /// </summary>
    public void HitExit()
    {
        outline.enabled = false;
        HitExitImpl();
    }

    /// <summary>
    /// 調べる
    /// </summary>
    public void Examine()
    {
        ExamineImpl();
    }

    protected virtual void HitEnterImpl() { }
    protected virtual void HitStayImpl() { }
    protected virtual void HitExitImpl() { }
    protected virtual void ExamineImpl() { }
}
