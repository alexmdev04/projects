using TMPro;
using UnityEngine;

public class CarPartSelector : MonoBehaviour
{
    public Car.CarPartEnum CarPart;
    TextMeshProUGUI Label;
    void Awake() { Label = GetComponentInChildren<TextMeshProUGUI>(); }
    void FixedUpdate() 
    {
        Label.text = "<color=#A3F9FF>" + CarPart.ToString().Replace("Car","") + "</color>\r\n<color=#B4B4B4>" + 
            typeof(Car).GetField("_" + CarPart.ToString()).GetValue(Car.instance).ToString().Replace("_", " ") + "</color>";
    }
}