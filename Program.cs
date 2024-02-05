using System;
using System.Collections.Generic;
using System.IO;
using aviationLib;

namespace awyToKml
{
    class CNavaids
    {
        public String facilityId;
        public String type;
        public String freq;
        public String name;

        public CNavaids(String facilityId, String type, String freq, String name)
        {
            this.facilityId = facilityId;
            this.type = type;
            this.freq = freq;
            this.name = name;
        }
    }

    class Program
    {
        static StreamReader rowset;
        static String row;
        static String[] columns;

        static List<CNavaids> navaids = new List<CNavaids>();

        static StreamWriter awyPointFix;
        static StreamWriter awyPointNdb;
        static StreamWriter awyVorPoint;
        static StreamWriter awyPointVor;
        static StreamWriter awyPointVortac;

        static String type;

        static void WriteKml(StreamWriter sw, String info, LatLon ll)
        {
            sw.WriteLine("\t<Placemark>");
            sw.WriteLine("\t\t<name>" + info + "</name>");
            sw.WriteLine("\t\t<Point>");
            
            sw.Write("\t\t\t<coordinates>");
            
            sw.Write(ll.decimalLon.ToString("F6") + "," + ll.decimalLat.ToString("F6"));
            
            sw.WriteLine("</coordinates>");

            sw.WriteLine("\t\t</Point>");
            sw.WriteLine("\t</Placemark>");

        }

        static StreamWriter InitNewWriter(String name)
        {
            StreamWriter writer = new StreamWriter(name);

            writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            writer.WriteLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
            writer.WriteLine("<Document>");

            return writer;
        }

        static void CloseWriter(StreamWriter writer)
        {
            writer.WriteLine("</Document>");
            writer.WriteLine("</kml>");

            writer.Close();
        }

        static void LoadNavaidList(String navFilename)
        {
            StreamReader nav = new StreamReader(navFilename, false);

            String r = nav.ReadLine();

            while (!nav.EndOfStream)
            {
                String[] navs = r.Split('~');

                CNavaids n = new CNavaids(navs[0], navs[1], navs[2], navs[3]);

                navaids.Add(n);

                r = nav.ReadLine();
            }

            String[] navs2 = r.Split('~');

            CNavaids n2 = new CNavaids(navs2[0], navs2[1], navs2[2], navs2[3]);

            navaids.Add(n2);

            nav.Close();
        }

        static String LookupNavaid(String name, String type)
        {
            foreach (CNavaids n in navaids)
            {
                if ((n.facilityId == name) && (n.type == type))
                {
                    return n.name + ":" + n.freq;
                }
            }

            Console.WriteLine("Navaid not found:" + name + "\r\n");

            return null;
        }

        static void ProcessRow(String row)
        {
            columns = row.Split('~');

            String pointName = columns[0];

            String pointType = columns[1];

            LatLon ll = new LatLon(columns[2], columns[3]);

            if (pointName.Length == 5)
            {
                WriteKml(awyPointFix, pointName, ll);
            }
            else if (pointType.Contains("NDB"))
            {
                String f = LookupNavaid(pointName, pointType);

                WriteKml(awyPointNdb, pointName + ":" + f, ll);
            }
            else if (pointType == "VOR")
            {
                String f = LookupNavaid(pointName, pointType);

                WriteKml(awyVorPoint, pointName + ":" + f, ll);
            }
            else if (pointType == "VOR/DME")
            {
                String f = LookupNavaid(pointName, pointType);

                WriteKml(awyPointVor, pointName + ":" + f, ll);
            }
            else if (pointType == "VORTAC")
            {
                String f = LookupNavaid(pointName, pointType);

                WriteKml(awyPointVortac, pointName + ":" + f, ll);
            }
            else
            {
                Console.WriteLine("Point type not found:" + pointType + "\r\n");
            }
        }

        static void Main(string[] args)
        {
            LoadNavaidList(args[0]);

            type = args[1];

            rowset = new StreamReader(args[2], false);

            awyPointFix = InitNewWriter("awyPointFix" + type + ".kml");
            awyPointNdb = InitNewWriter("awyPointNdb" + type + ".kml");
            awyVorPoint = InitNewWriter("awyPointVor" + type + ".kml");
            awyPointVor = InitNewWriter("awyPointVorDme" + type + ".kml");
            awyPointVortac = InitNewWriter("awyPointVortac" + type + ".kml");

            row = rowset.ReadLine();

            while (!rowset.EndOfStream)
            {
                ProcessRow(row);

                row = rowset.ReadLine();
            }

            ProcessRow(row);

            CloseWriter(awyPointFix);
            CloseWriter(awyPointNdb);
            CloseWriter(awyVorPoint);
            CloseWriter(awyPointVor);
            CloseWriter(awyPointVortac);

            rowset.Close();
        }
    }
}
