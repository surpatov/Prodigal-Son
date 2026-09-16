using UnityEngine;

public class CameraCar : MonoBehaviour
{
    [Header("Ограничения углов")]
    [SerializeField] private Vector2 maxXAngle = new Vector2(-35f, 35f); // Вверх/вниз
    [SerializeField] private Vector2 maxYAngle = new Vector2(-15f, 20f); // Влево/вправо
    [SerializeField] private float multiplier = 0.1f; // Множитель для чувствительности мыши

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
        // Получаем ввод с учетом ваших настроек
        float mouseX = Input.GetAxis("Mouse X") * GameSettings.MouseSensitivity * (GameSettings.IsXVert ? -1 : 1);
        float mouseY = Input.GetAxis("Mouse Y") * GameSettings.MouseSensitivity * (GameSettings.IsYVert ? -1 : 1);

        // Накапливаем вращение по обеим осям
        yRotation += mouseX * multiplier;
        xRotation -= mouseY * multiplier;

        // Ограничиваем углы
        xRotation = Mathf.Clamp(xRotation, maxYAngle.x, maxYAngle.y);
        yRotation = Mathf.Clamp(yRotation, maxXAngle.x, maxXAngle.y);

        // Применяем вращение к текущему объекту (камере)
        transform.localEulerAngles = new Vector3(xRotation, yRotation, 0f);
    }
}