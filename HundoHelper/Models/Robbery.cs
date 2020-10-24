using System;
using System.IO;
using System.Diagnostics;

namespace HundoHelper.Models
{
    public class Robbery : ICollectible
    {
        private readonly IntPtr PointerToArrayStart = (IntPtr)0x81E288;
        private string _name;
        public RobberyLocation Location { get; set; }
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
        public int IndexInMemory => (int)Location;
        public bool IsDone { get; private set; }
        public int OrderIndex { get; set; }


        public Robbery(RobberyLocation location)
        {
            Location = location;
        }
        public void Update()
        {
            var offset = IntPtr.Add(PointerToArrayStart, IndexInMemory * 4);
            Debug.WriteLine($"Robberies: {Utils.ReadStruct<int>(offset, false)}; idx: {IndexInMemory}; Location: {Location.ToString()}");
            IsDone = Utils.ReadStruct<int>(offset, false) == 1;
        }

    }


    public enum RobberyLocation
    {
        DoubleIP = 1544, //-859.2 -632.7 10.6 //0
        PsychoKiller = 1545, //-854.3 850.0 10.6 //1
        AlloyWheelsOfSteel = 1546, //-830.4 741.9 10.6 //2
        TwoBitHitPharmacy = 1547, //-846.6 -72.6 10.8 //3
        CopLand = 1548, //379.9 210.2 10.6 //4
        ChasePharmacy = 1549, //383.2 759.7 11.0 //5
        ChaseMarket = 1550, //449.7 781.5 12.2 //6
        MallVinylCountdown = 1551, //352.7 1111.3 24.5 //7
        MallGash = 1552, //423.5 1039.4 18.1 //8
        MallNorth = 1553, //468.7 1206.6 18.2 //9
        UmbertoCafe = 1554, //-1167.5 -613.5 11.0 //10
        HaitiLaundry = 1555, //-1192.2 -323.7 11.1 //11
        ToolStoreNearPolice = 886, //202.7 -474.1 10.1 //12
        MallToolStore = 887, //357.7 1016.6 30.1 //13
        HaitiToolStore = 888, //-967.5 -693.2 10.3 //14
    }
}
