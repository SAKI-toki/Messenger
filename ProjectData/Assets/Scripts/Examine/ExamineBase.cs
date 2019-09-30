using UnityEngine;

/// <summary>
/// 調べるクラスの抽象クラス
/// </summary>
public abstract class ExamineBase : MonoBehaviour
{
    public abstract void HitInitialize();
    public abstract void HitUpdate();
    public abstract void HitExit();
    public abstract void MainProcess();
}
