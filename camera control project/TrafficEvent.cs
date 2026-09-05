using System;

namespace camera_control_project
{
    public class TrafficEvent
    {
        public int Id { get; set; }

        public string PlateNo { get; set; } = string.Empty;

        public int CameraId { get; set; }

        public double Speed { get; set; }

        public double MaxSpeed { get; set; }

        public DateTime DateTime { get; set; }
    }
}