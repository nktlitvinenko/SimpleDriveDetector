using System;
using DriveDetector.Models.Enums;

namespace DriveDetector.Models
{
    public class VolumeChangeEventArgs : EventArgs
    {
        public string DriveName { get; set; }
        public EventType EventType { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
