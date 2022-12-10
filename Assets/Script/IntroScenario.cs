using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScenario : MonoBehaviour
{
    [SerializeField] private Movement[] movementsMoles;       //�δ������� ���� �̵���Ű�� ���� Movement
    [SerializeField] private GameObject[] textMoles;          //�δ��� ���� ���� ȹ�� ������ ȿ�� ��� Text
    [SerializeField] private GameObject textPressAnyKey;      //"Press Any Key ��� Test
    [SerializeField] private int maxY = 3;               //�δ����� �ö�� �� �ִ� �ִ� ����
    private int currentIndex = 0;                             //�δ����� ������� �����ϵ��� ������ ����

    private void Awake()
    {
        StartCoroutine("Scenario");
    }

    private IEnumerator Scenario()
    {
        //�δ����� Normal -> Red -> Blue ������� ����
        while(currentIndex < movementsMoles.Length)
        {
            yield return StartCoroutine("MoveMole");
        }

        //"Press Any Key" �ؽ�Ʈ ���
        textPressAnyKey.SetActive(true);

        //���콺 ���� ��ư Ŭ�� �� "Game"������ �̵�
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Game");
            }

            yield return null;
        }
    }

    private IEnumerator MoveMole()
    {
        movementsMoles[currentIndex].MoveTo(Vector3.up);

        while (true)
        {
            //�δ����� ��ǥ������ �����ϸ� while() �ݺ��� ����
            if(movementsMoles[currentIndex].transform.position.y >= maxY)
            {
                movementsMoles[currentIndex].MoveTo(Vector3.zero);
                break;
            }

            yield return null;
        }

        //�δ����� �ö���� �� ȿ�� �ؽ�Ʈ ���
        textMoles[currentIndex].SetActive(true);
        //���� �δ����� �����ϵ��� �ε��� ����
        currentIndex++;
    }
}
