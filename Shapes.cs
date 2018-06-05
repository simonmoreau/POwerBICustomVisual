/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ConvertDrawings.Shape
{
    [XmlRoot(ElementName = "polygon", Namespace = "http://www.w3.org/2000/svg")]
    public class Polygon
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "points")]
        public string Points { get; set; }
        [XmlAttribute(AttributeName = "transform")]
        public string Transform { get; set; }
    }

    [XmlRoot(ElementName = "path", Namespace = "http://www.w3.org/2000/svg")]
    public class Path
    {
        [XmlAttribute(AttributeName = "class")]
        public string Class { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "d")]
        public string D { get; set; }
        [XmlAttribute(AttributeName = "transform")]
        public string Transform { get; set; }

    }

    [XmlRoot(ElementName = "svg", Namespace = "http://www.w3.org/2000/svg")]
    public class Svg
    {
        [XmlElement(ElementName = "polygon", Namespace = "http://www.w3.org/2000/svg")]
        public List<Polygon> Polygon { get; set; }
        [XmlElement(ElementName = "path", Namespace = "http://www.w3.org/2000/svg")]
        public List<Path> Path { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "class")]
        public string Class { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xlink", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xlink { get; set; }
        [XmlAttribute(AttributeName = "viewBox")]
        public string ViewBox { get; set; }
        [XmlAttribute(AttributeName = "space", Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Space { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
    }

}
