using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;

namespace HundoHelper.Models
{

    public enum TAXI_DESTINATION
    {
        UNKNOWN,
        HOSPITAL,
        AMMUNATION,
        POLICE_STATION,
        BIKE_STORE
    }
    public class CheckList : INotifyPropertyChanged
    {
        private readonly Timer checkTimer;
        private readonly Timer vcHandleTimer;
        private int packagesCollected;
        private float robberiesMade;
        private int rampagesDone;
        private int usjsDone;
        private int car1;
        private int car2;
        private int car3;
        private TaxiCoordinates taxiDestination;
        private int tbdCounter;
        private string lastMissionName;
        public static List<ICollectible> collectibles = new List<ICollectible>();
        private Task tbdCounterTask;
        private bool tbdTaskTerminated;
        public static SettingsModel settings;

        public int PackagesCollected {
            get => packagesCollected;
            private set {
                if (packagesCollected == value) return;
                packagesCollected = value;
                OnPropertyChanged(nameof(PackagesCollected));
                UpdateCollectibles<HiddenPackage>();
                OnPropertyChanged(nameof(NotCollectedPackages));
            }
        }

        public IEnumerable<Robbery> NotDoneRobberies => collectibles.OfType<Robbery>().Where(r => !r.IsDone).OrderBy(r => r.OrderIndex).Take(5);

        public IEnumerable<UniqueStuntJump> NotDoneUsjs => collectibles.OfType<UniqueStuntJump>().Where(u => !u.IsDone).OrderBy(u => u.OrderIndex).Take(5);

        public IEnumerable<HiddenPackage> NotCollectedPackages => collectibles.OfType<HiddenPackage>().Where(p => p.NotPickedUp).OrderBy(p => p.OrderIndex).Take(5);


        public float RobberiesMade {
            get => robberiesMade;
            private set {
                if (robberiesMade == value) return;
                robberiesMade = value;
                OnPropertyChanged(nameof(RobberiesMade));
                UpdateCollectibles<Robbery>();
                OnPropertyChanged(nameof(NotDoneRobberies));
            }
        }
        public int RampagesDone {
            get => rampagesDone;
            private set {
                if (rampagesDone == value) return;
                rampagesDone = value;
                OnPropertyChanged(nameof(RampagesDone));
            }
        }
        public int UsjsDone {
            get => usjsDone;
            private set {
                if (usjsDone == value) return;
                usjsDone = value;
                OnPropertyChanged(nameof(UsjsDone));
                UpdateCollectibles<UniqueStuntJump>();
                OnPropertyChanged(nameof(NotDoneUsjs));
            }
        }

        public int TbdCounter {
            get => tbdCounter;
            private set {
                if (tbdCounter == value) return;
                tbdCounter = value;
                OnPropertyChanged(nameof(TbdCounter));
            }
        }

        public TaxiCoordinates TaxiCoordinate {
            get => taxiDestination;
            private set {
                if (taxiDestination.x == value.x && taxiDestination.y == value.y) return;
                taxiDestination = value;
                OnPropertyChanged(nameof(TaxiCoordinate));
                OnPropertyChanged(nameof(TaxiDestinationToDisplay));
            }
        }

        public string LastMissionName {
            get => lastMissionName;
            private set {
                if (lastMissionName == value) return;
                lastMissionName = value;
                OnPropertyChanged(nameof(LastMissionName));
            }
        }

        public string TaxiDestinationToDisplay {
            get {
                switch (TaxiCoordinate)
                {
                    case TaxiCoordinates c when c.x == (float)-661.7 && c.y == (float)755.7:
                        return TAXI_DESTINATION.POLICE_STATION.ToString();
                    case TaxiCoordinates c when c.x == (float)-800.4 && c.y == (float)1167.8:
                        return TAXI_DESTINATION.HOSPITAL.ToString();
                    case TaxiCoordinates c when c.x == (float)-679.1 && c.y == (float)1193.5:
                        return TAXI_DESTINATION.AMMUNATION.ToString();
                    case TaxiCoordinates c when c.x == (float)-600.6 && c.y == (float)667.3:
                        return TAXI_DESTINATION.BIKE_STORE.ToString();
                    default:
                        return TAXI_DESTINATION.UNKNOWN.ToString();
                }
            }
        }

        public int Car1 {
            get => car1;
            private set {
                if (car1 == value) return;
                car1 = value;
                OnPropertyChanged(nameof(Car1));
            }
        }
        public int Car2 {
            get => car2;
            private set {
                if (car2 == value) return;
                car2 = value;
                OnPropertyChanged(nameof(Car2));
            }
        }
        public int Car3 {
            get => car3;
            private set {
                if (car3 == value) return;
                car3 = value;
                OnPropertyChanged(nameof(Car3));
            }
        }

        public void UpdateCollectibles<T>()
        {
            foreach (var p in collectibles.OfType<T>())
                ((ICollectible)p).Update();
        }

        public CheckList(int intervalToCheck)
        {
            checkTimer = new Timer
            {
                Interval = intervalToCheck
            };
            Utils.GetVcProcessHandle();
            vcHandleTimer = new Timer
            {
                Interval = 1000
            };
            vcHandleTimer.Elapsed += VcHandleTimer_Elapsed;
            vcHandleTimer.Start();
            LoadCollectibles();
            LoadSettings();
            checkTimer.Elapsed += CheckTimer_Elapsed;
            checkTimer.Start();
        }

        private void LoadCollectibles()
        {
            collectibles.AddRange(JsonConvert.DeserializeObject<IEnumerable<HiddenPackage>>(File.ReadAllText("packages.json")));
            collectibles.AddRange(JsonConvert.DeserializeObject<IEnumerable<UniqueStuntJump>>(File.ReadAllText("usjs.json")));
            collectibles.AddRange(JsonConvert.DeserializeObject<IEnumerable<Robbery>>(File.ReadAllText("robberies.json")));
        }

        private void VcHandleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Utils.GetVcProcessHandle();
        }

        private void CheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            checkTimer.Stop();

            try
            {
                ReadProperties();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"CheckTimer_Elapsed: {exception.Message}");
            }
            finally
            {
                if (!checkTimer.Enabled)
                    checkTimer.Start();
            }
        }

        public static void SaveSettings()
        {
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(settings));
        }

        public void LoadSettings()
        {
            settings = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText("settings.json"));
        }

        public static void SaveCollectibles()
        {
            File.WriteAllText("packages.json", JsonConvert.SerializeObject(collectibles.OfType<HiddenPackage>()));
            File.WriteAllText("usjs.json", JsonConvert.SerializeObject(collectibles.OfType<UniqueStuntJump>()));
            File.WriteAllText("robberies.json", JsonConvert.SerializeObject(collectibles.OfType<Robbery>()));
        }

        private void ReadProperties()
        {
            PackagesCollected = Utils.ReadStruct<int>(Utils.memoryAddresses[nameof(PackagesCollected)]);
            RampagesDone = Utils.ReadStruct<int>(Utils.memoryAddresses[nameof(RampagesDone)]);
            UsjsDone = Utils.ReadStruct<int>(Utils.memoryAddresses[nameof(UsjsDone)]);
            RobberiesMade = Utils.ReadStruct<float>(Utils.memoryAddresses[nameof(RobberiesMade)]);

            Car1 = Utils.ReadStruct<int>(Utils.memoryAddresses["ssaSlot1"], false);
            OnPropertyChanged(nameof(Car1));
            Car2 = Utils.ReadStruct<int>(Utils.memoryAddresses["ssaSlot2"], false);
            OnPropertyChanged(nameof(Car2));
            Car3 = Utils.ReadStruct<int>(Utils.memoryAddresses["ssaSlot3"], false);
            OnPropertyChanged(nameof(Car3));

            TaxiCoordinate = Utils.ReadStruct<TaxiCoordinates>(Utils.memoryAddresses[nameof(TaxiCoordinates)], false);


            if (settings.ReadLastMission)
            {
                LastMissionName = Utils.GetLastMissionThread(Utils.memoryAddresses["lastMissionThreadPointer"]).Thread_id;
            }

            if (settings.TbdCounterEnabled)
            {

                var lastMission = Utils.GetLastMissionThread(Utils.memoryAddresses["lastMissionThreadPointer"]);
                LastMissionName = lastMission.Thread_id;

                if (lastMission.Thread_id == "bmx_1" && !tbdTaskTerminated)
                {
                    if (tbdCounterTask != null)
                    {
                        if (!(tbdCounterTask.Status == TaskStatus.Running))
                        {
                            StartTask();
                        }
                    }
                    else
                    {
                        StartTask();
                    }
                }
            }


            void StartTask()
            {
                tbdCounterTask = Task.Factory.StartNew(() =>
                {
                    while (!tbdTaskTerminated)
                    {
                        StartProcessingTbdCounter();
                        Task.Delay(300);

                        if (TbdCounter > 51)
                            tbdTaskTerminated = true;
                    }
                });
            }
        }

        private void StartProcessingTbdCounter()
        {
            TbdCounter = Utils.GetMissionsThreads(Utils.memoryAddresses["lastMissionThreadPointer"]).Count(m => m.Thread_id == "bmx_1") * 2;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
