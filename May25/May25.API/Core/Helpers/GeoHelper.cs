using System;

namespace May25.API.Core.Helpers
{
    public static class GeoHelper
    {
        public static bool ArePointsNear(double lat1, double lng1, double lat2, double lng2, int distance = 10000)
        {
            return GetDistanceBetweenPoints(lat1, lng1, lat2, lng2) < distance;
        }

        public static double GetDistanceBetweenPoints(double lat1, double lng1, double lat2, double lng2)
        {
            int earthRadius = 6371; // In km

            double dLat = DegToRad(lat2 - lat1);
            double dLon = DegToRad(lng2 - lng1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(DegToRad(lat1)) * Math.Cos(DegToRad(lat2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadius * c * 1000;
        }

        private static double DegToRad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
