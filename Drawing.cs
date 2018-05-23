/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ConvertDrawings
{
    [XmlRoot(ElementName = "polyline", Namespace = "http://www.w3.org/2000/svg")]
    public class Polyline
    {
        [XmlAttribute(AttributeName = "points")]
        public string Points { get; set; }
    }

    [XmlRoot(ElementName = "g", Namespace = "http://www.w3.org/2000/svg")]
    public class G
    {
        [XmlElement(ElementName = "polyline", Namespace = "http://www.w3.org/2000/svg")]
        public List<Polyline> Polyline { get; set; }
        [XmlAttribute(AttributeName = "class")]
        public string Class { get; set; }
        [XmlAttribute(AttributeName = "data-guid")]
        public string Dataguid { get; set; }
        [XmlAttribute(AttributeName = "data-id")]
        public string Dataid { get; set; }
        [XmlElement(ElementName = "g", Namespace = "http://www.w3.org/2000/svg")]
        public List<G> g { get; set; }
        [XmlElement(ElementName = "polygon", Namespace = "http://www.w3.org/2000/svg")]
        public List<Polygon> Polygon { get; set; }

        [XmlElement(ElementName = "text", Namespace = "http://www.w3.org/2000/svg")]
        public List<Text> Text { get; set; }
        [XmlAttribute(AttributeName = "data-bbox")]
        public string Databbox { get; set; }
        [XmlAttribute(AttributeName = "elevation")]
        public string Elevation { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "path", Namespace = "http://www.w3.org/2000/svg")]
    public class Path
    {
        [XmlAttribute(AttributeName = "d")]
        public string D { get; set; }
    }

    [XmlRoot(ElementName = "polygon", Namespace = "http://www.w3.org/2000/svg")]
    public class Polygon
    {
        [XmlAttribute(AttributeName = "points")]
        public string Points { get; set; }
    }

    [XmlRoot(ElementName = "text", Namespace = "http://www.w3.org/2000/svg")]
    public class Text
    {
        [XmlAttribute(AttributeName = "data-longname")]
        public string Datalongname { get; set; }
        [XmlAttribute(AttributeName = "transform")]
        public string Transform { get; set; }
        [XmlAttribute(AttributeName = "x")]
        public string X { get; set; }
        [XmlAttribute(AttributeName = "y")]
        public string Y { get; set; }
        [XmlText]
        public string text { get; set; }
    }

    [XmlRoot(ElementName = "svg", Namespace = "http://www.w3.org/2000/svg")]
    public class Svg
    {
        [XmlElement(ElementName = "g", Namespace = "http://www.w3.org/2000/svg")]
        public G G { get; set; }
        [XmlAttribute(AttributeName = "bimsync-version")]
        public string Bimsyncversion { get; set; }
        [XmlAttribute(AttributeName = "class")]
        public string Class { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "viewBox")]
        public string ViewBox { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

}
