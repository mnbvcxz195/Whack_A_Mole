using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���, ���� ���, ���� -> ���� �̵�, ���� -> ���� �̵�
public enum MoleState
{
    UnderGround = 0, OnGround, MoveUp, MoveDown
}

public class MoleFSM : MonoBehaviour
{
    [SerializeField] private float waitTimeOnGround;  //���鿡 �ö�ͼ� ����������� ��ٸ��� �ð�
    [SerializeField] private float limitMinY;         //������ �� �ִ� �ּ� y ��ġ
    [SerializeField] private float LimitMaxY;         //�ö�� �� �ִ� �ִ� y ��ġ
    private Movement movement;                        //��, �Ʒ� �̵��� ���� Movement

    //�δ����� ���� ���� (set�� MoleFSM Ŭ���� ���ο�����)
    public MoleState MoleState
    {
        private set;
        get;
    }

    private void Awake()
    {
        movement = GetComponent<Movement>();

        ChangeState(MoleState.UnderGround);
    }

    public void ChangeState(MoleState newState)
    {
        //������ ������ ToString() �޼ҵ带 �̿��� ���ڿ��� ��ȯ�ϸ�
        // "UnderGround"�� ���� ������ ��� �̸� ��ȯ

        //���� ���� ����
        StopCoroutine(MoleState.ToString());
        //���� ����
        MoleState = newState;
        //���ο� ���� ���
        StartCoroutine(MoleState.ToString());
    }

    //�δ����� �ٴڿ��� ����ϴ� ���·� ���� �ٴ� ��ġ�� �δ��� ��ġ ����
    private IEnumerator UnderGround()
    {
        //�̵������� (0, 0, 0) ���� ���·�
        movement.MoveTo(Vector3.zero);
        //�δ����� y��ġ�� Ȧ�� �����ִ� limitMinY ��ġ�� ����
        transform.position = new Vector3(transform.position.x, limitMinY, transform.position.z);

        yield return null;
    }

    //�δ����� Ȧ �ۿ� �ִ� ���·� waitTimeOnGround���� ���
    private IEnumerator OnGround()
    {
        //�̵������� (0, 0, 0) ���� ���·�
        movement.MoveTo(Vector3.zero);
        //�δ����� y������ Ȧ ������ �����ִ� limitMaxY ��ġ�� ����
        transform.position = new Vector3(transform.position.x, LimitMaxY, transform.position.z);
        //waitTimeOnGround �ð� ���� ���
        yield return new WaitForSeconds(waitTimeOnGround);
        //�δ����� ���¸� MoveDown���� ����
        ChangeState(MoleState.MoveDown);
    }

    //�δ����� Ȧ ������ ������ ���� (maxYPosOnGround ��ġ���� ���� �̵�)

    private IEnumerator MoveUp()
    {
        //�̵� ���� : (0, 1, 0) [��]
        movement.MoveTo(Vector3.up);

        while (true)
        {
            //�δ����� y��ġ�� limitMaxY�� �����ϸ� ���� ����
            if(transform.position.y >= LimitMaxY)
            {
                //OnGround ���·� ����
                ChangeState(MoleState.OnGround);
            }

            yield return null;
        }
    }

    //�δ����� Ȧ�� ���� ���� (minYPosUnderGround ��ġ���� �Ʒ��� �̵�)

    private IEnumerator MoveDown()
    {
        //�̵����� : (0, -1, 0) [�Ʒ�]
        movement.MoveTo(Vector3.down);

        while (true)
        {
            //�δ����� y ��ġ�� limitMinY�� �����ϸ� �ݺ��� ����
            if(transform.position.y <= limitMinY)
            {
                //UnderGround ���·� ����
                ChangeState(MoleState.UnderGround);
            }

            yield return null;
        }
    }
}
