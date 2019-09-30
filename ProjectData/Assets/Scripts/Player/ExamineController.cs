using UnityEngine;

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
        //ヒットしているExamineが同じならUpdate
        if (prevHitExamine == currentHitExamine)
        {
            if (currentHitExamine) currentHitExamine.HitUpdate();
        }
        //ヒットしているExamineが違うならexitとinitialize
        else if (prevHitExamine != currentHitExamine)
        {
            if (prevHitExamine)
            {
                prevHitExamine.HitExit();
            }
            if (currentHitExamine)
            {
                currentHitExamine.HitInitialize();
            }
        }
        if (SwitchInput.GetButtonDown(0, SwitchButton.Examine))
        {
            currentHitExamine.MainProcess();
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
