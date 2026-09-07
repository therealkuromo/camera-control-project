using camera_control_project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace camera_control_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrafficController : ControllerBase
    {
        // 1. Get all traffic events
        [HttpGet]
        public IActionResult GetAllTraffic()
        {
            using TrafficDbContext context =
                new TrafficDbContext();

            List<TrafficEvent> events =
                context.TrafficEvents
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToList();

            return Ok(events);
        }


        // 2. Get speeding violations
        [HttpGet("violations")]
        public IActionResult GetSpeedingViolations()
        {
            using TrafficDbContext context =
                new TrafficDbContext();

            List<TrafficEvent> violations =
                context.TrafficEvents
                    .AsNoTracking()
                    .Where(x => x.Speed > x.MaxSpeed)
                    .OrderByDescending(x => x.Speed)
                    .ToList();

            return Ok(violations);
        }


        // 3. Get number of violations for each camera
        [HttpGet("cameras")]
        public IActionResult GetViolationsByCamera()
        {
            using TrafficDbContext context =
                new TrafficDbContext();

            List<CameraViolationResult> result =
                context.TrafficEvents
                    .AsNoTracking()
                    .Where(x => x.Speed > x.MaxSpeed)
                    .GroupBy(x => x.CameraId)
                    .Select(group => new CameraViolationResult
                    {
                        CameraId = group.Key,
                        ViolationCount = group.Count()
                    })
                    .OrderBy(x => x.CameraId)
                    .ToList();

            return Ok(result);
        }


        // 4. Get last violation for each plate
        [HttpGet("last-violations")]
        public IActionResult GetLastViolationByPlate()
        {
            using TrafficDbContext context =
                new TrafficDbContext();

            List<TrafficEvent> violations =
                context.TrafficEvents
                    .AsNoTracking()
                    .Where(x => x.Speed > x.MaxSpeed)
                    .ToList();

            List<TrafficEvent> result =
                violations
                    .GroupBy(x => x.PlateNo)
                    .Select(group =>
                        group
                            .OrderByDescending(x => x.DateTime)
                            .First())
                    .OrderBy(x => x.PlateNo)
                    .ToList();

            return Ok(result);
        }


        // 5. Get plates with more than 5 violations
        [HttpGet("frequent-violators")]
        public IActionResult GetFrequentViolators()
        {
            using TrafficDbContext context =
                new TrafficDbContext();

            List<PlateViolationResult> result =
                context.TrafficEvents
                    .AsNoTracking()
                    .Where(x => x.Speed > x.MaxSpeed)
                    .GroupBy(x => x.PlateNo)
                    .Select(group => new PlateViolationResult
                    {
                        PlateNo = group.Key,
                        ViolationCount = group.Count()
                    })
                    .Where(x => x.ViolationCount > 5)
                    .OrderByDescending(x => x.ViolationCount)
                    .ToList();

            return Ok(result);
        }


        // 6. Get cameras without violations
        [HttpGet("cameras-without-violations")]
        public IActionResult GetCamerasWithoutViolations()
        {
            using TrafficDbContext context =
                new TrafficDbContext();

            HashSet<int> camerasWithViolations =
                context.TrafficEvents
                    .AsNoTracking()
                    .Where(x => x.Speed > x.MaxSpeed)
                    .Select(x => x.CameraId)
                    .ToHashSet();

            List<int> result =
                CameraConfiguration.CameraIds
                    .Where(cameraId =>
                        !camerasWithViolations.Contains(cameraId))
                    .ToList();

            return Ok(result);
        }


        // 7. Get maximum speed for each camera
        [HttpGet("max-speed")]
        public IActionResult GetMaxSpeedByCamera()
        {
            using TrafficDbContext context =
                new TrafficDbContext();

            List<MaxSpeedByCameraResult> result =
                CameraConfiguration.CameraIds
                    .Select(cameraId =>
                        new MaxSpeedByCameraResult
                        {
                            CameraId = cameraId,

                            MaxSpeed =
                                context.TrafficEvents
                                    .AsNoTracking()
                                    .Where(x =>
                                        x.CameraId == cameraId)
                                    .Select(x =>
                                        (double?)x.Speed)
                                    .Max()
                        })
                    .OrderBy(x => x.CameraId)
                    .ToList();

            return Ok(result);
        }


        // 8. Get top 3 plates with most violations
        [HttpGet("top-violators")]
        public IActionResult GetTopThreeViolators()
        {
            using TrafficDbContext context =
                new TrafficDbContext();

            List<PlateViolationResult> result =
                context.TrafficEvents
                    .AsNoTracking()
                    .Where(x => x.Speed > x.MaxSpeed)
                    .GroupBy(x => x.PlateNo)
                    .Select(group => new PlateViolationResult
                    {
                        PlateNo = group.Key,
                        ViolationCount = group.Count()
                    })
                    .OrderByDescending(x => x.ViolationCount)
                    .ThenBy(x => x.PlateNo)
                    .Take(3)
                    .ToList();

            return Ok(result);
        }


        // 9. Get violation percentage
        [HttpGet("violation-percentage")]
        public IActionResult GetViolationPercentage()
        {
            using TrafficDbContext context =
                new TrafficDbContext();

            int totalEvents =
                context.TrafficEvents.Count();

            if (totalEvents == 0)
            {
                return Ok(new
                {
                    TotalEvents = 0,
                    Violations = 0,
                    Percentage = 0
                });
            }

            int violations =
                context.TrafficEvents
                    .Count(x => x.Speed > x.MaxSpeed);

            double percentage =
                (double)violations /
                totalEvents *
                100;

            return Ok(new
            {
                TotalEvents = totalEvents,
                Violations = violations,
                Percentage = Math.Round(
                    percentage,
                    2
                )
            });
        }
    }
}

