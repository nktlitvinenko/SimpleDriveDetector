using System;
using DriveDetector.Models;

namespace DriveDetector.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            DriveDetector d = new DriveDetector();
            d.DriveArrivedEvent += DOnDriveArrivedEvent;
            d.DriveRemovedEvent +=DOnDriveRemovedEvent;
            d.Start();

            Console.ReadKey();
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
