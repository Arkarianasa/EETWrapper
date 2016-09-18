﻿

using System;
using System.Globalization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;
using EETTester.EETService_v3;

namespace EETTester
{
	public class EETMessage : Message
	{
		private readonly Message message;
		private readonly object[] parameters;

		public static IFormatProvider EETDecimalFormat = new NumberFormatInfo() { CurrencyDecimalSeparator = "." };
		public const string EETDateFormat = "yyyy-MM-ddTHH:mm:ssK";


		public EETMessage(Message message, object[] parameters)
		{
			this.message = message;
			this.parameters = parameters;
		}
		public override MessageHeaders Headers
		{
			get { return this.message.Headers; }
		}
		public override MessageProperties Properties
		{
			get { return this.message.Properties; }
		}
		public override MessageVersion Version
		{
			get { return this.message.Version; }
		}

		protected override void OnWriteStartHeaders(XmlDictionaryWriter writer)
		{
			base.OnWriteStartHeaders(writer);
		}

		protected override void OnWriteStartBody(XmlDictionaryWriter writer)
		{
			//base.OnWriteStartBody(writer);
			//s: Envelope[xmlns: s = http://www.w3.org/2003/05/soap-envelope xmlns:u=http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd ]

			 writer.WriteStartElement("Body", "http://www.w3.org/2003/05/soap-envelope");
			writer.WriteXmlnsAttribute("xsi", "http://www.w3.org/2001/XMLSchema-instance");
			writer.WriteXmlnsAttribute("xsd", "http://www.w3.org/2001/XMLSchema");
			//writer.WriteAttributeString("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", bodyKey);
		}
		protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
		{
			//< Trzba xmlns = "http://fs.mfcr.cz/eet/schema/v3" xmlns: xsi = "http://www.w3.org/2001/XMLSchema-instance" xmlns: xsd = "http://www.w3.org/2001/XMLSchema" >
			//  < Hlavicka uuid_zpravy = "b11a6061-6f8a-4f51-86a4-0e164ffa5a37" dat_odesl = "2016-09-15T22:01:51+02:00" prvni_zaslani = "true" />
			//	< Data dic_popl = "CZ00000019" dic_poverujiciho = "CZ683555118" id_provoz = "273" id_pokl = "/5546/RO24" porad_cis = "0/6460/ZQ42" dat_trzby = "2016-08-05T00:30:12+02:00" celk_trzba = "34113.00" urceno_cerp_zuct = "324" rezim = "0" />
			//	< KontrolniKody >
			//		< pkp digest = "SHA256" cipher = "RSA2048" encoding = "base64" > lspk8ii8PoVdgjvsGlCFly4wYnaM8jKS4CgVIf2vfnRwU4g0FsrPHdF3 / Bzu / EaWpMUUx3Gi7pc0MAZ + YYtFaw9DtYd9IcBzCXf79A4gPpujYJpdtI / qxGt1OSz7r / u + Sdah5ymUOO4p7YQVLcibIkLw7DX3kp6fjDeef6dtJAHHWGG32pwowR02uQIecIiq15IamJ8nikjZL2OtGX / rpOzux0ZytyTSmFWqx / KGGhPh + Cgj2UBMdgb / SVT7gvsME2yDqwgd6xyN1AtuX473IwZN5pgfkRG1 + zWhUTUgZWFdnSo3mpEidsxVat8t5l + RbgZY32W8rYT3EPlM2UaJzg ==</ pkp >
			//		< bkp digest = "SHA1" encoding = "base16" > 00a8afed - 5d07e87a - fa2acdaa - 230f97f2 - 4d13bfc9 </ bkp >
			//	</ KontrolniKody >
			//</ Trzba >

			//message.WriteBodyContents(writer);

			var message = parameters[0] as OdeslaniTrzbyRequest;

			writer.WriteElement("Trzba", (t =>
			{
				t.WriteElement("Hlavicka", (h) =>
				{
					var header = message.Hlavicka;
					h.WriteEETAttribute("uuid_zpravy", header.uuid_zpravy);
					h.WriteEETAttribute("dat_odesl", header.dat_odesl);
					h.WriteEETAttribute("prvni_zaslani", header.prvni_zaslani);

					h.WriteEETAttribute("overeni", header.overeni, () => header.overeniSpecified);
				});

				t.WriteElement("Data", d =>
				{
					var data = message.Data;

					d.WriteEETAttribute("id_provoz", data.id_provoz);
					d.WriteEETAttribute("celk_trzba", data.celk_trzba);
					d.WriteEETAttribute("dat_trzby", data.dat_trzby);
					d.WriteEETAttribute("dic_popl", data.dic_popl);
					d.WriteEETAttribute("dic_poverujiciho", data.dic_poverujiciho);
					d.WriteEETAttribute("id_pokl", data.id_pokl);
					d.WriteEETAttribute("porad_cis", data.porad_cis);
					d.WriteEETAttribute("rezim", data.rezim);

					d.WriteEETAttribute("urceno_cerp_zuct", data.urceno_cerp_zuct, () => data.urceno_cerp_zuctSpecified);
					d.WriteEETAttribute("cerp_zuct", data.cerp_zuct, () => data.cerp_zuctSpecified);
					d.WriteEETAttribute("cest_sluz", data.cest_sluz, () => data.cest_sluzSpecified);
					d.WriteEETAttribute("dan1", data.dan1, () => data.dan1Specified);
					d.WriteEETAttribute("dan2", data.dan2, () => data.dan2Specified);
					d.WriteEETAttribute("dan3", data.dan3, () => data.dan3Specified);
					d.WriteEETAttribute("pouzit_zboz1", data.pouzit_zboz1, () => data.pouzit_zboz1Specified);
					d.WriteEETAttribute("pouzit_zboz2", data.pouzit_zboz2, () => data.pouzit_zboz2Specified);
					d.WriteEETAttribute("pouzit_zboz3", data.pouzit_zboz3, () => data.pouzit_zboz3Specified);
					
					d.WriteEETAttribute("zakl_dan1", data.zakl_dan1, () => data.zakl_dan1Specified);
					d.WriteEETAttribute("zakl_dan2", data.zakl_dan2, () => data.zakl_dan2Specified);
					d.WriteEETAttribute("zakl_dan3", data.zakl_dan3, () => data.zakl_dan3Specified);
					d.WriteEETAttribute("zakl_nepodl_dph", data.zakl_nepodl_dph, () => data.zakl_nepodl_dphSpecified);

				});

				t.WriteElement("KontrolniKody", k =>
				{
					var cc = message.KontrolniKody;

					k.WriteElement("pkp", p =>
					{
						p.WriteAttributeString("cipher", cc.pkp.cipher.ToString());
						p.WriteAttributeString("digest", cc.pkp.digest.ToString());
						p.WriteAttributeString("encoding", cc.pkp.encoding.ToString());
						p.WriteString(cc.pkp.Text[0]);
					});

					k.WriteElement("bkp", b =>
					{
						b.WriteAttributeString("digest", cc.bkp.digest.ToString());
						b.WriteAttributeString("encoding", cc.bkp.encoding.ToString());
						b.WriteString(cc.bkp.Text[0]);
					});
				});
			}), "http://fs.mfcr.cz/eet/schema/v3");

		}



		protected override void OnWriteStartEnvelope(XmlDictionaryWriter writer)
		{
			base.OnWriteStartEnvelope(writer);
		}

		protected override void OnWriteMessage(XmlDictionaryWriter writer)
		{
			base.OnWriteMessage(writer);
		}

	}

	public static class XmlHelper
	{
		public static void WriteElement(this XmlDictionaryWriter writer, string name, Action<XmlDictionaryWriter> insideElement, string ns = "")
		{
			if(string.IsNullOrEmpty(ns))
				writer.WriteStartElement(name);
			else
			{
				writer.WriteStartElement(name, ns);
			}
			insideElement(writer);
			writer.WriteEndElement();
		}

		public static void WriteEETAttribute(this XmlDictionaryWriter writer, string key, object value, Func<bool> isSpecifiedFunc = null)
		{
			if (isSpecifiedFunc != null && !isSpecifiedFunc())
			{
				return;
			}

			string v = "";
			switch (value.GetType().Name.ToLower())
			{
				case "string":
					{
						v = (string)value;
						break;
					}
				case "boolean":
				case "int32":
					{
						v = value.ToString().ToLower();
						break;
					}
				case "datetime":
					{
						v = ((DateTime)value).ToString(EETMessage.EETDateFormat);
						break;
					}
				case "decimal":
					{
						v = ((decimal)value).ToString("F2", EETMessage.EETDecimalFormat);
						break;
					}
				case "pkpelementtype":
				case "bkpelementtype":
				{
						v = ((PkpElementType)value).Text[0];
						break;
				}
			}

			writer.WriteAttributeString(key, v);
		}
	}


	public class EETBodyWriter : BodyWriter
	{
		public EETBodyWriter(bool isBuffered) : base(isBuffered)
		{
		}

		protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}

	public class EETMessageFormatter : IClientMessageFormatter
	{
		private readonly IClientMessageFormatter formatter;

		public EETMessageFormatter(IClientMessageFormatter formatter)
		{
			this.formatter = formatter;
		}

		public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
		{
			var message = this.formatter.SerializeRequest(messageVersion, parameters);
			return new EETMessage(message, parameters);
		}

		public object DeserializeReply(Message message, object[] parameters)
		{
			return this.formatter.DeserializeReply(message, parameters);
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class EETFormatMessageAttribute : Attribute, IOperationBehavior
	{
		public void AddBindingParameters(OperationDescription operationDescription,
				BindingParameterCollection bindingParameters)
		{ }

		public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
		{
			var serializerBehavior = operationDescription.Behaviors.Find<XmlSerializerOperationBehavior>();

			if (clientOperation.Formatter == null)
				((IOperationBehavior)serializerBehavior).ApplyClientBehavior(operationDescription, clientOperation);

			IClientMessageFormatter innerClientFormatter = clientOperation.Formatter;

			clientOperation.Formatter = new EETMessageFormatter(innerClientFormatter);
		}

		public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
		{ }

		public void Validate(OperationDescription operationDescription) { }
	}
}