using System;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;

namespace HundoHelper.Models
{
    public class HiddenPackage : ICollectible
    {        
        public float x;
        public float y;
        public float z;
        public byte Type;
        private string _name;

        public event NameChanged OnNameChanged;

        public bool NotPickedUp => Type == 6;

        public int Index => (int)Location;

        public PackageLocation Location { get; set; }

        public HiddenPackage(PackageLocation location)
        {
            Location = location;
        }
        public void Update()
        {
            Type = Utils.GetPackageType(Utils.memoryAddresses["packagesArrayStart"], Index);
        }

        public double DistanceTo(XYZcoordinatesF coords)
        {
            return Math.Pow(Math.Pow(x - coords.X, 2) + Math.Pow(y - coords.Y, 2) + Math.Pow(z - coords.Z, 2), 0.5);
        }
        public string Name {
            get {
                if (string.IsNullOrWhiteSpace(_name))
                    return Location.ToString();
                return _name;
            }
            set {
                if (value != _name)
                {
                    _name = value;
                    OnNameChanged?.Invoke();
                }
            }
        }

        public int OrderIndex { get; set; }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct XYZcoordinatesF
    {
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float X;
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float Y;
        [MarshalAs(UnmanagedType.R4, SizeConst = 4)]
        public float Z;

        public XYZcoordinatesF(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public XYZcoordinatesF(string nonsense)
        {
            var split = nonsense.Split(' ');

            X = float.Parse(split[0], CultureInfo.InvariantCulture);
            Y = float.Parse(split[1], CultureInfo.InvariantCulture);
            Z = float.Parse(split[2], CultureInfo.InvariantCulture);
        }
    }
    public enum PackageLocation
    {
        LightHouse, //479.6 -1718.5 15.6
        GreenHouseAtTheBeach, //708.4 -498.2 12.3
        RocksAtTheSea, //-213.0 -1647.1 13.1
        RocksAtTheSea2, //-368.4 -1733.2 11.6
        BehindLanceHouseVCS, //-104.3 -1600.3 10.4
        AfterM4Rampage10MinutesIntoTheRun, //-234.7 -1082.6 13.6
        RoofAfterShotgunUSJ, //88.0 -991.7 19.1
        NearPoolWithArmor, //205.4 -888.9 12.1
        AutocideUnderBridge, //183.1 -652.9 10.8
        PoliceStation, //370.9 -469.5 13.8
        Riot, //298.8 -278.5 11.9
        RoofTSwine,// 321.8 -774.3 22.8
        BehindMalibu, //491.8 -45.3 10.0
        AfterPizzaBoy, //472.8 116.0 11.2
        CopLandAfterRobbery, //379.6 212.9 11.3
        LoveJuiceMersedesHouse, // 290.1 295.4 13.5
        MollLastPackage, //470.9 1123.6 24.5
        MollGashSecondFloor, //407.6 1018.2 25.3
        BeforeWarpToPrintWorks, //561.6 1157.1 18.9
        BeforeWarpToHymanCondoo, //891.8 873.8 16.4
        AutocideGreenRoofOldRoute, //353.7 -564.3 56.4
        MollParking, //306.9 1177.5 17.4
        MarthasUnderBridge, //271.3 932.9 9.8
        MarthasOnTopOfSomeNonsense, //328.7 717.2 22.8
        Gonzales, //473.3 16.4 33.0
        ChaseRoof, //352.5 282.2 19.6
        SSUToilets, //70.1 -479.6 13.6
        UnderTheBridgeWhileListeningToDiazPhoneCall, //53.6 -446.5 7.6
        FourIronOnTheEdge, //266.3 -249.9 36.1
        RaceSixMoll, //413.9 1217.4 18.4
        GAUndergroundParking, //-172.4 -1341.3 3.9
        StuntBoatChallengeUnderBridge, //-233.1 -931.2 9.8
        HookerPercent, //69.7 -796.6 11.7
        SSUAkaTwoBitHitNearMP5Rampage, //107.5 -551.9 14.7
        WasteTheWifeNearBoatParking, //233.9 -132.2 8.0
        WasteTheWifePizza, //424.5 89.3 11.3
        MarthasNearSparrow, //401.6 431.1 11.4
        ShakedownNearVCSGonzalesHotel, //193.9 678.8 12.9
        BehindPenisBuilding, //589.4 36.0 16.7
        OnRoofRightAfterGonzalesBuilding, //519.9 -135.4 35.2
        MarthasLastPackageOnRoofFilsStudio, //-41.8 922.4 19.4
        MarthasFirstPackageInHangas, //-16.1 991.7 10.9
        MarthasThirdPackageBackupOnRaceSix, //60.7 916.5 10.8
        MarthasSecondPackageBehindSharksBuilding, //-68.9 1124.0 17.0
        SupplyAndDemandSharksBuilding, //82.7 1113.8 18.7
        FourIronGuy, //70.5 599.3 14.5
        BeforeGSpotlightUnderTheBridgeGoldClub, //162.4 246.4 12.2
        GoldClubDontFallInWaterDeathRow, //43.1 -150.9 12.2
        GoldClubSRoadShakedown, //-46.6 257.7 24.5
        GoldClubInAPoolBeforeDeathRow, //43.4 -15.0 17.3
        DoubleIPFirstPackage, //-236.9 -588.1 10.3
        DoubleIPSecondPackkagem, //-519.0 -599.3 10.3
        NearRockStarPoolDoubleIP, //-580.5 -422.6 19.8
        PoolBeforeChaseOrOnShakedownHelicopter, //-310.4 -415.5 10.1
        StarfishAfterRainUsj, //-245.4 -323.8 10.2
        AlloyWheelsOfSteel, //-246.9 1219.5 10.9
        RecDrive, //-451.0 1286.6 12.6
        NearHymanCondoo, //-764.3 1266.0 11.5
        BehindStadium, //-1550.1 1403.1 8.7
        DowntownHospital, //-790.8 1119.4 9.8
        VCNMaverick, //-451.5 1119.7 61.7
        GSpotlightOffice, //-567.4 776.1 22.8
        MessingWithTheManParking, //-898.7 430.4 10.9
        NearPhilsPlaceOnRace6, //-979.2 266.7 8.9
        NearHaitiCheckpoint, //-856.3 228.5 12.9
        PhilsSaray, //-1072.5 351.7 11.2
        BehindPhilsPlaceDeathRow, //-1161.6 431.1 11.0
        BehindCaufman, //-975.1 191.9 12.6
        SpookySkeleton, //-1033.4 44.0 11.1
        BehindTheWallCtC, //-994.0 -250.3 10.7
        DeathRowRoofNearPrintworks, //-1104.9 -120.9 20.1
        GreenBuildingCtc, //-1131.6 -355.4 15.0
        WasherRobbery, //-1195.2 -317.7 10.9
        SniperNearUmberto, //-1093.7 -600.1 26.0
        NearUmberto, //-1179.9 -576.3 12.0
        SSA, //-1018.2 -874.1 17.9
        WhyYouDidntPickedUpThisOne, //-855.3 -631.8 11.3
        NearKrugerRampage, //-1179.2 -718.8 22.8
        VicVCNBuilding, //-802.9 -1184.6 11.1
        ShipRPGRampage, //-620.8 -1342.4 16.3
        ShipSSA, //-1024.6 -1494.6 19.4
        EightBalls, //-1090.2 -1199.2 11.2
        OnTheWayToHtC, //-829.2 -1461.0 12.6
        BeforeRaidPickUpInstapass, //-1160.6 -1333.8 14.9
        LooseEndsEnding, //-1369.3 -1255.7 18.2
        LoseEndsHangarRoof, //-1280.9 -1146.5 22.2
        Airport3rdHangar, //-1773.8 -1053.2 14.8
        AirportParking, //-1187.3 -1041.4 14.8
        AirportRoof, //-1477.3 -881.0 32.4
        UnderThePlane, //-1561.8 -1059.5 14.8
        AirportOnTopOfCorridor, //-1349.1 -1090.4 24.5
        AirportOnThePlance, //-1567.3 -1055.5 21.3
        AirportCOaTCISecond, //-1366.4 -928.0 20.8
        AirportCOaTCIFirst, //-1523.5 -819.1 14.8
        AirportBehindFirstBuilding, //-1829.1 -887.6 14.8
        UnderThePlaceNearArmyBase, //-1726.5 -419.2 14.8
        NearArmyBase, //-1737.2 -299.2 14.8
        VIP, //-1328.0 -537.1 13.9
        BeforeHtCDupong, //-1063.5 -965.5 14.8
        AirportCommandBuildingRoof //-1265.8 -1346.9 29.6
    }
}
