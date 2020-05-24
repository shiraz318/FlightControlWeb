using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FlightControlWeb.Models.FlightPlan;

namespace FlightControlWeb.Models
{
    public class FlightGenerator
    {
        int initialLocationLongitudeE;
        int initialLocationLatitudeE;
        int initalLocationDateTimeE;

        int segmentFlightIdE;
        int segmentLongitudeE;
        int segmentLatitudeE;
        int segmenTimespanSecondE;

        public FlightGenerator()
        {
            // InitialLocation Enum.
            initialLocationLongitudeE = (int)InitialLocationE.Longitude;
            initialLocationLatitudeE = (int)InitialLocationE.Latitude;
            initalLocationDateTimeE = (int)InitialLocationE.DateTime;

            // Segments Enum.
            segmentFlightIdE = (int)SegmentsE.FlightId;
            segmentLongitudeE = (int)SegmentsE.Longitude;
            segmentLatitudeE = (int)SegmentsE.Latitude;
            segmenTimespanSecondE = (int)SegmentsE.TimespanSecond;
        }

        // Calculate the number of the current segment.
        private int CalculateCurrentSegment(out DateTime soFarDuration, List<Object[]> segements,
            DateTime initialTime, DateTime requierdTime)
        {
            soFarDuration = initialTime;
            DateTime soFar = initialTime;
            int count = 0;
            foreach (object[] seg in segements)
            {
                TimeSpan duration = new TimeSpan(0, 0, 0,
                    Convert.ToInt32(seg[segmenTimespanSecondE]));
                soFar = soFar.AddSeconds(Convert.ToDouble(seg[segmenTimespanSecondE]));
                int result = DateTime.Compare(requierdTime, soFar);
                // requierdTime is earlier than soFar
                if (result < 0)
                {
                    return count;
                }
                count++;
                soFarDuration = 
                    soFarDuration.AddSeconds(Convert.ToDouble(seg[segmenTimespanSecondE]));
            }
            return count;
        }

        // Calculate the partial position in the segment.
        private void CalculatePartialPostition(out Location location, int segmentNumber,
        List<Object[]> segments, object[] initialLocation, double partialTime)
        {

            location = new Location(0, 0, new DateTime());
            double initLon = Convert.ToDouble(initialLocation[initialLocationLongitudeE]);
            double initLat = Convert.ToDouble(initialLocation[initialLocationLatitudeE]);

            double lon = Convert.ToDouble(segments[segmentNumber][segmentLongitudeE]);
            double lat = Convert.ToDouble(segments[segmentNumber][segmentLatitudeE]);
            int timespan = Convert.ToInt32(segments[segmentNumber][segmenTimespanSecondE]);
            Segment currentSegment = new Segment(lon, lat, timespan);
            Segment prevSegmen = new Segment(initLon, initLat, 0);
            if (segmentNumber > 0)
            {
                lon = Convert.ToDouble(segments[segmentNumber - 1][segmentLongitudeE]);
                lat = Convert.ToDouble(segments[segmentNumber - 1][segmentLatitudeE]);
                timespan = Convert.ToInt32(segments[segmentNumber - 1][segmenTimespanSecondE]);
                prevSegmen = new Segment(lon, lat, timespan);

            }
            // linear interpulation is needed.
            location.Latitude = prevSegmen.Latitude + 
                ((currentSegment.Latitude - prevSegmen.Latitude) * partialTime);
            location.Longitude = prevSegmen.Longitude + 
                ((currentSegment.Longitude - prevSegmen.Longitude) * partialTime);
        }

        // Calculate the position.
        public Location CalculatePosition(DateTime soFar, DateTime time, int segmentNumber,
            List<Object[]> segmennts, object[] initialLocation)
        {
            Location location;

            // How mach time we are in the segment until the requierd time = timeInSegmnent.
            TimeSpan timeInSegmnent = time - soFar;
            double timeToGo = timeInSegmnent.TotalSeconds;// Seconds;

            // timeInSegmnent/timespan = the partial time.
            double timespan = Convert.ToDouble(segmennts[segmentNumber][segmenTimespanSecondE]);
            double partialTime = timeToGo / timespan;

            CalculatePartialPostition(out location, segmentNumber, segmennts,
                initialLocation, partialTime);
            location.DateTime = time;

            return location;
        }

        // Create a FlightPlan object using a given data.
        public Flights CreateFlightFromGivenData(object[] initialLocation,
            List<Object[]> segements, bool isExternal, DateTime time)
        {
            Flights flight = new Flights();
            DateTime soFar;

            int segmentNumber = CalculateCurrentSegment(out soFar, segements,
                Convert.ToDateTime(initialLocation[initalLocationDateTimeE]), time);
            Location location = CalculatePosition(soFar, time, segmentNumber,
                segements, initialLocation);
            flight.DateTime = location.DateTime;
            flight.Latitude = location.Latitude;
            flight.Longitude = location.Longitude;
            flight.IsExternal = isExternal;
            flight.FlightId = segements[0][segmentFlightIdE].ToString();
            return flight;
        }
    }
}
