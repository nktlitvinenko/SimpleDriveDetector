using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using DriveDetector.Models;
using DriveDetector.Models.Enums;

namespace DriveDetector
{
    public class DriveDetector
    {
        private readonly ManagementEventWatcher _watcher = new ManagementEventWatcher();
        private readonly WqlEventQuery _query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent");


        public delegate void DriveArrivedEventHandler(object sender, VolumeChangeEventArgs e);
        public delegate void DriveRemovedEventHandler(object sender, VolumeChangeEventArgs e);

        public event DriveArrivedEventHandler DriveArrivedEvent;
        public event DriveRemovedEventHandler DriveRemovedEvent;


        public void Start()
        {
            _watcher.EventArrived += new EventArrivedEventHandler(Watcher_DriveChangedEvent);
            _watcher.Query = _query;
            _watcher.Start();
            _watcher.WaitForNextEvent();
        }

        private void Watcher_DriveChangedEvent(object sender, EventArrivedEventArgs e)
        {
            var eventAgrs = ConvertEventArrivedEventArgsToVolumeChangeEventArgs(e);
            
            switch (eventAgrs.EventType)
            {
                case EventType.ConfigurationChanged:
                    throw new NotImplementedException();
                    break;
                case EventType.DeviceArrival:
                    DriveArrivedEventHandler arrivedHandler = DriveArrivedEvent;
                    if (arrivedHandler != null)
                    {
                        arrivedHandler(this, eventAgrs);
                    }
                    break;
                case EventType.DeviceRemoval:
                    DriveRemovedEventHandler removedHandler = DriveRemovedEvent;
                    if (removedHandler != null)
                    {
                        removedHandler(this, eventAgrs);
                    }
                    break;
                case EventType.Docking:
                    throw new NotImplementedException();
                    break;
                default:
                    //TODO write own exception
                    throw new Exception("Undefined EventType");
                    break;
            }
        }

        private VolumeChangeEventArgs ConvertEventArrivedEventArgsToVolumeChangeEventArgs(EventArrivedEventArgs e)
        {
            VolumeChangeEventArgs resVolumeChangeEventArgs = new VolumeChangeEventArgs();
            resVolumeChangeEventArgs.DriveName = e.NewEvent.Properties["DriveName"].Value.ToString();
            Debug.WriteLine(e.NewEvent.Properties["DriveName"].Value.GetType());
            switch (Int32.Parse(e.NewEvent.Properties["EventType"].Value.ToString()))
            {
                case 1:
                    resVolumeChangeEventArgs.EventType = EventType.ConfigurationChanged;
                    break;
                case 2:
                    resVolumeChangeEventArgs.EventType = EventType.DeviceArrival;
                    break;
                case 3:
                    resVolumeChangeEventArgs.EventType = EventType.DeviceRemoval;
                    break;
                case 4:
                    resVolumeChangeEventArgs.EventType = EventType.Docking;
                    break;
                default:
                    resVolumeChangeEventArgs.EventType = EventType.Undefined;
                    break;
            }
            resVolumeChangeEventArgs.TimeCreated = 
                new DateTime(Int64.Parse(e.NewEvent.Properties["TIME_CREATED"].Value.ToString()));

            return resVolumeChangeEventArgs;
        }
    }
}
