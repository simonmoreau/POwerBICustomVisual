using System;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using Svg;

namespace ConvertDrawings
{
    class Program
    {
        static List<double> xValues = new List<double>();
        static List<double> yValues = new List<double>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string sourcePath = @"C:\Users\smoreau\OneDrive - Bouygues Immobilier\Bureau\PowerBI\ConvertDrawings\Sources\Architecte #14.svg";
            string sourceName = System.IO.Path.GetFileNameWithoutExtension(sourcePath);
            string testPath = @"C:\Users\smoreau\OneDrive - Bouygues Immobilier\Bureau\PowerBI\ConvertDrawings\test.xml";
            string tempPath = @"C:\Users\smoreau\OneDrive - Bouygues Immobilier\Bureau\PowerBI\ConvertDrawings\Sources\temp.svg";


            string content = File.ReadAllText(sourcePath);
            string encodedString = StripNonValidXMLCharacters(content);

            //Read XMl
            XmlSerializer serializer = new XmlSerializer(typeof(Svg));


            using (TextReader reader = new StringReader(encodedString))
            {
                Svg svg = (Svg)serializer.Deserialize(reader);

                List<G> levels = svg.G.g.FirstOrDefault().g;

                Shape.Svg resultingSVG = new Shape.Svg();

                resultingSVG.Version = "1.1";
                resultingSVG.Id = "Map";
                resultingSVG.Class = "gen-by-synoptic-designer";
                resultingSVG.Xmlns = "http://www.w3.org/2000/svg";
                resultingSVG.Xlink = "http://www.w3.org/1999/xlink";
                resultingSVG.Height = "100%";
                resultingSVG.Width = "100%";
                resultingSVG.Space = "preserve";



                int i = 1;
                foreach (G level in levels)
                {
                    List<Shape.Path> paths = new List<Shape.Path>();

                    //Find all spaces
                    List<G> elements = level.g;
                    paths.AddRange(ConvertSVGElements(elements, "bs-ifcwindow bs-ifcproduct"));
                    paths.AddRange(ConvertSVGElements(elements, "bs-ifcdoor bs-ifcproduct"));
                    paths.AddRange(ConvertSVGElements(elements, "bs-ifcspace bs-ifcproduct"));
                    paths.AddRange(ConvertSVGElements(elements, "bs-ifcstairflight bs-ifcproduct"));
                    paths.AddRange(ConvertSVGElements(elements, "bs-ifcwall bs-ifcproduct"));

                    resultingSVG.Path = paths;
                    resultingSVG.ViewBox = ReframeViewbox();

                    

                    string resultPath = System.IO.Path.Combine(@"C:\Users\smoreau\OneDrive - Bouygues Immobilier\Bureau\PowerBI\ConvertDrawings\Results", sourceName + ".svg");


                    StringWriter writer = new StringWriter ();

                    XmlSerializer ser = new XmlSerializer(typeof(Shape.Svg));
                    ser.Serialize(writer, resultingSVG);
                    string svgFileContents = writer.ToString();
                    writer.Close();

                    var byteArray = Encoding.ASCII.GetBytes(svgFileContents);
                    using (var stream = new MemoryStream(byteArray))
                    {
                        SvgDocument svgDocument = SvgDocument.Open<SvgDocument>(stream);
                        //System.Drawing.Bitmap bitmap = svgDocument.Draw();
                    }
                }



            }
        }

        static List<Shape.Path> ConvertSVGElements(List<G> elements, string elementClass)
        {
            List<Shape.Path> paths = new List<Shape.Path>();
            List<G> selectedElements = elements.Where(x => x.Class == elementClass).ToList();

            foreach (G selectedElement in selectedElements)
            {

                Shape.Path path = new Shape.Path();
                path.Id = selectedElement.Dataid;

                path.Class = elementClass;

                if (selectedElement.g.Count != 0)
                {
                    G innerXML = selectedElement.g.FirstOrDefault();

                    foreach (Polygon elementPolygon in innerXML.Polygon)
                    {
                        path.D = path.D + PointsToPath(elementPolygon.Points, 100);
                    }

                    foreach (Polyline elementPolyline in innerXML.Polyline)
                    {
                        path.D = path.D + PointsToPath(elementPolyline.Points, 100);
                    }

                    // foreach (Path elementPath in innerXML.Path)
                    // {
                    //     path.D = path.D + elementPath.D;
                    // }

                    if (innerXML.Text != null && innerXML.Text.Count != 0)
                    {
                        Text text = innerXML.Text.FirstOrDefault();

                        path.Title = text.Datalongname;
                    }

                }

                foreach (Polygon elementPolygon in selectedElement.Polygon)
                {
                    path.D = path.D + PointsToPath(elementPolygon.Points, 100);
                }

                foreach (Polyline elementPolyline in selectedElement.Polyline)
                {
                    path.D = path.D + PointsToPath(elementPolyline.Points, 100);
                }

                paths.Add(path);
            }

            return paths;
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
                xValues.Add(xValue);
                double yValue = double.Parse(y, CultureInfo.InvariantCulture) * value;
                yValues.Add(yValue);

                result = result + xValue.ToString(CultureInfo.InvariantCulture) + "," + yValue.ToString(CultureInfo.InvariantCulture) + " ";
            }

            return result;
        }

        static string PointsToPath(string points, double value)
        {
            string[] coordinates = points.Split(' ');
            string result = "";
            bool startingPoint = true;

            foreach (string coordinate in coordinates)
            {
                string command = "L ";
                if (startingPoint) { command = "M "; startingPoint = false; }
                //"-4.061,-16.7242 -4.061,-17.4829 -5.291,-17.4829 -5.291,-16.3916 -5.291,-15.6329 -4.061,-15.6329 -4.061,-16.7242"
                string x = coordinate.Split(",")[0];
                string y = coordinate.Split(",")[1];

                double xValue = double.Parse(x, CultureInfo.InvariantCulture) * value;
                xValues.Add(xValue);
                double yValue = double.Parse(y, CultureInfo.InvariantCulture) * value;
                yValues.Add(yValue);

                result = result + command + xValue.ToString(CultureInfo.InvariantCulture) + " " + yValue.ToString(CultureInfo.InvariantCulture) + " ";
            }

            return result + "z ";
        }

        static string ScaleViewbox(string viewport, double value)
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

        static string ReframeViewbox()
        {
            double offset = (xValues.Max() - xValues.Min()) / 20;
            double minx = xValues.Min() - offset;
            double miny = yValues.Min() - offset;
            double width = xValues.Max() - xValues.Min() + 2 * offset;
            double height = yValues.Max() - yValues.Min() + 2 * offset;
            string viewBox =
            minx.ToString(CultureInfo.InvariantCulture) + " " +
            miny.ToString(CultureInfo.InvariantCulture) + " " +
            width.ToString(CultureInfo.InvariantCulture) + " " +
            height.ToString(CultureInfo.InvariantCulture);

            return viewBox;
        }

        static string StripNonValidXMLCharacters(string textIn)
        {
            string resultText = textIn;

            Regex regex = new Regex(@"([""'])(?:(?=(\\?))\2.)*?\1");
            MatchCollection matchs = regex.Matches(textIn);
            if (matchs.Count != 0)
            {

                foreach (Match match in matchs)
                {
                    if (match.Value.Contains("<") || match.Value.Contains(">") || match.Value.Contains("&"))
                    {
                        string newValue = match.Value.Replace("<", "").Replace(">", "").Replace("&", "");
                        resultText = resultText.Replace(match.Value, newValue);
                    }
                }
            }

            return resultText;
        }

    }
}
