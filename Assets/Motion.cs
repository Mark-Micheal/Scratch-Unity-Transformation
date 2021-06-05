using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using System.Linq;


public class Motion : MonoBehaviour
{
    public TextMeshProUGUI text;
    private string textFile;
    private bool flagUsed;
    private float rotationIncrement;
    private float positionIncrement;
    public GameObject sayObject;
    public GameObject thinkObject;
    public TextMeshProUGUI say;
    public TextMeshProUGUI think;
    public float x;
    private bool execute = true;
    private bool repeat;
    private string[] lines;
    List<String> commands = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        sayObject.SetActive(false);
        thinkObject.SetActive(false);
        if (Application.isEditor)
        {
            textFile = "Assets/test.txt";
        }
        else
        {
            textFile = "code.txt";
        }


        lines = File.ReadAllLines(textFile);
        text.SetText("");
        for (int i = 0; i < lines.Length; i++)
        {
            text.SetText(text.text + lines[i] + '\n');
        }
        commands = lines.ToList();
    }
    void Update()
    {

        if (execute)
        {
            if (commands.Count > 0)
            {
                string i = commands[0];
                if (!repeat)
                    commands.RemoveAt(0);
                print("command : " + i);
                //print("stack : " + String.Join(",", commands));
                CheckKeyword(i);
            }
            else
            {
                execute = false;
            }


        }







    }
    private void Parserepeat(string line, String times)
    {

        // repeat 10 times #MV 10#
        // repeatuntil yPos = 50 and xPos = 50 #SET X 0;CH Y 10#
        string[] subStrings = line.Split('#');
        string body = subStrings[1];
        string[] lines = body.Split(';');

        if (times == "forever")
        {
            commands.InsertRange(0, lines);
            commands.Insert(lines.Length, line);


        }
        else
        {
            int repeatTimes = int.Parse(times);
            for (int i = 0; i < repeatTimes; i++)
            {
                commands.InsertRange(0, lines);
            }
        }


    }

    private void CheckKeyword(string line)
    {
        string[] subStrings = line.Split();
        
        switch (subStrings[0])
        {
            case "When": WhenParser(subStrings); return;
            case "say": SayThink(line,'S');return;
            case "think": SayThink(line, 'T');return;
            case "TL": Turn("L", float.Parse(subStrings[1])); return;
            case "TR": Turn("R", -float.Parse(subStrings[1])); return;
            case "MV": Move(float.Parse(subStrings[1])); return;
            case "CH": ChangePosition(subStrings[1], float.Parse(subStrings[2])); return;
            case "SET": SetPosition(subStrings[1], float.Parse(subStrings[2])); return;
            case "GO": SetPosition("X", float.Parse(subStrings[1])); SetPosition("Y", float.Parse(subStrings[2])); return;
            case "repeat": Parserepeat(line, subStrings[1]); return;
            case "repeatuntil": RepeatUntil(line); return;
            case "wait": Wait(float.Parse(subStrings[1])); return;
            case "waituntil": StartCoroutine(WaitUntil(line)); return;
            case "If": IfElseStatement(line, subStrings[1], subStrings[2], subStrings[3]); return;

        }

    }

    public void RepeatUntil(string line)
    {
        string[] subStrings = line.Split('#');
        string body = subStrings[1];
        string[] lines = body.Split(';');
        bool condition  = checkcondition(subStrings[0].Substring(0,subStrings[0].Length-1));
        if (!condition)
        {
        commands.InsertRange(0, lines);
        commands.Insert(lines.Length, line);
        }

        execute = true;

    }

    private void SetPosition(string variable, float value)
    {
        Vector3 newPos = Vector3.zero;
        if (variable == "X")
        {
            newPos = new Vector3(value, transform.position.y, transform.position.z);
        }
        else
        {
            newPos = new Vector3(transform.position.x, value, transform.position.z);
        }
        transform.position = newPos;
    }

    private void ChangePosition(string variable, float value)
    {
        print("Value" + value);
        if (variable == "X")
        {

            transform.Translate(value, 0, 0);
        }
        else
        {
            transform.Translate(0, value, 0);
        }
    }


    private void Turn(String direction, float value)
    {
        transform.Rotate(0, 0, value);
    }

    private void Move(float value)
    {
        transform.position += transform.right * value;
    }


    public void ButtonClicked()
    {
        if (flagUsed)
        {
            execute = true;
            flagUsed = false;
        }
    }
    public void SayThink(string line, char code)
    {
        string message = "";
        float time;
        Boolean isTime = line.Contains("for");
        
        // say Hello World! for 2 secs
        if (isTime)
        {
            string[] substrings = line.Split(new string[] { " for " }, StringSplitOptions.None);
            string[] messageStrings = substrings[0].Split();
            for (int i = 1; i < messageStrings.Length; i++)
            {
                message += messageStrings[i] + " ";
            }
            time = float.Parse(substrings[1].Split()[0]);


            if(code == 'S')
            {
                say.text = message;
                sayObject.SetActive(true);
                thinkObject.SetActive(false);
            }
            else if (code == 'T')
            {
                think.text = message;
                sayObject.SetActive(false);
                thinkObject.SetActive(true);
            }

            
            Wait(time);
            InvokeRepeating("ClearSay", time, 0);
        }
        else // say Hello World!
        {
            string[] messageStrings = line.Split();
            for (int i = 1; i < messageStrings.Length; i++)
            {
                message += messageStrings[i] + " ";
            }
            if (code == 'S')
            {
                say.text = message;
                sayObject.SetActive(true);
                thinkObject.SetActive(false);
            }
            else if (code == 'T')
            {
                think.text = message;
                sayObject.SetActive(false);
                thinkObject.SetActive(true);
            }
        }

        // play meo music !!!
        //Wait(time);
        //say.text = message;







    }

    private void ClearSay()
    {
        say.text = "";
        think.text = "";
        sayObject.SetActive(false);
        thinkObject.SetActive(false);
    }

    public void Wait(float time)
    {
        execute = false;
        InvokeRepeating("Execute", time, 0);

    }
    IEnumerator WaitUntil(string line)
    {
        execute = false;
        yield return new WaitUntil(() => checkcondition(line));
        execute = true;

    }

    public bool checkcondition(string line)
    {
        // execute = false;
        bool satisfied = false;
        string[] substrings = line.Split(new[] { ' ' }, 2);
        line = substrings[1];

        if (line.Contains("and"))
        {
            substrings = line.Split(new string[] { "and" }, StringSplitOptions.None);
            string[] conditions = substrings[0].Split();
            bool c1 = Operators(parseOperand(conditions[0]), parseOperand(conditions[2]), conditions[1]);
            conditions = substrings[1].Substring(1).Split();
            bool c2 = Operators(parseOperand(conditions[0]), parseOperand(conditions[2]), conditions[1]);
            satisfied = c1 && c2;
        }
        else if (line.Contains("or"))
        {
            substrings = line.Split(new string[] { "or" }, StringSplitOptions.None);
            string[] conditions = substrings[0].Split();
            bool c1 = Operators(parseOperand(conditions[0]), parseOperand(conditions[2]), conditions[1]);
            conditions = substrings[1].Substring(1).Split();
            bool c2 = Operators(parseOperand(conditions[0]), parseOperand(conditions[2]), conditions[1]);
            satisfied = c1 | c2;
        }
        else
        {
            substrings = line.Split();
            satisfied = Operators(parseOperand(substrings[0]), parseOperand(substrings[2]), substrings[1]);
        }
        return satisfied;
    }
    IEnumerator Whenkey(string key)
    {
        execute = false;
        yield return new WaitUntil(() => Input.GetKeyDown(key));
        execute = true;
    }

    public void WhenParser(string[] substrings)
    {
        
        if (substrings[1] == "KP")
        {
            StartCoroutine(Whenkey(substrings[2].ToLower()));
        }
        else
        {
            flagUsed = true;
        }
        execute = false;

    }

    public void Execute()
    {
        execute = true;
    }

    public bool Operators(float v1, float V2, string op)
    {
        if (op == "=")
            return v1 == V2;
        if (op == ">")
            return v1 > V2;
        if (op == "<")
            return v1 < V2;

        return false;

    }
    public void IfElseStatement(string line, string variable, string op, string v)
    {
        bool containsElse = line.Contains("else");
        string[] substrings = line.Split('#');
        string body = substrings[1];
        bool satisfied = checkcondition(substrings[0].Substring(0, substrings[0].Length - 1));
        line = line.Substring(3);

        /*
        if (line.Contains("and"))
        {
            substrings = line.Split(new string[] { "and" }, StringSplitOptions.None);
            string[] conditions = substrings[0].Split();
            bool c1 = operators(parseOperand(conditions[0]), parseOperand(conditions[2]), conditions[1]);
            conditions = substrings[1].Substring(1).Split();
            print("conditions : " + String.Join(",", conditions));
            bool c2 = operators(parseOperand(conditions[0]), parseOperand(conditions[2]), conditions[1]);
            satisfied = c1 && c2;
        }
        else if (line.Contains("or"))
        {
            substrings = line.Split(new string[] { "or" }, StringSplitOptions.None);
            string[] conditions = substrings[0].Split();
            bool c1 = operators(parseOperand(conditions[0]), parseOperand(conditions[2]), conditions[1]);
            conditions = substrings[1].Substring(1).Split();
            bool c2 = operators(parseOperand(conditions[0]), parseOperand(conditions[2]), conditions[1]);
            satisfied = c1 | c2;
        }
        else
        {
            substrings = line.Split();
            satisfied = operators(parseOperand(substrings[0]), parseOperand(substrings[2]), substrings[1]);
        }
        */

        if (satisfied)
        {
            string[] subs = line.Split('#');
            body = subs[1];
            string[] lines = body.Split(';');
            commands.InsertRange(0, lines);
        }
        else
        {
            if (containsElse)
            {
                string[] subs = line.Split(new string[] { "else" }, StringSplitOptions.None);
                subs = subs[1].Substring(1).Split('#');
                body = subs[1];
                string[] lines = body.Split(';');
                commands.InsertRange(0, lines);
            }
            return;
        }

    }

    public float parseOperand(string operand)

    {
        switch (operand)
        {
            case "xPos": return transform.position.x;
            case "yPos": return transform.position.y;
            case "dir": return transform.rotation.eulerAngles.z;
            default: return float.Parse(operand);
        }

    }

}
