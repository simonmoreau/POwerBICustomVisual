﻿using System;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ConvertDrawings
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string sourcePath = @"C:\Users\smoreau\OneDrive - Bouygues Immobilier\Bureau\PowerBI\ConvertDrawings\Sources\Architecte #14.svg";


            string sourceName = System.IO.Path.GetFileNameWithoutExtension(sourcePath);
            string content = File.ReadAllText(sourcePath);

            //Read XMl
            XmlSerializer serializer = new XmlSerializer(typeof(Svg));

            StringReader stringReader = new StringReader(XmlConvert.EncodeName(content));

            Svg svg = (Svg)serializer.Deserialize(XmlReader.Create(stringReader));

            List<G> levels = svg.G.g.FirstOrDefault().g;

            Shape.Svg resultingSVG = new Shape.Svg();

            resultingSVG.Version = "1.1";
            resultingSVG.Id = "Map";
            resultingSVG.Class = "gen-by-synoptic-designer";
            resultingSVG.Xmlns = "http://www.w3.org/2000/svg";
            resultingSVG.Xlink = "http://www.w3.org/1999/xlink";
            resultingSVG.ViewBox = ScaleViewPort(svg.ViewBox, 100);
            resultingSVG.Height = "100%";
            resultingSVG.Width = "100%";
            resultingSVG.Space = "preserve";

            List<Shape.Polygon> polygons = new List<Shape.Polygon>();

            int i = 1;
            foreach (G level in levels)
            {
                //Find all spaces
                List<G> elements = level.g;
                List<G> spaces = elements.Where(x => x.Class == "bs-ifcspace bs-ifcproduct").ToList();

                foreach (G space in spaces)
                {
                    foreach (Polygon spacePolygon in space.Polygon)
                    {
                        Shape.Polygon polygon = new Shape.Polygon();
                        polygon.Id = space.Dataid;
                        polygon.Title = "";
                        polygon.Points = ScalePoints(spacePolygon.Points, 30);

                        polygons.Add(polygon);
                        i++;
                    }
                }
            }

            resultingSVG.Polygon = polygons;

            XmlSerializer ser = new XmlSerializer(typeof(Shape.Svg));
            string path = System.IO.Path.Combine(@"C:\Users\smoreau\OneDrive - Bouygues Immobilier\Bureau\PowerBI\ConvertDrawings\Results", sourceName + ".edited.svg");

            TextWriter writer = new StreamWriter(path);
            ser.Serialize(writer, resultingSVG);
            writer.Close();



        }

        static string ScalePoints(string points, double value)
        {
            string[] coordinates = points.Split(' ');
            string result = "";

            foreach (string coordinate in coordinates)
            {
                //"-4.061,-16.7242 -4.061,-17.4829 -5.291,-17.4829 -5.291,-16.3916 -5.291,-15.6329 -4.061,-15.6329 -4.061,-16.7242"
                string x = coordinate.Split(",")[0];
                string y = coordinate.Split(",")[1];

                double xValue = double.Parse(x, CultureInfo.InvariantCulture) * value;
                double yValue = double.Parse(y, CultureInfo.InvariantCulture) * value;

                result = result + xValue.ToString(CultureInfo.InvariantCulture) + "," + yValue.ToString(CultureInfo.InvariantCulture) + " ";
            }

            return result;
        }

        static string ScaleViewPort(string viewport, double value)
        {
            string[] coordinates = viewport.Split(' ');
            string result = "";

            foreach (string coordinate in coordinates)
            {
                //"-4.061,-16.7242 -4.061,-17.4829 -5.291,-17.4829 -5.291,-16.3916 -5.291,-15.6329 -4.061,-15.6329 -4.061,-16.7242"

                double xValue = double.Parse(coordinate, CultureInfo.InvariantCulture) * value;

                result = result + xValue.ToString(CultureInfo.InvariantCulture) + " ";
            }

            return result;
        }

        static void RemoveInvalidChars(string path)
        {
            string text = File.ReadAllText(path);

            Regex regex = new Regex(@"data-longname="".*?""");
            MatchCollection matchs = regex.Matches(text);
            if (matchs.Count != 0)
            {
                foreach (Match match in matchs)
                {
                    if (match.Value.Contains("<"))
                    {
                        string newValue = match.Value.Replace("<", "").Replace("&", "");
                        text = text.Replace(match.Value, newValue);
                    }
                }
            }

            File.WriteAllText(path, text);
        }

    }
}

