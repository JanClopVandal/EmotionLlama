using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivityX = 100f; // Чувствительность по оси X
    public float sensitivityY = 100f; // Чувствительность по оси Y

    private float rotationX = 0f; // Текущий угол вращения по X
    private float rotationY = 0f; // Текущий угол вращения по Y

    void Start()
    {
        // Скрываем и блокируем курсор мыши в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Получаем движение мыши
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

        // Обновляем углы вращения
        rotationX -= mouseY;
        rotationY += mouseX;

        // Ограничиваем вращение по X (вверх-вниз) для предотвращения переворота камеры
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Применяем вращение к камере
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
