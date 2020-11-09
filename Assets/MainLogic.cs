using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLogic : MonoBehaviour
{
    public int mode;
    public Text[] textCounter;
    public InputField inputRef;
    public InputField inputSaiparkRef1;
    public InputField inputSaiparkRef2;
    public GameObject[] uiGroupRef;
    public Button goButton;
    public Button backButton;
    public Button runButton;
    public GameObject processText;

    private bool featuredEncounter;
    private bool running = false;
    private int inputNumber;
    private int encounterInputNumber;
    private int lumaInputNumber;
    private List<int> counter = new List<int>();
    private List<int> sinceLastList = new List<int>();
    private List<int> sinceLastFeaturedList = new List<int>();
    private int sinceLast = 0;
    private int sinceLastFeatured = 0;
    private int lumaCount1;
    private int lumaCount2;
    private int lumaCount3;
    private int lumaCount4;
    private bool hit;
    private bool featuredHit;

    private float tempMedian = 0.0f;
    private float tempFeaturedMedian = 0.0f;
    private float tempMean = 0.0f;
    private float tempFeaturedMean = 0.0f;

    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            counter.Add(0);
        }
    }

    private void Update()
    {
        if (running)
        {
            if (hit)
            {
                hit = false;
                featuredHit = false;
                running = false;
                goButton.interactable = true;
                runButton.interactable = true;
            }
            else
            {
                inputNumber = 1;
                while (!hit)
                {
                    ExecuteButton();
                }
            }
        }
    }

    bool StandardFunction()
    {
        if (Random.Range(0, 100) < encounterInputNumber)
        {
            featuredEncounter = true;
            counter[2]++;
            if (Random.Range(0, 10000) < lumaInputNumber)
            {
                featuredHit = true;
                counter[4]++;
                return true;
            }
        }
        else
        {
            featuredEncounter = false;
            counter[1]++;
            if (Random.Range(0, 10000) == 9999)
            {
                featuredHit = false;
                counter[3]++;
                return true;
            }
        }
        return false;
    }

    void RadarCheck()
    {
        bool lumaFound = false;
        lumaCount1 = 0;
        lumaCount2 = 0;
        lumaCount3 = 0;
        lumaCount4 = 0;
        for (int i = 0; i < 200; i++)
        {
            if (Random.Range(0, 10000) == 9999)
            {
                hit = true;
                lumaCount1++;
                lumaCount2 = 1;
                if (!lumaFound)
                {
                    lumaFound = true;
                    lumaCount3 = 1;
                }
                else
                {
                    lumaCount3 = 0;
                    lumaCount4 = 1;
                }
            }
        }
        for (int i = 0; i < 100; i++)
        {
            if (Random.Range(0, 2000) == 1999)
            {
                hit = true;
                lumaCount1++;
                lumaCount2 = 1;
                if (!lumaFound)
                {
                    lumaFound = true;
                    lumaCount3 = 1;
                }
                else
                {
                    lumaCount3 = 0;
                    lumaCount4 = 1;
                }
            }
        }
        for (int i = 0; i < 100; i++)
        {
            if (Random.Range(0, 1000) == 999)
            {
                hit = true;
                lumaCount1++;
                lumaCount2 = 1;
                if (!lumaFound)
                {
                    lumaFound = true;
                    lumaCount3 = 1;
                }
                else
                {
                    lumaCount3 = 0;
                    lumaCount4 = 1;
                }
            }
        }
    }

    int FiveCheck()
    {
        int matched = 0;
        if (Random.Range(0, 2) == 1)
        {
            matched++;
        }
        if (Random.Range(0, 2) == 1)
        {
            matched++;
        }
        if (Random.Range(0, 6) == 5)
        {
            matched++;
        }
        if (Random.Range(0, 6) == 5)
        {
            matched++;
        }
        if (Random.Range(0, 6) == 5)
        {
            matched++;
        }
        return matched;
    }

    public float CalculateMedian(List<int> inputList)
    {
        int middleSpot = 0;
        float median = 0.0f;

        middleSpot = inputList.Count / 2;

        // Even number count
        if (inputList.Count % 2 == 0)
        {
            // Even
            median = inputList[middleSpot] + inputList[middleSpot - 1];
            median *= 0.5f;
        }
        else
        {
            // Odd
            median = inputList[middleSpot];
        }
        
        return median;
    }

    public void RunButton()
    {
        hit = false;
        featuredHit = false;
        running = true;
        goButton.interactable = false;
        runButton.interactable = false;
    }

    public void ExecuteButton()
    {
        if (!running)
        {
            inputNumber = int.Parse(inputRef.text);
        }

        hit = false;
        featuredHit = false;

        processText.SetActive(true);
        switch (mode)
        {
            // menu
            case 0:
                break;
            // standard
            case 1:
                encounterInputNumber = int.Parse(inputSaiparkRef1.text);
                lumaInputNumber = int.Parse(inputSaiparkRef2.text);
                for (int i = 0; i < inputNumber; i++)
                {
                    counter[0]++;
                    if (StandardFunction())
                    {
                        hit = true;
                        sinceLastList.Add(sinceLast);

                        if (featuredHit)
                        {
                            sinceLastFeaturedList.Add(sinceLastFeatured);
                            sinceLastFeatured = 0;
                        }
                        else if (featuredEncounter)
                        {
                            sinceLastFeatured++;
                        }

                        // Calculate min and max
                        counter[5] = Mathf.Max(sinceLast, counter[5]);
                        counter[6] = Mathf.Min(sinceLast, counter[6]);

                        // Reset since last luma encounter
                        sinceLast = 0;
                    }
                    else
                    {
                        sinceLast++;
                        if (featuredEncounter)
                        {
                            sinceLastFeatured++;
                        }
                    }
                    featuredEncounter = false;
                }

                // Calculate median and average
                if (hit)
                {
                    if (sinceLastList.Count > 0)
                    {
                        sinceLastList.Sort();
                        tempMedian = CalculateMedian(sinceLastList);
                    }

                    if (sinceLastFeaturedList.Count > 0)
                    {
                        sinceLastFeaturedList.Sort();
                        tempFeaturedMedian = CalculateMedian(sinceLastFeaturedList);
                    }

                    // Average/Mean
                    int tempTotal = counter[3] + counter[4];
                    tempMean = (float)tempTotal / (float)counter[0];
                    tempMean *= 100.0f;
                    tempFeaturedMean = (float)counter[4] / (float)counter[0];
                    tempFeaturedMean *= 100.0f;
                }

                textCounter[0].text = counter[0].ToString();
                textCounter[1].text = counter[1].ToString();
                textCounter[2].text = counter[2].ToString();
                textCounter[3].text = counter[3].ToString();
                textCounter[4].text = counter[4].ToString();
                textCounter[5].text = tempMean.ToString() + "%";
                textCounter[6].text = tempFeaturedMean.ToString() + "%";
                textCounter[7].text = tempMedian.ToString();
                textCounter[8].text = tempFeaturedMedian.ToString();
                textCounter[9].text = counter[5].ToString();
                textCounter[10].text = counter[6].ToString();

                // Max
                if (counter[5] == 0)
                {
                    textCounter[9].text = "No encounters";
                }
                else
                {
                    textCounter[9].text = counter[5].ToString();
                }

                // Min
                if (counter[6] == 2147483647)
                {
                    textCounter[10].text = "No encounters";
                }
                else
                {
                    textCounter[10].text = counter[6].ToString();
                }
                break;
            case 2:
                // radar
                for (int i = 0; i < inputNumber; i++)
                {
                    RadarCheck();
                    counter[0]++;
                    counter[1] += lumaCount1;
                    counter[2] += lumaCount2;
                    counter[3] += lumaCount3;
                    counter[4] += lumaCount4;
                }

                textCounter[0].text = counter[0].ToString();
                textCounter[1].text = counter[1].ToString();
                textCounter[2].text = counter[2].ToString();
                textCounter[3].text = counter[3].ToString();
                textCounter[4].text = counter[4].ToString();

                float tempFloatHolder = 0.0f;
                float tempFloatHolder2 = 0.0f;

                // Calculate average
                tempFloatHolder = (float)counter[1] / (float)counter[0];
                textCounter[5].text = tempFloatHolder.ToString();

                // Chance of getting 1 luma per radar
                tempFloatHolder = (float)counter[2] / (float)counter[0];
                tempFloatHolder *= 100.0f;
                tempFloatHolder2 = 100.0f - tempFloatHolder;
                textCounter[6].text = tempFloatHolder.ToString() + "%";

                // Chance of getting more than 1 luma per radar
                tempFloatHolder = (float)counter[4] / (float)counter[0];
                tempFloatHolder *= 100.0f;
                textCounter[7].text = tempFloatHolder.ToString() + "%";

                // Chance of getting 0 lumas per radar
                textCounter[8].text = tempFloatHolder2.ToString() + "%";
                break;
            // fish
            case 3:
                for (int j = 0; j < inputNumber; j++)
                {
                    counter[0]++;
                    int matched = FiveCheck();
                    if (matched == 4)
                    {
                        counter[4]++;
                    }
                    if (matched == 5)
                    {
                        hit = true;
                        counter[1]++;
                        sinceLastList.Add(sinceLast);
                        counter[2] = Mathf.Max(sinceLast, counter[2]);
                        counter[3] = Mathf.Min(sinceLast, counter[3]);
                        sinceLast = 0;
                    }
                    else
                    {
                        sinceLast++;
                    }
                }

                sinceLastList.Sort();

                textCounter[0].text = counter[0].ToString();
                textCounter[1].text = counter[1].ToString();

                float tempFloatHolder3 = 0.0f;

                tempFloatHolder3 = (float)counter[1] / (float)counter[0];
                textCounter[2].text = tempFloatHolder3.ToString();
                tempFloatHolder3 *= 100.0f;
                textCounter[3].text = tempFloatHolder3.ToString() + "%";

                // Calculate median and average
                if (hit && sinceLastList.Count > 0)
                {
                    sinceLastList.Sort();
                    tempMedian = CalculateMedian(sinceLastList);
                    tempMean = (float)counter[0] / (float)counter[1];
                }
                textCounter[4].text = tempMean.ToString();
                textCounter[5].text = tempMedian.ToString();

                // Max
                if (counter[2] == 0)
                {
                    textCounter[6].text = "No encounters";
                }
                else
                {
                    textCounter[6].text = counter[2].ToString();
                }

                // Min
                if (counter[3] == 2147483647)
                {
                    textCounter[7].text = "No encounters";
                }
                else
                {
                    textCounter[7].text = counter[3].ToString();
                }

                // 4/5
                textCounter[8].text = counter[4].ToString();

                // Average 4/5
                float tempFloatHolder4 = 0.0f;
                tempFloatHolder4 = (float)counter[4] / (float)counter[0];
                tempFloatHolder4 *= 100.0f;
                textCounter[9].text = tempFloatHolder4.ToString() + "%";
                break;
            default:
                hit = true;
                featuredHit = true;
                break;
        }
        processText.SetActive(false);
    }

    public void BackButton()
    {
        for (int i = 0; i < counter.Count; i++)
        {
            counter[i] = 0;
        }
        for (int i = 0; i < textCounter.Length; i++)
        {
            textCounter[i].text = "0";
        }
        mode = 0;
        uiGroupRef[0].SetActive(false);
        uiGroupRef[1].SetActive(false);
        uiGroupRef[2].SetActive(false);
        uiGroupRef[3].SetActive(false);
        uiGroupRef[4].SetActive(true);
        uiGroupRef[5].SetActive(false);
        ToggleNumbers(11, false);
        sinceLastList.Clear();
        sinceLastFeaturedList.Clear();
        sinceLast = 0;
        sinceLastFeatured = 0;
        running = false;
        hit = false;
        featuredHit = false;
        goButton.interactable = true;
        runButton.interactable = true;

        tempMedian = 0.0f;
        tempFeaturedMedian = 0.0f;
        tempMean = 0.0f;
        tempFeaturedMean = 0.0f;
    }

    public void MenuButton(int inputMode)
    {
        mode = inputMode;
        uiGroupRef[0].SetActive(true);
        uiGroupRef[4].SetActive(false);
        uiGroupRef[mode].SetActive(true);
        switch (mode)
        {
            case 1:
                counter[6] = 2147483647;
                ToggleNumbers(11, true);
                uiGroupRef[5].SetActive(true);
                textCounter[9].text = "No encounters";
                textCounter[10].text = "No encounters";
                break;
            case 2:
                ToggleNumbers(9, true);
                break;
            case 3:
                counter[3] = 2147483647;
                ToggleNumbers(10, true);
                textCounter[6].text = "No encounters";
                textCounter[7].text = "No encounters";
                break;
        }
    }

    void ToggleNumbers(int inputNumber, bool status)
    {
        for (int i = 0; i < inputNumber; i++)
        {
            textCounter[i].gameObject.SetActive(status);
        }
    }
}
