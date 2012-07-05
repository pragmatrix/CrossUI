namespace CrossUI
{
    public sealed class PathCommand : CrossUI.IPathCommand
    {
        public const int MoveTo = 1;
        public const int LineTo = 2;
        public const int CurveTo = 3;

        readonly int _command;

        readonly double _x;
        readonly double _y;

        readonly double _xC1;
        readonly double _yC1;

        readonly double _xC2;
        readonly double _yC2;

        public PathCommand(double x, double y, int command, double xC1 = 0, double yC1 = 0, double xC2 = 0, double yC2 = 0)
        {
            _x = x;
            _y = y;
            _command = command;

            _xC1 = xC1;
            _yC1 = yC1;
            _xC2 = xC2;
            _yC2 = yC2;
        }

        public int Command { get { return _command; } }

        public double X { get { return _x; } }
        public double Y { get { return _y; } }

        public double XC1 { get { return _xC1; } }
        public double YC1 { get { return _xC2; } }

        public double XC2 { get { return _xC2; } }
        public double YC2 { get { return _yC2; } }
    }
}
