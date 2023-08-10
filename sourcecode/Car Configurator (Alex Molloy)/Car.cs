using UnityEngine;

public class Car : MonoBehaviour
{
    public static Car instance { get; private set; }
    public string CarName;
    public float CarPrice;
    public GameObject LoadedModel;
    public Color CarColor
    {  
        get { return _CarColor; }
        set 
        { 
            if (value != _CarColor) { CarCalculatePrice(); }
            _CarColor = value;
        }
    }
    public Color _CarColor;
    public enum CarPartEnum
    {
        CarModel,
        CarSuspension,
        CarBrakes,
        CarEngine,
        CarTransmission,
        CarFuelTank,
        CarTireUpgrades,
        CarNitrous
    }
    public CarModelEnum CarModel
    {
        get { return _CarModel; }
        set 
        { 
            _CarModel = value;
            Debug.Log("set CarModel to " + value.ToString());
            CarCalculatePrice();
            LoadCarModel();
        }
    }
    public CarModelEnum _CarModel = 0;

    public CarSuspensionEnum CarSuspension
    {
        get { return _CarSuspension; }
        set 
        {
            _CarSuspension = value;
            Debug.Log("set CarSuspension to " + value.ToString());
            CarCalculatePrice();
        }
    }
    public CarSuspensionEnum _CarSuspension = 0;

    public CarBrakesEnum CarBrakes
    {
        get { return _CarBrakes; }
        set 
        {
            _CarBrakes = value;
            Debug.Log("set CarBrakes to " + value.ToString());
            CarCalculatePrice(); 
        }
    }
    public CarBrakesEnum _CarBrakes = 0;

    public CarEngineEnum CarEngine
    {
        get { return _CarEngine; }
        set 
        {
            _CarEngine = value;
            Debug.Log("set CarEngine to " + value.ToString());
            CarCalculatePrice();
        }
    }
    public CarEngineEnum _CarEngine = 0;

    public CarTransmissionEnum CarTransmission
    {
        get { return _CarTransmission; }
        set 
        {
            _CarTransmission = value;
            Debug.Log("set CarTransmission to " + value.ToString());
            CarCalculatePrice();
        }
    }
    public CarTransmissionEnum _CarTransmission = 0;

    public CarFuelTankEnum CarFuelTank
    {
        get { return _CarFuelTank; }
        set 
        {
            _CarFuelTank = value;
            Debug.Log("set CarFuelTank to " + value.ToString());
            CarCalculatePrice(); 
        }
    }
    public CarFuelTankEnum _CarFuelTank = 0;

    public CarTireUpgradesEnum CarTireUpgrades
    {
        get { return _CarTireUpgrades; }
        set
        {
            _CarTireUpgrades = value;
            Debug.Log("set CarTireUpgrades to " + value.ToString());
            CarCalculatePrice(); 
        }
    }
    public CarTireUpgradesEnum _CarTireUpgrades = 0;
    public CarNitrousEnum CarNitrous
    {
        get { return _CarNitrous; }
        set
        {
            _CarNitrous = value;
            Debug.Log("set CarNitrous to " + value.ToString());
            CarCalculatePrice(); 
        }
    }
    public CarNitrousEnum _CarNitrous = 0;

    public enum CarModelEnum
    { // imagine this is the GTA:V economy
        Audi_Sport_Quattro_S1 = 75000,
        Citoren_Xsara_WRC = 85000,
        Fiat_131 = 90000,
        Ford_Escort_RS_1800 = 95000,
        Ford_Focus_RS_WRC = 100000,
        Ford_RS_200 = 105000,
        Lancia_037_Stradale = 110000,
        Lancia_Stratos_Group_4 = 115000,
        MG_Metro_6R4 = 120000,
        Mitsubishi_Lancer_GSR = 125000,
        Morris_Mini_Cooper = 130000,
        Opel_Ascona_A = 135000,
        Peugeot_205 = 140000,
        Peugeot_405_T16 = 145000,
        Porsche_959_Paris_Dakar = 150000,
        Renault_Alpine_A110 = 155000,
        Subaru_Impreza_WRC = 160000,
        Suzuki_Escudo = 165000,
        Toyota_Celica_GT = 170000,
        Audi = 1000000,
        RaceCar1 = 175000,
        RaceCar2 = 180000,
        Lambo = 2000000
    }
    public enum CarSuspensionEnum
    {
        Stock_Suspension = 0,
        Lowered_Suspension = 20000,
        Street_Suspension = 30000,
        Sport_Suspension = 40000,
        Competition_Suspension = 50000
    }
    public enum CarBrakesEnum
    {
        Stock_Brakes = 0,
        Street_Brakes = 20000,
        Sport_Brakes = 30000,
        Race_Brakes = 40000
    }
    public enum CarEngineEnum
    {
        Stock_Engine = 0,
        EMS_Upgrade_Level_1 = 20000,
        EMS_Upgrade_Level_2 = 30000,
        EMS_Upgrade_Level_3 = 40000,
        EMS_Upgrade_Level_4 = 50000
    }
    public enum CarTransmissionEnum
    {
        Stock_Transmission = 0,
        Street_Transmission = 20000,
        Sport_Transmission = 30000,
        Race_Transmission = 40000
    }
    public enum CarFuelTankEnum
    {
        Stock_Fuel_Tank = 0,
        Extended_Fuel_Tank = 20000,
        Large_Fuel_Tank = 30000,
        Gigantic_Fuel_Tank = 40000
    }
    public enum CarTireUpgradesEnum
    {
        Stock_Tires = 0,
        Bulletproof_Tires = 100000,
        Indestructible_Tires = 1000000
    }
    public enum CarNitrousEnum
    {
        Stock_Nitrous = 0,
        Nitrous_Level_1 = 5000,
        Nitrous_Level_2 = 25000,
        Nitrous_Level_3 = 50000,
    }
    void Awake() { instance = this; }
    void Start() { CarCalculatePrice(); }
    void LateUpdate()
    {
        CarName = CarModel.ToString().Replace("_", " ");
        if (LoadedModel != null) { transform.Find(CarName.Replace("_", " ") + "/Body").GetComponent<MeshRenderer>().materials[0].color = CarColor; }
    }
    void CarCalculatePrice()
    {
        CarPrice = 0;
        foreach (string carPart in System.Enum.GetNames(typeof(CarPartEnum))) { CarPrice += (int)typeof(Car).GetField("_" + carPart).GetValue(this); }
        if (CarColor != Color.white) { CarPrice += 5000; }
        Debug.Log("new car price is: £" + CarPrice);
    }
    void LoadCarModel()
    {
        GameObject newCar = Instantiate(Resources.Load<GameObject>("Prefabs/" + CarModel.ToString()), transform);
        newCar.name = CarModel.ToString().Replace("_"," ");
        Destroy(LoadedModel);
        LoadedModel = newCar;
    }
}