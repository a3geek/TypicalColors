using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TypicalColors.Clustering
{
	public static partial class KmeansPlusPlus
	{
		private class RandomProportional : Random
		{
			public double Sample01()
				=> base.Sample();
		}

		[ThreadStatic]
		private static RandomProportional RandomInternal = null;
		private static RandomProportional Random
		{
			get
			{
				if(RandomInternal == null)
				{
					RandomInternal = new RandomProportional();
				}
				return RandomInternal;
			}
		}

		private static int Rand(int min, int max)
			=> Random.Next(min, max);

		private static double Rand01()
			=> Random.Sample01();
	}
}
