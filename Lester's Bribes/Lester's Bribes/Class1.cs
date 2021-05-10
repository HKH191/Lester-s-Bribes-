
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;
using NativeUI;
using iFruitAddon2;

namespace Lester_s_Bribes
{

    public class Class1 : Script
    {
        ScriptSettings Config;
        Keys Key1;
        
        bool firstTime = true;
        string ModName = "Lester's Options";
        string Developer = "HKH191";
        string Version = "1";
        public bool CreatePeds;
        public int num;
        public bool boughtvehicle;
        MenuPool modMenuPool;
        UIMenu mainMenu;

        UIMenu Inventory;
        UIMenu Bribes;
        UIMenu TaxiService;

        public int purchasedsnacks;
        public int purchasedarmour;
        public int purchasedvehicle = 0;
        public int wantedlevelprice;
        public bool invunrablewait;
        public int invunrabletimer;
        public bool offradarbool;
        public int offradartimer;
        public bool mobileoptions;
        public Vector3 OfficeMarker1;
        public Vector3 OfficeMarker2;
        public Vector3 OfficeMarker3;
        public Vector3 OfficeMarker4;
        public Vector3 BunkerPos = new Vector3(-3023.31f, 3334.41f, 9f);
        public Vector3 FacilityPos = new Vector3(-2228.79f, 2399.25f, 11f);
        public Vector3 HangerPos = new Vector3(-1146.24f, -3409.77f, 13f);
        public Vector3 MethBusinessPos = new Vector3(1313.79f, 4367.51f, 40.5f);
        public Vector3 Safehouse1Pos = new Vector3(2479.4f, 4086.53f, 37f);
        public Vehicle TaxiCab;
        public bool taxisetup;
        public Blip TaxiBlip;
        public bool Deliveredplayer;
        public int officenum;
        iFruitAddon2.CustomiFruit ifruit;

        public Keys Snack_hotkey;
        public Keys ARMOUR_hotkey;
        public bool Blackoutwait;
        public bool noRestrictedzones;
        public List<Vehicle> SupportVeh = new List<Vehicle>();
        public List<Ped> SupportPed = new List<Ped>();
        public int T;
        public Ped Target;
        public Prop Armour;
        public Prop Armour2;
        public Prop Armour3;
        public Prop Armour4;
        public UIMenu CustomBodyGuardSetup;
        public int RecallTimer;
        public Keys OrderAllEnter=Keys.X;
        public Keys OrderAllExit = Keys.Z;
        public Keys OrderAllFollow = Keys.C;
        public int RobberAtimer = 0;
        public Class1()
        {
            ifruit = new iFruitAddon2.CustomiFruit()
            {
                CenterButtonColor = System.Drawing.Color.Orange,
                LeftButtonColor = System.Drawing.Color.LimeGreen,
                RightButtonColor = System.Drawing.Color.Purple,
                CenterButtonIcon = iFruitAddon2.SoftKeyIcon.Fire,
                LeftButtonIcon = iFruitAddon2.SoftKeyIcon.Police,
                RightButtonIcon = iFruitAddon2.SoftKeyIcon.Website
            };


            ifruit.SetWallpaper(new Wallpaper("char_facebook"));
            //or..
            ifruit.SetWallpaper(Wallpaper.BadgerDefault);
          
            var contact = new iFruitContact("Lesters Bribes");
            contact.Answered += loadMenu;
            contact.DialTimeout = 3000;
            contact.Active = true;

            //set custom icons by instantiating the ContactIcon class
            //set custom icons by instantiating the ContactIcon class
            contact.Icon = ContactIcon.Ammunation;
            contact.Name = "Lesters Bribes";
         
            ifruit.Contacts.Add(contact);
            Aborted += OnShutdown;
            Setup();
            Tick += onTick;
            KeyDown += onKeyDown;
            Interval = 1;
            LoadIniFile("scripts//Lester's Bribes.ini");

            //     Vector3 position = Game.Player.Character.Position;
            // ISSUE: explicit reference operation
            // num = (double)((Vector3)@position).DistanceTo(Agent) < 10.0 ? 1 : 0;

          
         
        }
        public UIResRectangle RectangleGUI = new UIResRectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(0, 0));

        public List<UIMenu> GUIMenus = new List<UIMenu>();

        public void SetBanner(UIMenu menu)
        {
            menu.SetBannerType(RectangleGUI);
        }
        public void CreateBanner()
        {
            RectangleGUI = new UIResRectangle();
            RectangleGUI.Color = System.Drawing.Color.FromArgb(255, 0, 0, 0);
        }

        void Setup()
        {
            CreateBanner();
            modMenuPool = new MenuPool();
            mainMenu = new UIMenu("Lester's Options", "Select an Option");
GUIMenus.Add(            mainMenu );
            modMenuPool.Add(mainMenu);
            Interval = 1;
            Inventory = modMenuPool.AddSubMenu(mainMenu, "Inventory");
GUIMenus.Add(            Inventory );
          
            Bribes = modMenuPool.AddSubMenu(mainMenu, "Bribes");
GUIMenus.Add(            Bribes );
           
            TaxiService = modMenuPool.AddSubMenu(mainMenu, "Taxi Service");
GUIMenus.Add(            TaxiService );
          
            CustomBodyGuardSetup = modMenuPool.AddSubMenu(mainMenu, "Bodyguard Setup");
GUIMenus.Add(            CustomBodyGuardSetup );
        

            OfficeMarker1 = new Vector3(-52.78f, -787.104f, 45);
            OfficeMarker2 = new Vector3(-1548.77f, -641.732f, 28);
            OfficeMarker3 = new Vector3(-1393.46f, -527.473f, 31);
            OfficeMarker4 = new Vector3(-104.988f, -611.686f, 36);



            SetupServices();
            setupInventory();
            SetupBribes();
            CBS();


            for (int i = 0; i < GUIMenus.Count(); i++)
            {
                UIMenu B = GUIMenus[i];

                SetBanner(B);

            }
        }
        public void CBS()
        {
            List<dynamic> PedsC = new List<dynamic>();
            PedsC.Add(1);
            PedsC.Add(2);
            PedsC.Add(3);
            PedsC.Add(4);
            PedsC.Add(5);
            PedsC.Add(6);
            List<dynamic> pedH = new List<dynamic>();
            foreach (PedHash P in (PedHash[])Enum.GetValues(typeof(PedHash)))
            {
                pedH.Add(P);
            }
            List<dynamic> Veh = new List<dynamic>();
            foreach (VehicleHash P in (VehicleHash[])Enum.GetValues(typeof(VehicleHash)))
            {
                Veh.Add(P);
            }
            List<dynamic> Pw = new List<dynamic>();
            foreach (WeaponHash P in (WeaponHash[])Enum.GetValues(typeof(WeaponHash)))
            {
                Pw.Add(P);
            }
            List<dynamic> HPA = new List<dynamic>();
            HPA.Add(100);
            HPA.Add(200);
            HPA.Add(300);
            HPA.Add(400);
            HPA.Add(500);
            HPA.Add(600);
            HPA.Add(700);
            HPA.Add(800);
            HPA.Add(900);
            HPA.Add(1000);
            HPA.Add(1100);
            HPA.Add(1200);
            HPA.Add(1300);
            HPA.Add(1400);
            HPA.Add(1500);
            HPA.Add(1600);
            HPA.Add(1700);
            HPA.Add(1800);
            HPA.Add(1900);
            HPA.Add(2000);
           
            UIMenu submenu5 = modMenuPool.AddSubMenu(CustomBodyGuardSetup, "Custom Body Guards");
GUIMenus.Add( submenu5 );
            
            UIMenuListItem NAI = new UIMenuListItem("Number of AI : ", PedsC, 0);
            submenu5.AddItem(NAI);
            UIMenuListItem pedM = new UIMenuListItem("Ped Model : ", pedH, 0);
            submenu5.AddItem(pedM);
            UIMenuListItem VehM = new UIMenuListItem("Vehicle Model : ", Veh, 0);
            submenu5.AddItem(VehM);
            UIMenuListItem PMW = new UIMenuListItem("Primary Weapon : ", Pw, 0);
            submenu5.AddItem(PMW);
            UIMenuListItem SMW = new UIMenuListItem("Secondary Weapon : ", Pw, 0);
            submenu5.AddItem(SMW);
            UIMenuListItem hp = new UIMenuListItem("Health : ", HPA, 0);
            submenu5.AddItem(hp);
            UIMenuListItem arm = new UIMenuListItem("Armour  : ", HPA, 0);
            submenu5.AddItem(arm);
            UIMenuItem Call = new UIMenuItem("Call Squad");
            submenu5.AddItem(Call);

            submenu5.OnItemSelect += (sender, item, index) => //Normal Weapons
            {
                if (item == Call)
                {
                    UI.Notify("Lester : Your Advanced Bodyguard Squad is on its way to you!");
                    SetupAddvacedSquad(NAI.Index + 1, pedH[pedM.Index], Veh[VehM.Index], Pw[PMW.Index], Pw[SMW.Index], HPA[hp.Index], HPA[arm.Index]);
                }
            };
          }
        public Model RequestModel(string Name)
        {

            var model = new Model(Name);
            model.Request(250);

            // Check the model is valid
            if (model.IsInCdImage && model.IsValid)
            {
                // Ensure the model is loaded before we try to create it in the world
                while (!model.IsLoaded) Script.Wait(50);
                return model;




            }

            // Mark the model as no longer needed to remove it from memory.

            model.MarkAsNoLongerNeeded();
            return model;
        }
        public Model RequestModel(PedHash Name)
        {

            var model = new Model(Name);
            model.Request(250);

            // Check the model is valid
            if (model.IsInCdImage && model.IsValid)
            {
                // Ensure the model is loaded before we try to create it in the world
                while (!model.IsLoaded) Script.Wait(50);
                return model;




            }

            // Mark the model as no longer needed to remove it from memory.

            model.MarkAsNoLongerNeeded();
            return model;
        }
        public Model RequestModel(VehicleHash Name)
        {

            var model = new Model(Name);
            model.Request(250);

            // Check the model is valid
            if (model.IsInCdImage && model.IsValid)
            {
                // Ensure the model is loaded before we try to create it in the world
                while (!model.IsLoaded) Script.Wait(50);
                return model;




            }

            // Mark the model as no longer needed to remove it from memory.

            model.MarkAsNoLongerNeeded();
            return model;
        }
        public void SetupAddvacedSquad(int NoP,PedHash P, VehicleHash V, WeaponHash PW,WeaponHash SW,int HP,int ARM)
        {
            var ee= World.CreateVehicle(RequestModel(V), Game.Player.Character.Position.Around(200));
            SupportVeh.Add(ee);
            for(int i=0;i<NoP;i++)
            {

                var ped = ee.CreatePedOnSeat((VehicleSeat)(i-1), P);
                if (ee.IsSeatFree(VehicleSeat.Driver) == true)
                {

                    ped.Alpha = 254;
                    var Car = ee;
                    ped.Task.WarpOutOfVehicle(Car);

                    ped.Task.WarpIntoVehicle(Car, VehicleSeat.Driver);
                    UI.Notify("NO DRIVER");
                }
                ped.AddBlip();
                ped.CurrentBlip.Sprite = BlipSprite.Enemy;
                ped.CurrentBlip.Color = BlipColor.Blue;
                ped.CurrentBlip.Name = "Heist Partner";
                ped.CanWrithe = false;

                ped.Health = HP;
                ped.Armor = ARM;
                ped.Weapons.Give(PW, 9999, true, true);
                ped.Weapons.Give(SW, 9999, true, true);
                ped.CanRagdoll = false;
                ped.CanSufferCriticalHits = false;
                ped.Task.FightAgainstHatedTargets(200, 10);
                SupportPed.Add(ped);
            }
            if (!ee.Model.IsHelicopter)
            { ee.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(ee, Game.Player.Character.Position, 20, 100, (int)DrivingStyle.Normal); ee.PlaceOnNextStreet(); }
            if (ee.Model.IsHelicopter)
            { ee.Position = new Vector3(ee.Position.X, ee.Position.Y, ee.Position.Z + 100); ee.GetPedOnSeat(VehicleSeat.Driver).Task.FightAgainst(Game.Player.Character); }


        }
        public void SetupVeh(VehicleHash Veh, int SeatCount,int Pedtype)
        {
            var V = World.CreateVehicle(Veh, Game.Player.Character.Position.Around(200));
            SupportVeh.Add(V);
            if (V.Model.IsHelicopter || V.Model.IsPlane)
            {
                V.Position = new Vector3(V.Position.X, V.Position.Y, V.Position.Z + 100);
            }
            else
            {
                V.PlaceOnNextStreet();
            }
            if (SeatCount==1)
            {
                SetupPed(VehicleSeat.Driver, Pedtype, V);
             
      
            }
            if (SeatCount ==2)
            {
                SetupPed(VehicleSeat.Driver, Pedtype, V);
                SetupPed(VehicleSeat.Passenger, Pedtype, V);
                if (!V.Model.IsHelicopter)
                { V.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(V, Game.Player.Character.Position, 20, 100, (int)DrivingStyle.Normal); V.PlaceOnNextStreet(); }
                if (V.Model.IsHelicopter)
                { V.GetPedOnSeat(VehicleSeat.Driver).Task.FightAgainst(Game.Player.Character); }
            }
            if (SeatCount==4)
            {
                SetupPed(VehicleSeat.Driver, Pedtype, V);
                SetupPed(VehicleSeat.LeftRear, Pedtype, V);
                SetupPed(VehicleSeat.Passenger, Pedtype, V);
                SetupPed(VehicleSeat.RightRear, Pedtype, V);
                if (!V.Model.IsHelicopter)
                { V.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(V, Game.Player.Character.Position, 20, 100, (int)DrivingStyle.Normal); V.PlaceOnNextStreet(); }
                if (V.Model.IsHelicopter)
                { V.GetPedOnSeat(VehicleSeat.Driver).Task.FightAgainst(Game.Player.Character); }
            }
        }
        public void SetupPed(VehicleSeat Seat,int Type,Vehicle Veh)
        {
            if(Type==1)
            {
                var P = World.CreatePed(PedHash.FreemodeMale01, Veh.Position);
                P.SetIntoVehicle(Veh, Seat);
                Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
                Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
                Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
                var Ped = P;
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 0, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 1, 91, 10, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 2, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 3, 46, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 4, 84, 10, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 5, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 6, 10, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 7, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 8, 97, 10, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 9, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 10, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 11, 186, 10, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 12, 0, 0, 1);

                Function.Call(Hash.SET_PED_USING_ACTION_MODE, Ped, true, -1, 0);
                Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);

                Function.Call(Hash.SET_PED_STRAFE_CLIPSET, Ped, "MOVE_STRAFE_BALLISTIC");
                Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, Ped, 0x5534A626);
            

                Ped.AddBlip();
                Ped.CurrentBlip.Sprite = BlipSprite.Enemy;
                Ped.CurrentBlip.Color = BlipColor.Blue;
                Ped.CurrentBlip.Name = "Heist Partner";
                Ped.CanWrithe = false;

                Ped.Health = 1200;
                Ped.Armor = 1400;
                Ped.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
                Ped.CanRagdoll = false;
                Ped.CanSufferCriticalHits = false;

                SupportPed.Add(Ped);
                Ped.Alpha = 0;
            }
            if (Type == 2)
            {
                var P = World.CreatePed(PedHash.FreemodeMale01, Veh.Position);
                P.SetIntoVehicle(Veh, Seat);
         
                var ped = P;
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 125, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 17, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 34, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 69, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 128, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 130, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);


                Ped bodyguard =ped;
                PedGroup playerGroup = Game.Player.Character.CurrentPedGroup; // gets the players current group
                Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, bodyguard, playerGroup); // puts the bodyguard into the players group
                Function.Call(Hash.SET_PED_COMBAT_ABILITY, bodyguard, 100); // 100 = attack
                ped.AddBlip();
                ped.CurrentBlip.Sprite = BlipSprite.Enemy;
               ped.CurrentBlip.Color = BlipColor.Blue;
               ped.CurrentBlip.Name = "Heist Partner";
                ped.CanWrithe = false;

                ped.Health = 600;
               ped.Armor = 500;
                ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
                ped.CanRagdoll = false;
                ped.CanSufferCriticalHits = false;
                ped.Task.FightAgainstHatedTargets(200, 10);
                SupportPed.Add(ped);
                ped.Alpha = 0;
            }
            if (Type == 3)
            {
                var P = World.CreatePed(PedHash.FreemodeMale01, Veh.Position);
                P.SetIntoVehicle(Veh, Seat);

                var ped = P;
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 125, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 17, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 34, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 69, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 128, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 130, 0, 1);
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);


                Ped bodyguard = ped;
                PedGroup playerGroup = Game.Player.Character.CurrentPedGroup; // gets the players current group
                Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, bodyguard, playerGroup); // puts the bodyguard into the players group
                Function.Call(Hash.SET_PED_COMBAT_ABILITY, bodyguard, 100); // 100 = attack
                ped.AddBlip();
                ped.CurrentBlip.Sprite = BlipSprite.Enemy;
                ped.CurrentBlip.Color = BlipColor.Blue;
                ped.CurrentBlip.Name = "Heist Partner";
                ped.CanWrithe = false;

                ped.Health = 600;
                ped.Armor = 500;
                ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
                ped.CanRagdoll = false;
                ped.CanSufferCriticalHits = false;
                ped.Task.FightAgainstHatedTargets(200, 10);
                SupportPed.Add(ped);
                ped.Alpha = 0;
            }
           
        }
        void LoadIniFile(string iniName)
        {
            try
            {
                Config = ScriptSettings.Load(iniName);

                Key1 = Config.GetValue<Keys>("Configurations", "Key1", Key1);
                purchasedarmour = Config.GetValue<int>("Setup", "Armour", purchasedarmour);
                purchasedsnacks= Config.GetValue<int>("Setup", "Snacks", purchasedsnacks);
                Snack_hotkey = Config.GetValue<Keys>("Setup", "Snack_hotkey", Snack_hotkey);
                ARMOUR_hotkey = Config.GetValue<Keys>("Setup", "ARMOUR_hotkey", ARMOUR_hotkey);
                OrderAllEnter = Config.GetValue<Keys>("Setup", "OrderAllEnter", OrderAllEnter);
                OrderAllExit = Config.GetValue<Keys>("Setup", "OrderAllExit", OrderAllExit);
                OrderAllFollow = Config.GetValue<Keys>("Setup", "OrderAllFollow", OrderAllFollow);
    }
            catch (Exception e)
            {
                UI.Notify("~r~Error~w~: Config.ini Failed To Load.");
            }
        }
        void SetupServices()
        {
            UIMenu submenu1 = modMenuPool.AddSubMenu(TaxiService, "Biker Services ");
GUIMenus.Add( submenu1 );
         
            UIMenu submenu0 = modMenuPool.AddSubMenu(TaxiService, "CEO Services ");
GUIMenus.Add( submenu0 );
          
            UIMenu submenu2 = modMenuPool.AddSubMenu(TaxiService, "Gun Running Services ");
GUIMenus.Add( submenu2 );
          
            UIMenu submenu3 = modMenuPool.AddSubMenu(TaxiService, "Doomsday Heist Services ");
GUIMenus.Add( submenu3 );
        
            UIMenu submenu4 = modMenuPool.AddSubMenu(TaxiService, "Smugglers Run Services ");
GUIMenus.Add( submenu4 );
          
            UIMenu submenu5 = modMenuPool.AddSubMenu(TaxiService, "Arena War Services");
GUIMenus.Add( submenu5 );
   
            UIMenuItem Taxi = new UIMenuItem("Taxi (Maze Bank) -$1000");
            submenu0.AddItem(Taxi);

            UIMenuItem Taxi2 = new UIMenuItem("Taxi (Lom Bank) -$1000");
            submenu0.AddItem(Taxi2);

            UIMenuItem Taxi3 = new UIMenuItem("Taxi (Del Perro) -$1000");
            submenu0.AddItem(Taxi3);

            UIMenuItem Taxi4 = new UIMenuItem("Taxi (Arcadius) -$1000");
            submenu0.AddItem(Taxi4);

            UIMenuItem Taxi5 = new UIMenuItem("Taxi (Bunker) -$1000");
            submenu2.AddItem(Taxi5);

            UIMenuItem Taxi6 = new UIMenuItem("Taxi (Facility) -$1000");
            submenu3.AddItem(Taxi6);

            UIMenuItem Taxi7 = new UIMenuItem("Taxi (Smuggler's Run Hanger) -$1000");
            submenu4.AddItem(Taxi7);

            UIMenuItem Taxi8 = new UIMenuItem("Taxi (Biker Business) -$1000");
            submenu1.AddItem(Taxi8);
            UIMenuItem Taxi9 = new UIMenuItem("Taxi (Safe House) -$1000");
            submenu1.AddItem(Taxi9);

            UIMenuItem Taxi10 = new UIMenuItem("Taxi (Arena) -$1000");
            submenu5.AddItem(Taxi10);
            submenu1.OnItemSelect += (sender, item, index) => //Normal Weapons
            {
                if (item == Taxi9)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum = 10;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Blue;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Alan : Ok i can arange to have someone pick you up");
                }
                if (item == Taxi9)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum = 9;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Blue;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Clay : Ok i can arange to have someone pick you up");
                }
                if (item == Taxi8)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum = 8;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Blue;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Clay : Ok i can arange to have someone pick you up");
                }
            };
            submenu4.OnItemSelect += (sender, item, index) => //Normal Weapons
            {
                if (item == Taxi7)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum =7;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Blue;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Ron : Ok i can arange to have someone pick you up");
                }
            };
            submenu3.OnItemSelect += (sender, item, index) => //Normal Weapons
            {
                if (item == Taxi6)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum = 6;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Blue;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Lester: Ok i can arange to have someone pick you up");
                }
            };
            submenu2.OnItemSelect += (sender, item, index) => //Normal Weapons
            {
                if (item == Taxi5)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum = 5;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Blue;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Agent 14: Ok i can arange to have someone pick you up");
                    }
                };

            submenu0.OnItemSelect += (sender, item, index) => //Normal Weapons
            {
                if (item == Taxi)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum = 1;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Blue;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Office Assistant: Ok i can arange to have someone pick you up");
                }
                if (item == Taxi2)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum = 2;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Green;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Office Assistant: Ok i can arange to have someone pick you up");
                }
                if (item == Taxi3)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum = 3;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Red;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Office Assistant: Ok i can arange to have someone pick you up");
                }
                if (item == Taxi4)
                {
                    if (TaxiCab != null)
                    {
                        TaxiCab.Delete();
                    }
                    if (TaxiBlip != null)
                    {
                        TaxiBlip.Remove();
                    }
                    var GetRandomNumber = new Random();
                    Game.Player.Money -= 1000;
                    officenum = 4;
                    TaxiCab = World.CreateVehicle(VehicleHash.Superd, Game.Player.Character.Position.Around(GetRandomNumber.Next(200, 500)));
                    TaxiCab.CreatePedOnSeat(VehicleSeat.Driver, PedHash.Bouncer01SMM);
                    TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(TaxiCab, Game.Player.Character.Position, 30, 90, 1);
                    TaxiCab.PlaceOnNextStreet();
                    taxisetup = true;
                    TaxiBlip = World.CreateBlip(TaxiCab.Position);
                    TaxiBlip.Sprite = BlipSprite.Cab;
                    TaxiBlip.Color = BlipColor.Yellow;
                    TaxiBlip.Name = "CEO Super Drop";
                    UI.Notify("Office Assistant: Ok i can arange to have someone pick you up");
                }
            };

        }
        void setupInventory()
        {
            UIMenu submenu0 = modMenuPool.AddSubMenu(Inventory, "Purchase Snacks/Body Armour");
GUIMenus.Add( submenu0 );
           
            UIMenu submenu1 = modMenuPool.AddSubMenu(Inventory, " Use Snacks/Body Armour");
GUIMenus.Add( submenu1 );
         

            UIMenuItem PurchaseSnack = new UIMenuItem("Purchase Snacks : $20");
            submenu0.AddItem(PurchaseSnack);

            UIMenuItem PurchaseArmour = new UIMenuItem("Purchase Body Armour : $2500");
            submenu0.AddItem(PurchaseArmour);


            UIMenuItem UseSnack = new UIMenuItem("Use Snacks");
            submenu1.AddItem(UseSnack);

            UIMenuItem UseArmour = new UIMenuItem("Use Body Armour");
            submenu1.AddItem(UseArmour);
            submenu1.OnItemSelect += (sender, item, index) => //Normal Weapons
            {
               
                if (item == UseSnack)
                {
                    UI.Notify("Purchased Snacks " + purchasedsnacks);
                    if (purchasedsnacks > 0)
                    {
                        var Randomhealth = new Random();
                        Game.Player.Character.Health = Game.Player.Character.Health + Randomhealth.Next(5, 20);
                        Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                        Game.Player.Character.Task.PlayAnimation("amb@code_human_wander_eating_donut_fat@male@base", "static", 1.0f, 2500, false, 0);
                        UI.Notify("Player: Eating a snack ");
                        purchasedsnacks--;
                        Config.SetValue<int>("Setup", "Armour", purchasedarmour);
                        Config.SetValue<int>("Setup", "Snacks", purchasedsnacks);
                        Config.Save();
                            
                    }
                    else
                    {
                        UI.Notify("Lester: you have no snacks ");
                    }
                }
                else
                if (item == UseArmour)
                {
                    UI.Notify("Purchased Armour " + purchasedarmour);
                    if (purchasedarmour > 0)
                    {
                        var Randomhealth = new Random();
                        Game.Player.Character.Armor = Game.Player.Character.Health + Randomhealth.Next(10, 100);
                        UI.Notify("Player: putting on body armor, cover me!");
                        purchasedarmour--;
                        Config.SetValue<int>("Setup", "Armour", purchasedarmour);
                        Config.SetValue<int>("Setup", "Snacks", purchasedsnacks);
                        Config.Save();

                        if(Armour!=null)
                        {
                            Armour.Delete();
                        }
                        Armour = World.CreateProp(RequestModel("prop_ld_armour"), Game.Player.Character.Position, false, false);
                        Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Armour, Game.Player.Character, Game.Player.Character.GetBoneIndex((Bone.SKEL_Spine2)), 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
                    }
                    else
                    {
                        UI.Notify("Lester: you have no body armour ");
                    }
                }
            };
            submenu0.OnItemSelect += (sender, item, index) => //Normal Weapons
            {

                if (item == PurchaseSnack)
                {
                    if (Game.Player.Money > 20)
                    {
                        Game.Player.Money = Game.Player.Money - 20;
                        purchasedsnacks++;
                        Config.SetValue<int>("Setup", "Armour", purchasedarmour);
                        Config.SetValue<int>("Setup", "Snacks", purchasedsnacks);
                        Config.Save();

                        UI.Notify("Agent 14: Here you go , i hope this works");
                    }
                    else
                    {
                        UI.Notify("Agent 14: you do not have enought money to purchase a parachute!");
                    }
                }
                if (item == PurchaseArmour)
                {
                    if (Game.Player.Money > 2500)
                    {
                        Game.Player.Money = Game.Player.Money - 2500;
                        purchasedarmour++;
                        Config.SetValue<int>("Setup", "Armour", purchasedarmour);
                        Config.SetValue<int>("Setup", "Snacks", purchasedsnacks);
                        Config.Save();

                        UI.Notify("Agent 14: Here you go , i hope this works");
                    }
                    else
                    {
                        UI.Notify("Agent 14: you do not have enought money to purchase a parachute!");
                    }
                }
            };
         }
       
  
        
        void SetupBribes()
        {
            UIMenu submenu0 = modMenuPool.AddSubMenu(Bribes, "Player");
GUIMenus.Add( submenu0 );
            
          

            UIMenuItem showPrices = new UIMenuItem("Clear Wanted Level Prices");
            submenu0.AddItem(showPrices);

            UIMenuItem Addarmour = new UIMenuItem("Give Body Armour: $5,000");
            submenu0.AddItem(Addarmour);

            UIMenuItem invunrable = new UIMenuItem("invunrable for 2 minute: $1,000,000");
            submenu0.AddItem(invunrable);

            UIMenuItem offradar = new UIMenuItem("Off Radar for 2 minute: $500,000");
            submenu0.AddItem(offradar);

            UIMenuItem RepairHealth = new UIMenuItem("Heal: $1,000");
            submenu0.AddItem(RepairHealth);

            UIMenuItem parachute = new UIMenuItem("Parachute: $2,000");
            submenu0.AddItem(parachute);

            UIMenuItem Blackout = new UIMenuItem("Blackout for 2 minutes: $750,000");
            submenu0.AddItem(Blackout);

            UIMenuItem Chaos = new UIMenuItem("Chaos: $3,000,000");
            submenu0.AddItem(Chaos);

            UIMenuItem NRZ = new UIMenuItem("No Restricted Zones : $2,200,000");
            submenu0.AddItem(NRZ);


            UIMenuItem JugS = new UIMenuItem("Juggernaut Support : $750,000");
            submenu0.AddItem(JugS);
            UIMenuItem ValS = new UIMenuItem("Valkyrie Support : $1,250,000");
            submenu0.AddItem(ValS);
            UIMenuItem BTS = new UIMenuItem("Bounty Hunter Support : $550,000");
            submenu0.AddItem(BTS);
            UIMenuItem CS = new UIMenuItem("Clear Support Units : $0");
            submenu0.AddItem(CS);
            submenu0.OnItemSelect += (sender, item, index) => //Normal Weapons
            {
             
                if(item == CS)
                {
                    foreach (Ped p in SupportPed)
                    {
                        if (p != null)
                        {
                            p.Delete();
                        }
                    }
                    foreach (Vehicle v in SupportVeh)
                    {
                        if (v != null)
                        {
                            v.Delete();
                        }
                    }

                }
                if (item == BTS)
                {
                    if (Game.Player.Money >550000)
                    {
                        Game.Player.Money = Game.Player.Money - 550000;
                        UI.Notify("Lester : Im calling in some backup for you");
                        SetupVeh(VehicleHash.XLS2, 4, 2);
                    }
                }
                if (item == JugS)
                {
                    if (Game.Player.Money > 750000)
                    {
                        Game.Player.Money = Game.Player.Money - 750000;
                        UI.Notify("Lester : Im calling in some backup for you");
                        SetupVeh(VehicleHash.Mesa3, 4, 1);
                    }
                }
                if (item == ValS)
                {
                    if (Game.Player.Money > 1250000)
                    {
                        Game.Player.Money = Game.Player.Money - 1250000;
                        UI.Notify("Lester : Im calling in some backup for you");
                        SetupVeh(VehicleHash.Valkyrie, 4,2);
                    }
                }
                    if (item == NRZ)
                {
                    if (Game.Player.Money >2200000)
                    {
                        Game.Player.Money = Game.Player.Money - 2200000;
                        UI.Notify("Agent 14: Here you go , i hope this works");
                        noRestrictedzones = true;
                        offradarbool = true;
                    }
                }
            
                if (item == Chaos)
                {
                    if (Game.Player.Money > 3000000)
                    {
                        Game.Player.Money = Game.Player.Money - 3000000;
                        foreach(Vehicle V in World.GetNearbyVehicles(Game.Player.Character.Position,600))
                        {
                            if (V.GetPedOnSeat(VehicleSeat.Driver) != null )
                            {
                                if (V.GetPedOnSeat(VehicleSeat.Driver).IsPlayer == false)
                                {
                                    V.Explode();
                                    V.MarkAsNoLongerNeeded();
                                }
                                
                               
                            }
                            if (V.GetPedOnSeat(VehicleSeat.Driver) == null && V.PreviouslyOwnedByPlayer)
                            {
                                V.Explode();
                                V.MarkAsNoLongerNeeded();
                            }

                          
                        }
                    }
                    else
                    {
                        UI.Notify("Agent 14: you do not have enought money to purchase this!");
                    }
                }
                    if (item == Blackout)

                {
                    if (Game.Player.Money > 500000)
                    {
                        Game.Player.Money = Game.Player.Money - 500000;

                        Game.Player.WantedLevel = 0;
                        offradarbool = true;
                        Blackoutwait = true;
                        Function.Call(Hash._SET_BLACKOUT, true);
                        UI.Notify("Agent 14: Here you go , i hope this works");
                    }
                    else
                    {
                        UI.Notify("Agent 14: you do not have enought money to purchase blackout!");
                    }
                }
                    if (item == parachute)
                {
                    if (Game.Player.Money > 2000)
                    {
                        Game.Player.Money = Game.Player.Money - 2000;


                        Game.Player.Character.Weapons.Give(WeaponHash.Parachute, 1, true, true);

                        UI.Notify("Agent 14: Here you go , i hope this works");
                    }
                    else
                    {
                        UI.Notify("Agent 14: you do not have enought money to purchase a parachute!");
                    }
                }
                if (item == showPrices)
                {
                    UI.Notify("Agent 14: 1 star = $200");
                    UI.Notify("Agent 14: 2 star = $1,000");
                    UI.Notify("Agent 14: 3 star = $10,000");
                    UI.Notify("Agent 14: 4 star = $100,000");
                    UI.Notify("Agent 14: 5 star = $3,000,000");
                }
                if (item == invunrable)
                {
                    if (Game.Player.Money > 1000000)
                    {
                        Game.Player.Money = Game.Player.Money - 1000000;


                        Game.Player.Character.IsInvincible = true;
                        invunrablewait = true;
                        UI.Notify("Agent 14: Here you go , i hope this works");
                    }
                    else
                    {
                        UI.Notify("Agent 14: you do not have enought money to purchase body armour!");
                    }
                }
                if (item == offradar)
                {
                    if (Game.Player.Money > 500000)
                    {
                        Game.Player.Money = Game.Player.Money - 500000;

                        Game.Player.WantedLevel = 0;
                        offradarbool = true;
                        UI.Notify("Agent 14: Here you go , i hope this works");
                    }
                    else
                    {
                        UI.Notify("Agent 14: you do not have enought money to purchase go off radar!");
                    }
                }
                if (item == Addarmour)
                {
                    if (Game.Player.Money > 5000)
                    {
                        Game.Player.Money = Game.Player.Money - 5000;

                        Game.Player.Character.Armor = 500;

                        UI.Notify("Agent 14: Here you go , i hope it fits");
                    }
                    else
                    {
                        UI.Notify("Agent 14: you do not have enought money to purchase to become invincible!");
                    }
                }
                if (item == RepairHealth)
                {
                    if (Game.Player.Money > 1000)
                    {
                        Game.Player.Money = Game.Player.Money - 1000;

                        Game.Player.Character.Health = 100;

                        UI.Notify("Agent 14: Here you go , i hope it fits");
                    }
                    else
                    {
                        UI.Notify("Agent 14: you do not have enought money to purchase body armour!");
                    }
                }

            };


            if (Game.Player.WantedLevel == 0)
            {

                wantedlevelprice = 0;



                UIMenuItem ClearWanted = new UIMenuItem("Clear Wanted Level");
                submenu0.AddItem(ClearWanted);



                submenu0.OnItemSelect += (sender, item, index) => //Normal Weapons
                {
                  
                        if (item == ClearWanted)
                    {
                        if (Game.Player.Money > wantedlevelprice)
                        {
                            Game.Player.Money = Game.Player.Money - wantedlevelprice;

                            Game.Player.WantedLevel = 0;

                            UI.Notify("Agent 14: Fine i'll do it , deverting the cops to a new target");
                        }
                        else
                        {
                            UI.Notify("Agent 14: you do not have enought money to clear your wanted level!");
                        }
                    }


                };
            }

            if (Game.Player.WantedLevel == 1)
            {
                wantedlevelprice = 200;
                UIMenuItem ClearWanted = new UIMenuItem("Clear Wanted Level");
                submenu0.AddItem(ClearWanted);
                submenu0.OnItemSelect += (sender, item, index) => //Normal Weapons
                {
                    if (item == ClearWanted)
                    {
                        if (Game.Player.Money > wantedlevelprice)
                        {
                            Game.Player.Money = Game.Player.Money - wantedlevelprice;

                            Game.Player.WantedLevel = 0;

                            UI.Notify("Agent 14: Fine i'll do it , deverting the cops to a new target");
                        }
                        else
                        {
                            UI.Notify("Agent 14: you do not have enought money to clear your wanted level!");
                        }
                    }


                };
            }
            else
            if (Game.Player.WantedLevel == 2)
            {
                wantedlevelprice = 2500;
                UIMenuItem ClearWanted = new UIMenuItem("Clear Wanted Level");
                submenu0.AddItem(ClearWanted);
                submenu0.OnItemSelect += (sender, item, index) => //Normal Weapons
                {
                    if (item == ClearWanted)
                    {
                        if (Game.Player.Money > wantedlevelprice)
                        {
                            Game.Player.Money = Game.Player.Money - wantedlevelprice;

                            Game.Player.WantedLevel = 0;

                            UI.Notify("Agent 14: Fine i'll do it , deverting the cops to a new target");
                        }
                        else
                        {
                            UI.Notify("Agent 14: you do not have enought money to clear your wanted level!");
                        }
                    }


                };
            }
            else
            if (Game.Player.WantedLevel == 3)
            {
                wantedlevelprice = 10000;
                UIMenuItem ClearWanted = new UIMenuItem("Clear Wanted Level");
                submenu0.AddItem(ClearWanted);
                submenu0.OnItemSelect += (sender, item, index) => //Normal Weapons
                {
                    if (item == ClearWanted)
                    {
                        if (Game.Player.Money > wantedlevelprice)
                        {
                            Game.Player.Money = Game.Player.Money - wantedlevelprice;

                            Game.Player.WantedLevel = 0;

                            UI.Notify("Agent 14: Fine i'll do it , deverting the cops to a new target");
                        }
                        else
                        {
                            UI.Notify("Agent 14: you do not have enought money to clear your wanted level!");
                        }
                    }


                };
            }
            else
            if (Game.Player.WantedLevel == 4)
            {
                wantedlevelprice = 100000;
                UIMenuItem ClearWanted = new UIMenuItem("Clear Wanted Level");
                submenu0.AddItem(ClearWanted);
                submenu0.OnItemSelect += (sender, item, index) => //Normal Weapons
                {
                    if (item == ClearWanted)
                    {
                        if (Game.Player.Money > wantedlevelprice)
                        {
                            Game.Player.Money = Game.Player.Money - wantedlevelprice;

                            Game.Player.WantedLevel = 0;

                            UI.Notify("Agent 14: Fine i'll do it , deverting the cops to a new target");
                        }
                        else
                        {
                            UI.Notify("Agent 14: you do not have enought money to clear your wanted level!");
                        }
                    }


                };
            }
            else
            if (Game.Player.WantedLevel == 5)
            {
                wantedlevelprice = 3000000;
                UIMenuItem ClearWanted = new UIMenuItem("Clear Wanted Level");
                submenu0.AddItem(ClearWanted);
                submenu0.OnItemSelect += (sender, item, index) => //Normal Weapons
                {
                    if (item == ClearWanted)
                    {
                        if (Game.Player.Money > wantedlevelprice)
                        {
                            Game.Player.Money = Game.Player.Money - wantedlevelprice;

                            Game.Player.WantedLevel = 0;

                            UI.Notify("Agent 14: Fine i'll do it , deverting the cops to a new target");
                        }
                        else
                        {
                            UI.Notify("Agent 14: you do not have enought money to clear your wanted level!");
                        }
                    }


                };
            }



        }

      
  
        public void ChopperGunnerTarget(Ped p)
        {
            var Targets = new List<dynamic>();
        
            try
            {
               
                    Ped[] peds = World.GetNearbyPeds(Game.Player.Character.Position, 200);
                    foreach (Ped pd in peds)
                    {
                        if (pd.IsInCombatAgainst(Game.Player.Character))
                        {
                            if (pd.IsAlive)
                                Targets.Add(pd);
                        }
                    }

                
                Random R = new Random();
                int t = R.Next(0, Targets.Count);
                if (Target == null)
                {
                    Target = Targets[t];
                    p.Task.FightAgainst(Target);

                }
                if ((Target!=null && Target.IsDead))
                {
                    Target = Targets[t];
                    p.Task.FightAgainst(Target);
                }
           
               
               
            }
            catch
            {
                UI.Notify("error 232/index");
            }
        }
        void FireGun(Vector3 Source, Ped Owner, Vector3 Target)
        {

            GTA.Model weaponAsset = Function.Call<int>(Hash.GET_HASH_KEY, "VEHICLE_WEAPON_TURRET_TECHNICAL");
            // I believe these are explosive shots.            
            if (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset))
            {
                Function.Call(Hash.REQUEST_WEAPON_ASSET, weaponAsset);
                while (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset))
                { Wait(0); }
            }


            World.ShootBullet(Source, Target.Around(1), Owner, weaponAsset, 3);
            World.ShootBullet(Source, Target.Around(1), Owner, weaponAsset, 3);
            World.ShootBullet(Source, Target.Around(1), Owner, weaponAsset, 3);
        }
        private void onTick(object sender, EventArgs e)
        {

            // Mod info
            ifruit.Update();
            if (firstTime)
            {
                UI.Notify(ModName + " " + Version + " by " + Developer + " Loaded");
                UI.Notify(ModName + ": Thank you for downloading!!");
                firstTime = false;
            }
            // start your script here:
            if (modMenuPool != null && modMenuPool.IsAnyMenuOpen() == true)
                modMenuPool.ProcessMenus();

  
            if (Target!=null)
            {
                GTA.World.DrawMarker(MarkerType.VerticalCylinder, Target.Position, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 100f), System.Drawing.Color.Yellow);
                if (World.GetDistance(Game.Player.Character.Position, Target.Position) > 200f)
                {
                    Target = null;return;
                }
              
                    if (Target.IsDead)
                {
                    Target = null; return;
                }
                  
            }

                if (Deliveredplayer == true && World.GetDistance(Game.Player.Character.Position, TaxiCab.Position) > 100f)
            {
                Deliveredplayer = false;
                TaxiCab.Delete();
            }
            #region New AI
            foreach (Ped RobberA in SupportPed)
            {
                if (RobberA != null)
                {
                    if (World.GetDistance(Game.Player.Character.Position, RobberA.Position) < 20f && RobberA.Alpha==0)
                    {
                        if (RobberA.Alpha==0)
                        {
                          //  UI.Notify("Pass ");
                            RobberA.Alpha = 255;
                        }
                    }
                    if ((World.GetDistance(Game.Player.Character.Position, RobberA.Position) <100f) && RobberA.Alpha!=0)
                    {
                       // UI.Notify("Pass 2");
                        #region Refollow with No Vehicle
                        //if (Lester != null)
                        //{
                        //    if (World.GetDistance(Game.Player.Character.Position, Lester.Position) > 50f)
                        //    {
                        //        Lester.Task.GoTo(Game.Player.Character.Position.Around(1));
                        //    }
                        //    if (World.GetDistance(Game.Player.Character.Position, Lester.Position) < 50f)
                        //    {
                        //        Ped bodyguard = Lester;
                        //        PedGroup playerGroup = Game.Player.Character.CurrentPedGroup; // gets the players current group
                        //        Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, bodyguard, playerGroup); // puts the bodyguard into the players group
                        //        Function.Call(Hash.SET_PED_COMBAT_ABILITY, bodyguard, 100); // 100 = attack
                        //    }
                        //}


                        if (Game.Player.Character.CurrentVehicle == null)
                    {

                        if (RobberA != null)
                        {

                            if (World.GetDistance(Game.Player.Character.Position, RobberA.Position) > 50f)
                            {
                                RobberA.Task.ClearAll();
                                RobberA.Task.RunTo(Game.Player.Character.Position.Around(1));
                                RobberA.Alpha = 254;
                            }
                            if (World.GetDistance(Game.Player.Character.Position, RobberA.Position) < 50f)
                            {
                                    if(RobberA.Alpha==254)
                                    {
                                        RobberA.Task.ClearAll();
                                        RobberA.Alpha = 255;
                                    }
                                 
                                 Ped bodyguard = RobberA;
                                PedGroup playerGroup = Game.Player.Character.CurrentPedGroup; // gets the players current group
                                Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, bodyguard, playerGroup); // puts the bodyguard into the players group
                                Function.Call(Hash.SET_PED_COMBAT_ABILITY, bodyguard, 100); // 100 = attack
                            }
                        }


                    }


                    #endregion
                        #region AddaptiveAi
                    if (RobberA != null)
                    {
                        if (Game.Player.Character.CurrentVehicle == null)
                        {
                            if (RobberA.CurrentVehicle != null)
                            {
                                RobberA.Task.ClearAll();
                                RobberA.Task.LeaveVehicle();
                                RobberA.Alpha = 254;
                            }
                            
                        }
                        if (Game.Player.Character.CurrentVehicle != null)
                        {
                            if (Game.Player.Character.CurrentVehicle.IsSeatFree(VehicleSeat.Any) == false)
                            {
                                if (RobberA.CurrentVehicle == null)
                                {

                                    Vehicle[] V = World.GetNearbyVehicles(Game.Player.Character.Position, 500);
                                    float dist = 20000;
                                    Vehicle v = null;
                                    foreach (Vehicle veh in V)
                                    {
                                        if (veh != Game.Player.Character.CurrentVehicle)
                                        {
                                            if (World.GetDistance(Game.Player.Character.Position, veh.Position) < dist)
                                            {
                                                #region Check
                                                if (veh.GetPedOnSeat(VehicleSeat.Driver) == null)
                                                {
                                                    dist = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                                    v = veh;
                                                }
                                                if (veh.IsSeatFree(VehicleSeat.Any) == true)
                                                {

                                                    if (veh.IsSeatFree(VehicleSeat.Driver) == true)
                                                    {
                                                        dist = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                                        v = veh;
                                                    }

                                                    if (veh.IsSeatFree(VehicleSeat.Driver) == false)
                                                    {
                                                        if (RobberA != null)
                                                        {
                                                            if (veh.GetPedOnSeat(VehicleSeat.Driver) == RobberA)
                                                            {
                                                                dist = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                                                v = veh;
                                                            }
                                                        }

                                                    }


                                                }
                                                #endregion

                                            }
                                        }
                                    }
                                    if (RobberA.LastVehicle != null)
                                    {
                                        if (RobberA.Alpha < 255 && RobberA.IsGettingIntoAVehicle == false)
                                        {
                                            RobberA.Alpha = 255;
                                        }

                                    }
                                    if (v != null)
                                    {

                                        if (v.IsSeatFree(VehicleSeat.Any) == true)
                                        {
                                            Vector3 N = new Vector3(v.Position.X, v.Position.Y, v.Position.Z + 3);
                                            GTA.World.DrawMarker(MarkerType.UpsideDownCone, N, Vector3.Zero, Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), System.Drawing.Color.OrangeRed);
                                            if (RobberA.Alpha == 255)
                                            {
                                                RobberA.Alpha = 254;
                                                if (v.GetPedOnSeat(VehicleSeat.Driver) == null)
                                                {
                                                    RobberA.Task.EnterVehicle(v, VehicleSeat.Driver, 9999, 100);
                                                }
                                                else
                                                if (v.GetPedOnSeat(VehicleSeat.Driver) != null)
                                                {
                                                    RobberA.Task.EnterVehicle(v, VehicleSeat.Any, 9999, 100);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (RobberA.CurrentVehicle != null)
                                {
                                    var ped = RobberA;
                                    int EnemyRelationShipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER");
                                    Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, EnemyRelationShipGroup);
                                    Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
                                    ped.RelationshipGroup = EnemyRelationShipGroup;
                                    if (RobberA.CurrentVehicle.Driver == RobberA)
                                    {

                                        //if (RobberA.Alpha == 254)
                                        //{
                                        //    RobberA.Alpha = 255;
                                        //  //  Function.Call(Hash._TASK_VEHICLE_FOLLOW, RobberA, RobberA.CurrentVehicle, Game.Player.Character, 2000, (int)DrivingStyle.IgnoreLights, 2);
                                        //  Function.Call(Hash.TASK_VEHICLE_MISSION_PED_TARGET, RobberA, RobberA.CurrentVehicle, Game.Player.Character,12,2000, (int)DrivingStyle.IgnoreLights, 2,0,true);
                                        //}

                                        if (RobberAtimer == 5)
                                        {
                                            RobberAtimer = 0;
                                        }
                                        if (RobberAtimer == 0)
                                        {
                                            if (RobberA.CurrentVehicle.Model.IsHelicopter == true)
                                            {
                                                RobberA.Task.ClearAll();
                                                var Car = Game.Player.Character.CurrentVehicle;
                                                Function.Call(Hash.TASK_HELI_MISSION, RobberA, RobberA.CurrentVehicle, 0, 0,
                                                Car.Position.X, Car.Position.Y, Car.Position.Z, 4, 300.0F, 150.0F, (Car.Position - RobberA.CurrentVehicle.Position).ToHeading(), -1, -1, -1, 0);
                                            }
                                            if (RobberA.CurrentVehicle.Model.IsHelicopter == false)
                                            {
                                                Vector3 Pxyz = Game.Player.Character.Position;
                                                if (World.GetDistance(Game.Player.Character.Position, RobberA.Position) < 10f)
                                                {

                                                    Function.Call(Hash.TASK_VEHICLE_GOTO_NAVMESH, RobberA, RobberA.CurrentVehicle, Pxyz.X, Pxyz.Y, Pxyz.Z, Game.Player.Character.CurrentVehicle.Speed, 156, 15.0f);



                                                }
                                                else
                                                {
                                                    if (RobberA.CurrentVehicle.Speed >= 0f)
                                                    {

                                                        Function.Call(Hash.TASK_VEHICLE_GOTO_NAVMESH, RobberA, RobberA.CurrentVehicle, Pxyz.X, Pxyz.Y, Pxyz.Z, 400.0f, 156, 0.0f);
                                                    }
                                                    if (RobberA.CurrentVehicle.Speed == 0)
                                                    {

                                                        GTA.World.DrawMarker(MarkerType.VerticalCylinder, RobberA.CurrentVehicle.GetOffsetFromWorldCoords(new Vector3(0, 5, 0)), Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 6f), System.Drawing.Color.Yellow);
                                                        RobberA.Task.DriveTo(RobberA.CurrentVehicle, RobberA.CurrentVehicle.GetOffsetFromWorldCoords(new Vector3(0, 5, 0)), 100, 20, (int)DrivingStyle.Rushed);
                                                    }
                                                }
                                            }
                                        }
                                        if (RobberAtimer != 5)
                                        {
                                            RobberAtimer++;
                                        }

                                    }

                                    if (RobberA.CurrentVehicle.IsSeatFree(VehicleSeat.Driver) == true)
                                    {
                                        UI.Notify("NO DRIVER");
                                        RobberA.Alpha = 254;
                                        var Car = RobberA.CurrentVehicle;
                                        RobberA.Task.WarpOutOfVehicle(Car);

                                        RobberA.Task.WarpIntoVehicle(Car, VehicleSeat.Driver);
                                        foreach (Ped p in SupportPed)
                                        {
                                            if (p != null)
                                            {
                                                if (p.CurrentVehicle == null)
                                                {
                                                        if(Car.IsSeatFree(VehicleSeat.Any)==true)
                                                        {
                                                            p.Task.ClearAll();
                                                            p.Task.EnterVehicle(Car, VehicleSeat.Any, 9999, 100);
                                                        }
                                                  
                                                }

                                            }
                                        }
                                    }

                                    if (RobberA.CurrentVehicle.Driver != null)
                                    {
                                        RobberA.Task.LookAt(Game.Player.Character.Position, 200);
                                        RobberA.Alpha = 255;
                                    }



                                }
                            }
                        }
                        if (RobberA.IsInWater == true)
                        {
                            Function.Call(Hash.SET_ENABLE_SCUBA, RobberA, true);
                        }
                    }
                    #endregion
                    }
                }
            }
         
            #endregion
            #region Old AI
            //foreach(Vehicle v in SupportVeh)
            //{
            //    if(v!=null)
            //    {
            //        if(v.IsAlive==false )
            //        {
            //            v.IsPersistent = false;
            //            v.MarkAsNoLongerNeeded();
            //        }
            //        if(v.GetPedOnSeat(VehicleSeat.Driver)!=null)
            //        {
            //            if (World.GetDistance(Game.Player.Character.Position, v.Position) > 40f)
            //            {
            //                if(!v.Model.IsHelicopter)
            //                {
            //                    if(RecallTimer==30)
            //                    {
            //                        v.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(v, Game.Player.Character.Position, 20, 100,(int)DrivingStyle.Normal);
            //                    }
            //                    if(RecallTimer!=30)
            //                    {
            //                        RecallTimer++;
            //                    }
            //                }

            //                if (v.Model.IsHelicopter)
            //                {
            //                    if(Target==null)
            //                    {
            //                        v.GetPedOnSeat(VehicleSeat.Driver).Task.VehicleChase(Game.Player.Character);
            //                    }
            //                    if(Target != null)
            //                    {
            //                        if (World.GetDistance(v.Position, Target.Position) < 300f)
            //                        {
            //                            FireGun(v.Position.Around(3), v.GetPedOnSeat(VehicleSeat.Driver), Target.Position);
            //                            FireGun(v.Position.Around(3), v.GetPedOnSeat(VehicleSeat.Driver), Target.Position);
            //                            FireGun(v.Position.Around(3), v.GetPedOnSeat(VehicleSeat.Driver), Target.Position);
            //                       }
            //                        v.GetPedOnSeat(VehicleSeat.Driver).Task.VehicleChase(Target);
            //                    }
            //                    ChopperGunnerTarget(v.GetPedOnSeat(VehicleSeat.Passenger));
            //                    ChopperGunnerTarget(v.GetPedOnSeat(VehicleSeat.LeftRear));
            //                    ChopperGunnerTarget(v.GetPedOnSeat(VehicleSeat.RightRear));
            //                }

            //            }
            //            if (World.GetDistance(Game.Player.Character.Position, v.Position) < 40f)
            //            {
            //                if(Game.Player.Character.CurrentVehicle==null)
            //                {
            //                    v.GetPedOnSeat(VehicleSeat.Driver).Task.WarpOutOfVehicle(v);
            //                }

            //            }
            //        }
            //    }
            //}
            //foreach (Ped p in SupportPed)
            //{
            //    if(p.CurrentVehicle!=null)
            //    {
            //        if (p.CurrentVehicle.IsSeatFree(VehicleSeat.Driver) == true)
            //        {
            //            UI.Notify("NO DRIVER");
            //            p.Alpha = 254;
            //            var Car = p.CurrentVehicle;
            //            p.Task.WarpOutOfVehicle(Car);

            //            p.Task.WarpIntoVehicle(Car, VehicleSeat.Driver);
            //        }
            //        if (p.CurrentVehicle.Driver != null)
            //        {
            //            p.Task.LookAt(Game.Player.Character.Position, 200);
            //            p.Alpha = 255;
            //        }
            //    }

            //    if (Game.Player.Character.CurrentVehicle!=null)
            //    {
            //        var car = Game.Player.Character.CurrentVehicle;
            //        if (car.IsSeatFree(VehicleSeat.Driver) == false &&
            //            car.IsSeatFree(VehicleSeat.Passenger) == false&&
            //        car.IsSeatFree(VehicleSeat.LeftRear) == false&&
            //        car.IsSeatFree(VehicleSeat.RightRear) == false)
            //        {
            //            if(p.CurrentVehicle==null)
            //            {
            //                if(p.IsGettingIntoAVehicle==false)
            //                {
            //                    p.Task.EnterVehicle(p.LastVehicle, VehicleSeat.Any);

            //                }

            //            }
            //        }
            //    }
            //    if (p.IsAlive==false )
            //    {
            //        p.IsPersistent = false;
            //        if(p.CurrentBlip!=null)
            //        {
            //            p.CurrentBlip.Remove();
            //        }
            //        p.MarkAsNoLongerNeeded();

            //    }
            //    if (p != null)
            //    {


            //        if (World.GetDistance(Game.Player.Character.Position, p.Position) < 400f)
            //        {
            //            Ped bodyguard = p;
            //            PedGroup playerGroup = Game.Player.Character.CurrentPedGroup; // gets the players current group
            //            Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, bodyguard, playerGroup); // puts the bodyguard into the players group
            //            Function.Call(Hash.SET_PED_COMBAT_ABILITY, bodyguard, 100); // 100 = attack
            //        }
            //    }
            //}
            #endregion
            if (taxisetup == true)

            {
                TaxiBlip.Position = TaxiCab.Position;
                if (TaxiCab != null)
                {
                    if (World.GetDistance(Game.Player.Character.Position, TaxiCab.Position) < 30f)
                    {
                        Game.FadeScreenOut(600);
                        Script.Wait(600);
                        taxisetup = false;
                        if (TaxiBlip != null)
                        {
                            TaxiBlip.Remove();
                        }
                        var GetRandomNumber = new Random();
                        TaxiCab.Position = Game.Player.Character.Position.Around(GetRandomNumber.Next(50, 200));
                        TaxiCab.PlaceOnNextStreet();
                        TaxiCab.GetPedOnSeat(VehicleSeat.Driver).Task.FleeFrom(Game.Player.Character);


                        Deliveredplayer = true;
                        if (officenum == 1)
                        {
                            Game.Player.Character.Position = OfficeMarker1;

                        }
                        if (officenum == 2)
                        {
                            Game.Player.Character.Position = OfficeMarker2;

                        }
                        if (officenum == 3)
                        {
                            Game.Player.Character.Position = OfficeMarker3;

                        }
                        if (officenum == 4)
                        {
                            Game.Player.Character.Position = OfficeMarker4;

                        }
                        if (officenum == 5)
                        {
                            Game.Player.Character.Position =BunkerPos;

                        }
                        if (officenum == 6)
                        {
                            Game.Player.Character.Position = FacilityPos;

                        }
                        if (officenum == 7)
                        {
                            Game.Player.Character.Position = HangerPos;

                        }
                        if (officenum == 8)
                        {
                            Game.Player.Character.Position = MethBusinessPos;

                        }
                        if (officenum == 9)
                        {
                            Game.Player.Character.Position = Safehouse1Pos;

                        }
                        if (officenum == 10)
                        {
                            Game.Player.Character.Position = new Vector3(-395f,-1858f,20);

                        }
                        Script.Wait(600);
                        Game.FadeScreenIn(300);
                        UI.Notify("Taxi Driver: thank you, have a nice day!");
                    }
                }


            }

            if (invunrablewait == true)
            {
                invunrabletimer++;
            }

            if (invunrablewait == true && invunrabletimer ==2880)
            {
                invunrablewait = false;
                invunrablewait = false;
                invunrabletimer = 0;

                UI.Notify("Agent 14: ok thats it, invunrability has worn off");
                Game.Player.Character.IsInvincible = false;

            }
            if(Blackoutwait==true)
            {
                Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, true);
                var vehicles = World.GetNearbyVehicles(Game.Player.Character.Position,10000);
                foreach(Vehicle V in vehicles)
                {
                    if(V.GetPedOnSeat(VehicleSeat.Driver)!=null)
                    {
                        if (V.Model != "POLICE" ||
                            V.Model != "POLICE2" ||
                           V.Model != "POLICE3" ||
                           V.Model != "POLICE4" ||
                            V.Model != "FBI2" ||
                             V.Model != "FBI" ||
                              V.Model != "POLICET")
                        {
                            V.GetPedOnSeat(VehicleSeat.Driver).DrivingStyle = DrivingStyle.Rushed;
                            var Peds = World.GetNearbyPeds(Game.Player.Character.Position, 1000);
                            var r = Peds.Length;
                            var ran = new Random();
                            var P = ran.Next(1, r);
                            if(V.GetPedOnSeat(VehicleSeat.Driver).IsPlayer==false)
                            {
                                V.GetPedOnSeat(VehicleSeat.Driver).Task.ReactAndFlee(Peds[P]);
                            }
                            UI.Notify("test22a");
                        }
                           
                           
                    }
                }
               Game.Player.IgnoredByPolice = true;
                Game.MaxWantedLevel = 0;
                Game.Player.WantedLevel = 0;
                offradartimer++;
                offradarbool = true;

            }
            if (offradarbool == true)
            {
                uint MapArea;
                MapArea = Function.Call<uint>(Hash.GET_HASH_OF_MAP_AREA_AT_COORDS, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z);
                if(MapArea != 2072609373 &&Blackoutwait==false)
                {
                    offradartimer++;
                 
                    Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, true);
                    Game.Player.IgnoredByPolice = true;
                    Game.MaxWantedLevel = 0;
                    Game.Player.WantedLevel = 0;
                 
                }
                else
                {
                    if(noRestrictedzones==true)
                    {
                        offradartimer++;
                        
                        Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, true);
                        Game.Player.IgnoredByPolice = true;
                        Game.MaxWantedLevel = 0;
                        Game.Player.WantedLevel = 0;
                        if(Blackoutwait==true)
                        {
                            Blackoutwait = false;
                            var vehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 10000);
                            foreach (Vehicle V in vehicles)
                            {
                                if (V.GetPedOnSeat(VehicleSeat.Driver) != null)
                                {
                                    V.GetPedOnSeat(VehicleSeat.Driver).DrivingStyle = DrivingStyle.Normal;
                                    V.GetPedOnSeat(VehicleSeat.Driver).Task.ClearAll();
                                }
                            }
                        }
                    }
                    else
                    {
                        UI.Notify("Agent 14: your in a restricted zone, off radar has worn off");
                        offradarbool = false;
                        if (Blackoutwait == true)
                        {
                            Blackoutwait = false;
                            var vehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 10000);
                            foreach (Vehicle V in vehicles)
                            {
                                if (V.GetPedOnSeat(VehicleSeat.Driver) != null)
                                {
                                    V.GetPedOnSeat(VehicleSeat.Driver).DrivingStyle = DrivingStyle.Normal;
                                    V.GetPedOnSeat(VehicleSeat.Driver).Task.ClearAll();

                                }
                            }
                        }
                    }
                }
              
            
              
             
            }
            if (offradarbool == true && offradartimer == 2880)
            {
                offradarbool = false;
                offradartimer = 0;
                Game.Player.IgnoredByPolice =false;
                Game.MaxWantedLevel = 5;
                Function.Call(Hash._SET_BLACKOUT, false);
                Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, false);
                UI.Notify("Agent 14: ok thats it, you are back on the radar ");
                if(Blackoutwait==true)
                {
                    Function.Call(Hash._SET_BLACKOUT, false);
                    UI.Notify("Agent 14: ok thats it, blackout has disipated ");
                }
            }
            if (Game.Player.WantedLevel == 0)
            {
                wantedlevelprice = 0;
            }
            else
            if (Game.Player.WantedLevel == 1)
            {
                wantedlevelprice = 200;
            }
            else
            if (Game.Player.WantedLevel == 2)
            {
                wantedlevelprice = 2500;
            }
            else
            if (Game.Player.WantedLevel == 3)
            {
                wantedlevelprice = 10000;
            }
            else
            if (Game.Player.WantedLevel == 4)
            {
                wantedlevelprice = 100000;
            }
            else
            if (Game.Player.WantedLevel == 5)
            {
                wantedlevelprice = 3000000;
            }

           

            



        }
        public void loadMenu(iFruitContact contact)
        {

            //  UI.Notify("Map Area " + MapArea);
            ifruit.Close();
            mainMenu.Visible = !mainMenu.Visible;
        }

        private void OnShutdown(object sender, EventArgs e)
        {
            var A_0 = true;
            if (A_0)
            {
                if (Armour3 != null)
                {
                    Armour3.Delete();
                }
                if (Armour4 != null)
                {
                    Armour4.Delete();
                }
                if (Armour != null)
                {
                    Armour.Delete();
                }
                if (Armour2!=null)
                {
                    Armour2.Delete();
                }
                foreach(Ped p in SupportPed)
                {
                    if(p!=null)
                    {
                        p.Delete();
                    }
                }
                foreach(Vehicle v in SupportVeh)
                {
                    if(v!=null)
                    {
                        v.Delete();
                    }
                }
                if (TaxiCab != null)
                {
                    TaxiCab.Delete();
                }
                if (TaxiBlip != null)
                {
                    TaxiBlip.Remove();
                }
                Function.Call(Hash._SET_BLACKOUT, false);
            }
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (SupportPed.Count > 0)
            { 
                    if (e.KeyCode == OrderAllFollow)
                    {
                        UI.Notify("~b~ Lester's Bribes ~w~ Ordering All AI to Follow");

                        try
                        {

                            foreach (Ped RobberA in SupportPed)
                            {
                                if (RobberA != null)
                                {
                                    if (RobberA.CurrentVehicle == null)
                                    {
                                        RobberA.Task.ClearAll();
                                        Ped bodyguard = RobberA;
                                        PedGroup playerGroup = Game.Player.Character.CurrentPedGroup; // gets the players current group
                                        Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, bodyguard, playerGroup); // puts the bodyguard into the players group
                                        Function.Call(Hash.SET_PED_COMBAT_ABILITY, bodyguard, 100); // 100 = attack
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    if (e.KeyCode == OrderAllExit)
                    {
                        UI.Notify("~b~ Lester's Bribes ~w~ Ordering All AI to Exit their vehicle");
                        try
                        {
                            foreach (Ped RobberA in SupportPed)
                            {
                                if (RobberA != null)
                                {
                                    if (RobberA.CurrentVehicle != null)
                                    {
                                        RobberA.Task.LeaveVehicle();
                                        RobberA.Task.ClearAll();
                                        Ped bodyguard = RobberA;
                                        PedGroup playerGroup = Game.Player.Character.CurrentPedGroup; // gets the players current group
                                        Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, bodyguard, playerGroup); // puts the bodyguard into the players group
                                        Function.Call(Hash.SET_PED_COMBAT_ABILITY, bodyguard, 100); // 100 = attack
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }

                    }
                    if (e.KeyCode == OrderAllEnter)
                    {
                        UI.Notify("~b~ Lester's Bribes ~w~ Ordering All AI to Enter the nearest vehicle");
                        try
                        {
                            foreach (Ped RobberA in SupportPed)
                            {
                                if (RobberA != null)
                                {
                                    Vehicle[] V = World.GetNearbyVehicles(Game.Player.Character.Position, 500);
                                    float dist = 20000;
                                    Vehicle v = null;
                                    foreach (Vehicle veh in V)
                                    {
                                        if (veh != Game.Player.Character.CurrentVehicle)
                                        {
                                            if (World.GetDistance(Game.Player.Character.Position, veh.Position) < dist)
                                            {
                                                #region Check
                                                if (veh.GetPedOnSeat(VehicleSeat.Driver) == null)
                                                {
                                                    dist = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                                    v = veh;
                                                }
                                                if (veh.IsSeatFree(VehicleSeat.Any) == true)
                                                {

                                                    if (veh.IsSeatFree(VehicleSeat.Driver) == true)
                                                    {
                                                        dist = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                                        v = veh;
                                                    }

                                                    if (veh.IsSeatFree(VehicleSeat.Driver) == false)
                                                    {
                                                        if (RobberA != null)
                                                        {
                                                            if (veh.GetPedOnSeat(VehicleSeat.Driver) == RobberA)
                                                            {
                                                                dist = World.GetDistance(Game.Player.Character.Position, veh.Position);
                                                                v = veh;
                                                            }
                                                        }

                                                    }


                                                }
                                                #endregion

                                            }
                                        }
                                    }
                                    if (RobberA.LastVehicle != null)
                                    {
                                        if (RobberA.Alpha < 255 && RobberA.IsGettingIntoAVehicle == false)
                                        {
                                            RobberA.Alpha = 255;
                                        }

                                    }
                                    if (v != null)
                                    {

                                        if (v.IsSeatFree(VehicleSeat.Any) == true)
                                        {
                                            Vector3 N = new Vector3(v.Position.X, v.Position.Y, v.Position.Z + 3);
                                            GTA.World.DrawMarker(MarkerType.UpsideDownCone, N, Vector3.Zero, Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), System.Drawing.Color.OrangeRed);
                                            if (RobberA.CurrentVehicle == null)
                                            {
                                                RobberA.Alpha = 254;
                                                if (v.GetPedOnSeat(VehicleSeat.Driver) == null)
                                                {
                                                    RobberA.Task.EnterVehicle(v, VehicleSeat.Driver, 9999, 100);
                                                }
                                                else
                                                if (v.GetPedOnSeat(VehicleSeat.Driver) != null)
                                                {
                                                    RobberA.Task.EnterVehicle(v, VehicleSeat.Any, 9999, 100);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        catch
                        {
                            UI.Notify("~b~ Lester's Bribes ~w~ AI cannot find any vehicles around Player!");
                        }
                    }

                
            }
            if(e.KeyCode == Snack_hotkey)
            {
                if (purchasedsnacks > 0)
                {
                    var Randomhealth = new Random();
                    Game.Player.Character.Health = Game.Player.Character.Health + Randomhealth.Next(5, 20);
                    UI.Notify("Player: Eating a snack ");
                    //Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                    // Game.Player.Character.Task.PlayAnimation("anim@heists@ornate_bank@grab_cash_heels", "grab", 10.0f, 2500, false, 0);

                   //  Game.Player.Character.Task.PlayAnimation("missheist_jewel@first_person", "smash_case_e", 15.0f, 2500, false, 0);
                    purchasedsnacks--;
                    Config.SetValue<int>("Setup", "Armour", purchasedarmour);
                    Config.SetValue<int>("Setup", "Snacks", purchasedsnacks);
                    Config.Save();

                }
                else
                {
                    UI.Notify("Lester: you have no snacks ");
                }
            }
            if (e.KeyCode == ARMOUR_hotkey)
            {
                if (purchasedarmour > 0)
                {
                    var Randomhealth = new Random();
                    if (Game.Player.Character.Armor + 99 < Game.Player.MaxArmor)
                    {
                        Game.Player.Character.Armor += 99;
                        UI.Notify("Player: putting on body armor, cover me!");
                        purchasedarmour--;
                        Config.SetValue<int>("Setup", "Armour", purchasedarmour);
                        Config.SetValue<int>("Setup", "Snacks", purchasedsnacks);
                        Config.Save();
                    }
                    else
                    {
                        UI.Notify("Player: Armour Full!");
                    }
                    //Armour = World.CreateProp(RequestModel("prop_bodyarmour_03"), Game.Player.Character.Position, false, false);
                    //Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Armour, Game.Player.Character, Game.Player.Character.GetBoneIndex((Bone.SKEL_Spine2)), 0.22f, 0.095f, 0f, 180f,90f,0f, 0, 0, 0, 0, 2, 1);
                    //Armour2 = World.CreateProp(RequestModel("prop_bodyarmour_03"), Game.Player.Character.Position, false, false);
                    //Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Armour2, Game.Player.Character, Game.Player.Character.GetBoneIndex((Bone.SKEL_Spine2)), 0.22f, -0.035f, 0f, 180f, 90f, 0f, 0, 0, 0, 0, 2, 1);

                    //Armour3 = World.CreateProp(RequestModel("p_steve_scuba_hood_s"), Game.Player.Character.Position, false, false);
                    //Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Armour3, Game.Player.Character, Game.Player.Character.GetBoneIndex((Bone.IK_Head)), 0f, 0f, 0f, 180f, 90f, 0f, 0, 0, 0, 0, 2, 1);
                    //Armour4 = World.CreateProp(RequestModel("prop_player_gasmask"), Game.Player.Character.Position, false, false);
                    //Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Armour4, Game.Player.Character, Game.Player.Character.GetBoneIndex((Bone.IK_Head)), 0f, 0f, 0f, 180f, 90f, 0f, 0, 0, 0, 0, 2, 1);
                    //Armour4.se

                }
                else
                {
                    UI.Notify("Lester: you have no body armour ");
                }
            }
            if (e.KeyCode == Key1 && !this.modMenuPool.IsAnyMenuOpen())
            {
                mainMenu.Visible = !mainMenu.Visible;
            }
            
           

        }



    }

}





