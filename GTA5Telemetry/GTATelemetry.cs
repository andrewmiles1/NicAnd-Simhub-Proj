/// <summary>
/// GTA V Codemasters Telemetry Plugin
/// 
/// This plugin enables GTA 5 to send telemetry data packets just like a Codemasters game (e.g. DiRT Rally) can do
/// Now you can use any Codemasters-compatible simracing dashboard with GTA5!
/// 
/// If this code works, it has been written by Carlo Iovino (carlo.iovino@outlook.com)
/// The Green Dragon Youtube Channel (www.youtube.com/carloxofficial)
/// 
/// </summary>
using System;
using System.Configuration;
using CodemastersTelemetry;
public class Program
{
    // Uncomment the following line to resolve.
    static void Main()
    {
            
    }
}
namespace GTA5Telemetry
{
    class GTA5TelemetryPluginSettings
    {
        public bool SequentialFix = false;
        public bool NeutralGearInference1 = false;
        public bool NeutralGearInference2 = false;
        public bool WaypointsNavigation = false;
        public bool CaptureManualTransmissionGearing = true; // Active by default
        public float NeutralGearSpeedKMH = 10f; // A minimum speed (in KMH) for inferring the car is on Neutral gear
        public float NeutralGearIdleRPMs = 0.4f;  // A minimum rpms value for inferring the car is on Neutral gear
        public Int32 port = 20777; // The UDP communication port
        public string ManualTransmissionNeutralGearDecorator = "hunt_weapon";
    }

    class GTA5Telemetry : Script
    {
        TelemetryWriter DataWriter;
        TelemetryPacket Data = new TelemetryPacket();
        int PreviousGear = -1;
        GTA5TelemetryPluginSettings Settings = new GTA5TelemetryPluginSettings();

        public GTA5Telemetry()
        {

            this.DataWriter = new TelemetryWriter(Settings.port);         
        }

        /*override protected void Dispose(bool disposing)
        {
            if (disposing) DataWriter.Dispose();
        }
        */
        void OnTick(object sender, EventArgs e)
        {

            if (true) // player.IsInVehicle())
            {
                // Player in vehicle
                //Vehicle vehicle = player.CurrentVehicle;

                Data.Speed = 0;//vehicle.Speed;
                Data.EngineRevs = Data.Speed / 2;// vehicle.CurrentRPM;
                Data.WorldSpeedX = Data.Speed / 2;//vehicle.Velocity.X;
                Data.WorldSpeedY = Data.Speed / 2;//vehicle.Velocity.Y;
                Data.WorldSpeedZ = Data.Speed / 2;//vehicle.Velocity.Z;
                Data.X = 0;//vehicle.Position.X;
                Data.Y = 0;//vehicle.Position.Y;
                Data.Z = 0;//vehicle.Position.Z;
                Data.XR = 0;//vehicle.Rotation.X;
                Data.ZR = 0;//vehicle.Rotation.Z;
                Data.Steer = 0;//vehicle.SteeringAngle;
                Data.Throttle = 0;//vehicle.Acceleration;
                Data.MaxRpm = 1;
                Data.IdleRpm = 0.2f;
                Data.FuelRemaining = 0;

                /*
                if (Game.IsWaypointActive)
                {
                    Data.Distance = vehicle.Position.DistanceTo(World.GetWaypointPosition());
                }
                

                // Helicopter/Airplane specific
                if (player.IsInAir || player.IsInHeli)
                {
                    Data.Steer = vehicle.Rotation.Y;
                    Data.Distance = vehicle.HeightAboveGround;
                }
                
                else
                {
                    bool manualTransmissionNeutral = false;

                    Data.Gear = vehicle.CurrentGear;    // unless Neutral inference or Manual Transmission Gearing

                    if (Settings.CaptureManualTransmissionGearing)
                    {
                        try
                        {
                            // This is a cross-script communication with the Manual Transmission mod
                            // for capturing the simulated Neutral gear
                            GTA.Native.InputArgument[] args =
                                {new GTA.Native.InputArgument(vehicle), Settings.ManualTransmissionNeutralGearDecorator};

                            // cross-script communication
                            manualTransmissionNeutral =
                                Convert.ToBoolean(
                                    GTA.Native.Function.Call<Int32>(GTA.Native.Hash.DECOR_GET_INT, args));
                        }
                        catch { }
                    }

                    if (manualTransmissionNeutral)
                    {
                        Data.Gear = 0;
                    }
                    else if (vehicle.CurrentGear == 0)
                    {
                        // Reverse gear is the number 10 in the Codemasters F1 implementation
                        Data.Gear = 10;
                    }
                    else if (vehicle.CurrentGear == 1)
                    {
                        if (Settings.SequentialFix)
                        {
                            if (PreviousGear == 0)
                            {
                                // SEQUENTIAL FIX
                                // effectiveGear == 0 means 'R', shifting up means 'N'
                                // This fix is needed because in the original game the "Neutral" gear
                                // is not implemented, so you shift directly from R to 1
                                Data.Gear = 0;
                            }
                        }
                        if (Settings.NeutralGearInference1)
                        {
                            // Inference 1
                            // When te speed is very low, but the Engine RPMs are high
                            // it is very likely that the Gear is N (or the clutch is down)
                            if (vehicle.Speed * 3.9f <= Settings.NeutralGearSpeedKMH &&
                                vehicle.CurrentRPM >= Settings.NeutralGearIdleRPMs)
                            {
                                Data.Gear = 0;
                            }
                            // Inference 2
                            // When te speed is very low, but the Engine RPMs are high
                            // it is very likely that the Gear is N (or the clutch is down)
           
                        }
                    }

                    PreviousGear = vehicle.CurrentGear;
                }
                */


                // Share data
                byte[] bytes = PacketUtilities.ConvertPacketToByteArray(Data);
                DataWriter.SendPacket(bytes);

            }
        }
    }


}

    internal class Script
    {
    }