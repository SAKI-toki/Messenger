using UnityEngine;

public enum VM : int
{
    GXSY = 0x0,
    GXGY = 0x1,
    None = 0x2
};

/// <summary>
/// 視点移動の設定を保持しているクラス
/// </summary>
public class ViewpointMovementConfig : Singleton<ViewpointMovementConfig>
{
    IViewpointMovement vmInterface;
    [SerializeField, Header("回転速度")]
    float rotationSpeed = 0.0f;

    void Start()
    {
        vmInterface = new GyroXStickY();
    }

    /// <summary>
    /// 視点移動の実装部を取得
    /// </summary>
    public IViewpointMovement GetViewpointMovement()
    {
        return vmInterface;
    }

    /// <summary>
    /// 視点移動の速さを取得
    /// </summary>
    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    /// <summary>
    /// 視点移動の実装部をセット
    /// </summary>
    public void SetViewpointMovement(VM vm)
    {
        switch (vm)
        {
            case VM.GXSY:
                vmInterface = new GyroXStickY();
                break;
            case VM.GXGY:
                vmInterface = new GyroXGyroY();
                break;

        }
    }
}

/// <summary>
/// 視点移動の実装部呼び出しのインターフェイス
/// </summary>
public interface IViewpointMovement
{
    void ViewpointMovementImpl(Transform cameraTransform, Transform playerTransform, Transform flashLightTransform);
}

/// <summary>
/// ジャイロでX軸の視点移動をし、スティックでY軸の視点移動をする
/// </summary>
public class GyroXStickY : IViewpointMovement
{
    public void ViewpointMovementImpl(Transform cameraTransform, Transform playerTransform, Transform flashLightTransform)
    {
        ViewpointMovementImplement.GyroX(cameraTransform, flashLightTransform);
        ViewpointMovementImplement.StickY(playerTransform);
    }
}

/// <summary>
/// ジャイロでX軸の視点移動をし、ジャイロでY軸の視点移動をする
/// </summary>
public class GyroXGyroY : IViewpointMovement
{
    public void ViewpointMovementImpl(Transform cameraTransform, Transform playerTransform, Transform flashLightTransform)
    {
        ViewpointMovementImplement.GyroX(cameraTransform, flashLightTransform);
        ViewpointMovementImplement.GyroY(playerTransform, flashLightTransform);

    }
}

/// <summary>
/// 視点移動の実装部
/// </summary>
static public class ViewpointMovementImplement
{
    static Quaternion q, camRotation, playerRotation;
    static float rotateStick;

    /// <summary>
    /// ジャイロでX軸の視点移動
    /// </summary>
    static public void GyroX(Transform cameraTransform, Transform flashLightTransform)
    {
        camRotation = cameraTransform.localRotation;
        q = Quaternion.Euler(flashLightTransform.localEulerAngles.x, 0, 0);
        if (Quaternion.Angle(camRotation, q) > 10)
        {
            camRotation = Quaternion.Slerp(camRotation, q, Time.deltaTime);
        }
        cameraTransform.localRotation = camRotation;
    }
    /// <summary>
    /// ジャイロでY軸の視点移動
    /// </summary>
    static public void GyroY(Transform playerTransform, Transform flashLightTransform)
    {
        playerRotation = playerTransform.localRotation;
        q = Quaternion.Euler(0, flashLightTransform.eulerAngles.y, 0);
        if (Quaternion.Angle(playerRotation, q) > 25)
        {
            playerRotation = Quaternion.Slerp(playerRotation, q, Time.deltaTime);
        }
        playerTransform.localRotation = playerRotation;
    }
    /// <summary>
    /// スティックでY軸の視点移動
    /// </summary>
    static public void StickY(Transform playerTransform)
    {
        rotateStick = SwitchInput.GetHorizontal(0);
        playerTransform.Rotate(0,
            rotateStick * ViewpointMovementConfig.GetInstance().GetRotationSpeed() * Time.deltaTime,
            0);
    }
}
