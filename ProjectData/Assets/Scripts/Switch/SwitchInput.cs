﻿using UnityEngine;
#if UNITY_SWITCH  && !(UNITY_EDITOR)
using nn.hid;
#endif

/// <summary>
/// スイッチの入力情報
/// </summary>
static public class SwitchInput
{
    const float DeadZone = 0.4f;
    //1フレーム前のボタンの状態
    static long[] prevButtons;
    //現在のボタンの状態
    static long[] currentButtons;
#if UNITY_SWITCH && !(UNITY_EDITOR)
    //スティックの情報
    struct StickInfo
    {
        public float horizontal, vertical;
    }
    //スティックの水平
    static StickInfo[] stickInfos;
    //コントローラーの状態
    static NpadState npadState = new NpadState();
#endif
    /// <summary>
    /// 入力の初期化
    /// </summary>
    /// <param name="npadIdsLength">使用するIDの配列の長さ</param>
    static public void InputInit(int npadIdsLength)
    {
        //配列の要素確保
        prevButtons = new long[npadIdsLength];
        currentButtons = new long[npadIdsLength];
#if UNITY_SWITCH && !(UNITY_EDITOR)
        stickInfos = new StickInfo[npadIdsLength];
#endif
#if UNITY_EDITOR
        xboxCurrentButtons = new bool[npadIdsLength, (int)XboxInput.None];
        xboxPrevButtons = new bool[npadIdsLength, (int)XboxInput.None];
#endif
    }

    /// <summary>
    /// 入力の更新
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="npadId">パッドのID</param>
    static public void InputUpdate(int index
#if UNITY_SWITCH && !(UNITY_EDITOR)
, NpadId npadId
#endif
)
    {
#if UNITY_EDITOR || !(UNITY_SWITCH)
        for (int i = 0; i < (int)XboxInput.None; ++i)
        {
            xboxPrevButtons[index, i] = xboxCurrentButtons[index, i];
            xboxCurrentButtons[index, i] = InputXboxButton(index, (XboxInput)i);
        }
#endif
        prevButtons[index] = currentButtons[index];
        //未接続
        if (!SwitchManager.GetInstance().IsConnect(index)) return;

#if UNITY_SWITCH && !(UNITY_EDITOR)
        //スタイルを取得
        NpadStyle npadStyle = Npad.GetStyleSet(npadId);
        //スタイルが合うかどうか
        if ((npadStyle & SwitchManager.GetInstance().GetNpadStyle()) == 0) return;
        //入力情報を取得
        Npad.GetState(ref npadState, npadId, npadStyle);
        npadState.buttons &= ~(NpadButton.StickLUp | NpadButton.StickRUp |
                                NpadButton.StickLDown | NpadButton.StickRDown |
                                NpadButton.StickLRight | NpadButton.StickRRight |
                                NpadButton.StickLLeft | NpadButton.StickRLeft);
        //スティックの更新
        //右のジョイスティック
        if (npadStyle == NpadStyle.JoyRight)
        {
            //デッドゾーンを超えているかどうか
            if (Mathf.Abs(npadState.analogStickR.fx) > DeadZone)
            {
                stickInfos[index].horizontal = npadState.analogStickR.fx;
                npadState.buttons |= (npadState.analogStickR.fx > 0) ? NpadButton.StickLRight : NpadButton.StickLLeft;
            }
            else
            {
                stickInfos[index].horizontal = 0.0f;
            }
            //デッドゾーンを超えているかどうか
            if (Mathf.Abs(npadState.analogStickR.fy) > DeadZone)
            {
                stickInfos[index].vertical = npadState.analogStickR.fy;
                npadState.buttons |= (npadState.analogStickR.fy > 0) ? NpadButton.StickLUp : NpadButton.StickLDown;
            }
            else
            {
                stickInfos[index].vertical = 0.0f;
            }
        }
        //左のジョイスティック
        else
        {
            //デッドゾーンを超えているかどうか
            if (Mathf.Abs(npadState.analogStickL.fx) > DeadZone)
            {
                stickInfos[index].horizontal = npadState.analogStickL.fx;
                npadState.buttons |= (npadState.analogStickL.fx > 0) ? NpadButton.StickLRight : NpadButton.StickLLeft;
            }
            else
            {
                stickInfos[index].horizontal = 0.0f;
            }
            //デッドゾーンを超えているかどうか
            if (Mathf.Abs(npadState.analogStickL.fy) > DeadZone)
            {
                stickInfos[index].vertical = npadState.analogStickL.fy;
                npadState.buttons |= (npadState.analogStickL.fy > 0) ? NpadButton.StickLUp : NpadButton.StickLDown;
            }
            else
            {
                stickInfos[index].vertical = 0.0f;
            }
        }
        currentButtons[index] = (long)npadState.buttons;
#endif
    }

    /// <summary>
    /// ボタンを今のフレームに押したか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押したならtrueを返す</returns>
    static public bool GetButtonDown(int index, SwitchButton button)
    {
        //未接続ならfalse
        if (!SwitchManager.GetInstance().IsConnect(index)) return false;
        return !IsPrevButton(index, button) && IsCurrentButton(index, button);
    }

    /// <summary>
    /// 今のフレームにボタンを押しているか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押しているならtrueを返す</returns>
    static public bool GetButton(int index, SwitchButton button)
    {
        //未接続ならfalse
        if (!SwitchManager.GetInstance().IsConnect(index)) return false;
        return IsCurrentButton(index, button);
    }

    /// <summary>
    /// 今のフレームにボタンを離したか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>離したならtrueを返す</returns>
    static public bool GetButtonUp(int index, SwitchButton button)
    {
        //未接続ならfalse
        if (!SwitchManager.GetInstance().IsConnect(index)) return false;
        return IsPrevButton(index, button) && !IsCurrentButton(index, button);
    }

    /// <summary>
    /// スティックの水平を取得
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>スティックの垂直</returns>
    static public float GetHorizontal(int index)
    {
        //未接続なら0.0f
        if (!SwitchManager.GetInstance().IsConnect(index)) return 0.0f;
#if UNITY_SWITCH && !(UNITY_EDITOR)
        return stickInfos[index].horizontal;
#else
        return ConvertSwitchHorizontalToXboxHorizontal(index);
#endif
    }

    /// <summary>
    /// スティックの垂直を取得
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>スティックの垂直</returns>
    static public float GetVertical(int index)
    {
        //未接続なら0.0f
        if (!SwitchManager.GetInstance().IsConnect(index)) return 0.0f;
#if UNITY_SWITCH && !(UNITY_EDITOR)
        return stickInfos[index].vertical;
#else
        return ConvertSwitchVerticalToXboxVertical(index);
#endif
    }

    /// <summary>
    /// 1フレーム前にボタンを押していたか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押しているならtrueを返す</returns>
    static bool IsPrevButton(int index, SwitchButton button)
    {
#if UNITY_SWITCH && !(UNITY_EDITOR)
        return (prevButtons[index] & (long)button) != 0;
#else
        return xboxPrevButtons[index, (int)SwitchConvertXbox(button)];
#endif
    }

    /// <summary>
    /// 今のフレームにボタンを押しているか
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">取得するボタン</param>
    /// <returns>押しているならtrueを返す</returns>
    static bool IsCurrentButton(int index, SwitchButton button)
    {
#if UNITY_SWITCH && !(UNITY_EDITOR)
        return (currentButtons[index] & (long)button) != 0;
#else
        return xboxCurrentButtons[index, (int)SwitchConvertXbox(button)];
#endif
    }

#if UNITY_EDITOR || !(UNITY_SWITCH)

    static bool[,] xboxCurrentButtons;
    static bool[,] xboxPrevButtons;
    enum XboxInput
    {
        Up, Down, Right, Left, SR, SL, StickUp, StickDown, StickRight, StickLeft, Pause, Stick, None
    }

    /// <summary>
    /// Switchの入力からXboxの入力にコンバートする
    /// </summary>
    /// <param name="button">Switchの入力</param>
    /// <returns>Xboxの入力</returns>
    static XboxInput SwitchConvertXbox(SwitchButton button)
    {
        switch (button)
        {
            case SwitchButton.Up:
                return XboxInput.Up;
            case SwitchButton.Down:
                return XboxInput.Down;
            case SwitchButton.Right:
                return XboxInput.Right;
            case SwitchButton.Left:
                return XboxInput.Left;
            case SwitchButton.SR:
                return XboxInput.SR;
            case SwitchButton.SL:
                return XboxInput.SL;
            case SwitchButton.StickUp:
                return XboxInput.StickUp;
            case SwitchButton.StickDown:
                return XboxInput.StickDown;
            case SwitchButton.StickRight:
                return XboxInput.StickRight;
            case SwitchButton.StickLeft:
                return XboxInput.StickLeft;
            case SwitchButton.Pause:
                return XboxInput.Pause;
            case SwitchButton.Stick:
                return XboxInput.Stick;
            default:
                return XboxInput.None;
        }
    }

    /// <summary>
    /// XBoxの入力
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <param name="button">ボタン</param>
    /// <returns>入力されているかどうか</returns>
    static bool InputXboxButton(int index, XboxInput button)
    {
        const int AddNum = KeyCode.Joystick2Button0 - KeyCode.Joystick1Button0;
        switch (button)
        {
            case XboxInput.Up:
                return Input.GetKey(KeyCode.Joystick1Button3 + index * AddNum) ||
                (index == 0 && Input.GetKey(KeyCode.UpArrow));
            case XboxInput.Down:
                return Input.GetKey(KeyCode.Joystick1Button0 + index * AddNum) ||
                (index == 0 && Input.GetKey(KeyCode.DownArrow));
            case XboxInput.Right:
                return Input.GetKey(KeyCode.Joystick1Button1 + index * AddNum) ||
                (index == 0 && Input.GetKey(KeyCode.RightArrow));
            case XboxInput.Left:
                return Input.GetKey(KeyCode.Joystick1Button2 + index * AddNum) ||
                (index == 0 && Input.GetKey(KeyCode.LeftArrow));
            case XboxInput.SR:
                return Input.GetKey(KeyCode.Joystick1Button5 + index * AddNum) ||
                (index == 0 && Input.GetKey(KeyCode.E));
            case XboxInput.SL:
                return Input.GetKey(KeyCode.Joystick1Button4 + index * AddNum) ||
                (index == 0 && Input.GetKey(KeyCode.Q));
            case XboxInput.StickUp:
                return Input.GetAxisRaw("Vertical" + (index + 1).ToString()) > DeadZone ||
                (index == 0 && Input.GetKey(KeyCode.W));
            case XboxInput.StickDown:
                return Input.GetAxisRaw("Vertical" + (index + 1).ToString()) < -DeadZone ||
                (index == 0 && Input.GetKey(KeyCode.S));
            case XboxInput.StickRight:
                return Input.GetAxisRaw("Horizontal" + (index + 1).ToString()) > DeadZone ||
                (index == 0 && Input.GetKey(KeyCode.D));
            case XboxInput.StickLeft:
                return Input.GetAxisRaw("Horizontal" + (index + 1).ToString()) < -DeadZone ||
                (index == 0 && Input.GetKey(KeyCode.A));
            case XboxInput.Pause:
                return Input.GetKey(KeyCode.Joystick1Button7 + index * AddNum) ||
                (index == 0 && Input.GetKey(KeyCode.Alpha1));
            case XboxInput.Stick:
                return Input.GetKey(KeyCode.Joystick1Button8 + index * AddNum) ||
                 Input.GetKey(KeyCode.Joystick1Button9 + index * AddNum) ||
                 (index == 0 && Input.GetKey(KeyCode.Space));
            default:
                return false;
        }
    }

    /// <summary>
    /// スティックの水平入力のコンバーター
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>スティックの水平入力</returns>
    static float ConvertSwitchHorizontalToXboxHorizontal(int index)
    {
        float stick = Input.GetAxisRaw("Horizontal" + (index + 1).ToString());
        if (stick != 0) return stick;
        //同時押しは0
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) return 0;
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) return 1;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) return Mathf.Sqrt(0.5f);
            return 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) return -1;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) return -Mathf.Sqrt(0.5f);
            return -1;
        }
        return 0;
    }

    /// <summary>
    /// スティックの垂直入力のコンバーター
    /// </summary>
    /// <param name="index">コントローラーの番号</param>
    /// <returns>スティックの垂直入力</returns>
    static float ConvertSwitchVerticalToXboxVertical(int index)
    {
        float stick = Input.GetAxisRaw("Vertical" + (index + 1).ToString());
        if (stick != 0) return stick;
        //同時押しは0
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) return 0;
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) return 1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) return Mathf.Sqrt(0.5f);
            return 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) return -1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) return -Mathf.Sqrt(0.5f);
            return -1;
        }
        return 0;
    }
#endif
}
/// <summary>
/// Switchでジョイコンを横持ちにした場合の入力
/// </summary>
public enum SwitchButton : long
{
#if UNITY_SWITCH && !(UNITY_EDITOR)
    Up = NpadButton.X |  NpadButton.Up,
    Down = NpadButton.B | NpadButton.Down,
    Right = NpadButton.A | NpadButton.Right,
    Left =  NpadButton.Y| NpadButton.Left,
    SR = NpadButton.RightSR | NpadButton.LeftSR,
    SL = NpadButton.RightSL | NpadButton.LeftSL,
    Trigger = NpadButton.R | NpadButton.L,
    ZTrigger = NpadButton.ZR | NpadButton.ZL,
    StickUp = NpadButton.StickRUp | NpadButton.StickLUp,
    StickDown = NpadButton.StickRDown |NpadButton.StickLDown,
    StickRight = NpadButton.StickRRight | NpadButton.StickLRight,
    StickLeft = NpadButton.StickRLeft | NpadButton.StickLLeft,
    Pause = NpadButton.Plus | NpadButton.Minus,
    Stick = NpadButton.StickR | NpadButton.StickL,
    Ok = SwitchButton.Right,
    Cancel = SwitchButton.Down,
#else
    Up = 0x1 << 0,
    Down = 0x1 << 1,
    Right = 0x1 << 2,
    Left = 0x1 << 3,
    SR = 0x1 << 4,
    SL = 0x1 << 5,
    StickUp = 0x1 << 6,
    StickDown = 0x1 << 7,
    StickRight = 0x1 << 8,
    StickLeft = 0x1 << 9,
    Pause = 0x1 << 10,
    Stick = 0x1 << 11,
    Ok = SwitchButton.Right,
    Cancel = SwitchButton.Down,
    Jump = SwitchButton.Down,
    Bomb = SwitchButton.Left,
    Horn = SwitchButton.Down,
#endif
    None = 0
}