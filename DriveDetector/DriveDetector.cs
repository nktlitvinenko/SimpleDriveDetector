using System;
using System.Management;
using DriveDetector.Exceptions;
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
        public delegate void DriveConfirurationChangedEventHandler(object sender, VolumeChangeEventArgs e);
        public delegate void DriveDockingEventHandler(object sender, VolumeChangeEventArgs e);

        public event DriveArrivedEventHandler DriveArrivedEvent;
        public event DriveRemovedEventHandler DriveRemovedEvent;
        public event DriveConfirurationChangedEventHandler DriveConfigurationChangedEvent;
        public event DriveDockingEventHandler DriveDockingEvent;


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
                    DriveConfirurationChangedEventHandler confirurationChangedHandler = DriveConfigurationChangedEvent;
                    if (confirurationChangedHandler != null)
                    {
                        confirurationChangedHandler(this, eventAgrs);
                    }
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
                    DriveDockingEventHandler dockingHandler = DriveDockingEvent;
                    if (dockingHandler != null)
                    {
                        dockingHandler(this, eventAgrs);
                    }
                    break;
                default:
                    throw new UndefinedEventTypeException("Undefined event type was handled by DriveDetector");
                    break;
            }
        }

        private VolumeChangeEventArgs ConvertEventArrivedEventArgsToVolumeChangeEventArgs(EventArrivedEventArgs e)
        {
            VolumeChangeEventArgs resVolumeChangeEventArgs = new VolumeChangeEventArgs
            {
                DriveName = e.NewEvent.Properties["DriveName"].Value.ToString(),
                TimeCreated = new DateTime(Int64.Parse(e.NewEvent.Properties["TIME_CREATED"].Value.ToString()))
            };
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

            return resVolumeChangeEventArgs;
        }
    }
}
