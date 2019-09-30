using UnityEngine;

/// <summary>
/// 懐中電灯の制御クラス
/// </summary>
public class FlashLightController : MonoBehaviour
{
    [SerializeField, Header("回転の制限")]
    RotLimit rotLimitAbs = new RotLimit();

    void Update()
    {
        FlashLightRotation();
        //Stick押し込みでリセット
        if (SwitchInput.GetButtonDown(0, SwitchButton.Stick))
        {
            SwitchGyro.SetBaseGyro(0);
        }
    }

    /// <summary>
    /// 懐中電灯の回転
    /// </summary>
    void FlashLightRotation()
    {
#if UNITY_SWITCH && !(UNITY_EDITOR)
        //コントローラーの向きと同じ方向に回転する
        transform.localRotation = SwitchGyro.GetGyro(0);
#endif
        var eulerAngles = transform.localEulerAngles;
        //X軸回転の制限
        if (eulerAngles.x > rotLimitAbs.down &&
        eulerAngles.x < 360 - rotLimitAbs.up)
        {
            eulerAngles.x = (eulerAngles.x < 180 ? rotLimitAbs.down : -rotLimitAbs.up);
        }
        //Y軸回転の制限
        if (eulerAngles.y > rotLimitAbs.right &&
        eulerAngles.y < 360 - rotLimitAbs.left)
        {
            eulerAngles.y = (eulerAngles.y < 180 ? rotLimitAbs.right : -rotLimitAbs.left);
        }
        //Z軸回転の制限
        if (eulerAngles.z > rotLimitAbs.counterclockwise &&
        eulerAngles.z < 360 - rotLimitAbs.clockwise)
        {
            eulerAngles.z = (eulerAngles.z < 180 ? rotLimitAbs.counterclockwise : -rotLimitAbs.clockwise);
        }
        transform.localEulerAngles = eulerAngles;
    }
    /// <summary>
    /// Vector3ではRangeを付けれないのでSerializableなVector3クラスを実装
    /// </summary>
    [System.Serializable]
    struct RotLimit
    {
        [SerializeField, Range(0, 180), Header("右回転(Y軸)")]
        public float right;
        [SerializeField, Range(0, 180), Header("左回転(Y軸)")]
        public float left;
        [SerializeField, Range(0, 180), Header("上回転(X軸)")]
        public float up;
        [SerializeField, Range(0, 180), Header("下回転(X軸)")]
        public float down;
        [SerializeField, Range(0, 180), Header("時計回り回転(Z軸)")]
        public float clockwise;
        [SerializeField, Range(0, 180), Header("反時計回り回転(Z軸)")]
        public float counterclockwise;
    }
}
