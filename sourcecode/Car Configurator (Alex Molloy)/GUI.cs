using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public Transform
        ListContentsBox,
        SelectACarGridContents;
    public CarPartSelector 
        SuspensionSelector,
        BrakesSelector,
        EngineSelector,
        TransmissionSelector,
        FuelTankSelector,
        TireSelector,
        NitrousSelector;
    public GameObject 
        ListMenu,
        ListOption,
        PartSelectorMenu,
        Fade,
        GridPiecePrefab,
        HoverTooltip,
        SelectACarMenu;
    public Slider
        ColourMenuRed,
        ColourMenuGreen,
        ColourMenuBlue;
    List<GameObject> 
        CurrentListOptions = new(),
        CurrentGridPieces = new();
    public TextMeshProUGUI MenuTitle, PriceBreakdownText;
    Vector3 previousPosition;
    public static bool DisableCameraRotation { get; set; }
    void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        Fade.SetActive(true);
    }
    void Start()
    {
        StartCoroutine(StartCarGrid());
        RotateCamera();
    }
    void LateUpdate()
    {
        if (!DisableCameraRotation)
        {
            if (Input.GetMouseButtonDown(0)) { previousPosition = Camera.main.ScreenToViewportPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue()); }
            else if (Input.GetMouseButton(0)) { RotateCamera(); }
        }
        CalculateColour();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F2)) { StartCoroutine(CreatePreviews()); }
#endif
    }
    void RotateCamera()
    {
        Vector3 newPosition = Camera.main.ScreenToViewportPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
        Vector3 direction = previousPosition - newPosition;
        float rotationAroundYAxis = -direction.x * 180;
        Camera.main.transform.position = new Vector3(Car.instance.transform.position.x, 1f, Car.instance.transform.position.z);
        Camera.main.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
        Camera.main.transform.Translate(new Vector3(0, 0, -3.4f));
        previousPosition = newPosition;
    }
    public void OpenListMenu(int index)
    {
        MenuTitle.text = "Select A Part";
        DisableCameraRotation = false;
        PartSelectorMenu.SetActive(false); 
        Type carPart = typeof(Car.CarEngineEnum);
        switch (index)
        {
            case 0: { carPart = typeof(Car.CarModelEnum); MenuTitle.text = "Select A Car"; break; }
            case 1: { carPart = typeof(Car.CarSuspensionEnum); MenuTitle.text = "Suspension"; break; }
            case 2: { carPart = typeof(Car.CarBrakesEnum); MenuTitle.text = "Brakes"; break; }
            case 3: { carPart = typeof(Car.CarEngineEnum); MenuTitle.text = "Engine"; break; }
            case 4: { carPart = typeof(Car.CarTransmissionEnum); MenuTitle.text = "Transmission"; break; }
            case 5: { carPart = typeof(Car.CarFuelTankEnum); MenuTitle.text = "Fuel Tank"; break; }
            case 6: { carPart = typeof(Car.CarTireUpgradesEnum); MenuTitle.text = "Tire Upgrades"; break; }
            case 7: { carPart = typeof(Car.CarNitrousEnum); MenuTitle.text = "Nitrous"; break; }
        }
        ListMenu.SetActive(true);
        int i = 0;
        foreach (string carPartName in Enum.GetNames(carPart))
        {
            GameObject newListOption = Instantiate(ListOption, ListContentsBox);
            CurrentListOptions.Add(newListOption);
            if (carPartName == typeof(Car).GetField("_" + carPart.ToString().Replace("Car+","").Replace("Enum","")).GetValue(Car.instance).ToString())
            { newListOption.transform.Find("Indicator").gameObject.SetActive(true); }
            newListOption.name = carPartName;
            newListOption.GetComponentInChildren<TextMeshProUGUI>().text = carPartName.Replace("_", " ");
            int x = i; newListOption.GetComponent<Button>().onClick.AddListener(delegate { SelectCarPart((Car.CarPartEnum)index, carPartName); });
            newListOption.SetActive(true);
            i++;
        }
    }
    void SelectCarPart(Car.CarPartEnum CarPart, string carPartName)
    {
        CloseListMenu();
        switch (CarPart)
        {
            case Car.CarPartEnum.CarSuspension:
                {
                    Enum.TryParse(carPartName, out Car.CarSuspensionEnum newCarPart);
                    Car.instance.CarSuspension = newCarPart;
                    break;
                }
            case Car.CarPartEnum.CarBrakes:
                {
                    Enum.TryParse(carPartName, out Car.CarBrakesEnum newCarPart);
                    Car.instance.CarBrakes = newCarPart;
                    break;
                }
            case Car.CarPartEnum.CarEngine:
                {
                    Enum.TryParse(carPartName, out Car.CarEngineEnum newCarPart);
                    Car.instance.CarEngine = newCarPart;
                    break;
                }
            case Car.CarPartEnum.CarTransmission:
                {
                    Enum.TryParse(carPartName, out Car.CarTransmissionEnum newCarPart);
                    Car.instance.CarTransmission = newCarPart;
                    break;
                }
            case Car.CarPartEnum.CarFuelTank:
                {
                    Enum.TryParse(carPartName, out Car.CarFuelTankEnum newCarPart);
                    Car.instance.CarFuelTank = newCarPart;
                    break;
                }
            case Car.CarPartEnum.CarTireUpgrades:
                {
                    Enum.TryParse(carPartName, out Car.CarTireUpgradesEnum newCarPart);
                    Car.instance.CarTireUpgrades = newCarPart;
                    break;
                }
            case Car.CarPartEnum.CarNitrous:
                {
                    Enum.TryParse(carPartName, out Car.CarNitrousEnum newCarPart);
                    Car.instance.CarNitrous = newCarPart;
                    break;
                }
        }
    }
    public void CloseListMenu()
    {
        foreach (GameObject Option in CurrentListOptions) { Destroy(Option); }
        ListMenu.SetActive(false);
        MenuTitle.text = Car.instance.CarModel.ToString().Replace("_"," ");
        PartSelectorMenu.SetActive(true);
    }
    IEnumerator StartCarGrid()
    {
        DisableCameraRotation = true;
        foreach (GameObject gridPiece in CurrentGridPieces) { Destroy(gridPiece); }
        int i = 0;
        foreach(GameObject carPrefab in Resources.LoadAll<GameObject>("Prefabs/"))
        {
            GameObject NewGridPiece = Instantiate(GridPiecePrefab, SelectACarGridContents);
            NewGridPiece.name = "(UI, " + i + ")" + carPrefab.name;
            NewGridPiece.GetComponentInChildren<TextMeshProUGUI>().text = carPrefab.name.Replace("_"," ");
            NewGridPiece.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("Textures/Previews/" + carPrefab.name + "_Preview");
            int x = i; NewGridPiece.GetComponent<Button>().onClick.AddListener(delegate { SelectCar(carPrefab.name); });
            CurrentGridPieces.Add(NewGridPiece);
            NewGridPiece.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            i++;
        }
    }
    void SelectCar(string carName)
    {
        Enum.TryParse(carName, out Car.CarModelEnum carModel);
        Car.instance.CarModel = carModel;
        SelectACarMenu.SetActive(false);
        DisableCameraRotation = false;
        MenuTitle.text = Car.instance.CarModel.ToString().Replace("_"," ");
        PartSelectorMenu.SetActive(true);
    }
    public void BackToCarSelect()
    {
        PartSelectorMenu.SetActive(false);
        MenuTitle.text = "Select A Car";
        SelectACarMenu.SetActive(true);
        StartCoroutine(StartCarGrid());
    }
    public void HoverToolTip(int index)
    {
        if (!DisableCameraRotation)
        {
            switch (index)
            {
                case 0: { HoverTooltipSet("Colour", new Vector3(-125, 100, 0)); break; }
                case 1: { HoverTooltipSet("Cost", new Vector3(-125, 0, 0)); break; }
                case 2: { HoverTooltipSet("Change Car", new Vector3(-125, -100, 0)); break; }
            }
        }
        else { HoverTooltip.SetActive(false); }
    }
    void HoverTooltipSet(string text, Vector3 position)
    {
        HoverTooltip.SetActive(false);
        HoverTooltip.SetActive(true);
        HoverTooltip.GetComponentInChildren<TextMeshProUGUI>().text = text;
        HoverTooltip.GetComponent<RectTransform>().localPosition = position;
    }
    void CalculateColour()
    {
        ColourMenuRed.GetComponentInChildren<Image>().color = new Color(ColourMenuRed.value, 0, 0, 1);
        ColourMenuGreen.GetComponentInChildren<Image>().color = new Color(0, ColourMenuGreen.value, 0, 1);
        ColourMenuBlue.GetComponentInChildren<Image>().color = new Color(0, 0, ColourMenuBlue.value, 1);
        Car.instance.CarColor = new Color(ColourMenuRed.value, ColourMenuGreen.value, ColourMenuBlue.value, 1);
    }
    public void PriceBreakdown()
    {
        string breakdown = "Base Car Price = £" + (int)Car.instance.CarModel;
        foreach (string carPartName in Enum.GetNames(typeof(Car.CarPartEnum)))
        {
            if (carPartName == "CarModel") { continue; };
            var carPart = typeof(Car).GetField("_" + carPartName).GetValue(Car.instance);
            if ((int)carPart != 0) { breakdown += "\n+ £" + (int)carPart + " - " + carPart.ToString().Replace("_"," "); }
        }
        if (Car.instance.CarColor != Color.white) { breakdown += "\n+ £5000 - Paintjob"; }
        breakdown += "\nTotal = £" + Car.instance.CarPrice;
        PriceBreakdownText.text = breakdown;
    }
#if UNITY_EDITOR
    IEnumerator CreatePreviews()
    {
        foreach (string carName in Enum.GetNames(typeof(Car.CarModelEnum)))
        {
            Enum.TryParse(carName, out Car.CarModelEnum carModel);
            Car.instance.CarModel = carModel;
            yield return new WaitForSeconds(0.1f);
            Camera.main.transform.SetPositionAndRotation(new Vector3(-1.75f, 1.4f, 2.88f), Quaternion.Euler(15f, 142.9f, 0f));
            PartSelectorMenu.SetActive(false);
            MenuTitle.gameObject.SetActive(false);
            ScreenCapture.CaptureScreenshot("Assets/Resources/Textures/Previews/" + Car.instance.CarModel.ToString() + "_Preview.png");
            yield return new WaitForSeconds(0.15f);
        }
    }
#endif
}