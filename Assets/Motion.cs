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
    public float factor = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
       // textFile = "/buildTest.txt";
        textFile = "Assets/test.txt";
        string[] lines = File.ReadAllLines(textFile);
        Parser(lines);

        /*foreach (string line in lines)
        {
            print(line);
            CheckKeyword(line);

        }*/

        print("total rotation applied: " + rotationIncrement);
        print("total movement applied: " + positionIncrement);

        

        //----testing----
        print("testing start");
        //Wait(30);
        //Say("hello", 2, true);
        print("testing finish");
        StartCoroutine(WaitUntil());
        // transform.position += transform.right * positionIncrement * 0.1f;
        // transform.Translate(0,5,0);

    }

    private void CheckKeyword(string line)
    {
        string[] subStrings = line.Split();
        
        switch (subStrings[0])
        {
            case "When": flagUsed = true; return;
            case "TL" : Turn("L", float.Parse(subStrings[1])); return; 
            case "TR": Turn("R", -float.Parse(subStrings[1])); return;
            case "MV": Move(float.Parse(subStrings[1])); return;
            case "CH": ChangePosition(subStrings[1], float.Parse(subStrings[2])); return;
            case "SET": SetPosition(subStrings[1], float.Parse(subStrings[2])); return;
            case "GO": SetPosition("X", float.Parse(subStrings[1])); SetPosition("Y", float.Parse(subStrings[2])); return;
            case "repeat": Repeat(line, subStrings[1]); return;
        }

    }

    private void SetPosition(string variable, float value)
    {
        Vector3 newPos = Vector3.zero;
        if (variable == "X")
        {
            newPos = new Vector3(value*factor, transform.position.y, transform.position.z);
        }
        else
        {
            newPos = new Vector3(transform.position.x, value*factor, transform.position.z);
        }
        transform.position = newPos;
    }

    private void ChangePosition(string variable, float value)
    {
        if (variable == "X")
        {
            transform.Translate(value*factor, 0, 0);
        }
        else
        {
            transform.Translate(0, value*factor, 0);
        }
    }

    private void Parser(string[] lines)
    {
        foreach (string line in lines)
        {
            print(line);
            CheckKeyword(line);

        }
    }

    private void Repeat(string line, String times)
    {
        // repeat 10 times #MV 10#
        // repeatuntil yPos = 50 and xPos = 50 #SET X 0;CH Y 10#
        string[] subStrings = line.Split('#');
        String body = subStrings[1];
        string[] lines = body.Split(';');

        if(times == "forever")
        {
            // Should repeat forever, unity crashed !!!
            /*while (true)
            {
                Parser(lines);
            }*/
        }
        else
        {
            int repeatTimes = int.Parse(times);
            for (int i = 0; i < repeatTimes; i++)
            {
                Parser(lines);
            }
        }





    }

    private void Turn(String direction, float value)
    {
        transform.Rotate(0, 0, value);
        /*if(direction == "L")
        {
            // rotate left (positive rotation)
            transform.Rotate(0, 0, value);
        }
        else
        {
            // rotate right (negative rotation)
            transform.Rotate(0, 0, -value);
        }*/

    }

    private void Move(float value)
    {
        transform.position += transform.right * value * factor;
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
        // play meo music !!!
        say.text = message;
        if (isTime)
        {
            InvokeRepeating("ClearSay", time, 0);  //1s delay
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
