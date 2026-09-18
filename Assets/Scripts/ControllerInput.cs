using UnityEngine;

public static class ControllerInput
{
    public const float Threshold = 0.5f;
    public const float DPadThreshold = 0.5f;

    // ===== Внутреннее состояние =====
    private static float lastLT = 0f;
    private static float lastRT = 0f;

    private static bool ltDown, ltUp;
    private static bool rtDown, rtUp;

    private static float lastDpadX = 0f;
    private static float lastDpadY = 0f;
    private static bool dpadLeftDown, dpadRightDown, dpadUpDown, dpadDownDown;

    // ⚠️ ОБЯЗАТЕЛЬНО вызывать один раз в Update() ДО всех проверок
    public static void Update()
    {
        // ===== Курки =====
        float lt = Input.GetAxis("Left Trigger");
        float rt = Input.GetAxis("Right Trigger");

        ltDown = lt > Threshold && lastLT <= Threshold;
        ltUp = lt <= Threshold && lastLT > Threshold;

        rtDown = rt > Threshold && lastRT <= Threshold;
        rtUp = rt <= Threshold && lastRT > Threshold;

        lastLT = lt;
        lastRT = rt;

        // ===== D-Pad (оси Horizontal/Vertical или отдельные оси) =====
        // Если D-Pad забиндён на отдельные оси "DPad X" / "DPad Y" — используйте их.
        // Здесь — вариант через те же "Horizontal"/"Vertical" (если они у вас настроены на геймпад).
        float dpadX = Input.GetAxis("DPad X");
        float dpadY = Input.GetAxis("DPad Y");

        bool nowLeft = dpadX < -DPadThreshold;
        bool nowRight = dpadX > DPadThreshold;
        bool nowUp = dpadY > DPadThreshold;
        bool nowDown = dpadY < -DPadThreshold;

        bool wasLeft = lastDpadX < -DPadThreshold;
        bool wasRight = lastDpadX > DPadThreshold;
        bool wasUp = lastDpadY > DPadThreshold;
        bool wasDown = lastDpadY < -DPadThreshold;

        dpadLeftDown = nowLeft && !wasLeft;
        dpadRightDown = nowRight && !wasRight;
        dpadUpDown = nowUp && !wasUp;
        dpadDownDown = nowDown && !wasDown;

        lastDpadX = dpadX;
        lastDpadY = dpadY;
    }

    // ============================================================
    // ====================== КУРКИ (LT / RT) ====================
    // ============================================================

    public static bool GetLeftTriggerDown() => ltDown;
    public static bool GetLeftTriggerUp() => ltUp;
    public static bool GetLeftTrigger() => Input.GetAxis("Left Trigger") > Threshold;
    public static float GetLeftTriggerValue() => Input.GetAxis("Left Trigger");

    public static bool GetRightTriggerDown() => rtDown;
    public static bool GetRightTriggerUp() => rtUp;
    public static bool GetRightTrigger() => Input.GetAxis("Right Trigger") > Threshold;
    public static float GetRightTriggerValue() => Input.GetAxis("Right Trigger");

    // ============================================================
    // ======================== КНОПКИ (Xbox) ====================
    // ============================================================
    // A = 0, B = 1, X = 2, Y = 3
    // LB = 4, RB = 5
    // Back = 6, Start = 7
    // L3 = 8, R3 = 9

    // --- A ---
    public static bool A() => Input.GetKey(KeyCode.JoystickButton0);
    public static bool ADown() => Input.GetKeyDown(KeyCode.JoystickButton0);
    public static bool AUp() => Input.GetKeyUp(KeyCode.JoystickButton0);

    // --- B ---
    public static bool B() => Input.GetKey(KeyCode.JoystickButton1);
    public static bool BDown() => Input.GetKeyDown(KeyCode.JoystickButton1);
    public static bool BUp() => Input.GetKeyUp(KeyCode.JoystickButton1);

    // --- X ---
    public static bool X() => Input.GetKey(KeyCode.JoystickButton2);
    public static bool XDown() => Input.GetKeyDown(KeyCode.JoystickButton2);
    public static bool XUp() => Input.GetKeyUp(KeyCode.JoystickButton2);

    // --- Y ---
    public static bool Y() => Input.GetKey(KeyCode.JoystickButton3);
    public static bool YDown() => Input.GetKeyDown(KeyCode.JoystickButton3);
    public static bool YUp() => Input.GetKeyUp(KeyCode.JoystickButton3);

    // --- LB (Left Bumper) ---
    public static bool LB() => Input.GetKey(KeyCode.JoystickButton4);
    public static bool LBDown() => Input.GetKeyDown(KeyCode.JoystickButton4);
    public static bool LBUp() => Input.GetKeyUp(KeyCode.JoystickButton4);

    // --- RB (Right Bumper) ---
    public static bool RB() => Input.GetKey(KeyCode.JoystickButton5);
    public static bool RBDown() => Input.GetKeyDown(KeyCode.JoystickButton5);
    public static bool RBUp() => Input.GetKeyUp(KeyCode.JoystickButton5);

    // --- Back / View ---
    public static bool Back() => Input.GetKey(KeyCode.JoystickButton6);
    public static bool BackDown() => Input.GetKeyDown(KeyCode.JoystickButton6);
    public static bool BackUp() => Input.GetKeyUp(KeyCode.JoystickButton6);

    // --- Start / Menu ---
    public static bool Start() => Input.GetKey(KeyCode.JoystickButton7);
    public static bool StartDown() => Input.GetKeyDown(KeyCode.JoystickButton7);
    public static bool StartUp() => Input.GetKeyUp(KeyCode.JoystickButton7);

    // --- L3 (нажатие левого стика) ---
    public static bool L3() => Input.GetKey(KeyCode.JoystickButton8);
    public static bool L3Down() => Input.GetKeyDown(KeyCode.JoystickButton8);
    public static bool L3Up() => Input.GetKeyUp(KeyCode.JoystickButton8);

    // --- R3 (нажатие правого стика) ---
    public static bool R3() => Input.GetKey(KeyCode.JoystickButton9);
    public static bool R3Down() => Input.GetKeyDown(KeyCode.JoystickButton9);
    public static bool R3Up() => Input.GetKeyUp(KeyCode.JoystickButton9);

    // ============================================================
    // ======================== D-PAD ============================
    // ============================================================

    public static bool DPadLeft() => Input.GetAxis("DPad X") < -DPadThreshold;
    public static bool DPadRight() => Input.GetAxis("DPad X") > DPadThreshold;
    public static bool DPadUp() => Input.GetAxis("DPad Y") > DPadThreshold;
    public static bool DPadDown() => Input.GetAxis("DPad Y") < -DPadThreshold;

    public static bool DPadLeftDown() => dpadLeftDown;
    public static bool DPadRightDown() => dpadRightDown;
    public static bool DPadUpDown() => dpadUpDown;
    public static bool DPadDownDown() => dpadDownDown;
}