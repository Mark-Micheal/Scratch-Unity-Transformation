using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Display : MonoBehaviour
{
    public GameObject cat;
    public TextMeshProUGUI coordinates;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coordinates.SetText("X= " + cat.transform.position.x + "   Y= " + cat.transform.position.y + "   dir= " +  cat.transform.eulerAngles.z);
    }
}
