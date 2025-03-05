using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private List<Dictionary<string, string>> dataToPutInUI;
    [SerializeField] private TextMeshProUGUI content;
    string wholeData = "";
    private void Awake()
    {
        dataToPutInUI = new List<Dictionary<string, string>>();

    }

    private void Start()
    {
        UpdateWholeData();
    }

    private void UpdateWholeData()
    {
        foreach (Dictionary<string, string> debugData in dataToPutInUI)
        {
            wholeData += string.Join(", ", debugData.Select(kv => $"{kv.Key}: {kv.Value}")) + "\n";
        }
    }

    private void OnEnable()
    {
        PlayerMovement.OnSendDebugData += AddDataToUI;
    }

    private void AddDataToUI(Dictionary<string, string> data)
    {
        // future implementation
        if (dataToPutInUI.Count < 1)
        {
            dataToPutInUI.Add(data);
            UpdateWholeData();
        }
    }

    private void Update()
    {
        ShowDatainUI();
    }

    private void ShowDatainUI()
    {
        // add data to content 
        if (content != null)
        {

            // add content to UI
            content.text = wholeData;
            
        }

    }
}
