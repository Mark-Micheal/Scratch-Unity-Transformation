using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using System.Linq;


public class Motion : MonoBehaviour
{
    public TextMeshPro text;
    public TMP_InputField input;
    private string textFile;
    private bool flagUsed;
    private float rotationIncrement;
    private float positionIncrement;
    public TextMeshProUGUI say;
    public float x;
    public float factor = 0.1f;
    private bool execute = true;
    private bool repeat;
    private string[] lines;
    List<String> commands = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        // textFile = "/buildTest.txt";
        textFile = "Assets/test.txt";
        lines = File.ReadAllLines(textFile);
        commands = lines.ToList();
        print(String.Join(",", commands));

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
                print("stack : " + String.Join(",", commands));
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
            case "TL": Turn("L", float.Parse(subStrings[1])); return;
            case "TR": Turn("R", -float.Parse(subStrings[1])); return;
            case "MV": Move(float.Parse(subStrings[1])); return;
            case "CH": ChangePosition(subStrings[1], float.Parse(subStrings[2])); return;
            case "SET": SetPosition(subStrings[1], float.Parse(subStrings[2])); return;
            case "GO": SetPosition("X", float.Parse(subStrings[1])); SetPosition("Y", float.Parse(subStrings[2])); return;
            case "repeat": Parserepeat(line, subStrings[1]); return;
            case "repeatuntil": RepeatUntil(line); return;
            case "Wait": Wait(float.Parse(subStrings[1])); return;
            case "waituntil": StartCoroutine(WaitUntil(line)); return;
            case "If": IfElseStatement(line, subStrings[1], subStrings[2], subStrings[3]); return;

        }

    }

    public void RepeatUntil(string line)
    {
        string[] subStrings = line.Split('#');
        string body = subStrings[1];
        string[] lines = body.Split(';');
        print(subStrings[0].Substring(0, subStrings.Length - 1));
        bool condition  = checkcondition(subStrings[0].Substring(0,subStrings[0].Length-1));
        print("ccc" + condition);
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
            newPos = new Vector3(value * factor, transform.position.y, transform.position.z);
        }
        else
        {
            newPos = new Vector3(transform.position.x, value * factor, transform.position.z);
        }
        transform.position = newPos;
    }

    private void ChangePosition(string variable, float value)
    {
        if (variable == "X")
        {
            transform.Translate(value * factor, 0, 0);
        }
        else
        {
            transform.Translate(0, value * factor, 0);
        }
    }


    private void Turn(String direction, float value)
    {
        transform.Rotate(0, 0, value);
    }

    private void Move(float value)
    {
        transform.position += transform.right * value * factor;
    }


    public void ButtonClicked()
    {
        String output = input.text;
        text.SetText(output);

    }
    public void Say(string message, float time, Boolean isTime)
    {
        // play meo music !!!
        Wait(time);
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
        execute = false;
        InvokeRepeating("Excute", time, 0);

    }
    IEnumerator WaitUntil(string line)
    {
        execute = false;
        yield return new WaitUntil(() => checkcondition(line));
        execute = true;

    }

    public bool checkcondition(string line)
    {
        execute = false;
        bool satisfied = false;
        string[] substrings = line.Split(new[] { ' ' }, 2);
        line = substrings[1];
        print(line);

        if (line.Contains("and"))
        {
            substrings = line.Split(new string[] { "and" }, StringSplitOptions.None);
            string[] conditions = substrings[0].Split();
            bool c1 = operators(parseOperand(conditions[0]), parseOperand(conditions[2]), conditions[1]);
            conditions = substrings[1].Substring(1).Split();
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
            Whenkey(substrings[2]);
        }
        else
        {
            flagUsed = true;
        }

    }

    public void Excute()
    {
        execute = true;
    }

    public bool operators(float v1, float V2, string op)
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
        string[] substrings;
        bool satisfied = false;
        line = line.Substring(3);
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

        if (satisfied)
        {
            string[] subs = line.Split('#');
            string body = subs[1];
            string[] lines = body.Split(';');
            commands.InsertRange(0, lines);
        }
        else
        {
            if (containsElse)
            {
                string[] subs = line.Split(new string[] { "else" }, StringSplitOptions.None);
                subs = subs[1].Substring(1).Split('#');
                string body = subs[1];
                string[] lines = body.Split(';');
                commands.InsertRange(0, lines);
            }
            return;
        }

    }

    public float parseOperand(string operand)

    {
        print("op :: " + operand);
        switch (operand)
        {
            case "xPos": return transform.position.x;
            case "yPos": return transform.position.y;
            case "dir": return transform.rotation.eulerAngles.z;
            default: return float.Parse(operand);
        }

    }

}
