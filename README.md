# SimpleDriveDetector
## How to use?
```
    class Program
    {
        static void Main(string[] args)
        {
            //Create a new instance of DriveDetector
            DriveDetector d = new DriveDetector();
            //Subscribes on events
            d.DriveArrivedEvent += DOnDriveArrivedEvent;
            d.DriveRemovedEvent +=DOnDriveRemovedEvent;
            //Start drive detecting
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
```
