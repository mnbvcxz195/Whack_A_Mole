using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private float maxY;                        //망치의 최대 y 위치
    [SerializeField] private float minY;                        //망치의 최소 y 위치
    [SerializeField] private GameObject moleHitEffectPrefab;    //두더지 타격 효과 프리팹
    [SerializeField] private GameController gameController;     //점수 증가를 위한 GameController
    [SerializeField] private ObjectDetector objectDetector;     //마우스 클릭으로 오브젝트 선택을 위한 ObjectDetector
    private Movement movement;                                  //망치 오브젝트 이동을 위한 Movement

    private void Awake()
    {
        movement = GetComponent<Movement>();

        //OnHit 메소드를 ObjectDetector Class의 raycastEvent에 이벤트로 등록
        //ObjectDetector의 raycastEvent.Invoke(hit.transform); 메소드가
        //호출될 때마다 OnHit(Transform target) 메소드가 호출된다.
        objectDetector.raycastEvent.AddListener(OnHit);
    }

    private void OnHit(Transform target)
    {
        if (target.CompareTag("Mole"))
        {
            Debug.Log("공격");
            MoleFSM mole = target.GetComponent<MoleFSM>();

            //두더지가 홀 안에 있을 때는 공격 불가
            if (mole.MoleState == MoleState.UnderGround) return;

            //망치의 위치 설정
            transform.position = new Vector3(target.position.x, minY, target.position.z);

            //망치에 맞았기 때문에 두더지의 상태를 바로 "UnderGround"로 설정
            mole.ChangeState(MoleState.UnderGround);

            //카메라 흔들기
            ShakeCamera.Instance.OnShakeCamera(0.1f, 0.1f);

            //두더지 타격 효과 생성 (Particle의 색상을 두더지 색상과 동일하게 설정)
            GameObject clone = Instantiate(moleHitEffectPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            ParticleSystem.MainModule main = clone.GetComponent<ParticleSystem>().main;
            main.startColor = mole.GetComponent<MeshRenderer>().material.color;

            //점수 증가 (+10)
            gameController.Score += 10;

            //망치를 위로 이동시키는 코루틴 재생
            StartCoroutine("MoveUp");
        }
    }

    private IEnumerator MoveUp()
    {
        //이동방향 ( 0, 1, 0) [위]
        movement.MoveTo(Vector3.up);
        while (true)
        {
            if(transform.position.y >= maxY)
            {
                movement.MoveTo(Vector3.zero);

                break;
            }

            yield return null;
        }
    }
}
