using camera_control_project;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            List<TrafficEvent> events = new List<TrafficEvent>();

            List<int> cameraIds = new List<int>
            {
                1, 2, 3, 4, 5,
                6, 7, 8, 9, 10
            };

            TrafficEventAnalyzer analyzer =
                new TrafficEventAnalyzer(events, cameraIds);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("What do you want?");
                Console.WriteLine("1. Generate database.");
                Console.WriteLine("2. List of traffic.");
                Console.WriteLine("3. Speed violations.");
                Console.WriteLine("4. Number of violations for each camera.");
                Console.WriteLine("5. Last violation for each plate.");
                Console.WriteLine("6. Plates with more than 5 violations");
                Console.WriteLine("7. Cameras with no violations");
                Console.WriteLine("8. Maximum speed of each camera");
                Console.WriteLine("9. Top 3 most violating plates");
                Console.WriteLine("10. Violation percentage");
                Console.WriteLine("0. Finish");

                try
                {
                    int choice = int.Parse(Console.ReadLine());

                    // Validate menu choice
                    if (choice < 0 || choice > 10)
                    {
                        Console.WriteLine();
                        Console.WriteLine(
                            "Please enter a number between 0 and 10."
                        );

                        continue;
                    }

                    // Check if database has been generated
                    if (choice >= 2 && choice <= 10 && events.Count == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine(
                            "Database not generated! Press 1 to generate."
                        );

                        continue;
                    }

                    switch (choice)
                    {
                        case 1:

                            Console.WriteLine("Generating database.");

                            TrafficEventGenerator generator =
                                new TrafficEventGenerator();

                            events = generator.GenerateEvents(500);

                            analyzer = new TrafficEventAnalyzer(
                                events,
                                cameraIds
                            );

                            Console.WriteLine(
                                "Database generated successfully."
                            );

                            break;

                        case 2:

                            Console.WriteLine();
                            Console.WriteLine("List of traffics:");

                            Console.WriteLine(
                                $"Total Events: {events.Count}"
                            );

                            Console.WriteLine();

                            foreach (TrafficEvent item in events)
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

                            break;

                        case 3:

                            Console.WriteLine();
                            Console.WriteLine("Speeding Violations:");

                            List<TrafficEvent> speedingViolations =
                                analyzer.GetSpeedingViolations();

                            if (speedingViolations.Count == 0)
                            {
                                Console.WriteLine(
                                    "No speeding violations found."
                                );
                            }
                            else
                            {
                                foreach (TrafficEvent item in speedingViolations)
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

                            break;

                        case 4:

                            Console.WriteLine();
                            Console.WriteLine("Each Camera Violation:");

                            List<CameraViolationResult> cameraViolations =
                                analyzer.GetViolationsByCamera();

                            if (cameraViolations.Count == 0)
                            {
                                Console.WriteLine(
                                    "No violations found for any camera."
                                );
                            }
                            else
                            {
                                foreach (
                                    CameraViolationResult item
                                    in cameraViolations)
                                {
                                    Console.WriteLine(
                                        $"Camera {item.CameraId}: " +
                                        $"{item.ViolationCount} violations"
                                    );
                                }
                            }

                            break;

                        case 5:

                            Console.WriteLine();
                            Console.WriteLine(
                                "Each Plate Last Violation:"
                            );

                            List<TrafficEvent> lastViolations =
                                analyzer.GetLastViolationByPlate();

                            if (lastViolations.Count == 0)
                            {
                                Console.WriteLine(
                                    "No violations found."
                                );
                            }
                            else
                            {
                                foreach (TrafficEvent item in lastViolations)
                                {
                                    Console.WriteLine(
                                        $"Plate: {item.PlateNo} | " +
                                        $"Speed: {item.Speed} | " +
                                        $"Camera: {item.CameraId} | " +
                                        $"Date: {item.DateTime}"
                                    );
                                }
                            }

                            break;

                        case 6:

                            Console.WriteLine();
                            Console.WriteLine(
                                "Plates With More Than Five Violations:"
                            );

                            List<FrequentViolatorResult> frequentViolators =
                                analyzer.GetFrequentViolators();

                            if (frequentViolators.Count == 0)
                            {
                                Console.WriteLine(
                                    "No plate has more than five violations."
                                );
                            }
                            else
                            {
                                foreach (
                                    FrequentViolatorResult item
                                    in frequentViolators)
                                {
                                    Console.WriteLine(
                                        $"Plate: {item.PlateNo} | " +
                                        $"Violations: {item.ViolationCount}"
                                    );
                                }
                            }

                            break;

                        case 7:

                            Console.WriteLine();
                            Console.WriteLine(
                                "Cameras With No Violations:"
                            );

                            List<int> camerasWithoutViolations =
                                analyzer.GetCamerasWithoutViolations();

                            if (camerasWithoutViolations.Count == 0)
                            {
                                Console.WriteLine(
                                    "All cameras have at least one violation."
                                );
                            }
                            else
                            {
                                foreach (
                                    int cameraId
                                    in camerasWithoutViolations)
                                {
                                    Console.WriteLine(
                                        $"Camera {cameraId} has no violations."
                                    );
                                }
                            }

                            break;

                        case 8:

                            Console.WriteLine();
                            Console.WriteLine(
                                "Each Camera Max Speed:"
                            );

                            List<MaxSpeedByCameraResult> maxSpeeds =
                                analyzer.GetMaxSpeedByCamera();

                            foreach (
                                MaxSpeedByCameraResult item
                                in maxSpeeds)
                            {
                                if (item.MaxSpeed.HasValue)
                                {
                                    Console.WriteLine(
                                        $"Camera: {item.CameraId} | " +
                                        $"Max Speed: {item.MaxSpeed.Value}"
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

                            break;

                        case 9:

                            Console.WriteLine();
                            Console.WriteLine(
                                "Top 3 Violators:"
                            );

                            List<TopViolatorResult> topThreeViolators =
                                analyzer.GetTopThreeViolators();

                            if (topThreeViolators.Count == 0)
                            {
                                Console.WriteLine(
                                    "No violations found."
                                );
                            }
                            else
                            {
                                foreach (
                                    TopViolatorResult item
                                    in topThreeViolators)
                                {
                                    Console.WriteLine(
                                        $"Plate: {item.PlateNo} | " +
                                        $"Violations: {item.ViolationCount}"
                                    );
                                }
                            }

                            break;

                        case 10:

                            Console.WriteLine();
                            Console.WriteLine(
                                "Violation Percentage:"
                            );

                            double violationPercentage =
                                analyzer.GetViolationPercentage();

                            Console.WriteLine(
                                $"Total Events: {events.Count}"
                            );

                            Console.WriteLine(
                                $"Violation Percentage: " +
                                $"{violationPercentage:F2}%"
                            );

                            break;

                        case 0:

                            return;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine(
                        "Please enter a valid number!"
                    );
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"An unexpected error occurred: {ex.Message}"
            );
        }
    }
}