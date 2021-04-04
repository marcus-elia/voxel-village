using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI chunksVisitedText;
    public TextMeshProUGUI chunksGeneratedText;
    public TextMeshProUGUI maxHeightText;

    // Start is called before the first frame update
    void Start()
    {
        chunksVisitedText.text = "0";
        chunksGeneratedText.text = "0";
        maxHeightText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
