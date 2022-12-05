using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    [SerializeField] private MoleFSM[] moles;  //�ʿ� �����ϴ� �δ�����
    [SerializeField] private float spawnTime;  //�δ��� ���� �ֱ�

    //�δ��� ���� Ȯ�� (Normal : 75%, Red : 15%, Blue : 10%
    private int[] spawnPercents = new int[3] { 75, 15, 10 };

    //public void Start()
    public void Setup() // ī��Ʈ �ٿ� �� �δ����� �����ؾ� ������ Start�� �ƴ� Setup �Լ� ����
    {
        StartCoroutine("SpawnMole");
    }

    private IEnumerator SpawnMole()
    {
        while (true)
        {
            // 0 ~ Moles.Length - 1 �� ������ ���� ����
            int index = Random.Range(0, moles.Length);

            //���õ� �δ��� �Ӽ� ����
            moles[index].MoleType = SpawnMoleType();

            //index��° �δ����� ���¸� "MoveUp"���� ����
            moles[index].ChangeState(MoleState.MoveUp);

            //spqwnTime �ð����� ���
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private MoleType SpawnMoleType()
    {
        int percent = Random.Range(0, 100);
        float cumulative = 0;

        for(int i = 0; i < spawnPercents.Length; ++i)
        {
            cumulative += spawnPercents[i];

            if(percent < cumulative)
            {
                return (MoleType)i;
            }
        }

        return MoleType.Normal;
    }

}
