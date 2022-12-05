using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0; //이동 속도 선언
    private Vector3 moveDirection = Vector3.zero; //이동 방향 선언
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime; //오브젝트 이동
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction; //이동 방향 설정
    }
}
