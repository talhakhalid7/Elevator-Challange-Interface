using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElevatorInterface_level1
{

    
    // Enumeration for elevator direction
    public enum ElevatorDirection
    {
        Up,
        Down,
        None
    }

    // Enumeration for elevator state
    public enum ElevatorState
    {
        Stopped,
        Moving
    }

    public partial class Form1 : Form
    {
        //ElevatorController elevator = new ElevatorController();
        public Form1()
        {
            // Initialize the elevator controller with default values
            _sensor = new ElevatorSensor();
            _floorRequests = new List<FloorRequestButton>();
            _insideRequests = new List<int>();
            _visitedFloors = new HashSet<int>();
            _logFilePath = @"C:\Users\talha\Desktop\elevator.txt";
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = ElevatorInterface_level1.Properties.Resources.stopped;
            
            if (File.Exists(_logFilePath))
            {
                // Clear the content of the file
                File.Delete(_logFilePath);

            }
        }
        

        private void insidefloor1_Click(object sender, EventArgs e)
        {
            AddInsideRequest(Convert.ToInt16((sender as Button).Text));
            
        }
        private void outside8d_Click(object sender, EventArgs e)
        {
            int floor = Convert.ToInt16(Regex.Match((sender as Button).Text, @"\d+").Value);
            var direction = (sender as Button).Text.Contains("U") ? ElevatorDirection.Up :
                                    (sender as Button).Text.Contains("D") ? ElevatorDirection.Down :
                                    ElevatorDirection.None;
            if (direction != ElevatorDirection.None)
                AddFloorRequest(floor, direction);
        }

        public void updateImage(int floor, Image path)
        {
            switch (floor)
            {
                case 1:
                    if (floor == 1)
                    {
                        this.pictureBox1.Image = (path);
                        this.pictureBox2.Image = null;
                        this.pictureBox3.Image = null;
                        this.pictureBox4.Image = null;
                        this.pictureBox5.Image = null;
                        this.pictureBox6.Image = null;
                        this.pictureBox7.Image = null;
                        this.pictureBox8.Image = null;
                    }

                    break;
                case 2:
                    if (floor == 2)
                    {
                        this.pictureBox2.Image = (path);
                        this.pictureBox1.Image = null;
                        this.pictureBox3.Image = null;
                        this.pictureBox4.Image = null;
                        this.pictureBox5.Image = null;
                        this.pictureBox6.Image = null;
                        this.pictureBox7.Image = null;
                        this.pictureBox8.Image = null;
                    }
                    break;
                case 3:
                    if (floor == 3)
                    {
                        this.pictureBox3.Image = (path);
                        this.pictureBox1.Image = null;
                        this.pictureBox2.Image = null;
                        this.pictureBox4.Image = null;
                        this.pictureBox5.Image = null;
                        this.pictureBox6.Image = null;
                        this.pictureBox7.Image = null;
                        this.pictureBox8.Image = null;
                    }
                    break;
                case 4:
                    if (floor == 4)
                    {
                        this.pictureBox4.Image = (path);
                        this.pictureBox1.Image = null;
                        this.pictureBox2.Image = null;
                        this.pictureBox3.Image = null;
                        this.pictureBox5.Image = null;
                        this.pictureBox6.Image = null;
                        this.pictureBox7.Image = null;
                        this.pictureBox8.Image = null;
                    }
                    break;
                case 5:
                    if (floor == 5)
                    {
                        this.pictureBox5.Image = (path);
                        this.pictureBox1.Image = null;
                        this.pictureBox2.Image = null;
                        this.pictureBox3.Image = null;
                        this.pictureBox4.Image = null;
                        this.pictureBox6.Image = null;
                        this.pictureBox7.Image = null;
                        this.pictureBox8.Image = null;
                    }
                    break;
                case 6:
                    if (floor == 6)
                    {
                        this.pictureBox6.Image = (path);
                        this.pictureBox1.Image = null;
                        this.pictureBox2.Image = null;
                        this.pictureBox3.Image = null;
                        this.pictureBox4.Image = null;
                        this.pictureBox5.Image = null;
                        this.pictureBox7.Image = null;
                        this.pictureBox8.Image = null;
                    }
                    break;
                case 7:
                    if (floor == 7)
                    {
                        this.pictureBox7.Image = (path);
                        this.pictureBox1.Image = null;
                        this.pictureBox2.Image = null;
                        this.pictureBox3.Image = null;
                        this.pictureBox4.Image = null;
                        this.pictureBox5.Image = null;
                        this.pictureBox6.Image = null;
                        this.pictureBox8.Image = null;
                    }
                    break;
                case 8:
                    if (floor == 8)
                    {
                        this.pictureBox8.Image = (path);
                        this.pictureBox1.Image = null;
                        this.pictureBox2.Image = null;
                        this.pictureBox3.Image = null;
                        this.pictureBox4.Image = null;
                        this.pictureBox5.Image = null;
                        this.pictureBox6.Image = null;
                        this.pictureBox7.Image = null;
                    }
                    break;
                default:
                    this.pictureBox1.Image = (ElevatorInterface_level1.Properties.Resources.stopped);
                    break;
            }
        }

        public void thread()
        {
            if (bw.IsBusy)
            {
                return;
            }

            System.Diagnostics.Stopwatch sWatch = new System.Diagnostics.Stopwatch();
            bw.DoWork += (bwSender, bwArg) =>
            {
                //what happens here must not touch the form
                //as it's in a different thread        
                sWatch.Start();


                var _child = new Thread(() =>
                {
                    Run();
                });
                _child.Start();
                while (_child.IsAlive)
                {
                    if (bw.CancellationPending)
                    {
                        _child.Abort();
                        bwArg.Cancel = true;
                    }
                    Thread.SpinWait(1);
                }

            };

            bw.RunWorkerCompleted += (bwSender, bwArg) =>
            {
                //now you're back in the UI thread you can update the form
                //remember to dispose of bw now  
                sWatch.Stop();
                bw.Dispose();
            };
            
            //Starts the actual work - triggerrs the "DoWork" event
            bw.RunWorkerAsync();
        }
        // Class to represent the elevator sensor
        public class ElevatorSensor
        {
            public int CurrentFloor { get; set; }
            public ElevatorDirection Direction { get; set; }
            public ElevatorState State { get; set; }
            public bool IsOverweight { get; set; }

            public ElevatorSensor()
            {
                // Initialize the sensor with default values
                CurrentFloor = 1;
                Direction = ElevatorDirection.None;
                State = ElevatorState.Stopped;
                IsOverweight = false;
            }
        }
        // Class to represent a floor request button
        public class FloorRequestButton
        {
            public int FloorNumber { get; private set; }
            public ElevatorDirection Direction { get; private set; }

            public FloorRequestButton(int floorNumber, ElevatorDirection direction)
            {
                FloorNumber = floorNumber;
                Direction = direction;
            }
        }
        // Class to represent the elevator controller
        //public class ElevatorController
        //{
            public readonly ElevatorSensor _sensor;
            public readonly List<FloorRequestButton> _floorRequests;
            public readonly List<int> _insideRequests;
            public readonly HashSet<int> _visitedFloors;
            public readonly string _logFilePath;

            BackgroundWorker bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
        
            public void AddFloorRequest(int floorNumber, ElevatorDirection direction)
            {
                // Add a floor request to the list
                _floorRequests.Add(new FloorRequestButton(floorNumber, direction));
                Log($"[{DateTime.Now}] Floor {floorNumber} {direction} request added.");
                thread();

        }

        public async Task AddInsideRequest(int floorNumber)
            {
                //ElevatorController elevator = new ElevatorController();
                // Add an inside request to the list
                _insideRequests.Add(floorNumber);
                Log($"[{DateTime.Now}] Inside request for floor {floorNumber} added.");
                thread();
            }

            public async Task Run()
            {
                // Start the elevator and process requests until there are no more requests
                while (_floorRequests.Any() || _insideRequests.Any())
                {
                    if (_sensor.IsOverweight)
                    {
                        // If the elevator is overweight, stop only at floors that were selected from inside the elevator
                        if (_insideRequests.Any())
                        {
                            var nextFloor = GetNextInsideRequestFloor();
                            await MoveElevatorToFloor(nextFloor);
                        }
                        else
                        {
                            // If there are no more inside requests, wait until weight limit is no longer exceeded
                            Log($"[{DateTime.Now}] Waiting for passengers to exit (overweight).");
                            Wait(5000);
                        }
                    }
                    else
                    {
                        // If elevator is not overweight, process any floor requests
                        if (_floorRequests.Any())
                        {
                            var nextFloor = GetNextRequestedFloor();
                            await MoveElevatorToFloor(nextFloor);
                        }
                        else
                        {
                            // If there are no more floor requests, process any inside requests
                            if (_insideRequests.Any())
                            {
                                var nextFloor = GetNextInsideRequestFloor();
                                await MoveElevatorToFloor(nextFloor);
                            }
                        }
                    }
                }

                Log($"[{DateTime.Now}] All requests completed. Elevator stopped.");
            }

            private int GetNextRequestedFloor()
            {
                //_floorRequests.Sort();
                // Get the closest floor request in the direction of motion
                var nextRequest = _floorRequests
                    .Where(fr => fr.Direction == _sensor.Direction || _sensor.Direction == ElevatorDirection.None)
                    .OrderBy(fr => Math.Abs(_sensor.CurrentFloor - fr.FloorNumber))
                    .FirstOrDefault();

                if (nextRequest != null)
                {
                    // Remove the floor request from the list and return the next floor to visit
                    _floorRequests.Remove(nextRequest);
                    return nextRequest.FloorNumber;
                }
                else
                {
                    // If there are no floor requests in the direction of motion, reverse the direction and try again
                    _sensor.Direction = _sensor.Direction == ElevatorDirection.Up ? ElevatorDirection.Down : ElevatorDirection.Up;
                    return GetNextRequestedFloor();
                }
            }

            private int GetNextInsideRequestFloor()
            {
                // Get the closest inside request in any direction
                var nextRequest = _insideRequests
                    .OrderBy(i => Math.Abs(_sensor.CurrentFloor - i))
                    .First();

                // Remove the inside request from the list and return the next floor to visit
                _insideRequests.Remove(nextRequest);
                return nextRequest;
            }

            public async Task MoveElevatorToFloor(int floorNumber)
            {
               
                // Update the sensor data for the next floor
                var waitTime = 3000;
                var distance = Math.Abs(_sensor.CurrentFloor - floorNumber);
                var direction = floorNumber > _sensor.CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
                _sensor.State = ElevatorState.Moving;
                _sensor.Direction = direction;

                // Move the elevator to the next floor
                while (_sensor.CurrentFloor != floorNumber)
                {
                    if (_floorRequests.Any(fr => fr.FloorNumber == _sensor.CurrentFloor && fr.Direction == _sensor.Direction))
                    {
                        // If there is a request for this floor in the direction of motion, stop and remove the request
                        _floorRequests.RemoveAll(fr => fr.FloorNumber == _sensor.CurrentFloor && fr.Direction == _sensor.Direction);
                        _visitedFloors.Add(_sensor.CurrentFloor);
                        Log($"[{DateTime.Now}] Floor {_sensor.CurrentFloor} {direction} request serviced.");

                    }
                    else if (_sensor.IsOverweight && _insideRequests.Contains(_sensor.CurrentFloor))
                    {
                        // If the elevator is overweight and there is an inside request for this floor, stop and remove the request
                        _insideRequests.Remove(_sensor.CurrentFloor);
                        _visitedFloors.Add(_sensor.CurrentFloor);
                        Log($"[{DateTime.Now}] Inside request for floor {_sensor.CurrentFloor} serviced (overweight).");

                    }

                    // Update the sensor data for the next floor
                    if (_sensor.CurrentFloor < floorNumber)
                        _sensor.CurrentFloor++;
                    else if (_sensor.CurrentFloor > floorNumber)
                        _sensor.CurrentFloor--;

                    Log($"[{DateTime.Now}] Passed floor {_sensor.CurrentFloor}.");
                    Wait(1000);
                    updateImage(_sensor.CurrentFloor, ElevatorInterface_level1.Properties.Resources.moving);


                    // If there are any new floor requests in the opposite direction, adjust the wait time accordingly
                    var oppositeRequests = _floorRequests
                        .Where(fr => fr.FloorNumber > _sensor.CurrentFloor && _sensor.Direction == ElevatorDirection.Down ||
                                     fr.FloorNumber < _sensor.CurrentFloor && _sensor.Direction == ElevatorDirection.Up);
                    if (oppositeRequests.Any())
                    {
                        waitTime = Math.Max(waitTime, 5000);
                    }

                    //Wait(3000);
                }

                // Stop the elevator and wait for passengers to exit
                _sensor.State = ElevatorState.Stopped;
                _visitedFloors.Add(_sensor.CurrentFloor);
                Log($"Stopped at floor {_sensor.CurrentFloor}.");
                Wait(1000);
                updateImage(_sensor.CurrentFloor, ElevatorInterface_level1.Properties.Resources.stopped);
                Wait(waitTime);
            }

            private void Wait(int milliseconds)
            {
                // Helper method to pause execution for a specified number of milliseconds
                System.Threading.Thread.Sleep(milliseconds);
            }

            private void Log(string message)
            {
                if (!File.Exists(_logFilePath))
                {
                    FileStream fs = File.Create(_logFilePath);
                    fs.Close();
                    // writing elevator logs to newly created file
                    using (var writer = File.AppendText(_logFilePath))
                    {
                        writer.WriteLine(message);
                    }

                }
                else
                {
                    // writing elevator logs to newly created file
                    using (var writer = File.AppendText(_logFilePath))
                    {
                        writer.WriteLine(message);
                    }
                }



            }

       
    }


    }

