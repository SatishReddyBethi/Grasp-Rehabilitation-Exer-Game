using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptivePlatformPositioning : MonoBehaviour
{
    public GameObject[] Platforms;
    public GameObject[] DisabledPlatforms;
    public PlayerController PC;
    public float LinkingDistance;
    public float height = 1.8f;
    public float Texture;
    public float Weight;
    public GameObject Player;
    private float PlatformEndPt = 0f;
    public GameObject[] CurrentPlatforms;
    public int Index = 0;
    public GameObject[] Coins;
    public int coinset = 0;
    public float[] PlatformTriggerPosition;
    public float MaxJumpDistance;
    // Start is called before the first frame update
    void Start()
    {
        DisabledPlatforms = new GameObject[3];
        CurrentPlatforms = new GameObject[3];
        PlatformTriggerPosition = new float[6];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            CreateNewPlatform();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlatforms();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            CheckDisabledPlatforms();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CoinSet();
        }
        if (Player.transform.position.x > (PlatformTriggerPosition[2]))
        {
            CreateNewPlatform();
        }
    }

    public void ResetPlatforms()
    {
        MaxJumpDistance = (PC.Speed * PC.thrust) / 9.98f;
        PlatformEndPt = 0f;
        Index = 3;
        for (int i = 0; i < Platforms.Length; i++)
        {
            if (i < 3)
            {
                switch (i)
                {
                    case 0:
                        Platforms[i].transform.localPosition = new Vector3(-12.0f, 0f, 0f);// Length of this platform 8.855
                        PlatformEndPt = -7.5725f;
                        PlatformTriggerPosition[3] = PlatformEndPt;
                        CoinSet();
                        break;
                    case 1:
                        Platforms[i].transform.localPosition = new Vector3(-2.38f + LinkingDistance, 0f, 0f);// Length of this platform 10.115
                        PlatformEndPt += LinkingDistance + 10.115f;
                        PlatformTriggerPosition[4] = PlatformEndPt;
                        CoinSet();
                        break;
                    case 2:
                        Platforms[i].transform.localPosition = new Vector3(6.339f + (2 * LinkingDistance), 0.43f, 0f);// Length of this platform 7.323
                        PlatformEndPt += LinkingDistance + 7.323f;
                        PlatformTriggerPosition[5] = PlatformEndPt;
                        CoinSet();
                        break;
                }
                CurrentPlatforms[i] = Platforms[i];
                Platforms[i].SetActive(true);
            }
            else
            {
                //Platforms[i].SetActive(false);
            }
        }
        CreateNewPlatform();
        CreateNewPlatform();
        CreateNewPlatform();
    }

    public void CoinSet()
    {
        int iteration = 0;
        float X, Y = 0;
        float m = LinkingDistance + 1.0f;
        for (int i = coinset * 5; i < (coinset + 1) * 5; i++)
        {
            switch (iteration)
            {
                case 0:
                    X = 0f;
                    Y = ((4 * height) / (m * m)) * ((X * m) - (X * X));
                    Coins[i].transform.localPosition = new Vector3(PlatformEndPt - 0.5f, Y + 1.0f, 0f);
                    Coins[i].SetActive(true);
                    break;
                case 1:
                    X = m / 4.0f;
                    Y = ((4 * height) / (m * m)) * ((X * m) - (X * X));
                    Coins[i].transform.localPosition = new Vector3(PlatformEndPt - 0.5f + (m / 4.0f), Y + 1.0f, 0f);
                    Coins[i].SetActive(true);
                    break;
                case 2:
                    X = m / 2.0f;
                    Y = ((4 * height) / (m * m)) * ((X * m) - (X * X));
                    Coins[i].transform.localPosition = new Vector3(PlatformEndPt - 0.5f + (m / 2.0f), Y + 1.0f, 0f);
                    Coins[i].SetActive(true);
                    break;
                case 3:
                    X = (3 * m) / 4.0f;
                    Y = ((4 * height) / (m * m)) * ((X * m) - (X * X));
                    Coins[i].transform.localPosition = new Vector3(PlatformEndPt - 0.5f + ((3 * m) / 4.0f), Y + 1.0f, 0f);
                    Coins[i].SetActive(true);
                    break;
                case 4:
                    X = m;
                    Y = ((4 * height) / (m * m)) * ((X * m) - (X * X));
                    Coins[i].transform.localPosition = new Vector3(PlatformEndPt - 0.5f + m, Y + 1.0f, 0f);
                    Coins[i].SetActive(true);
                    break;
            }
            iteration += 1;
        }
        coinset = (coinset < 5) ? coinset + 1 : 0;
    }

    public void CalcPlatformEndPt(int PlatformType)
    {
        
        switch (PlatformType)
        {
            case 0:
                PlatformEndPt += LinkingDistance + 8.855f;
                CurrentPlatforms[2].transform.localPosition = new Vector3(PlatformEndPt - (8.855f / 2.0f), 0f, 0f);
                break;
            case 1:
                PlatformEndPt += LinkingDistance + 10.115f;
                CurrentPlatforms[2].transform.localPosition = new Vector3(PlatformEndPt - (10.115f / 2.0f), 0f, 0f);
                break;
            case 2:
                PlatformEndPt += LinkingDistance + 7.323f;
                CurrentPlatforms[2].transform.localPosition = new Vector3(PlatformEndPt - (7.323f / 2.0f), 0.43f, 0f);
                break;
            case 3:
                PlatformEndPt += LinkingDistance + 8.855f;
                CurrentPlatforms[2].transform.localPosition = new Vector3(PlatformEndPt - (8.855f / 2.0f), 0f, 0f);
                break;
            case 4:
                PlatformEndPt += LinkingDistance + 10.115f;
                CurrentPlatforms[2].transform.localPosition = new Vector3(PlatformEndPt - (10.115f / 2.0f), 0f, 0f);
                break;
            case 5:
                PlatformEndPt += LinkingDistance + 7.323f;
                CurrentPlatforms[2].transform.localPosition = new Vector3(PlatformEndPt - (7.323f / 2.0f), 0.43f, 0f);
                break;
        }
    }

    public void CreateNewPlatform()
    {
        Index = (Index > 5) ? Index - 6 : Index;
        CheckDisabledPlatforms();
        //CurrentPlatforms[0].SetActive(false);//Disabling last platform
        SetLastPlatform(Platforms[Index]);// Adding the new platform to the queue  
        CoinSet();
        CalcPlatformEndPt(Index);
        CurrentPlatforms[2].SetActive(true);
        UpdateTriggerPosition();
        Index += 1;
    }

    public void SetLastPlatform(GameObject NewPlaform)
    {
        CurrentPlatforms[0] = CurrentPlatforms[1];
        CurrentPlatforms[1] = CurrentPlatforms[2];
        CurrentPlatforms[2] = NewPlaform;
    }

    public void UpdateTriggerPosition()
    {
        PlatformTriggerPosition[0] = PlatformTriggerPosition[1];
        PlatformTriggerPosition[1] = PlatformTriggerPosition[2];
        PlatformTriggerPosition[2] = PlatformTriggerPosition[3];
        PlatformTriggerPosition[3] = PlatformTriggerPosition[4];
        PlatformTriggerPosition[4] = PlatformTriggerPosition[5];
        PlatformTriggerPosition[5] = PlatformEndPt;
    }

    public void CheckDisabledPlatforms()
    {
        for (int i = 0; i < Platforms.Length; i++)
        {
            int p = i;
            if (p > 2)
            {
                p -= 3;
            }

            if (!Platforms[i].activeInHierarchy)
            {
                DisabledPlatforms[p] = Platforms[i];
            }
        }
    }
}
