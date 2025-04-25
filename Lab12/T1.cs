using System;
using System.Threading;

namespace AlarmConsoleApp
{
    // Define a delegate for the alarm event
    public delegate void AlarmEventHandler(object source, EventArgs args);

    // Publisher class that monitors time
    public class TimeMonitor
    {
        // Define the event using the delegate
        public event AlarmEventHandler? RaiseAlarm;

        // Method to start monitoring time
        public void StartMonitoring(DateTime targetTime)
        {
            Console.WriteLine("Time monitoring started...");
            
            while (true)
            {
                DateTime currentTime = DateTime.Now;
                
                // Show current time
                Console.WriteLine($"Current time: {currentTime:HH:mm:ss}");
                
                // Check if target time matches current time
                if (currentTime.Hour == targetTime.Hour && 
                    currentTime.Minute == targetTime.Minute && 
                    currentTime.Second == targetTime.Second)
                {
                    // Trigger the event
                    OnRaiseAlarm();
                    break;
                }
                
                // Wait for 1 second before checking again
                Thread.Sleep(1000);
            }
        }

        // Method to raise the alarm event
        protected virtual void OnRaiseAlarm()
        {
            if (RaiseAlarm != null)
            {
                RaiseAlarm(this, EventArgs.Empty);
            }
        }
    }

    // Subscriber class that handles the alarm
    public class AlarmHandler
    {
        public void Ring_Alarm(object source, EventArgs args)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("ALARM! The target time has been reached!");
            Console.WriteLine("===================================");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Alarm Clock Application");
            Console.WriteLine("Enter time in HH:MM:SS format:");
            
            string timeInput = Console.ReadLine();
            DateTime targetTime;
            
            // Validate input time format
            if (DateTime.TryParseExact(timeInput, "HH:mm:ss", null, 
                System.Globalization.DateTimeStyles.None, out targetTime))
            {
                // Create publisher and subscriber instances
                TimeMonitor monitor = new TimeMonitor();
                AlarmHandler handler = new AlarmHandler();
                
                // Subscribe to the event
                monitor.RaiseAlarm += handler.Ring_Alarm;
                
                // Start monitoring
                monitor.StartMonitoring(targetTime);
            }
            else
            {
                Console.WriteLine("Invalid time format. Please use HH:MM:SS format.");
            }
        }
    }
}
