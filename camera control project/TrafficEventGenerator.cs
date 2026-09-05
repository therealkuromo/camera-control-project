using System;
using System.Collections.Generic;

namespace camera_control_project
{
    public class TrafficEventGenerator
    {
        private const int PlateCount = 50;

        private const int MinSpeed = 40;

        private const int MaxSpeed = 160;

        private readonly List<int> _cameraIds =
            CameraConfiguration.CameraIds;

        private readonly TrafficSettings _settings =
            AppConfiguration.TrafficSettings;

        public List<TrafficEvent> GenerateEvents(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException(
                    "Event count must be greater than zero.",
                    nameof(count)
                );
            }

            List<TrafficEvent> events =
                new List<TrafficEvent>(count);

            List<string> plates =
                GeneratePlates();

            DateTime now = DateTime.Now;

            for (int i = 1; i <= count; i++)
            {
                TrafficEvent trafficEvent =
                    new TrafficEvent
                    {
                        Id = i,

                        PlateNo =
                            plates[
                                Random.Shared.Next(
                                    plates.Count
                                )
                            ],

                        CameraId =
                            _cameraIds[
                                Random.Shared.Next(
                                    _cameraIds.Count
                                )
                            ],

                        Speed =
                            Random.Shared.Next(
                                MinSpeed,
                                MaxSpeed + 1
                            ),

                        MaxSpeed =
                            _settings.MaxAllowedSpeed,

                        DateTime =
                            now.AddDays(
                                -Random.Shared.Next(
                                    _settings.DaysBack
                                )
                            )
                    };

                events.Add(trafficEvent);
            }

            return events;
        }

        private List<string> GeneratePlates()
        {
            List<string> plates =
                new List<string>(PlateCount);

            for (int i = 0; i < PlateCount; i++)
            {
                string plate =
                    $"{Random.Shared.Next(10, 100)}" +
                    $"{(char)Random.Shared.Next(
                        'A',
                        'Z' + 1
                    )}" +
                    $"{Random.Shared.Next(100, 1000)}";

                plates.Add(plate);
            }

            return plates;
        }
    }
}