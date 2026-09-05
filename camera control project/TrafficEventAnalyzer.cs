using System;
using System.Collections.Generic;
using System.Linq;

namespace camera_control_project
{
    public class TrafficEventAnalyzer
    {
        private readonly List<TrafficEvent> _events;

        private readonly List<int> _cameraIds;

        private readonly List<TrafficEvent> _violations;

        public TrafficEventAnalyzer(
            List<TrafficEvent> events,
            List<int> cameraIds)
        {
            _events = events
                ?? throw new ArgumentNullException(
                    nameof(events)
                );

            _cameraIds = cameraIds
                ?? throw new ArgumentNullException(
                    nameof(cameraIds)
                );

            _violations = _events
                .Where(IsViolation)
                .ToList();
        }

        private bool IsViolation(TrafficEvent trafficEvent)
        {
            return trafficEvent.Speed >
                   trafficEvent.MaxSpeed;
        }

        // 1 & 2. Speeding violations
        // Sorted by highest speed
        public List<TrafficEvent> GetSpeedingViolations()
        {
            return _violations
                .OrderByDescending(x => x.Speed)
                .ToList();
        }

        // 3. Number of violations for each camera
        public List<CameraViolationResult>
            GetViolationsByCamera()
        {
            return _violations
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
        public List<TrafficEvent>
            GetLastViolationByPlate()
        {
            return _violations
                .GroupBy(x => x.PlateNo)
                .Select(group =>
                    group.MaxBy(x => x.DateTime)!
                )
                .OrderBy(x => x.PlateNo)
                .ToList();
        }

        // 5. Plates with more than 5 violations
        public List<PlateViolationResult>
            GetFrequentViolators()
        {
            return _violations
                .GroupBy(x => x.PlateNo)
                .Select(group => new PlateViolationResult
                {
                    PlateNo = group.Key,
                    ViolationCount = group.Count()
                })
                .Where(x => x.ViolationCount > 5)
                .OrderByDescending(x => x.ViolationCount)
                .ToList();
        }

        // 6. Cameras with no violations
        public List<int>
            GetCamerasWithoutViolations()
        {
            HashSet<int> camerasWithViolations =
                _violations
                    .Select(x => x.CameraId)
                    .ToHashSet();

            return _cameraIds
                .Where(cameraId =>
                    !camerasWithViolations.Contains(cameraId)
                )
                .ToList();
        }

        // 7. Maximum speed of each camera
        public List<MaxSpeedByCameraResult>
            GetMaxSpeedByCamera()
        {
            return _cameraIds
                .Select(cameraId =>
                    new MaxSpeedByCameraResult
                    {
                        CameraId = cameraId,

                        MaxSpeed = _events
                            .Where(x =>
                                x.CameraId == cameraId
                            )
                            .Select(x =>
                                (double?)x.Speed
                            )
                            .Max()
                    }
                )
                .OrderBy(x => x.CameraId)
                .ToList();
        }

        // 8. Top 3 most violating plates
        public List<PlateViolationResult>
            GetTopThreeViolators()
        {
            return _violations
                .GroupBy(x => x.PlateNo)
                .Select(group =>
                    new PlateViolationResult
                    {
                        PlateNo = group.Key,
                        ViolationCount = group.Count()
                    }
                )
                .OrderByDescending(x =>
                    x.ViolationCount
                )
                .Take(3)
                .ToList();
        }

        // 9. Violation percentage
        public double GetViolationPercentage()
        {
            if (_events.Count == 0)
            {
                return 0;
            }

            return
                (double)_violations.Count /
                _events.Count *
                100;
        }
    }
}