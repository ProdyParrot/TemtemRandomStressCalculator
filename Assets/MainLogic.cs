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

    private bool running = false;
    private int inputNumber;
    private int encounterInputNumber;
    private int lumaInputNumber;
    private List<int> counter = new List<int>();
    private List<int> sinceLastList = new List<int>();
    private int sinceLast = 0;
    private float percent;
    private float percent2;
    private int lumaCount1;
    private int lumaCount2;
    private int lumaCount3;
    private int lumaCount4;
    private bool hit;

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
            counter[2]++;
            if (Random.Range(0, 10000) < lumaInputNumber)
            {
                counter[4]++;
                return true;
            }
        }
        else
        {
            counter[1]++;
            if (Random.Range(0, 10000) == 9999)
            {
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

    bool FiveCheck()
    {
        if (Random.Range(0, 2) == 1)
        {
            return false;
        }
        if (Random.Range(0, 2) == 1)
        {
            return false;
        }
        if (Random.Range(0, 6) > 0)
        {
            return false;
        }
        if (Random.Range(0, 6) > 0)
        {
            return false;
        }
        if (Random.Range(0, 6) > 0)
        {
            return false;
        }
        return true;
    }

    public void RunButton()
    {
        hit = false;
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

        int total = 0;
        int count = 0;
        int middleSpot = 0;
        float median = 0.0f;
        bool evenMedian = false;
        bool evenMedianStep = false;
        bool medianCalculated = false;

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
                        counter[5] = Mathf.Max(sinceLast, counter[5]);
                        counter[6] = Mathf.Min(sinceLast, counter[6]);
                        sinceLast = 0;
                    }
                    else
                    {
                        sinceLast++;
                    }
                }

                // Recalculate median
                if (hit && sinceLastList.Count > 0)
                {
                    middleSpot = (int)Mathf.Floor(sinceLastList.Count / 2);

                    // Even number count
                    if (sinceLastList.Count % 2 == 0)
                    {
                        // Even
                        evenMedian = true;
                    }
                    else
                    {
                        // Odd
                        evenMedian = false;
                    }

                    foreach (int item in sinceLastList)
                    {
                        total += item;
                        count++;
                        if (!medianCalculated && count == middleSpot)
                        {
                            if (evenMedian)
                            {
                                if (evenMedianStep)
                                {
                                    median += item;
                                    median *= 0.5f;
                                    medianCalculated = true;
                                }
                                else
                                {
                                    evenMedianStep = true;
                                    middleSpot += 1;
                                    median = item;
                                }
                            }
                            else
                            {
                                median = item;
                                medianCalculated = true;
                            }
                        }
                    }
                }

                if (sinceLastList.Count > 0)
                {
                    percent = (float)total / (float)sinceLastList.Count;
                }
                else
                {
                    percent = 0;
                }

                textCounter[0].text = counter[0].ToString();
                textCounter[1].text = counter[1].ToString();
                textCounter[2].text = counter[2].ToString();
                textCounter[3].text = counter[3].ToString();
                textCounter[4].text = counter[4].ToString();
                textCounter[5].text = percent.ToString();
                textCounter[6].text = median.ToString();
                textCounter[7].text = counter[5].ToString();
                textCounter[8].text = counter[6].ToString();

                if (counter[5] == 0)
                {
                    textCounter[7].text = "No encounters";
                }
                else
                {
                    textCounter[7].text = counter[5].ToString();
                }

                if (counter[6] == 2147483647)
                {
                    textCounter[8].text = "No encounters";
                }
                else
                {
                    textCounter[8].text = counter[6].ToString();
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

                percent = (float)counter[1] / (float)counter[0];
                textCounter[5].text = percent.ToString();

                percent = (float)counter[2] / (float)counter[0];
                percent *= 100.0f;
                percent2 = 100.0f - percent;
                textCounter[6].text = percent.ToString() + "%";

                percent = (float)counter[4] / (float)counter[0];
                percent *= 100.0f;
                textCounter[7].text = percent.ToString() + "%";

                textCounter[8].text = percent2.ToString() + "%";
                break;
            // fish
            case 3:
                for (int j = 0; j < inputNumber; j++)
                {
                    counter[0]++;
                    if (FiveCheck())
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

                percent = (float)counter[1] / (float)counter[0];
                textCounter[2].text = percent.ToString();
                percent *= 100.0f;
                textCounter[3].text = percent.ToString() + "%";

                // Recalculate median
                if (hit && sinceLastList.Count > 0)
                {
                    middleSpot = (int)Mathf.Floor(sinceLastList.Count / 2);

                    // Even number count
                    if (sinceLastList.Count % 2 == 0)
                    {
                        // Even
                        evenMedian = true;
                    }
                    else
                    {
                        // Odd
                        evenMedian = false;
                    }

                    foreach (int item in sinceLastList)
                    {
                        total += item;
                        count++;
                        if (!medianCalculated && count == middleSpot)
                        {
                            if (evenMedian)
                            {
                                if (evenMedianStep)
                                {
                                    median += item;
                                    median *= 0.5f;
                                    medianCalculated = true;
                                }
                                else
                                {
                                    evenMedianStep = true;
                                    middleSpot += 1;
                                    median = item;
                                }
                            }
                            else
                            {
                                median = item;
                                medianCalculated = true;
                            }
                        }
                    }
                }

                if (sinceLastList.Count > 0)
                {
                    percent = (float)total / (float)sinceLastList.Count;
                }
                else
                {
                    percent = 0;
                }
                textCounter[4].text = percent.ToString();

                textCounter[5].text = median.ToString();

                if (counter[2] == 0)
                {
                    textCounter[6].text = "No encounters";
                }
                else
                {
                    textCounter[6].text = counter[2].ToString();
                }

                if (counter[3] == 2147483647)
                {
                    textCounter[7].text = "No encounters";
                }
                else
                {
                    textCounter[7].text = counter[3].ToString();
                }
                break;
            default:
                hit = true;
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
        ToggleNumbers(9, false);
        sinceLastList.Clear();
        sinceLast = 0;
        running = false;
        hit = false;
        goButton.interactable = true;
        runButton.interactable = true;
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
                ToggleNumbers(9, true);
                uiGroupRef[5].SetActive(true);
                textCounter[7].text = "No encounters";
                textCounter[8].text = "No encounters";
                break;
            case 2:
                ToggleNumbers(9, true);
                break;
            case 3:
                counter[3] = 2147483647;
                ToggleNumbers(8, true);
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
