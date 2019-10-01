using UnityEngine;

/// <summary>
/// 視点移動
/// </summary>
public class ViewpointMovement : MonoBehaviour
{
    [SerializeField, Header("カメラ")]
    Transform cameraTransform = null;
    [SerializeField, Header("プレイヤー")]
    Transform playerTransform = null;
    [SerializeField, Header("懐中電灯")]
    Transform flashLightTransform = null;
    void Update()
    {
        ViewpointMovementConfig.GetInstance().GetViewpointMovement().
            ViewpointMovementImpl(cameraTransform, playerTransform, flashLightTransform);
    }
}