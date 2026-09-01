using System;
using System.Collections.Generic;

namespace camera_control_project
{
    public class TrafficEventGenerator
    {
        public List<TrafficEvent> GenerateEvents(int count)
        {
            List<TrafficEvent> events = new List<TrafficEvent>();

            Random random = new Random();

            List<int> cameraIds = new List<int>
            {
                1, 2, 3, 4, 5,
                6, 7, 8, 9, 10
            };

            // Generate random plates

            List<string> plates = new List<string>();

            for (int i = 0; i < 50; i++)
            {
                string plate =
                    $"{random.Next(10, 100)}" +
                    $"{(char)random.Next('A', 'Z' + 1)}" +
                    $"{random.Next(100, 1000)}";

                plates.Add(plate);
            }

            // Generate traffic events

            for (int i = 1; i <= count; i++)
            {
                int cameraId =
                    cameraIds[random.Next(cameraIds.Count)];

                double maxSpeed = 121;
                double speed = random.Next(40, 161);

                TrafficEvent trafficEvent = new TrafficEvent
                {
                    Id = i,
                    PlateNo = plates[random.Next(plates.Count)],
                    CameraId = cameraId,
                    Speed = speed,
                    MaxSpeed = maxSpeed,
                    DateTime = DateTime.Now.AddDays(
                        -random.Next(0, 30)
                    )
                };

                events.Add(trafficEvent);
            }

            return events;
        }
    }
}