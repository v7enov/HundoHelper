using System;
using System.IO;
using System.Diagnostics;

namespace HundoHelper.Models
{
    public class UniqueStuntJump : ICollectible
    {
        private readonly IntPtr PointerToArrayStart = (IntPtr)0x81EEF4;
        private string _name;

        public USJLocation Location;
        public int IndexInMemory => (int)Location;
        public bool IsDone { get; private set; }
        public string Name {
            get {
                if (string.IsNullOrWhiteSpace(_name))
                    return Location.ToString();
                return _name;
            }
            set {
                if (value != _name)
                    _name = value;
            }
        }

    public int OrderIndex { get; set; }

        public void Update()
        {
            var offset = IntPtr.Add(PointerToArrayStart, (IndexInMemory - 1) * 4);
            Debug.WriteLine($"Update USJ: {Utils.ReadStruct<int>(offset, false)}; idx: {IndexInMemory}; Location: {Location.ToString()}");
            IsDone = Utils.ReadStruct<int>(offset, false) == 1;
        }

        public UniqueStuntJump(USJLocation location)
        {
            Location = location;
        }
    }

    public enum USJLocation
    {
        SecondStairsToEast = 1, //-1487.781 -1044.546 18.707 // 1
        ToTheAirportRoof = 2, //-1352.695 -755.212 28.673 //2
        OutOfTheAirportToRoof = 3, //-1216.49 -911.833 19.0 //3
        OutOfTheAirportToTheParking = 4, //-1252.139 -1054.685 22.016 //4
        AirportStairsToWest = 5, //-1551.685 -1075.674 19.121 //5
        ToTheWestOverRotatingThing = 6, //-1595.712 -1272.881 19.068 //6
        ToTheEastOverRotatingThing = 7, //-1553.337 -1230.952 17.194 //7
        FirstStairsToEast = 8, //-1340.022 -998.257 19.115 //8
        ToTheFilmStudio = 9, //24.721 897.801 25.103 //9
        ColonelSecond = 25, //-321.028 -1379.498 10.929 //25
        ColonelFirst = 26, //-321.028 -1276.589 10.929 //26
        HogTied = 11, //-674.345 1162.422 29.916 //11
        GSpotlightFirst = 12, //-529.84 830.062 98.717 //12
        GSpotlightSecond = 13, //-839.022 1153.526 31.94 //13
        GSpotlightLastOne = 14, //-312.447 1109.196 47.741 //14
        TwoBitHitFirst = 15, //-1011.583 -30.098 15.181 //15
        TwoBitHitSecond = 16, //-942.702 -114.506 15.181 //16
        ToHaitiCheckpoint = 17, //-900.789 260.804 15.915 //17
        BeforeStuntBoat = 18, //-1041.895 -569.323 21.855 //18
        TSwineAkaAutocideOutOfRoof = 19, //208.993 -963.672 19.967 //19
        NearShotgunPickup = 20, //46.115 -964.415 25.883 //20
        ConeCrazy = 22, //110.481 -1230.6 35.67 //22
        DoubleUSJFirst = 23, //7.435 -1245.895 17.752 //23
        DoubleUSJSecond = 24, //9.103 -1326.505 20.361 //24
        BehindOceanView = 27, //218.05 -1152.0 12.84 //27
        AnnoyingWoodenPlanks = 28, //259.056 -945.833 12.84 //28
        OverARiverToTheStarfish = 30, //284.4732 -494.1143 16.0 //30
        TSwineToRoofWithAPackage = 31, //370.79 -709.863 19.895 //31
        GA = 32, //461.589 -522.23 18.931 //32
        TSwineFirst = 33, //454.105 -504.736 18.931 //33
        BaB = 34, //460.91 -383.362 14.222 //34
        OverARiverToPoliceDep = 35, //259.041 -480.608 14.688 //35
        FourIron = 10, //317.2051 -223.2012 42.3731 //10
        FromMalibuToAvery = 29, //444.5 -118.4 16.0 //29
        NoEscape = 21, //435.8542 -334.3212 15.8977 //21
        FuckRainStarfish = 36 //-346.818 -290.741 12.926 //36
    }
}
