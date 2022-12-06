using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private float maxY;                        //��ġ�� �ִ� y ��ġ
    [SerializeField] private float minY;                        //��ġ�� �ּ� y ��ġ
    [SerializeField] private GameObject moleHitEffectPrefab;    //�δ��� Ÿ�� ȿ�� ������
    [SerializeField] private AudioClip[] audioClips;              //�δ��� Ÿ�� �� ����
    [SerializeField] private GameController gameController;     //���� ������ ���� GameController
    [SerializeField] private ObjectDetector objectDetector;     //���콺 Ŭ������ ������Ʈ ������ ���� ObjectDetector
    private Movement movement;                                  //��ġ ������Ʈ �̵��� ���� Movement
    private AudioSource audioSource;                            //�δ��� Ÿ�� �� ���� ����ϴ� AudioSource

    private void Awake()
    {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();

        //OnHit �޼ҵ带 ObjectDetector Class�� raycastEvent�� �̺�Ʈ�� ���
        //ObjectDetector�� raycastEvent.Invoke(hit.transform); �޼ҵ尡
        //ȣ��� ������ OnHit(Transform target) �޼ҵ尡 ȣ��ȴ�.
        objectDetector.raycastEvent.AddListener(OnHit);
    }

    private void OnHit(Transform target)
    {
        if (target.CompareTag("Mole"))
        {
            Debug.Log("����");
            MoleFSM mole = target.GetComponent<MoleFSM>();

            //�δ����� Ȧ �ȿ� ���� ���� ���� �Ұ�
            if (mole.MoleState == MoleState.UnderGround) return;

            //��ġ�� ��ġ ����
            transform.position = new Vector3(target.position.x, minY, target.position.z);

            //��ġ�� �¾ұ� ������ �δ����� ���¸� �ٷ� "UnderGround"�� ����
            mole.ChangeState(MoleState.UnderGround);

            //ī�޶� ����
            ShakeCamera.Instance.OnShakeCamera(0.1f, 0.1f);

            //�δ��� Ÿ�� ȿ�� ���� (Particle�� ������ �δ��� ����� �����ϰ� ����)
            GameObject clone = Instantiate(moleHitEffectPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            ParticleSystem.MainModule main = clone.GetComponent<ParticleSystem>().main;
            main.startColor = mole.GetComponentInChildren<MeshRenderer>().material.color;

            //���� ���� (+10)
            //gameController.Score += 10;
            //�δ��� ���� ���� ó�� (����, �ð�, ���� ���)
            MoleHitProcess(mole);

            //��ġ�� ���� �̵���Ű�� �ڷ�ƾ ���
            StartCoroutine("MoveUp");
        }
    }

    private IEnumerator MoveUp()
    {
        //�̵����� ( 0, 1, 0) [��]
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

    private void MoleHitProcess(MoleFSM mole)
    {
        if(mole.MoleType == MoleType.Normal)
        {
            gameController.Score += 10;
        }

        else if(mole.MoleType == MoleType.Red)
        {
            gameController.Score -= 50;
        }

        else if(mole.MoleType == MoleType.Blue)
        {
            gameController.Score += 30;
            gameController.CurrentTime += 5;
        }

        //���� ��� (Normal = 0, Red = 1, Blue = 2)
        PlaySound((int)mole.MoleType);
    }

    private void PlaySound(int index)
    {
        audioSource.Stop();
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
