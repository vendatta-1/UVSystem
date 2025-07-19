namespace UVS.Domain.Common;

public static class GradeScale
{
    public static double ToGpaPoint(this double grade)
    {
        return grade switch
        {
            >= 90 => 4.0,
            >= 80 => 3.0,
            >= 70 => 2.0,
            >= 60 => 1.0,
            _ => 0.0
        };
    }
    
}