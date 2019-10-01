using UnityEngine;

/// <summary>
/// 調べる行動の制御
/// </summary>
public class ExamineController : MonoBehaviour
{
    [SerializeField, Header("調べる行動が届く範囲")]
    float examineDistance = 0.0f;
    //レイを飛ばすTransform
    Transform rayTransform = null;
    //レイが飛ばす球体の大きさ
    const float raySphereSize = 5.0f;
    //レイが当たっているオブジェクト
    ExamineBase rayHitExamine = null;
    void Start()
    {
        //レイは懐中電灯のライトから飛ばす
        rayTransform = GetComponentInChildren<Light>().transform;
    }

    void Update()
    {
        ExamineBase prevHitExamine = rayHitExamine;
        //現在ヒットしているExamineを取得
        var currentHitExamine = GetExamineWithRay();
        //違うオブジェクトにヒットしたらヒットしていたオブジェクトをExit,
        //現在ヒットしているオブジェクトをEnterを実行
        if (prevHitExamine != currentHitExamine)
        {
            if (prevHitExamine) prevHitExamine.HitExit();
            if (currentHitExamine) currentHitExamine.HitEnter();
        }
        //Stayは常に実行
        if (currentHitExamine) currentHitExamine.HitStay();
        //Examineを押したらExamineを実行
        if (SwitchInput.GetButtonDown(0, SwitchButton.Examine))
        {
            if (currentHitExamine) currentHitExamine.Examine();
        }
        rayHitExamine = currentHitExamine;
    }

    /// <summary>
    /// 調べるオブジェクトをレイで取得
    /// </summary>
    ExamineBase GetExamineWithRay()
    {
        Ray ray = new Ray(rayTransform.position, rayTransform.forward);
        RaycastHit hit;
        //レイを飛ばす
        if (Physics.Raycast(ray, out hit, examineDistance))
        {
            return hit.transform.GetComponent<ExamineBase>();
        }
        return null;
    }
}
