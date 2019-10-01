/// <summary>
/// Debug用の調べるクラス
/// </summary>
public class DebugExamine : ExamineBase
{
    protected override void ExamineImpl()
    {
        Destroy(gameObject);
    }
}
