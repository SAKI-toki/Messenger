using UnityEngine;

/// <summary>
/// 懐中電灯の制御クラス
/// </summary>
public class FlashLightController : MonoBehaviour
{
    [SerializeField, Header("回転の制限(絶対値)")]
    RotLimitVector3 rotLimitAbs = new RotLimitVector3();
    void Update()
    {
        Rotation();
        //Stick押し込みでリセット
        if (SwitchInput.GetButtonDown(0, SwitchButton.Stick))
        {
            SwitchGyro.SetBaseGyro(0);
        }
    }

    void Rotation()
    {
        //コントローラーの向きと同じ方向に回転する
        transform.localRotation = SwitchGyro.GetGyro(0);
        var eulerAngles = transform.localEulerAngles;
        //X軸回転の制限
        if (eulerAngles.x > rotLimitAbs.x &&
        eulerAngles.x < 360 - rotLimitAbs.x)
        {
            eulerAngles.x = (eulerAngles.x < 180 ? 1 : -1) * rotLimitAbs.x;
        }
        //Y軸回転の制限
        if (eulerAngles.y > rotLimitAbs.y &&
        eulerAngles.y < 360 - rotLimitAbs.y)
        {
            eulerAngles.y = (eulerAngles.y < 180 ? 1 : -1) * rotLimitAbs.y;
        }
        //Z軸回転の制限
        if (eulerAngles.z > rotLimitAbs.z &&
        eulerAngles.z < 360 - rotLimitAbs.z)
        {
            eulerAngles.z = (eulerAngles.z < 180 ? 1 : -1) * rotLimitAbs.z;
        }
        transform.localEulerAngles = eulerAngles;
    }

    /// <summary>
    /// Vector3ではRangeを付けれないのでSerializableなVector3クラスを実装
    /// </summary>
    [System.Serializable]
    struct RotLimitVector3
    {
        [SerializeField, Range(0, 180)]
        public float x, y, z;
    }
}
