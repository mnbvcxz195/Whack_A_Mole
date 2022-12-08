using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� ���, ���� ���, ���� -> ���� �̵�, ���� -> ���� �̵�
public enum MoleState
{
    UnderGround = 0, OnGround, MoveUp, MoveDown
}

//�δ��� ���� (�⺻, ���� -, �ð� +)
public enum MoleType
{
    Normal = 0, Red, Blue
}

public class MoleFSM : MonoBehaviour
{
    [SerializeField] private GameController gameController;  //�޺� �ʱ�ȭ�� ���� GameController

    [SerializeField] private float waitTimeOnGround;  //���鿡 �ö�ͼ� ����������� ��ٸ��� �ð�

    [SerializeField] private float limitMinY;         //������ �� �ִ� �ּ� y ��ġ

    [SerializeField] private float LimitMaxY;         //�ö�� �� �ִ� �ִ� y ��ġ

    private Movement movement;                        //��, �Ʒ� �̵��� ���� Movement
    [SerializeField]private MeshRenderer[] meshRenderers;//�δ����� ���� ������ ���� MeshRenderer

    private MoleType moleType;                        //�δ����� ����
    private Color defaultColor;                       //�⺻ �δ����� ����
    
    

    //�δ����� ���� ���� (set�� MoleFSM Ŭ���� ���ο�����)
    public MoleState MoleState
    {
        private set;
        get;
    }

    //�δ��� ���� (MoleType�� ���� �δ��� ���� ����)
    public MoleType MoleType
    {
        set
        {
            moleType = value;

            Color color = defaultColor;
            
            switch (moleType)
            {
                case MoleType.Red:
                    color = Color.red;
                    break;
                case MoleType.Blue:
                    color = Color.blue;
                    break;                    
            }

            for (int i = 0; i < meshRenderers.Length; i++)
                meshRenderers[i].material.color = color;
        }
        get => moleType;
    }

    //�δ����� ��ġ�Ǿ� �ִ� ���� (���� ��ܺ��� 0)
    [field:SerializeField] public int MoleIndex { private set; get; }

    private void Awake()
    {
        movement = GetComponent<Movement>();
        //meshRenderer = GetComponentInChildren<MeshRenderer>();

        defaultColor = meshRenderers[0].material.color; //�δ��� ���� ���� ����

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
                //ChangeState(MoleState.UnderGround);
                break;  //while() �Ʒ��� ������ ���� �̵� �Ϸ� �� break;
            }

            yield return null;
        }

        //��ġ�� Ÿ�� ������ �ʰ� �ڿ������� �������� �� �� ȣ��
        //MoveDown -> UnderGround

        //��ġ�� ������ ���ϰ� �������� �� �δ����� �Ӽ��� Normal�̸� �޺� �ʱ�ȭ
        if(moleType == MoleType.Normal)
        {
            gameController.Combo = 0;
        }

        //UnderGround ���·� ����
        ChangeState(MoleState.UnderGround);
    }
}
