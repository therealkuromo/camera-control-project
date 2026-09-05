using System.Collections.Generic;
using System.Linq;

namespace camera_control_project
{
    public class TrafficEventAnalyzer
    {
        private readonly List<TrafficEvent> _events;
        private readonly List<int> _cameraIds;

        public TrafficEventAnalyzer(
            List<TrafficEvent> events,
            List<int> cameraIds)
        {
            _events = events;
            _cameraIds = cameraIds;
        }

        // Check if a traffic event is a violation
        private bool IsViolation(TrafficEvent trafficEvent)
        {
            return trafficEvent.Speed > trafficEvent.MaxSpeed;
        }

        // 1 & 2. Speeding violations
        // Sorted by highest speed
        public List<TrafficEvent> GetSpeedingViolations()
        {
            return _events
                .Where(IsViolation)
                .OrderByDescending(x => x.Speed)
                .ToList();
        }

        // 3. Number of violations for each camera
        public List<CameraViolationResult> GetViolationsByCamera()
        {
            return _events
                .Where(IsViolation)
                .GroupBy(x => x.CameraId)
                .Select(group => new CameraViolationResult
                {
                    CameraId = group.Key,
                    ViolationCount = group.Count()
                })
                .OrderBy(x => x.CameraId)
                .ToList();
        }

        // 4. Last violation for each plate
        public List<TrafficEvent> GetLastViolationByPlate()
        {
            return _events
                .Where(IsViolation)
                .GroupBy(x => x.PlateNo)
                .Select(group =>
                    group
                        .OrderByDescending(x => x.DateTime)
                        .First()
                )
                .OrderBy(x => x.PlateNo)
                .ToList();
        }

        // 5. Plates with more than 5 violations
        public List<PlateViolationResult> GetFrequentViolators()
        {
            return _events
                .Where(IsViolation)
                .GroupBy(x => x.PlateNo)
                .Where(group => group.Count() > 5)
                .OrderByDescending(group => group.Count())
                .Select(group => new PlateViolationResult
                {
                    PlateNo = group.Key,
                    ViolationCount = group.Count()
                })
                .ToList();
        }

        // 6. Cameras with no violations
        public List<int> GetCamerasWithoutViolations()
        {
            return _cameraIds
                .Where(cameraId =>
                    !_events.Any(x =>
                        x.CameraId == cameraId &&
                        IsViolation(x)
                    )
                )
                .ToList();
        }

        // 7. Maximum speed of each camera
        public List<MaxSpeedByCameraResult> GetMaxSpeedByCamera()
        {
            return _cameraIds
                .Select(cameraId => new MaxSpeedByCameraResult
                {
                    CameraId = cameraId,

                    MaxSpeed = _events
                        .Where(x => x.CameraId == cameraId)
                        .Select(x => (double?)x.Speed)
                        .Max()
                })
                .OrderBy(x => x.CameraId)
                .ToList();
        }

        // 8. Top 3 most violating plates
        public List<PlateViolationResult> GetTopThreeViolators()
        {
            return _events
                .Where(IsViolation)
                .GroupBy(x => x.PlateNo)
                .OrderByDescending(group => group.Count())
                .Take(3)
                .Select(group => new PlateViolationResult
                {
                    PlateNo = group.Key,
                    ViolationCount = group.Count()
                })
                .ToList();
        }

        // 9. Violation percentage
        public double GetViolationPercentage()
        {
            int totalEvents = _events.Count;

            if (totalEvents == 0)
            {
                return 0;
            }

            int totalViolations = _events.Count(IsViolation);

            return (double)totalViolations / totalEvents * 100;
        }
    }
}