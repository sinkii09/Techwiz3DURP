using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float rotationSpeed = 2f;
    public float minYAngle = -45f;
    public float maxYAngle = 45f;
    public bool autoRotate = false;
    public float autoRotateSpeed = 10f;

    private float currentYAngle = 0f;
    private Vector3 currentVelocity;
    private float easeTimer = 0f;
    private Vector3 startPosition;
    private bool isEasing = false;

    private void Start()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Ease in và ease out khi di chuyển camera
        if (isEasing)
        {
            easeTimer += Time.deltaTime;
            float t = easeCurve.Evaluate(easeTimer / smoothTime);
            transform.position = Vector3.Lerp(startPosition, target.position, t);

            if (easeTimer >= smoothTime)
            {
                isEasing = false;
            }
        }
        else if (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            StartEasing();
        }

        // Xoay theo trục Y với giới hạn min và max
        if (!autoRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            currentYAngle += mouseX;
            currentYAngle = Mathf.Clamp(currentYAngle, minYAngle, maxYAngle);
        }
        else
        {
            // Chức năng tự xoay
            currentYAngle += autoRotateSpeed * Time.deltaTime;
            if (currentYAngle > maxYAngle || currentYAngle < minYAngle)
            {
                autoRotateSpeed = -autoRotateSpeed;
            }
            currentYAngle = Mathf.Clamp(currentYAngle, minYAngle, maxYAngle);
        }

        transform.rotation = Quaternion.Euler(0f, currentYAngle, 0f);
    }

    private void StartEasing()
    {
        isEasing = true;
        easeTimer = 0f;
        startPosition = transform.position;
    }

    // Phương thức để bật/tắt chế độ tự xoay
    public void ToggleAutoRotate()
    {
        autoRotate = !autoRotate;
    }
}