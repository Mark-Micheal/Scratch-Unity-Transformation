using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Motion : MonoBehaviour
{
    public TextMeshPro text;
    public TMP_InputField input;
    private string textFile;
    private bool flagUsed;
    private float rotationIncrement;
    private float positionIncrement;
    public TextMeshProUGUI say;
    public int x;

    // Start is called before the first frame update
    void Start()
    {
       // textFile = "/buildTest.txt";
        textFile = "E:/unity/VP3/Assets/test.txt";
        string[] lines = File.ReadAllLines(textFile);
        foreach (string line in lines)
        {
            print(line);
            CheckKeyword(line);

        }

        print("total rotation applied: " + rotationIncrement);
        print("total movement applied: " + positionIncrement);

        transform.position += transform.right * positionIncrement * 0.1f;
        transform.Translate(0,5,0);

        //----testing----
        print("testting start");
        //Wait(30);
        Say("hello", 2, false);
        print("testting finish");
        StartCoroutine(WaitUntil());

    }

    private void CheckKeyword(string line)
    {
        string[] subStrings = line.Split();
        
        switch (subStrings[0])
        {
            case "When": flagUsed = true; return;
            case "TL" : Turn("L", float.Parse(subStrings[1])); return; 
            case "TR": Turn("R", float.Parse(subStrings[1])); return;
            case "MV": Move(float.Parse(subStrings[1])); return;
        }

    }

    private void Turn(String direction, float value)
    {
        if(direction == "L")
        {
            // rotate left (positive rotation)
        }
        else
        {
            // rotate right (negative rotation)
        }

    }

    private void Move(float value)
    {
        transform.position += transform.right * value * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        Wait(0.10f);
        String keyPressed = "a";
        if (Input.GetKeyDown(keyPressed))
        {
            print("ok");
        }
        /*        Vector3 pos = this.transform.position;
                pos.y += 0.01f;
                this.transform.position = pos;*/
    }

    public void ButtonClicked()
    {
        String output = input.text;
        text.SetText(output);

    }
    public void Say(string message,float time,Boolean isTime)
    {
        say.text = message;
        if (isTime)
        {
            InvokeRepeating("ClearSay", time, 0);  //1s delay, repeat every 1s
        }
        

    }

    private void ClearSay()
    {
        say.text = "";
    }

    public void Wait(float time)
    {
        //float Start = Time.deltaTime;
        //float end = Start + time;
        //print(Start + "::" + end);
        //int i = 0;
        //while (i<1000)
        //{
        //    print("end::" + end);
        //    print("delta" + Time.deltaTime);
        //    //if (end < Time.deltaTime) 
        //    //    break;
        //    i++;
        //}
       
    }
    IEnumerator WaitUntil()
    {
        Debug.Log("Waiting for princess to be rescued...");
        yield return new WaitUntil(() => x >= 10);
        Debug.Log("Princess was rescued!");
    }

}
