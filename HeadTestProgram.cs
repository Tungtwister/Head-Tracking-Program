using System;
using Tobii.Interaction;
using Tobii.Interaction.Framework;


namespace headtest4
{
    public class Program
    {
        private static Host _host;
        private static HeadPoseStream _headPoseStream;

        private static SamplePage _currentPage;
        private static ConsoleColor _defaultForegroundColor;

        public static void Main(string[] args)
        {
            InitializeHost();
            _defaultForegroundColor = Console.ForegroundColor;
            ConstructHeadPoseStreamPage();
            _currentPage = SamplePage.HeadPoseMenu;

            var userResponce = Console.ReadKey(true);
            while (!ReadyToExit(userResponce))
            {
                switch (userResponce.Key)
                {
                    case ConsoleKey.D1:
                        //Console.WriteLine("Stream");
                        CreateAndVisualizeHeadPoseStream();
                        break;

                    case ConsoleKey.D2:
                        //Console.WriteLine("Toggle");
                        ToggleHeadPoseStream();
                        break;
                    case ConsoleKey.D3:
                        Console.WriteLine("Calibrate");
                        break;
                    default:
                        Console.WriteLine();
                        _headPoseStream.IsEnabled = false;
                        PrintHeadPoseStreamDescription();
                        PrintHeadPoseStreamPageActions();
                        break;
                }
                userResponce = Console.ReadKey(true);
            }
            DisableConnectionWithTobiiEngine();
        }

        private static bool ReadyToExit(ConsoleKeyInfo readKey)
        {
            return _currentPage == SamplePage.HeadPoseMenu && readKey.Key == ConsoleKey.D0;
        }
        private enum SamplePage
        {
            Main = 1,
            GazePointMenu = 100,
            FixationMenu = 200,
            EyePositionMenu = 300,
            HeadPoseMenu = 400,
        }
        private static void InitializeHost()
        {
            // Everything starts with initializing Host, which manages connection to the 
            // Tobii Engine and provides all the Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            _host = new Host();
        }
        private static void DisableConnectionWithTobiiEngine()
        {
            // We should disable connection with TobiiEngine before exit the application.
            _host.DisableConnection();
        }
        private static void ConstructHeadPoseStreamPage()
        {
            Console.Clear();

            PrintHeadPoseStreamDescription();
            PrintHeadPoseStreamPageActions();
        }
        private static void PrintHeadPoseStreamDescription()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("|                    Head pose stream                      |");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("Head pose stream provides the user´s head pose by two coordinates \n" +
                              "in the the three-dimensional space. The data stream contains one \n " +
                              "coordinate for the head positions and the other one \n " +
                              "for head rotation");
        }

        private static void PrintHeadPoseStreamPageActions()
        {
            Console.WriteLine();
            Console.WriteLine("");
            Console.WriteLine();
            Console.WriteLine("'1' - start head pose stream");
            Console.WriteLine("'2' - toggle the head pose stream ON/OFF");
            Console.WriteLine("'3' - calibrate head pose");
            Console.WriteLine("'0' - to exit the program");
            Console.WriteLine();
            Console.WriteLine("You can use any other key to pause the running stream: ");
        }
        private static void ToggleHeadPoseStream()
        {
            if (_headPoseStream != null)
                _headPoseStream.IsEnabled = !_headPoseStream.IsEnabled;
        }
        private static void CreateAndVisualizeHeadPoseStream()
        {
            _headPoseStream = _host.Streams.CreateHeadPoseStream();
            _headPoseStream.Next += OnNextHeadPose;
        }

        private static void OnNextHeadPose(object sender, StreamData<HeadPoseData> headPose)
        {
            var timestamp = headPose.Data.Timestamp;
            var hasHeadPosition = headPose.Data.HasHeadPosition;
            var headPosition = headPose.Data.HeadPosition;
            var hasRotation = headPose.Data.HasRotation;
            var headRotation = headPose.Data.HeadRotation;

            Console.WriteLine($"Head pose timestamp  : {timestamp}");
            Console.WriteLine($"Has head position    : {hasHeadPosition}");
            Console.WriteLine($"Has rotation  (X,Y,Z): ({hasRotation.HasRotationX},{hasRotation.HasRotationY},{hasRotation.HasRotationZ})");
            if(headPosition.X <= -40)
            {
                Console.WriteLine("HEAD POSITION LEFT");
            }
            if (headPosition.X >= 40)
            {
                Console.WriteLine("HEAD POSITION RIGHT");
            }
            Console.WriteLine($"Head position (X,Y,Z): ({headPosition.X},{headPosition.Y},{headPosition.Z})");
            if (headRotation.Y <= -0.2)
            {
                Console.WriteLine("HEAD ROTATION RIGHT");
            }
            if (headRotation.Y >= 0.2)
            {
                Console.WriteLine("HEAD ROTATION LEFT");
            }
            Console.WriteLine($"Head rotation (X,Y,Z): ({headRotation.X},{headRotation.Y},{headRotation.Z})");
            Console.WriteLine("-----------------------------------------------------------------");
        }
    }
}
