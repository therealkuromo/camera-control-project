namespace camera_control_project
{
    public class CameraViolationResult
    {
        public int CameraId { get; set; }
        public int ViolationCount { get; set; }
    }

    public class FrequentViolatorResult
    {
        public string PlateNo { get; set; }
        public int ViolationCount { get; set; }
    }

    public class MaxSpeedByCameraResult
    {
        public int CameraId { get; set; }
        public double? MaxSpeed { get; set; }
    }

    public class TopViolatorResult
    {
        public string PlateNo { get; set; }
        public int ViolationCount { get; set; }
    }
}