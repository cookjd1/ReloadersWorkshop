﻿//============================================================================*
// cBullet.cs
//
// Copyright © 2013-2014, Kevin S. Beebe
// All Rights Reserved
//============================================================================*

//============================================================================*
// .Net Using Statements
//============================================================================*

using System;

//============================================================================*
// CommonLib Using Statements
//============================================================================*

using CommonLib.Conversions;

//============================================================================*
// NameSpace
//============================================================================*

namespace ReloadersWorkShop
	{
	//============================================================================*
	// cBullet class
	//============================================================================*

	[Serializable]
	public class cBullet : cSupply
		{
		//============================================================================*
		// Private Static Data Members
		//============================================================================*

		private static bool sm_fMetricDimensions = false;
		private static bool sm_fMetricWeights = false;

		private static int sm_nDimensionDecimals = 3;
		private static int sm_nWeightDecimals = 0;

		//============================================================================*
		// Private Data Members
		//============================================================================*

		private string m_strPartNumber = "";
		private string m_strType = "";
		private Double m_dDiameter = 0.0;
		private double m_dLength = 0.0;
		private double m_dWeight = 0.0;
		private double m_dBallisticCoefficient = 0.0;

		private bool m_fSelfCast = false;
		private int m_nTopPunch = 0;

		private cBulletCaliberList m_CaliberList = new cBulletCaliberList();

		//============================================================================*
		// cBullet() - Constructor
		//============================================================================*

		public cBullet()
			: base(cSupply.eSupplyTypes.Bullets)
			{
			}

		//============================================================================*
		// cBullet() - Copy Constructor
		//============================================================================*

		public cBullet(cBullet Bullet)
			: base(Bullet)
			{
			Copy(Bullet, false);
			}

		//============================================================================*
		// BallisticCoefficient Property
		//============================================================================*

		public double BallisticCoefficient
			{
			get
				{
				return (m_dBallisticCoefficient);
				}
			set
				{
				m_dBallisticCoefficient = value;
				}
			}

		//============================================================================*
		// BulletCaliber()
		//============================================================================*

		public cBulletCaliber BulletCaliber(cCaliber Caliber)
			{
			cBulletCaliber BulletCaliber = null;

			foreach (cBulletCaliber CheckBulletCaliber in CaliberList)
				{
				if (CheckBulletCaliber.Caliber.CompareTo(Caliber) == 0)
					{
					BulletCaliber = CheckBulletCaliber;

					break;
					}
				}

			return (BulletCaliber);
			}

		//============================================================================*
		// CalculateSectionalDensity()
		//============================================================================*

		public static double CalculateSectionalDensity(double dDiameter, double dWeight)
			{
			double dSectionalDensity = 0.0;

			try
				{
				dSectionalDensity = dWeight / (7000.0 * (dDiameter * dDiameter));
				}
			catch
				{
				dSectionalDensity = 0.0;
				}

			return (dSectionalDensity);
			}

		//============================================================================*
		// CaliberList Property
		//============================================================================*

		public cBulletCaliberList CaliberList
			{
			get
				{
				return (m_CaliberList);
				}
			set
				{
				m_CaliberList = value;
				}
			}

		//============================================================================*
		// CanBeCaliber()
		//============================================================================*

		public bool CanBeCaliber(cCaliber Caliber)
			{
			if (FirearmType == Caliber.FirearmType && m_dDiameter >= Caliber.MinBulletDiameter && m_dDiameter <= Caliber.MaxBulletDiameter)
				return (true);

			return (false);
			}

		//============================================================================*
		// Comparer()
		//============================================================================*

		public static int Comparer(cBullet Bullet1, cBullet Bullet2)
			{
			if (Bullet1 == null)
				{
				if (Bullet2 != null)
					return (-1);
				else
					return (0);
				}
			else
				{
				if (Bullet2 == null)
					return (1);
				}

			return (Bullet1.CompareTo(Bullet2));
			}

		//============================================================================*
		// CompareTo()
		//============================================================================*

		public override int CompareTo(Object obj)
			{
			if (obj == null)
				return (1);

			//----------------------------------------------------------------------------*
			// Base Class
			//----------------------------------------------------------------------------*

			cSupply Supply = (cSupply) obj;

			int rc = base.CompareTo(Supply);

			//----------------------------------------------------------------------------*
			// Compare Part Numbers
			//----------------------------------------------------------------------------*

			if (rc == 0)
				{
				cBullet Bullet = (cBullet) Supply;

				if (string.IsNullOrEmpty(m_strPartNumber))
					{
					if (!string.IsNullOrEmpty(Bullet.m_strPartNumber))
						rc = -1;
					else
						rc = 0;
					}
				else
					{
					if (string.IsNullOrEmpty(Bullet.m_strPartNumber))
						rc = 1;
					else
						rc = cDataFiles.ComparePartNumbers(m_strPartNumber, Bullet.m_strPartNumber);
					}
				}

			return (rc);
			}

		//============================================================================*
		// Copy()
		//============================================================================*

		public void Copy(cBullet Bullet, bool fCopyBase = true)
			{
			if (fCopyBase)
				base.Copy(Bullet);

			m_strPartNumber = Bullet.m_strPartNumber;
			m_strType = Bullet.m_strType;
			m_dDiameter = Bullet.m_dDiameter;
			m_dLength = Bullet.m_dLength;
			m_dWeight = Bullet.m_dWeight;
			m_dBallisticCoefficient = Bullet.m_dBallisticCoefficient;

			m_fSelfCast = Bullet.m_fSelfCast;
			m_nTopPunch = Bullet.m_nTopPunch;

			if (Bullet.CaliberList != null)
				m_CaliberList = new cBulletCaliberList(Bullet.CaliberList);
			else
				m_CaliberList = new cBulletCaliberList();
			}

		//============================================================================*
		// Diameter Property
		//============================================================================*

		public double Diameter
			{
			get
				{
				return (m_dDiameter);
				}
			set
				{
				m_dDiameter = value;
				}
			}

		//============================================================================*
		// DimensionDecimals Property
		//============================================================================*

		public static int DimensionDecimals
			{
			get
				{
				return (sm_nDimensionDecimals);
				}

			set
				{
				sm_nDimensionDecimals = value;
				}
			}

		//============================================================================*
		// HasCaliber()
		//============================================================================*

		public bool HasCaliber(cCaliber Caliber, bool fHideCalibers = false)
			{
			foreach (cBulletCaliber CheckCaliber in m_CaliberList)
				{
				if (CheckCaliber.CompareTo(Caliber) == 0)
					{
					if (!fHideCalibers || CheckCaliber.Caliber.Checked)
						return (true);
					}
				}

			return (false);
			}

		//============================================================================*
		// Length Property
		//============================================================================*

		public double Length
			{
			get
				{
				return (m_dLength);
				}
			set
				{
				m_dLength = value;
				}
			}

		//============================================================================*
		// MetricDimensions Property
		//============================================================================*

		public static bool MetricDimensions
			{
			get
				{
				return (sm_fMetricDimensions);
				}

			set
				{
				sm_fMetricDimensions = value;
				}
			}

		//============================================================================*
		// MetricWeights Property
		//============================================================================*

		public static bool MetricWeights
			{
			get
				{
				return (sm_fMetricWeights);
				}

			set
				{
				sm_fMetricWeights = value;
				}
			}

		//============================================================================*
		// PartNumber Property
		//============================================================================*

		public string PartNumber
			{
			get
				{
				if (m_strPartNumber == null)
					m_strPartNumber = "";

				return (m_strPartNumber);
				}

			set
				{
				m_strPartNumber = value;
				}
			}

		//============================================================================*
		// SectionalDensity Property
		//============================================================================*

		public double SectionalDensity
			{
			get
				{
				return (CalculateSectionalDensity(m_dDiameter, m_dWeight));
				}
			}

		//============================================================================*
		// SelfCast Property
		//============================================================================*

		public bool SelfCast
			{
			get
				{
				return (m_fSelfCast);
				}
			set
				{
				m_fSelfCast = value;
				}
			}

		//============================================================================*
		// Synch() - Caliber
		//============================================================================*

		public bool Synch(cCaliber Caliber)
			{
			bool fFound = false;

			foreach (cBulletCaliber CheckBulletCaliber in m_CaliberList)
				fFound = CheckBulletCaliber.Synch(Caliber);

			return (fFound);
			}

		//============================================================================*
		// TopPunch Property
		//============================================================================*

		public int TopPunch
			{
			get
				{
				return (m_nTopPunch);
				}
			set
				{
				m_nTopPunch = value;
				}
			}

		//============================================================================*
		// ToShortString()
		//============================================================================*

		public string ToShortString()
			{
			string strString = "";

			if (Manufacturer != null)
				strString = Manufacturer.Name;

			if (m_strPartNumber != null && m_strPartNumber.Length > 0)
				strString += String.Format(" {0}, {1}", m_strPartNumber, m_strType);
			else
				strString += String.Format(", {0}", m_strType);

			return (strString);
			}

		//============================================================================*
		// ToString()
		//============================================================================*

		public override string ToString()
			{
			string strString = "";

			if (Manufacturer != null)
				strString = Manufacturer.Name;

			string strDiameterFormat = ", {0:F";
			strDiameterFormat += String.Format("{0:G0}", sm_nDimensionDecimals);
			strDiameterFormat += "}";

			string strWeightFormat = ", {0:F";
			strWeightFormat += String.Format("{0:G0}", sm_nWeightDecimals);
			strWeightFormat += "}";

			bool fType = false;

			if (m_strPartNumber != null && m_strPartNumber.Length > 0)
				strString += String.Format(" {0}", m_strPartNumber);
			else
				{
				strString += String.Format(", {0}", m_strType);

				fType = true;
				}

			strString += String.Format(strDiameterFormat, sm_fMetricDimensions ?  cConversions.InchesToMillimeters(m_dDiameter) : m_dDiameter);

			strString += sm_fMetricDimensions ? " mm" : " in";

			strString += String.Format(strWeightFormat, sm_fMetricWeights ? cConversions.GrainsToGrams(m_dWeight) : m_dWeight);

			strString += sm_fMetricWeights ? " g" : " gr";

			if (!fType)
				strString += String.Format(", {0}", m_strType);

			return (strString);
			}

		//============================================================================*
		// ToWeightString()
		//============================================================================*

		public string ToWeightString()
			{
			string strString = "";

			if (Manufacturer != null)
				strString = Manufacturer.Name;

			string strWeightFormat = ", {0:F";
			strWeightFormat += String.Format("{0:G0}", sm_nWeightDecimals);
			strWeightFormat += "}";

			bool fType = false;

			if (m_strPartNumber != null && m_strPartNumber.Length > 0)
				strString += String.Format(" {0}", m_strPartNumber);
			else
				{
				strString += String.Format(", {0}", m_strType);

				fType = true;
				}

			strString += String.Format(strWeightFormat, sm_fMetricWeights ? cConversions.GrainsToGrams(m_dWeight) : m_dWeight);

			strString += sm_fMetricWeights ? " g" : " gr";

			if (!fType)
				strString += String.Format(", {0}", m_strType);

			return (strString);
			}

		//============================================================================*
		// Type Property
		//============================================================================*

		public string Type
			{
			get
				{
				return (m_strType);
				}
			set
				{
				m_strType = value;
				}
			}

		//============================================================================*
		// Weight Property
		//============================================================================*

		public double Weight
			{
			get
				{
				return (m_dWeight);
				}
			set
				{
				m_dWeight = value;
				}
			}

		//============================================================================*
		// WeightDecimals Property
		//============================================================================*

		public static int WeightDecimals
			{
			get
				{
				return (sm_nWeightDecimals);
				}

			set
				{
				sm_nWeightDecimals = value;
				}
			}
		}
	}
