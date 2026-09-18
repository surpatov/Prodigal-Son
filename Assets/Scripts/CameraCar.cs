using UnityEngine;

public class CameraCar : MonoBehaviour
{
    [Header("Ограничения углов")]
    [SerializeField] private Vector2 maxXAngle = new Vector2(-35f, 35f); // Вверх/вниз
    [SerializeField] private Vector2 maxYAngle = new Vector2(-15f, 20f); // Влево/вправо
    [SerializeField] private float multiplier = 0.1f; // Множитель для чувствительности мыши
    [Header("Gamepad Settings")]
    [SerializeField] private float gamepadSensitivity = 20f;   // чувствительность стика
    [SerializeField] private float gamepadDeadzone = 0.15f;     // мёртвая зона
    [SerializeField] private string gamepadXAxis = "Right Stick X";
    [SerializeField] private string gamepadYAxis = "Right Stick Y";

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Start()
    {
        // Запоминаем начальные углы, чтобы камера не прыгнула
        Vector3 startAngles = transform.localEulerAngles;
        xRotation = startAngles.x;
        yRotation = startAngles.y;

        GameSettings.GetSettings(); // Загружаем настройки игры при старте

        Cursor.lockState = CursorLockMode.Locked; // Блокируем курсор в центре экрана
        Cursor.visible = false; // Делаем курсор невидимым
    }

    private void Update()
    {
        // === Мышь ===
        float mouseX = Input.GetAxis("Mouse X") * GameSettings.MouseSensitivity * (GameSettings.IsXVert ? -1 : 1);
        float mouseY = Input.GetAxis("Mouse Y") * GameSettings.MouseSensitivity * (GameSettings.IsYVert ? -1 : 1);

        // === Геймпад ===
        float gamepadX = Input.GetAxis(gamepadXAxis);
        float gamepadY = Input.GetAxis(gamepadYAxis) * -1; // инверсия Y стика

        // Мёртвая зона
        gamepadX = ApplyDeadzone(gamepadX, gamepadDeadzone);
        gamepadY = ApplyDeadzone(gamepadY, gamepadDeadzone);

        // Умножаем на deltaTime (стик даёт абсолютное значение, а не дельту как мышь)
        gamepadX *= gamepadSensitivity * GameSettings.MouseSensitivity * Time.deltaTime * (GameSettings.IsXVert ? -1 : 1);
        gamepadY *= gamepadSensitivity * GameSettings.MouseSensitivity * Time.deltaTime * (GameSettings.IsYVert ? -1 : 1);

        // === Суммируем ===
        float totalX = mouseX + gamepadX;
        float totalY = mouseY + gamepadY;

        // Накапливаем вращение
        yRotation += totalX * multiplier;
        xRotation -= totalY * multiplier;

        // Ограничиваем углы
        xRotation = Mathf.Clamp(xRotation, maxYAngle.x, maxYAngle.y);
        yRotation = Mathf.Clamp(yRotation, maxXAngle.x, maxXAngle.y);

        // Применяем
        transform.localEulerAngles = new Vector3(xRotation, yRotation, 0f);
    }

    private float ApplyDeadzone(float value, float deadzone)
    {
        if (Mathf.Abs(value) < deadzone) return 0f;
        float sign = Mathf.Sign(value);
        return sign * ((Mathf.Abs(value) - deadzone) / (1f - deadzone));
    }
}