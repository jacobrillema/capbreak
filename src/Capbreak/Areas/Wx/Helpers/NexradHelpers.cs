using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Capbreak.Protocol.Warnings;

namespace Capbreak.Areas.Wx.Helpers
{
    public static class NexradHelpers
    {
        // TODO add tryparse/error handling
        public static List<PointF> ConvertStringToPolygon(string raw)
        {
            // "34,-87.68 34,-87.29 33.96,-87.26 33.74,-87.82 33.85,-87.9 34,-87.68"

            var polygon = new List<PointF>();
            if (!String.IsNullOrEmpty(raw))
            {
                var pairs = raw.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in pairs)
                {
                    var coords = pair.Split(',');
                    polygon.Add(new System.Drawing.PointF { X = float.Parse(coords[0]), Y = float.Parse(coords[1]) });
                }
            }

            return polygon;
        }
    }
}