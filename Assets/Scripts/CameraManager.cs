using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 targetPosition;
    public float smoothSpeed = 0.125f;

    private Vector3 velocity = Vector3.zero;
    private void LateUpdate()
    {
        Vector3 desiredPosition = transform.position;
        desiredPosition.y = targetPosition.y;

        if (targetPosition.y < 0)
        {
            desiredPosition.y = 0;
        }

        else
        {
            desiredPosition.y = targetPosition.y;
        }

        transform.position = desiredPosition;

    }

    public void SetTarget(Vector3 newPosition)
    {
        // 받은 위치 값을 카메라 타겟값으로 변경
        targetPosition = newPosition;
    }
}

