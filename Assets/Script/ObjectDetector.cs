using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectDetector : MonoBehaviour
{
    [System.Serializable]
    public class RaycastEvent : UnityEvent<Transform> { }    //�̺�Ʈ Ŭ���� ����
                                                             //��ϵǴ� �̺�Ʈ �޼ҵ�� Transform �Ű����� 1���� ������ 

    [HideInInspector]
    public RaycastEvent raycastEvent = new RaycastEvent();   //�̺�Ʈ Ŭ���� �ν��Ͻ� ���� �� �޸� �Ҵ�

    private Camera mainCamera;                               //������ �����ϱ� ���� Camera
    private Ray ray;                                         //������ ���� ���� ������ ���� Ray
    private RaycastHit hit;                                  //������ �ε��� ������Ʈ ���� ������ ���� RayCastHit;
    private void Awake()
    {
        //"MainCamera" �±׸� ������ �ִ� ������Ʈ Ž�� �� Camera ������Ʈ ���� ����
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //���콺 ���� ��ư ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            //ī�޶� ��ġ���� ȭ���� ���콺 ��ġ�� �����ϴ� ���� ����
            //ray.origin : ������ ������ġ ( = ī�޶� ��ġ)
            //ray.direction : ������ ���� ����
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //2D ����͸� ���� 3D ������ ������Ʈ�� ���콺�� �����ϴ� ���
            //������ �ε����� ������Ʈ�� �����ؼ� hit�� ����
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //�ε��� ������Ʈ�� Transform ������ �Ű������� �̺�Ʈ ȣ��
                raycastEvent.Invoke(hit.transform);

            }
        }
    }
}
