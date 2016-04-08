using System;
using DriveDetector.Models;

namespace DriveDetector.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a new instance of DriveDetector
            DriveDetector d = new DriveDetector();
            //Subscribes on events
            d.DriveArrivedEvent += DOnDriveArrivedEvent;
            d.DriveRemovedEvent +=DOnDriveRemovedEvent;
            d.DriveConfigurationChangedEvent += DOnDriveConfigurationChangedEvent;
            d.DriveDockingEvent +=DOnDriveDockingEvent;
            //Start drive detecting
            d.Start();

            Console.ReadKey();
        }

        private static void DOnDriveDockingEvent(object sender, VolumeChangeEventArgs volumeChangeEventArgs)
        {
            Console.WriteLine("Drive " + volumeChangeEventArgs.DriveName + " is docking");
        }

        private static void DOnDriveConfigurationChangedEvent(object sender, VolumeChangeEventArgs volumeChangeEventArgs)
        {
            Console.WriteLine("Drive " + volumeChangeEventArgs.DriveName + " configuration changed");
        }

        private static void DOnDriveArrivedEvent(object sender, VolumeChangeEventArgs volumeChangeEventArgs)
        {
            Console.WriteLine("Drive " + volumeChangeEventArgs.DriveName + " is arrived");
        }

        private static void DOnDriveRemovedEvent(object sender, VolumeChangeEventArgs volumeChangeEventArgs)
        {
            Console.WriteLine("Drive " + volumeChangeEventArgs.DriveName + " is removed");
        }
    }
}
