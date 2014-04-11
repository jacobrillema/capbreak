using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capbreak.Areas.Wx.Models
{
    public class CoordinateModel
    {
        public const string CentralPlains =
            "43.34, -107.92, 0, 0	;Top Left\r\n" +
            "37.98, -107.36, 0, .5	;Middle Left\r\n" +
            "43.54, -97.12, .5, 0	;Top Middle\r\n" +
            "37.98, -107.36, 0, .5	;Middle Left\r\n" +
            "43.54, -97.12, .5, 0	;Top Middle\r\n" +
            "38.14, -97.33, .5, .5	;Middle Middle\r\n" +
            "43.54, -97.12, .5, 0	;Top Middle\r\n" +
            "38.14, -97.33, .5, .5	;Middle Middle\r\n" +
            "42.82, -86.36, 1, 0	;Top Right\r\n" +
            "38.14, -97.33, .5, .5	;Middle Middle\r\n" +
            "42.82, -86.36, 1, 0	;Top Right\r\n" +
            "37.48, -87.40, 1, .5	;Middle Right\r\n" +
            "37.98, -107.36, 0, .5	;Middle Left\r\n" +
            "32.57, -106.84, 0, 1	;Bottom Left\r\n" +
            "38.14, -97.33, .5, .5	;Middle Middle\r\n" +
            "32.57, -106.84, 0, 1	;Bottom Left\r\n" +
            "38.14, -97.33, .5, .5	;Middle Middle\r\n" +
            "32.77, -97.50, .5, 1	;Bottom Middle\r\n" +
            "38.14, -97.33, .5, .5	;Middle Middle\r\n" +
            "32.76, -97.50, .5, 1	;Bottom Middle\r\n" +
            "37.48, -87.40, 1, .5	;Middle Right\r\n" +
            "32.76, -97.50, .5, 1	;Bottom Middle\r\n" +
            "37.48, -87.40, 1, .5	;Middle Right\r\n" +
            "32.17, -88.27, 1, 1	;Bottom Right\r\n";

        public const string NorthernPlains =
            "50.73, -109.13, 0, 0	;TL\r\n" +
            "50.88, -96.65, .5, 0	;TM\r\n" +
            "44.70, -108.33, 0, .5	;ML\r\n" +
            "45.00, -96.98, .5, .5	;MM\r\n" +
            "44.70, -108.33, 0, .5	;ML\r\n" +
            "50.88, -96.65, .5, 0	;TM\r\n" +
            "45.00, -96.98, .5, .5	;MM\r\n" +
            "50.88, -96.65, .5, 0	;TM\r\n" +
            "49.95, -84.20, 1, 0	;TR\r\n" +
            "45.00, -96.98, .5, .5	;MM\r\n" +
            "49.95, -84.20, 1, 0	;TR\r\n" +
            "44.28, -85.75, 1, .5	;MR\r\n" +
            "45.00, -96.98, .5, .5	;MM\r\n" +
            "44.70, -108.33, 0, .5	;ML\r\n" +
            "38.68, -107.66, 0, 1	;BL\r\n" +
            "38.68, -107.66, 0, 1	;BL\r\n" +
            "45.00, -96.98, .5, .5	;MM\r\n" +
            "38.85, -97.24, .5, 1	;BM\r\n" +
            "45.00, -96.98, .5, .5	;MM\r\n" +
            "38.85, -97.24, .5, 1	;BM\r\n" +
            "44.28, -85.75, 1, .5	;MR\r\n" +
            "38.85, -97.24, .5, 1	;BM\r\n" +
            "44.28, -85.75, 1, .5	;MR\r\n" +
            "38.00, -86.95, 1, 1	;BR\r\n";
    }
}