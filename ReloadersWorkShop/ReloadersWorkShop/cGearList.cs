﻿//============================================================================*
// cGearList.cs
//
// Copyright © 2013-2014, Kevin S. Beebe
// All Rights Reserved
//============================================================================*

//============================================================================*
// .Net Using Statements
//============================================================================*

using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

//============================================================================*
// NameSpace
//============================================================================*

namespace ReloadersWorkShop
	{
	//============================================================================*
	// cGearList Class
	//============================================================================*

	[Serializable]
	public class cGearList : List<cGear>
		{
		//============================================================================*
		// Export()
		//============================================================================*

		public void Export(StreamWriter Writer)
			{
			if (Count <= 0)
				return;

			string strLine = "";

			Writer.WriteLine("Firearm Accessories");
			Writer.WriteLine();

			for (int i = 0; i < (int) cGear.eGearTypes.NumGearTypes; i++)
				{
				cGear.eGearTypes eType = (cGear.eGearTypes) i;

				Writer.WriteLine(cGear.GearTypeString(eType));
				Writer.WriteLine();

				bool fHeader = false;

				foreach (cGear Gear in this)
					{
					if (Gear.GearType != eType)
						continue;

					if (!fHeader)
						{
						Writer.WriteLine(Gear.CSVGearLineHeader);
						Writer.WriteLine();

						fHeader = true;
						}

					strLine = Gear.CSVLine;

					Writer.WriteLine(strLine);
					}

				Writer.WriteLine();
				}
			}

		//============================================================================*
		// Export()
		//============================================================================*

		public void Export(XmlDocument XMLDocument, XmlElement XMLParentElement)
			{
			if (Count > 0)
				{
				XmlElement XMLElement = XMLDocument.CreateElement(string.Empty, "FirearmAccessories", string.Empty);
				XMLParentElement.AppendChild(XMLElement);

				foreach (cGear Gear in this)
					Gear.Export(XMLDocument, XMLElement);
				}
			}

		//============================================================================*
		// Sort()
		//============================================================================*

		public void Sort(bool fSortByType = true)
			{
			cGear.SortByType = fSortByType;

			Sort(cGear.Comparer);
			}
		}
	}