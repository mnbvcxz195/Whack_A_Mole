using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{
    [SerializeField] private MoleFSM[] moles;  //맵에 존재하는 두더지들
    [SerializeField] private float spawnTime;  //두더지 등장 주기

    //두더지 등장 확률 (Normal : 75%, Red : 15%, Blue : 10%
    private int[] spawnPercents = new int[3] { 75, 15, 10 };

    //public void Start()
    public void Setup() // 카운트 다운 후 두더지를 생성해야 함으로 Start가 아닌 Setup 함수 생성
    {
        StartCoroutine("SpawnMole");
    }

    private IEnumerator SpawnMole()
    {
        while (true)
        {
            // 0 ~ Moles.Length - 1 중 임의의 숫자 선택
            int index = Random.Range(0, moles.Length);

            //선택된 두더지 속성 설정
            moles[index].MoleType = SpawnMoleType();

            //index번째 두더지의 상태를 "MoveUp"으로 변경
            moles[index].ChangeState(MoleState.MoveUp);

            //spqwnTime 시간동안 대기
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
