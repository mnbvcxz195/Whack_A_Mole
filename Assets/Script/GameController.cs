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
        set => score = Mathf.Max(0, value); //0�� ����� ������ �� �ְ�
        // Mathf.Max() �޼ҵ� float result = Mathf.Max(float a, float b); a�� b �� �� ū ���� ��ȯ
        get => score;
    }

    private void Start()
    {
        countDown.StartCountDown(GameStart);
    }

    private void GameStart()
    {
        moleSpawner.Setup();
    }
}
