using CrossUI.Drawing;
using Math = System.Math;

namespace CrossUI.Geometry
{
	static class BezierGeometry
	{

		public static double nearestTOfPoint(this CubicBezier b, Point p)
		{
			var points = new[] {b.Start, b.Span1, b.Span2, b.End};
			return nearestTOnCubicBezier(points, p);
		}

		public static Point AtT(this CubicBezier b, double t)
		{
			var points = new[] { b.Start, b.Span1, b.Span2, b.End };
			return bezier(points, DEGREE, t, null, null);
		}
		/**
			Solving the Nearest Point-on-Curve Problem and
			A Bezier Curve-Based Root-Finder
			by Philip J. Schneider
			from "Graphics Gems", Academic Press, 1990
		**/

		const int MAXDEPTH = 64;	/*  Maximum depth for recursion */

		// #define	EPSILON	(ldexp(1.0,-MAXDEPTH-1)) /*Flatness control value */
		// 1 * (2 ^ (-65))
		const double EPSILON = 2.7105054312137610850186320021749e-20;

		const int DEGREE = 3;			/*  Cubic Bezier curve		*/
		const int W_DEGREE = 5;

		public static Point nearestPointOnCubicBezier(Point[] V, Point P)
		{
			double t = nearestTOnCubicBezier(V, P);
			return bezier(V, DEGREE, t, null, null);
		}

		public static double nearestTOnCubicBezier(Point[] V, Point P)
		{

			Point[] w;			/* Ctl pts for 5th-degree eqn	*/
			double[] t_candidate = new double[W_DEGREE];	/* Possible roots		*/
			int n_solutions;		/* Number of roots found	*/
			double t;				/* Parameter value of closest pt*/

			/*  Convert problem to 5th-degree Bezier form	*/
			w = ConvertToBezierForm(P, V);

			/* Find all possible roots of 5th-degree equation */
			n_solutions = FindRoots(w, W_DEGREE, t_candidate, 0);

			/* Compare distances of P to all candidates, and to t=0, and t=1 */
			{
				double dist, new_dist;
				Point p;
				Point v = new Point();
				int i;


				/* Check distance to beginning of curve, where t = 0	*/
				dist = V2SquaredLength(V2Sub(P, V[0], ref v));
				t = 0.0;

				/* Find distances for candidate points	*/
				for (i = 0; i < n_solutions; i++)
				{
					p = bezier(V, DEGREE, t_candidate[i],
					null, null);
					new_dist = V2SquaredLength(V2Sub(P, p, ref v));
					if (new_dist < dist)
					{
						dist = new_dist;
						t = t_candidate[i];
					}
				}

				/* Finally, look at distance to end point, where t = 1.0 */
				new_dist = V2SquaredLength(V2Sub(P, V[DEGREE], ref v));
				if (new_dist < dist)
				{
					dist = new_dist;
					t = 1.0;
				}
			}

			return t;
			/*  Return the point on the curve at parameter value t */
			// printf("t : %4.12f\n", t);
		}

		static readonly double[,] z = new double[3, 4] 
		{	/* Precomputed "z" for cubics	*/
			{1.0, 0.6, 0.3, 0.1},
			{0.4, 0.6, 0.6, 0.4},
			{0.1, 0.3, 0.6, 1.0},
	    };

		/*
		 *  ConvertToBezierForm :
		 *		Given a point and a Bezier curve, generate a 5th-degree
		 *		Bezier-format equation whose solution finds the point on the
		 *      curve nearest the user-defined point.
			P;			 The point to find t for	
			 V;			 The control points		
		 */
		static Point[] ConvertToBezierForm(Point P, Point[] V)
		{
			int i, j, k, m, n, ub, lb;
			int row, column;		/* Table indices		*/
			Point[] c = new Point[DEGREE + 1];		/* V(i)'s - P			*/
			Point[] d = new Point[DEGREE];		/* V(i+1) - V(i)		*/
			Point[] w;			/* Ctl pts of 5th-degree curve  */
			double[,] cdTable = new double[3, 4];		/* Dot product of c, d		*/


			/*Determine the c's -- these are vectors created by subtracting*/
			/* point P from each of the control points				*/
			for (i = 0; i <= DEGREE; i++)
			{
				V2Sub(V[i], P, ref c[i]);
			}
			/* Determine the d's -- these are vectors created by subtracting*/
			/* each control point from the next					*/
			for (i = 0; i <= DEGREE - 1; i++)
			{
				d[i] = V2ScaleII(V2Sub(V[i + 1], V[i], ref d[i]), 3.0);
			}

			/* Create the c,d table -- this is a table of dot products of the */
			/* c's and d's							*/
			for (row = 0; row <= DEGREE - 1; row++)
			{
				for (column = 0; column <= DEGREE; column++)
				{
					cdTable[row, column] = V2Dot(d[row], c[column]);
				}
			}

			/* Now, apply the z's to the dot products, on the skew diagonal*/
			/* Also, set up the x-values, making these "points"		*/
			w = new Point[W_DEGREE + 1]; // Point *)malloc((unsigned)(W_DEGREE+1) * sizeof(Point));
			for (i = 0; i <= W_DEGREE; i++)
			{
				// w[i].Y = 0.0;
				// w[i].X = (double)(i) / W_DEGREE;
				w[i] = new Point((double)(i) / W_DEGREE, 0.0);
			}

			n = DEGREE;
			m = DEGREE - 1;
			for (k = 0; k <= n + m; k++)
			{
				lb = Math.Max(0, k - m);
				ub = Math.Min(k, n);
				for (i = lb; i <= ub; i++)
				{
					j = k - i;
					// w[i + j].Y += cdTable[j, i] * z[j, i];
					var old = w[i + j];
					w[i + j] = new Point(old.X, old.Y + cdTable[j, i] * z[j, i]);
				}
			}

			return (w);
		}
		/*
		 *  FindRoots :
		 *	Given a 5th-degree equation in Bernstein-Bezier form, find
		 *	all of the roots in the interval [0, 1].  Return the number
		 *	of roots found.
				   Point 	*w;			The control points	
			int 	degree;		The degree of the polynomial	
			double 	*t;			RETURN candidate t-values	
			int 	depth;		The depth of the recursion	
		 */

		static int FindRoots(Point[] w, int degree, double[] t, int depth)
		{
			int i;
			Point[] Left = new Point[W_DEGREE + 1];	/* New left and right 		*/
			Point[] Right = new Point[W_DEGREE + 1];	/* control polygons		*/
			int left_count,		/* Solution count from		*/
				right_count;		/* children			*/
			double[] left_t = new double[W_DEGREE + 1];	/* Solutions from kids		*/
			double[] right_t = new double[W_DEGREE + 1];

			switch (CrossingCount(w, degree))
			{
				case 0:
					{	/* No solutions here	*/
						return 0;
					}
				case 1:
					{	/* Unique solution	*/
						/* Stop recursion when the tree is deep enough	*/
						/* if deep enough, return 1 solution at midpoint 	*/
						if (depth >= MAXDEPTH)
						{
							t[0] = (w[0].X + w[W_DEGREE].X) / 2.0;
							return 1;
						}
						if (ControlPolygonFlatEnough(w, degree))
						{
							t[0] = ComputeXIntercept(w, degree);
							return 1;
						}
						break;
					}
			}

			/* Otherwise, solve recursively after	*/
			/* subdividing control polygon		*/
			bezier(w, degree, 0.5, Left, Right);
			left_count = FindRoots(Left, degree, left_t, depth + 1);
			right_count = FindRoots(Right, degree, right_t, depth + 1);


			/* Gather solutions together	*/
			for (i = 0; i < left_count; i++)
			{
				t[i] = left_t[i];
			}
			for (i = 0; i < right_count; i++)
			{
				t[i + left_count] = right_t[i];
			}

			/* Send back total number of solutions	*/
			return (left_count + right_count);
		}


		/*
		 * CrossingCount :
		 *	Count the number of times a Bezier control polygon 
		 *	crosses the 0-axis. This number is >= the number of roots.
			Point	*V;			Control pts of Bezier curve	
			int		degree;			  Degreee of Bezier curve 
		 *
		 */
		static int CrossingCount(Point[] V, int degree)
		{
			int i;
			int n_crossings = 0;	/*  Number of zero-crossings	*/
			int sign, old_sign;		/*  Sign of coefficients	*/

			sign = old_sign = SGN(V[0].Y);
			for (i = 1; i <= degree; i++)
			{
				sign = SGN(V[i].Y);
				if (sign != old_sign) n_crossings++;
				old_sign = sign;
			}
			return n_crossings;
		}



		/*
		 *  ControlPolygonFlatEnough :
		 *	Check if the control polygon of a Bezier curve is flat enough
		 *	for recursive subdivision to bottom out.
			Point	*V;		Control points	
			int 	degree;		Degree of polynomiaL
		 *
		 */
		static bool ControlPolygonFlatEnough(Point[] V, int degree)
		{
			int i;			/* Index variable		*/
			double[] distance;		/* Distances from pts to line	*/
			double max_distance_above;	/* maximum of these		*/
			double max_distance_below;
			double error;			/* Precision of root		*/
			double intercept_1,
					intercept_2,
					left_intercept,
					right_intercept;
			double a, b, c;		/* Coefficients of implicit	*/
			/* eqn for line from V[0]-V[deg]*/

			/* Find the  perpendicular distance		*/
			/* from each interior control point to 	*/
			/* line connecting V[0] and V[degree]	*/
			distance = new double[degree + 1]; //  (double*)malloc((unsigned)(degree + 1) * sizeof(double));
			{
				double abSquared;

				/* Derive the implicit equation for line connecting first *'
				/*  and last control points */
				a = V[0].Y - V[degree].Y;
				b = V[degree].X - V[0].X;
				c = V[0].X * V[degree].Y - V[degree].X * V[0].Y;

				abSquared = (a * a) + (b * b);

				for (i = 1; i < degree; i++)
				{
					/* Compute distance from each of the points to that line	*/
					distance[i] = a * V[i].X + b * V[i].Y + c;
					if (distance[i] > 0.0)
					{
						distance[i] = (distance[i] * distance[i]) / abSquared;
					}
					if (distance[i] < 0.0)
					{
						distance[i] = -((distance[i] * distance[i]) / abSquared);
					}
				}
			}


			/* Find the largest distance	*/
			max_distance_above = 0.0;
			max_distance_below = 0.0;
			for (i = 1; i < degree; i++)
			{
				if (distance[i] < 0.0)
				{
					max_distance_below = Math.Min(max_distance_below, distance[i]);
				};
				if (distance[i] > 0.0)
				{
					max_distance_above = Math.Max(max_distance_above, distance[i]);
				}
			}
			// free((char*)distance);

			{
				double det, dInv;
				double a1, b1, c1, a2, b2, c2;

				/*  Implicit equation for zero line */
				a1 = 0.0;
				b1 = 1.0;
				c1 = 0.0;

				/*  Implicit equation for "above" line */
				a2 = a;
				b2 = b;
				c2 = c + max_distance_above;

				det = a1 * b2 - a2 * b1;
				dInv = 1.0 / det;

				intercept_1 = (b1 * c2 - b2 * c1) * dInv;

				/*  Implicit equation for "below" line */
				a2 = a;
				b2 = b;
				c2 = c + max_distance_below;

				det = a1 * b2 - a2 * b1;
				dInv = 1.0 / det;

				intercept_2 = (b1 * c2 - b2 * c1) * dInv;
			}

			/* Compute intercepts of bounding box	*/
			left_intercept = Math.Min(intercept_1, intercept_2);
			right_intercept = Math.Max(intercept_1, intercept_2);

			error = 0.5 * (right_intercept - left_intercept);
			return error < EPSILON;
		}



		/*
		 *  ComputeXIntercept :
		 *	Compute intersection of chord from first control point to last
		 *  	with 0-axis.
		 * 
		 */
		/* NOTE: "T" and "Y" do not have to be computed, and there are many useless
		 * operations in the following (e.g. "0.0 - 0.0").
			Point 	*V;			Control points	
			int		degree; 		Degree of curve	
		 */
		static double ComputeXIntercept(Point[] V, int degree)
		{
			double XLK, YLK, XNM, YNM, XMK, YMK;
			double det, detInv;
			double S;
			double X;

			XLK = 1.0 - 0.0;
			YLK = 0.0 - 0.0;
			XNM = V[degree].X - V[0].X;
			YNM = V[degree].Y - V[0].Y;
			XMK = V[0].X - 0.0;
			YMK = V[0].Y - 0.0;

			det = XNM * YLK - YNM * XLK;
			detInv = 1.0 / det;

			S = (XNM * YMK - YNM * XMK) * detInv;
			/*  T = (XLK*YMK - YLK*XMK) * detInv; */

			X = 0.0 + XLK * S;
			/*  Y = 0.0 + YLK * S; */

			return X;
		}


		/*
		 *  Bezier : 
		 *	Evaluate a Bezier curve at a particular parameter value
		 *      Fill in control points for resulting sub-curves if "Left" and
		 *	"Right" are non-null.
		 * 
			int 	degree;		Degree of bezier curve	
			Point 	*V;			Control pts			
			double 	t;			Parameter value		
			Point 	*Left;		RETURN left half ctl pts	
			Point 	*Right;		 RETURN right half ctl pts	
		 */

		public static Point bezier(Point[] V, int degree, double t, Point[] Left, Point[] Right)
		{
			int i, j;		/* Index variables	*/
			Point[,] Vtemp = new Point[W_DEGREE + 1, W_DEGREE + 1];


			/* Copy control points	*/
			for (j = 0; j <= degree; j++)
			{
				Vtemp[0, j] = V[j];
			}

			/* Triangle computation	*/
			for (i = 1; i <= degree; i++)
			{
				for (j = 0; j <= degree - i; j++)
				{
					// Vtemp[i, j].X =
					// 	(1.0 - t) * Vtemp[i - 1, j].X + t * Vtemp[i - 1, j + 1].X;
					// Vtemp[i, j].Y =
					// 	(1.0 - t) * Vtemp[i - 1, j].Y + t * Vtemp[i - 1, j + 1].Y;
					Vtemp[i, j] = new Point(
						(1.0 - t) * Vtemp[i - 1, j].X + t * Vtemp[i - 1, j + 1].X,
						(1.0 - t) * Vtemp[i - 1, j].Y + t * Vtemp[i - 1, j + 1].Y);
				}
			}

			if (Left != null)
			{
				for (j = 0; j <= degree; j++)
				{
					Left[j] = Vtemp[j, 0];
				}
			}
			if (Right != null)
			{
				for (j = 0; j <= degree; j++)
				{
					Right[j] = Vtemp[degree - j, j];
				}
			}

			return (Vtemp[degree, 0]);
		}

		static Point V2ScaleII(Point v, double s)
		{
			return new Point(v.X * s, v.Y * s);
		}

		static double V2SquaredLength(Point v)
		{
			return v.Vector.SquaredLength();
		}

		static Point V2Sub(Point a, Point b, ref Point c)
		{
			c = (a.Vector - b.Vector).AsPoint();
			return c;
		}

		/* return the dot product of vectors a and b */
		static double V2Dot(Point a, Point b)
		{
			return ((a.X * b.X) + (a.Y * b.Y));
		}

		static int SGN(double v)
		{
			return v < 0.0 ? -1 : 1;
		}
	}
}
