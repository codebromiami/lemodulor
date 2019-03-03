/* 
Licensed under the Apache License, Version 2.0

http://www.apache.org/licenses/LICENSE-2.0
*/
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Xml2CSharp
{
	[XmlRoot(ElementName="span")]
	public class Span {
		[XmlAttribute(AttributeName="class")]
		public string Class { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="div")]
	public class Div {
		[XmlElement(ElementName="span")]
		public List<Span> Span { get; set; }
		[XmlAttribute(AttributeName="class")]
		public string Class { get; set; }
		[XmlAttribute(AttributeName="style")]
		public string Style { get; set; }
	}

	[XmlRoot(ElementName="root")]
	public class Root {
		[XmlElement(ElementName="div")]
		public List<Div> Div { get; set; }
		[XmlAttribute(AttributeName="id")]
		public string Id { get; set; }
	}

}
