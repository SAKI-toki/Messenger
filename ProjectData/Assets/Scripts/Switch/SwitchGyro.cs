using UnityEngine;
using System.Collections.Generic;
#if UNITY_SWITCH  && !(UNITY_EDITOR)
using nn.hid;
#endif

/// <summary>
/// Switchのジャイロ
/// </summary>
static public class SwitchGyro
{
#if UNITY_SWITCH && !(UNITY_EDITOR)
    //ジャイロのハンドラ
    static SixAxisSensorHandle[] gyroHandles = new SixAxisSensorHandle[1];
    //ジャイロの状態
    static SixAxisSensorState gyroState = new SixAxisSensorState();
    static nn.util.Float4 refQuaternion = new nn.util.Float4();
#endif
    //ジャイロの基準を保持
    static Dictionary<int, Quaternion> baseGyro = new Dictionary<int, Quaternion>();
    static Quaternion quat = new Quaternion();

    /// <summary>
    /// ジャイロの基準をセット
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    static public void SetBaseGyro(int index)
    {
        //キーがない場合は追加しておく
        if (!baseGyro.ContainsKey(index)) baseGyro.Add(index, Quaternion.identity);
        var q = GetGyroImpl(index);
        q.w *= -1;
        baseGyro[index] = q;
    }

    /// <summary>
    /// ジャイロの取得の実装部
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>ジャイロの回転</returns>
    static Quaternion GetGyroImpl(int index)
    {
#if UNITY_SWITCH && !(UNITY_EDITOR)
        //未接続ならQuaternion.identity
        if (!SwitchManager.GetInstance().IsConnect(index)) return Quaternion.identity;
        //IDの取得
        NpadId npadId = SwitchManager.GetInstance().GetNpadId(index);
        //スタイルの取得
        NpadStyle npadStyle = Npad.GetStyleSet(npadId);
        //ハンドラを取得
        int handleCount = SixAxisSensor.GetHandles(gyroHandles, 1, npadId, npadStyle);
        //ハンドラの数が1じゃない場合はQuaternion.identity
        if (handleCount != 1) return Quaternion.identity;
        //ジャイロスタート
        SixAxisSensor.Start(gyroHandles[0]);
        //状態の取得
        SixAxisSensor.GetState(ref gyroState, gyroHandles[0]);
        gyroState.GetQuaternion(ref refQuaternion);
        quat.Set(refQuaternion.x, refQuaternion.z, refQuaternion.y, -refQuaternion.w);
#endif
        return quat;
    }

    /// <summary>
    /// ジャイロの取得
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>ジャイロの回転</returns>
    static public Quaternion GetGyro(int index)
    {
        //キーがない場合は追加しておく
        if (!baseGyro.ContainsKey(index)) baseGyro.Add(index, Quaternion.identity);
#if UNITY_SWITCH && !(UNITY_EDITOR)
        return baseGyro[index] * GetGyroImpl(index);
#else
        return GetGyroImpl(index);
#endif
    }
}