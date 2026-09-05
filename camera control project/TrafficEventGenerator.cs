using System;
using System.Collections.Generic;

namespace camera_control_project
{
    public class TrafficEventGenerator
    {
        private const int PlateCount = 50;

        private const double MaxAllowedSpeed = 121;

        private const int MinSpeed = 40;

        private const int MaxSpeed = 160;

        private const int DaysBack = 30;


        private readonly List<int> _cameraIds = CameraConfiguration.CameraIds;

        public List<TrafficEvent> GenerateEvents(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException(
                    "Event count must be greater than zero.",
                    nameof(count)
                );
            }

            List<TrafficEvent> events = new List<TrafficEvent>();

            Random random = new Random();

            List<string> plates = GeneratePlates(random);

            for (int i = 1; i <= count; i++)
            {
                TrafficEvent trafficEvent = new TrafficEvent
                {
                    Id = i,

                    PlateNo =
                        plates[random.Next(plates.Count)],

                    CameraId =
                        _cameraIds[random.Next(_cameraIds.Count)],

                    Speed =
                        random.Next(MinSpeed, MaxSpeed + 1),

                    MaxSpeed =
                        MaxAllowedSpeed,

                    DateTime =
                        DateTime.Now.AddDays(
                            -random.Next(DaysBack)
                        )
                };

                events.Add(trafficEvent);
            }

            return events;
        }

        private List<string> GeneratePlates(Random random)
        {
            List<string> plates = new List<string>();

            for (int i = 0; i < PlateCount; i++)
            {
                string plate =
                    $"{random.Next(10, 100)}" +
                    $"{(char)random.Next('A', 'Z' + 1)}" +
                    $"{random.Next(100, 1000)}";

                plates.Add(plate);
            }

            return plates;
        }
    }
}
