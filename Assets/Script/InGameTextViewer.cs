using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameTextViewer : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textPlayTime;
    [SerializeField] private Slider sliderPlayTime;
    [SerializeField] private TextMeshProUGUI textCombo;

    private void Update()
    {
        textScore.text = string.Format("Score  " + "{0:#,##0}", gameController.Score);

        textPlayTime.text = gameController.CurrentTime.ToString("F1");
        sliderPlayTime.value = gameController.CurrentTime / gameController.MaxTime;

        textCombo.text = string.Format("Combo  " + "{0:#,##0}", gameController.Combo);
    }
}
