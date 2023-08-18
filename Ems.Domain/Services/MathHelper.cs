namespace Ems.Domain.Services;

public static class MathHelper
{
    public static double Median(this List<int> data)
    {
        data.Sort();
        return data.Count % 2 == 1 ? data[data.Count / 2] : (data[data.Count / 2] + data[data.Count / 2 - 1]) / 2.0;
    }

    private static double Mean(this List<int> arr)
    {
        return (double)arr.Sum() / arr.Count;
    }

    public static double StandardDeviation(this List<int> arr)
    {
        return Math.Sqrt(arr.Sum(value => Math.Pow(value - arr.Mean(), 2)) / arr.Count);
    }
    
    public static int HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const int earthRadius = 6371000; // Радиус Земли в метрах

        // Преобразование градусов в радианы
        var lat1Rad = lat1.ToRadians();
        var lon1Rad = lon1.ToRadians();
        var lat2Rad = lat2.ToRadians();
        var lon2Rad = lon2.ToRadians();

        // Разница между широтами и долготами
        var latDiff = lat2Rad - lat1Rad;
        var lonDiff = lon2Rad - lon1Rad;

        // Вычисление расстояния с использованием формулы хаверсина
        var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = (int)Math.Floor(earthRadius * c);
        
        return distance;
    }

    private static double ToRadians(this double degrees)
    {
        return degrees * Math.PI / 180;
    }
}