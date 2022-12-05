using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CountDown countDown;
    [SerializeField] private MoleSpawner moleSpawner;
    private int score;

    public int Score
    {
        set => score = Mathf.Max(0, value); //0과 양수만 저장할 수 있게
        // Mathf.Max() 메소드 float result = Mathf.Max(float a, float b); a와 b 중 더 큰 값을 반환
        get => score;
    }

    [field:SerializeField] public float MaxTime { private set; get; }
    public float CurrentTime { private set; get; }

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
