using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Capbreak.Protocol.Models;
using Capbreak.Core;

namespace Capbreak.Protocol.NexradParser
{
    public interface INexradParserService : IService
    {
        Task<List<string>> FetchNexradFileListAsync(string site, string product);
    }

    public class NexradParserService
    {
        public async Task<List<string>> FetchNexradFileListAsync(string site, string product)
        {
            // http://level3.allisonhouse.com/level3/f410ba3360389cc8401869160fc0a4cb/MPX/N0U/dir.list
            var filelist = new List<string>();
            //var endpoint = String.Format(nexradbase, site, product);

            //using (var client = new HttpClient())
            //{
            //    var response = await client.GetStringAsync(endpoint);
            //    if (!String.IsNullOrEmpty(response))
            //        filelist = response.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //}

            return filelist;
        }

        public async Task<NexradScan> FetchNexradScanAsync(string site, string product, string filename)
        {
            // http://level3.allisonhouse.com/level3/f410ba3360389cc8401869160fc0a4cb/MPX/N0U/N0U_20140416_1641
            // // http://weather.noaa.gov/pub/SL.us008001/DF.of/DC.radar/DS.p19r0/SI.kmpx/
            var scan = new NexradScan();
            var filelist = new List<string>();
            var nexradbase = "http://weather.noaa.gov/pub/SL.us008001/DF.of/DC.radar/DS.{0}/SI.{1}/{2}";

            switch (product.ToLowerInvariant())
            {
                case "n0r":
                    product = "p19r0";
                    break;
            }

            var endpoint = String.Format(nexradbase, product, site, filename);
            Stream response;

            using (var client = new HttpClient())
            {
                response = await client.GetStreamAsync(endpoint);
            }

            if (response != null)
            {
                var ms = new MemoryStream();
                response.CopyTo(ms);
                scan = ParseNexrad(ms);
            }

            return scan;
        }

        public static NexradScan ParseNexrad(MemoryStream stream)
        {
            var nexrad = new NexradScan();

            try
            {
                var headerData = new byte[30];
                byte[] descriptorData;
                var bytes = new byte[stream.Length];
                descriptorData = new byte[stream.Length - 30];
                var descriptor = new Descriptor();
                var symbology = new Symbology();

                // Header
                stream.Position = 0;
                stream.Read(headerData, 0, 30);
                var headerString = System.Text.Encoding.UTF8.GetString(headerData);
                headerString = headerString.Replace("\n", string.Empty).Replace("\r", " ").Replace("  ", " ");
                var headerStrings = headerString.Split(' ');
                var header = new Header { Code = headerStrings[0], Site = headerStrings[1], Date = headerStrings[2], FullProductCode = headerStrings[3] };
                header.BodyCode = ReadHalfWord(stream);
                header.BodyDate = ReadHalfWord(stream);
                header.BodyTime = ReadWord(stream);
                header.Length = ReadWord(stream);
                header.SourceId = ReadHalfWord(stream);
                header.DestinationId = ReadHalfWord(stream);
                header.BlockCount = ReadHalfWord(stream);

                // Descriptor
                descriptor.Divider = ReadHalfWord(stream);
                descriptor.Latitude = ReadWord(stream);
                descriptor.Longitude = ReadWord(stream);
                descriptor.Height = ReadHalfWord(stream);
                descriptor.Code = ReadHalfWord(stream);
                descriptor.Mode = ReadHalfWord(stream);
                descriptor.VolumeCoveragePattern = ReadHalfWord(stream);
                descriptor.SequenceNumber = ReadHalfWord(stream);
                descriptor.ScanNumber = ReadHalfWord(stream);
                descriptor.ScanDate = ReadHalfWord(stream);
                descriptor.ScanTime = ReadWord(stream);
                descriptor.GenerationDate = ReadHalfWord(stream);
                descriptor.GenerationTime = ReadWord(stream);
                descriptor.ProductSpecific1 = ReadHalfWord(stream);
                descriptor.ProductSpecific2 = ReadHalfWord(stream);
                descriptor.ElevationNumber = ReadHalfWord(stream);
                descriptor.ProductSpecific3 = ReadHalfWord(stream);
                descriptor.Threshold1 = ReadHalfWord(stream);
                descriptor.Threshold2 = ReadHalfWord(stream);
                descriptor.Threshold3 = ReadHalfWord(stream);
                descriptor.Threshold4 = ReadHalfWord(stream);
                descriptor.Threshold5 = ReadHalfWord(stream);
                descriptor.Threshold6 = ReadHalfWord(stream);
                descriptor.Threshold7 = ReadHalfWord(stream);
                descriptor.Threshold8 = ReadHalfWord(stream);
                descriptor.Threshold9 = ReadHalfWord(stream);
                descriptor.Threshold10 = ReadHalfWord(stream);
                descriptor.Threshold11 = ReadHalfWord(stream);
                descriptor.Threshold12 = ReadHalfWord(stream);
                descriptor.Threshold13 = ReadHalfWord(stream);
                descriptor.Threshold14 = ReadHalfWord(stream);
                descriptor.Threshold15 = ReadHalfWord(stream);
                descriptor.Threshold16 = ReadHalfWord(stream);
                descriptor.ProductSpecific4 = ReadHalfWord(stream);
                descriptor.ProductSpecific5 = ReadHalfWord(stream);
                descriptor.ProductSpecific6 = ReadHalfWord(stream);
                descriptor.ProductSpecific7 = ReadHalfWord(stream);
                descriptor.ProductSpecific8 = ReadHalfWord(stream);
                descriptor.ProductSpecific9 = ReadHalfWord(stream);
                descriptor.ProductSpecific10 = ReadHalfWord(stream);
                descriptor.Version = ReadByte(stream);
                descriptor.SpotBlank = ReadByte(stream);
                descriptor.SymbologyOffset = ReadWord(stream);
                descriptor.GraphicOffset = ReadWord(stream);
                descriptor.TabularOffset = ReadWord(stream);

                if (descriptor.SymbologyOffset != 0)
                {
                    symbology.Divider = ReadHalfWord(stream);
                    symbology.BlockId = ReadHalfWord(stream);
                    symbology.Length = ReadWord(stream);
                    symbology.Layers = ReadHalfWord(stream);
                    symbology.LayerDivider = ReadHalfWord(stream);
                    symbology.DataLength = ReadWord(stream);
                    symbology.PacketCode = ReadHalfWord(stream);
                    symbology.IndexFirstRangeBin = ReadHalfWord(stream);
                    symbology.RangeBinCount = ReadHalfWord(stream);
                    symbology.SweepCenterI = ReadHalfWord(stream);
                    symbology.SweepCenterJ = ReadHalfWord(stream);
                    symbology.ScaleFactor = ReadHalfWord(stream);
                    symbology.RadialCount = ReadHalfWord(stream);
                    symbology.RadialData = new List<Radial>();

                    var radialCount = symbology.RadialCount;
                    for (var i = 0; i < radialCount; i++)
                    {
                        var radial = new Radial();
                        radial.ColorValues = new List<Int32>();
                        radial.RleCount = ReadHalfWord(stream);
                        radial.StartAngle = ReadHalfWord(stream) / 10;
                        radial.AngleDelta = ReadHalfWord(stream) / 10;

                        for (var j = 0; j < radial.RleCount * 2; j++)
                        {
                            var values = DecodeRLES(stream, descriptor);
                            radial.ColorValues.AddRange(values);
                        }

                        symbology.RadialData.Add(radial);
                    }
                }

                nexrad = new NexradScan { Header = header, Descriptor = descriptor, Symbology = symbology };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Generic error while parsing Nexrad data", ex.ToString());
            }

            return nexrad;
        }

        // TODO why can't I use the default .NET binary methods?
        private static byte ReadByte(Stream ms)
        {
            var buffer = new byte[1];
            ms.Read(buffer, 0, 1);

            return buffer[0];
        }

        private static short ReadHalfWord(Stream ms)
        {
            var result = string.Empty;
            var buffer = new byte[2];
            ms.Read(buffer, 0, 2);

            return (short)((buffer[0] << 8) + buffer[1]);
        }

        private static Int32 ReadWord(Stream ms)
        {
            var result = string.Empty;
            var buffer = new byte[4];
            ms.Read(buffer, 0, 4);

            return (Int32)((buffer[0] << 24) | (buffer[1] << 16) + (buffer[2] << 8) + buffer[3]);
        }

        private static List<Int32> DecodeRLES(Stream ms, Descriptor descriptor)
        {
            var data = ReadByte(ms);    // read a byte, MSB is the RUN value, LSB is the COLOR CODE
            var value = data & 15;  // get MSB
            var length = data >> 4; // get LSB
            var values = new List<Int32>();

            if (descriptor.Mode == 1 && descriptor.Code >= 16 && descriptor.Code <= 21)
            {
                if (value >= 8)
                    value = value - 8;
                else
                    value = 0;
            }

            for (var i = 0; i < length; i++)
            {
                values.Add(value);
            }

            return values;
        }
    }
}
