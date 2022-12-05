using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameTextViewer : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private TextMeshProUGUI textScore;

    private void Update()
    {
        textScore.text = string.Format("Score  " + "{0:#,##0}", gameController.Score);
    }
}
