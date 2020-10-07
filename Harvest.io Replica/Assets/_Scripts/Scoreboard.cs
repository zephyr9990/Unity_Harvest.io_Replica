using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] GameObject[] playerScores;
    [SerializeField] GameObject[] podiumTransforms;
    private int[] scores;
    private Text[] percentageText;
    private Text[] nameText;
    private int[] podiumPlacements;
    private float lerpDuration = 5f;

    private List<GameObject> grains;

    private void Awake()
    {
        scores = new int[4];
        percentageText = new Text[4];
        nameText = new Text[4];
        podiumPlacements = new int[4];

        for (int i = 0; i < playerScores.Length; i++)
        {
            percentageText[i] = playerScores[i].transform.GetChild(0).GetComponent<Text>();
            percentageText[i].text = "0.0%";
            nameText[i] = playerScores[i].transform.GetChild(1).GetComponent<Text>();
            podiumPlacements[i] = i;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grains = GameObject.FindGameObjectsWithTag("Grain").ToList();
    }

    public void updateScore(int playerIndex, int numOfGrains)
    {
        scores[playerIndex] = numOfGrains;
        float grainsObtained = numOfGrains;

        // Round to one decimal place.
        float percentageObtained = Mathf.Round((grainsObtained / grains.Count * 100f * 10f))/10f;
        percentageText[playerIndex].text = string.Format("{0:F1}", percentageObtained) + "%";

        UpdatePlace(playerIndex);
    }

    private void UpdatePlace(int playerIndex)
    {
        for (int current = 0; current < scores.Length; current++)
        {
            for (int other = 0; other < scores.Length; other++)
            {
                if (other == current || podiumPlacements[other] < podiumPlacements[current])
                        continue;

                if (scores[current] < scores[other])
                {
                    int temp = podiumPlacements[current];
                    podiumPlacements[current] = podiumPlacements[other];
                    podiumPlacements[other] = temp;
                }
            }
        }

        StartCoroutine( UpdateUIPlacement());
    }

    private IEnumerator UpdateUIPlacement()
    {
        for (int i = 0; i < playerScores.Length; i++)
        {
            int podiumPlace = podiumPlacements[i];
            Vector2 start = playerScores[i].transform.position;
            Vector2 end = podiumTransforms[podiumPlace].transform.position;
            float elapsedTime = 0.0f;
            while (elapsedTime < lerpDuration)
            {
                playerScores[i].transform.position = Vector2.Lerp(start, end, elapsedTime / lerpDuration);
                elapsedTime += Time.deltaTime;
            }
            playerScores[i].transform.position = end;
            yield return null;
        }
    }

    public string GetWinner()
    {
        string winnerName = "";
        for (int i = 0; i < podiumPlacements.Length; i++)
        {
            if (podiumPlacements[i] != 0)
                continue;

            winnerName = nameText[i].text;
            break;
        }

        return winnerName;
    }
}
