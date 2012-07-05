using System;
namespace CrossUI
{
    public interface IPathCommand
    {
        int Command { get; }
        double X { get; }
        double Y { get; }

        double XC1 { get; }
        double YC1 { get; }

        double XC2 { get; }
        double YC2 { get; }
    }
}
