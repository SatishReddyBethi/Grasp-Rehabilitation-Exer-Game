using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System.IO;

public class Data : MonoBehaviour
{
    private SerialPort sp;
    private bool SaveStatics;
    private int timeout = 0;
    public bool start;
    public Text DebugText;
    public bool debug = true;
    private static string savedDataPath;
    public List<int[]> Calibrations = new List<int[]>();
    public float lifting_force;
    public float grasping_force;
    public float initial_lf;
    public float saved_lf;
    public float saved_gf;
    public float lfr;
    public float gfr;
    public bool start_thing = false;
    public string score_thing = "";
    public bool Jump;
    public int jump_iteration;
    public int Data_Iteration;
    public float timer;

    // Use this for initialization
    void Start()
    {
        savedDataPath = Application.persistentDataPath + "/savedData";
        Time.timeScale = 1;
        Scan();
        GetComPort();        
        //Invoke("Save_Statics", 2.0f);

    }

    #region Code Connecting With Arduino

    public List<string> portExists;
    public Dropdown ComPorts;

    public void Scan()
    {
        portExists = new List<string>();
        portExists.AddRange(SerialPort.GetPortNames());
        if (portExists.Count != 0)
        {
            ComPorts.ClearOptions();
            ComPorts.AddOptions(portExists);
        }
    }

    public void Initialize()
    {
        if (portExists.Count > 0)
        {
            sp = new SerialPort(portExists[ComPorts.value], 115200);
            sp.NewLine = "\n";
            sp.DtrEnable = true;
            sp.ReadTimeout = 2;//25 for query
            sp.WriteTimeout = 5;

            try
            {
                sp.Open();
            }
            catch (System.Exception)
            {
                Verbose_Logging("No Device Connected to that COM Port");
            }
            Verbose_Logging("Initialized " + portExists[ComPorts.value]);

        }
    }

    private void OnDisable()
    {
        if (sp != null)
        {
            sp.Close();
        }
        else
        {
            Debug.Log("COM Port Does Not Exist");
        }
    }

    private void OnApplicationQuit()
    {
        if (sp != null)
        {
            sp.Close();
        }
    }
    #endregion

    #region Code for Data Streaming and Parsing

    public float Hts;//Highest Time Stamp
    void FixedUpdate()//SerialPort sp)
    {
    }


    public void DataRecieve()
    {
        if (start)
        {
            if (sp != null)
            {
                float ts = 0;
                ts = Time.realtimeSinceStartup;
                ReadFromArduino();
                if (Line != "")
                {
                    ParseAngles();

                }
                ts = Time.realtimeSinceStartup - ts;
                if (ts > Hts)
                {
                    Hts = ts;
                }
            }
            else
            {
                Verbose_Logging("No Device Connected");
            }
        }
    }

    public void WriteToArduino(string message)
    {
        sp.WriteLine(message);
        sp.BaseStream.Flush();
    }

    public string Line;
    public void ReadFromArduino()
    {
        Line = "";
        if (sp != null && sp.IsOpen)
        {
            try
            {
                Line = sp.ReadLine();
                DebugText.text = Line;
                timeout = 0;
                sp.BaseStream.Flush();
            }
            catch (System.TimeoutException)
            {
                Line = "";
                timeout = timeout + 1;
                Verbose_Logging("Receive Data Error. Read Timeout");
            }

            if (timeout > 50)
            {
                sp.Close();
                Invoke("ResetComPort", 0.5f);
            }
        }
    }

    void ResetComPort()
    {
        timeout = 0;
        Initialize();
    }
    public float LF;

    public void ParseAngles()
    {
        lifting_force = 0f;
        bool ReadStatus = true;
        string[] forces = Line.Split(',');
        if (forces.Length == 3)
        {
            for (int i = 0; i < forces.Length; i++)
            {
                if (forces[i] == "")
                {
                    ReadStatus = false;
                }
            }
            if (ReadStatus)
            {                          

                
                try
                {
                    Verbose_Logging("Got Data");
                    float TempLF = float.Parse(forces[0]);
                    float TempGF = float.Parse(forces[1]) + float.Parse(forces[2]) / 2;
                    LF = TempLF;
                    if (TempLF < 10 && TempLF > 0)
                    {                        
                        if (start_thing && TempLF!=0)
                        {
                            initial_lf = TempLF;
                            start_thing = false;
                            //Debug.Log("Entered");
                        }
                        else
                        {
                            lifting_force = initial_lf - TempLF;
                            if (lifting_force < 0)
                            {
                                lifting_force = 0;
                            }
                            Data_Iteration += 1;
                        }
                    }                    

                    if (TempGF > 0 && TempGF < 26)
                    {
                        grasping_force = TempGF;
                        
                    }
                }
                catch (System.FormatException)
                {
                    Verbose_Logging("Format Error");
                }

            }

            else
            {
                Debug.Log("Wrong Data: " + Line);
            }
            timer += Time.deltaTime;
            lfr = (lifting_force - saved_lf) / Time.deltaTime;
            gfr = (grasping_force - saved_gf) / Time.deltaTime;            
            SaveData(timer.ToString("F2") + "," + grasping_force.ToString("F2") + "," + lifting_force.ToString("F2") + "," + lfr.ToString("F2") + "," + gfr.ToString("F2") + "\n", false);
            saved_lf = lifting_force;
            saved_gf = grasping_force;
        }

        if (Data_Iteration > 100)
        {
            jump_iteration = 0;
            Data_Iteration = 0;
        }
        if(lifting_force > 1.0f)
        {
            jump_iteration += 1;
        }

        if (jump_iteration > 10)
        {
            jump_iteration = 0;
            Jump = true;
            Debug.Log("Jump");
        }
        else
        {
            Jump = false;
        }        
    }
    public void Flush()
    {
        sp.BaseStream.Flush();
    }
        #endregion

    #region Buttons
        string Time_;
        public void Save_Statics()
        {
        start = true;
            start_thing = true;
            InvokeRepeating("DataRecieve", 0.5f, 0.01f);
            Time_ = System.DateTime.Now.ToString("_dd_MM_yyyy_HH_mm_ss");
        }

        public void Exit()
        {
            start_thing = false;
            start = false;
            CancelInvoke();
        CloseSerialPort();
        GetComPort();
        }

        public void CloseSerialPort()
        {
            if (sp != null)
            {
                sp.Close();
            }
        }

        public void GetComPort()
        {
            if (sp != null)
            {
                sp.Close();
            }
            Initialize();
        }

        public void Verbose_Logging(string msg)
        {
            if (debug)
            {
                Debug.Log(msg);
                DebugText.text = msg;
            }
        }
        #endregion

    #region Saving Data
        public void SaveData(string Data, bool Replace)
        {

            string FileName = savedDataPath + "/" + "GR_" + Time_ + ".txt";

            if (!File.Exists(FileName))
            {
                Directory.CreateDirectory(savedDataPath);
            }

            if (Replace)
            {
                File.WriteAllText(FileName, Data);
            }
            else
            {
                File.AppendAllText(FileName, Data);
            }
        }
        #endregion

}

