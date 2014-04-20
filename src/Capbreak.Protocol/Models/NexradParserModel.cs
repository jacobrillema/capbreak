using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capbreak.Protocol.Models
{
    public class Header
    {
        public string Code { get; set; }
        public string Site { get; set; }
        public string Date { get; set; }
        public string FullProductCode { get; set; }
        public short BodyCode { get; set; }
        public short BodyDate { get; set; }
        public int BodyTime { get; set; }
        public int Length { get; set; }
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
        public int BlockCount { get; set; }
    }

    public class Descriptor
    {
        public Int16 Divider { get; set; }
        public Int32 Latitude { get; set; }
        public Int32 Longitude { get; set; }
        public Int16 Height { get; set; }
        public Int16 Code { get; set; }
        public Int16 Mode { get; set; }
        public Int16 VolumeCoveragePattern { get; set; }
        public Int16 SequenceNumber { get; set; }
        public Int16 ScanNumber { get; set; }
        public Int16 ScanDate { get; set; }
        public Int32 ScanTime { get; set; }
        public Int16 GenerationDate { get; set; }
        public Int32 GenerationTime { get; set; }
        public Int16 ElevationNumber { get; set; }
        public byte Version { get; set; }
        public byte SpotBlank { get; set; }
        public Int32 SymbologyOffset { get; set; }
        public Int32 GraphicOffset { get; set; }
        public Int32 TabularOffset { get; set; }
        public Int16 ProductSpecific1 { get; set; }
        public Int16 ProductSpecific2 { get; set; }
        public Int16 ProductSpecific3 { get; set; }
        public Int16 ProductSpecific4 { get; set; }
        public Int16 ProductSpecific5 { get; set; }
        public Int16 ProductSpecific6 { get; set; }
        public Int16 ProductSpecific7 { get; set; }
        public Int16 ProductSpecific8 { get; set; }
        public Int16 ProductSpecific9 { get; set; }
        public Int16 ProductSpecific10 { get; set; }
        public Int16 Threshold1 { get; set; }
        public Int16 Threshold2 { get; set; }
        public Int16 Threshold3 { get; set; }
        public Int16 Threshold4 { get; set; }
        public Int16 Threshold5 { get; set; }
        public Int16 Threshold6 { get; set; }
        public Int16 Threshold7 { get; set; }
        public Int16 Threshold8 { get; set; }
        public Int16 Threshold9 { get; set; }
        public Int16 Threshold10 { get; set; }
        public Int16 Threshold11 { get; set; }
        public Int16 Threshold12 { get; set; }
        public Int16 Threshold13 { get; set; }
        public Int16 Threshold14 { get; set; }
        public Int16 Threshold15 { get; set; }
        public Int16 Threshold16 { get; set; }
    }

    public class Symbology
    {
        public Int16 Divider { get; set; }
        public Int16 BlockId { get; set; }
        public Int32 Length { get; set; }
        public Int16 Layers { get; set; }
        public Int16 LayerDivider { get; set; }
        public Int32 DataLength { get; set; }
        public Int16 PacketCode { get; set; }
        public Int16 IndexFirstRangeBin { get; set; }
        public Int16 RangeBinCount { get; set; }
        public Int16 SweepCenterI { get; set; }
        public Int16 SweepCenterJ { get; set; }
        public Int16 ScaleFactor { get; set; }
        public Int16 RadialCount { get; set; }
        public List<Radial> RadialData { get; set; }
    }

    public class Radial
    {
        public int RleCount { get; set; }
        public double StartAngle { get; set; }
        public double AngleDelta { get; set; }
        public List<Int32> ColorValues { get; set; }
    }

    public class NexradScan
    {
        public Header Header { get; set; }
        public Descriptor Descriptor { get; set; }
        public Symbology Symbology { get; set; }
    }
}
