using System;
using System.Collections.Generic;

namespace camera_control_project
{
    public class TrafficMenu
    {
        private const int EventCount = 500;

        private readonly List<int> _cameraIds =
            CameraConfiguration.CameraIds;

        private List<TrafficEvent> _events =
            new List<TrafficEvent>();

        private TrafficEventAnalyzer _analyzer;

        public TrafficMenu()
        {
            _analyzer = new TrafficEventAnalyzer(
                _events,
                _cameraIds
            );
        }

        public void Run()
        {
            while (true)
            {
                ShowMenu();

                Console.Write("Enter your choice: ");

                if (!int.TryParse(
                    Console.ReadLine(),
                    out int choice))
                {
                    Console.WriteLine(
                        "Please enter a valid number!"
                    );

                    continue;
                }

                if (choice < 0 || choice > 10)
                {
                    Console.WriteLine(
                        "Please enter a number between 0 and 10."
                    );

                    continue;
                }

                if (
                    choice >= 2 &&
                    choice <= 10 &&
                    _events.Count == 0
                )
                {
                    Console.WriteLine(
                        "Database not generated! " +
                        "Press 1 to generate."
                    );

                    continue;
                }

                switch (choice)
                {
                    case 1:
                        GenerateDatabase();
                        break;

                    case 2:
                        ListTrafficEvents();
                        break;

                    case 3:
                        ShowSpeedingViolations();
                        break;

                    case 4:
                        ShowViolationsByCamera();
                        break;

                    case 5:
                        ShowLastViolationByPlate();
                        break;

                    case 6:
                        ShowFrequentViolators();
                        break;

                    case 7:
                        ShowCamerasWithoutViolations();
                        break;

                    case 8:
                        ShowMaxSpeedByCamera();
                        break;

                    case 9:
                        ShowTopThreeViolators();
                        break;

                    case 10:
                        ShowViolationPercentage();
                        break;

                    case 0:
                        return;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("What do you want?");
            Console.WriteLine("1. Generate database.");
            Console.WriteLine("2. List of traffic.");
            Console.WriteLine("3. Speed violations.");
            Console.WriteLine(
                "4. Number of violations for each camera."
            );
            Console.WriteLine(
                "5. Last violation for each plate."
            );
            Console.WriteLine(
                "6. Plates with more than 5 violations."
            );
            Console.WriteLine(
                "7. Cameras with no violations."
            );
            Console.WriteLine(
                "8. Maximum speed of each camera."
            );
            Console.WriteLine(
                "9. Top 3 most violating plates."
            );
            Console.WriteLine(
                "10. Violation percentage."
            );
            Console.WriteLine("0. Finish");
        }

        private void GenerateDatabase()
        {
            Console.WriteLine(
                "Generating database..."
            );

            TrafficEventGenerator generator =
                new TrafficEventGenerator();

            _events =
                generator.GenerateEvents(EventCount);

            _analyzer =
                new TrafficEventAnalyzer(
                    _events,
                    _cameraIds
                );

            Console.WriteLine(
                "Database generated successfully."
            );
        }

        private void ListTrafficEvents()
        {
            Console.WriteLine();
            Console.WriteLine("List of traffics:");
            Console.WriteLine(
                $"Total Events: {_events.Count}"
            );
            Console.WriteLine();

            foreach (TrafficEvent item in _events)
            {
                PrintTrafficEvent(item);
            }
        }

        private void ShowSpeedingViolations()
        {
            Console.WriteLine();
            Console.WriteLine("Speeding Violations:");

            List<TrafficEvent> violations =
                _analyzer.GetSpeedingViolations();

            if (violations.Count == 0)
            {
                Console.WriteLine(
                    "No speeding violations found."
                );

                return;
            }

            foreach (TrafficEvent item in violations)
            {
                PrintTrafficEvent(item);
            }
        }

        private void ShowViolationsByCamera()
        {
            Console.WriteLine();
            Console.WriteLine(
                "Each Camera Violation:"
            );

            List<CameraViolationResult> results =
                _analyzer.GetViolationsByCamera();

            if (results.Count == 0)
            {
                Console.WriteLine(
                    "No violations found for any camera."
                );

                return;
            }

            foreach (CameraViolationResult item in results)
            {
                Console.WriteLine(
                    $"Camera {item.CameraId}: " +
                    $"{item.ViolationCount} violations"
                );
            }
        }

        private void ShowLastViolationByPlate()
        {
            Console.WriteLine();
            Console.WriteLine(
                "Each Plate Last Violation:"
            );

            List<TrafficEvent> results =
                _analyzer.GetLastViolationByPlate();

            if (results.Count == 0)
            {
                Console.WriteLine(
                    "No violations found."
                );

                return;
            }

            foreach (TrafficEvent item in results)
            {
                Console.WriteLine(
                    $"Plate: {item.PlateNo} | " +
                    $"Speed: {item.Speed} | " +
                    $"Camera: {item.CameraId} | " +
                    $"Date: {item.DateTime}"
                );
            }
        }

        private void ShowFrequentViolators()
        {
            Console.WriteLine();
            Console.WriteLine(
                "Plates With More Than Five Violations:"
            );

            List<PlateViolationResult> results =
                _analyzer.GetFrequentViolators();

            if (results.Count == 0)
            {
                Console.WriteLine(
                    "No plate has more than five violations."
                );

                return;
            }

            foreach (PlateViolationResult item in results)
            {
                Console.WriteLine(
                    $"Plate: {item.PlateNo} | " +
                    $"Violations: {item.ViolationCount}"
                );
            }
        }

        private void ShowCamerasWithoutViolations()
        {
            Console.WriteLine();
            Console.WriteLine(
                "Cameras With No Violations:"
            );

            List<int> results =
                _analyzer.GetCamerasWithoutViolations();

            if (results.Count == 0)
            {
                Console.WriteLine(
                    "All cameras have at least one violation."
                );

                return;
            }

            foreach (int cameraId in results)
            {
                Console.WriteLine(
                    $"Camera {cameraId} has no violations."
                );
            }
        }

        private void ShowMaxSpeedByCamera()
        {
            Console.WriteLine();
            Console.WriteLine(
                "Each Camera Max Speed:"
            );

            List<MaxSpeedByCameraResult> results =
                _analyzer.GetMaxSpeedByCamera();

            foreach (MaxSpeedByCameraResult item in results)
            {
                if (item.MaxSpeed.HasValue)
                {
                    Console.WriteLine(
                        $"Camera: {item.CameraId} | " +
                        $"Max Speed: " +
                        $"{item.MaxSpeed.Value}"
                    );
                }
                else
                {
                    Console.WriteLine(
                        $"Camera: {item.CameraId} | " +
                        "Max Speed: No traffic recorded"
                    );
                }
            }
        }

        private void ShowTopThreeViolators()
        {
            Console.WriteLine();
            Console.WriteLine(
                "Top 3 Violators:"
            );

            List<PlateViolationResult> results =
                _analyzer.GetTopThreeViolators();

            if (results.Count == 0)
            {
                Console.WriteLine(
                    "No violations found."
                );

                return;
            }

            foreach (PlateViolationResult item in results)
            {
                Console.WriteLine(
                    $"Plate: {item.PlateNo} | " +
                    $"Violations: {item.ViolationCount}"
                );
            }
        }

        private void ShowViolationPercentage()
        {
            Console.WriteLine();
            Console.WriteLine(
                "Violation Percentage:"
            );

            double percentage =
                _analyzer.GetViolationPercentage();

            Console.WriteLine(
                $"Total Events: {_events.Count}"
            );

            Console.WriteLine(
                $"Violation Percentage: " +
                $"{percentage:F2}%"
            );
        }

        private void PrintTrafficEvent(
            TrafficEvent item)
        {
            Console.WriteLine(
                $"Id: {item.Id} | " +
                $"Plate: {item.PlateNo} | " +
                $"Camera: {item.CameraId} | " +
                $"Speed: {item.Speed} | " +
                $"MaxSpeed: {item.MaxSpeed} | " +
                $"Date: {item.DateTime}"
            );
        }
    }
}