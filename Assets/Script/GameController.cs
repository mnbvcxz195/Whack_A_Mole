using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CountDown countDown;
    [SerializeField] private MoleSpawner moleSpawner;
    private int score;
    private int combo;
    private float currentTime;

    public int Score
    {
        set => score = Mathf.Max(0, value); //0�� ����� ������ �� �ְ�
        // Mathf.Max() �޼ҵ� float result = Mathf.Max(float a, float b); a�� b �� �� ū ���� ��ȯ
        get => score;
    }

    public int Combo
    {
        set => combo = Mathf.Max(0, value);
        get => combo;
    }

    [field:SerializeField] public float MaxTime { private set; get; }
    public float CurrentTime
    {
        set => currentTime = Mathf.Clamp(value, 0, MaxTime);
        get => currentTime;
    }

    private void Start()
    {
        countDown.StartCountDown(GameStart);
    }

    private void GameStart()
    {
        moleSpawner.Setup();

        StartCoroutine("OnTimeCount");
    }

    private IEnumerator OnTimeCount()
    {
        CurrentTime = MaxTime;

        while (CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;

            yield return null;
        }
    }
}
