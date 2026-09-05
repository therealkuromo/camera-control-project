namespace camera_control_project
{
    public class CameraViolationResult
    {
        public int CameraId { get; set; }

        public int ViolationCount { get; set; }
    }

    public class PlateViolationResult
    {
        public string PlateNo { get; set; } = string.Empty;

        public int ViolationCount { get; set; }
    }

    public class MaxSpeedByCameraResult
    {
        public int CameraId { get; set; }

        public double? MaxSpeed { get; set; }
    }
}