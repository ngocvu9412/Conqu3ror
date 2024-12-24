using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class Utilities
{
    /// <summary>
    /// Helper method to animate potential matches
    /// </summary>
    /// <param name="potentialMatches"></param>
    /// <returns></returns>
    public static IEnumerator AnimatePotentialMatches(IEnumerable<GameObject> potentialMatches)
    {
        // Kiểm tra xem danh sách potentialMatches có null hay không
        if (potentialMatches == null || !potentialMatches.Any())
        {
            yield break;  // Nếu không có đối tượng nào để animate, thoát khỏi phương thức
        }

        for (float i = 1f; i >= 0.3f; i -= 0.1f)
        {
            foreach (var item in potentialMatches)
            {
                if (item != null)  // Kiểm tra nếu item không phải null
                {
                    SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
                    if (sr != null)  // Kiểm tra nếu SpriteRenderer còn tồn tại
                    {
                        Color c = sr.color;
                        c.a = i;
                        sr.color = c;
                    }
                }
            }
            yield return new WaitForSeconds(Constants.OpacityAnimationFrameDelay);
        }

        for (float i = 0.3f; i <= 1f; i += 0.1f)
        {
            foreach (var item in potentialMatches)
            {
                if (item != null)  // Kiểm tra nếu item không phải null
                {
                    SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
                    if (sr != null)  // Kiểm tra nếu SpriteRenderer còn tồn tại
                    {
                        Color c = sr.color;
                        c.a = i;
                        sr.color = c;
                    }
                }
            }
            yield return new WaitForSeconds(Constants.OpacityAnimationFrameDelay);
        }
    }

    /// <summary>
    /// Checks if a shape is next to another one
    /// either horizontally or vertically
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public static bool AreVerticalOrHorizontalNeighbors(Shape s1, Shape s2)
    {
        return (s1.Column == s2.Column ||
                        s1.Row == s2.Row)
                        && Mathf.Abs(s1.Column - s2.Column) <= 1
                        && Mathf.Abs(s1.Row - s2.Row) <= 1;
    }

    /// <summary>
    /// Will check for potential matches vertically and horizontally
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<GameObject> GetPotentialMatches(ShapesArray shapes)
    {
        //list that will contain all the matches we find
        List<List<GameObject>> matches = new List<List<GameObject>>();
        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {


                var matches1 = CheckHorizontal1(row, column, shapes);
                var matches2 = CheckHorizontal2(row, column, shapes);
                var matches3 = CheckHorizontal3(row, column, shapes);
                var matches4 = CheckHorizontal4(row, column, shapes);
                var matches5 = CheckVertical1(row, column, shapes);
                var matches6 = CheckVertical2(row, column, shapes);
                var matches7 = CheckVertical3(row, column, shapes);
                var matches8 = CheckVertical4(row, column, shapes);


                if (matches1 != null) matches.Add(matches1);
                if (matches2 != null) matches.Add(matches2);
                if (matches3 != null) matches.Add(matches3);
                if (matches4 != null) matches.Add(matches4);
                if (matches5 != null) matches.Add(matches5);
                if (matches6 != null) matches.Add(matches6);
                if (matches7 != null) matches.Add(matches7);
                if (matches8 != null) matches.Add(matches8);


                //if we have >= 3 matches, return a random one
                if (matches.Count >= 3)
                    return matches[UnityEngine.Random.Range(0, matches.Count - 1)];

                //if we are in the middle of the calculations/loops
                //and we have less than 3 matches, return a random one
                if (row >= Constants.Rows / 2 && matches.Count > 0 && matches.Count <= 2)
                    return matches[UnityEngine.Random.Range(0, matches.Count - 1)];
            }
        }

        return null;
    }

    public static List<GameObject> CheckHorizontal1(int row, int column, ShapesArray shapes)
    {
        if (column <= Constants.Columns - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().
                IsSameType(shapes[row, column + 1].GetComponent<Shape>()))
            {
                if (row >= 1 && column >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row - 1, column - 1]
                                };

                /* example *\
                 * * * * *
                 * * * * *
                 * * * * *
                 * & & * *
                 & * * * *
                \* example  */

                if (row <= Constants.Rows - 2 && column >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row + 1, column - 1]
                                };

                /* example *\
                 * * * * *
                 * * * * *
                 & * * * *
                 * & & * *
                 * * * * *
                \* example  */
            }
        }
        return null;
    }

    public static (List<GameObject>,string) Horizontal1Matches(int row, int column, ShapesArray shapes)
    {
        if (column <= Constants.Columns - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().
                IsSameType(shapes[row, column + 1].GetComponent<Shape>()))
            {
                if (row >= 1 && column >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()))
                        return (new List<GameObject>()
                        {
                            shapes[row, column - 1],
                            shapes[row - 1, column - 1]
                        }, shapes[row, column].GetComponent<Shape>().Type);

                /* example *\
                 * * * * *
                 * * * * *
                 * * * * *
                 * & & * *
                 & * * * *
                \* example  */

                if (row <= Constants.Rows - 2 && column >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()))
                        return (new List<GameObject>()
                        {
                            shapes[row, column - 1],
                            shapes[row + 1, column - 1]
                        }, shapes[row, column].GetComponent<Shape>().Type);

                /* example *\
                 * * * * *
                 * * * * *
                 & * * * *
                 * & & * *
                 * * * * *
                \* example  */
            }
        }
        return (null,null);
    }


    public static List<GameObject> CheckHorizontal2(int row, int column, ShapesArray shapes)
    {
        if (column <= Constants.Columns - 3)
        {
            if (shapes[row, column].GetComponent<Shape>().
                IsSameType(shapes[row, column + 1].GetComponent<Shape>()))
            {

                if (row >= 1 && column <= Constants.Columns - 3)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row - 1, column + 2].GetComponent<Shape>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row - 1, column + 2]
                                };

                /* example *\
                 * * * * *
                 * * * * *
                 * * * * *
                 * & & * *
                 * * * & *
                \* example  */

                if (row <= Constants.Rows - 2 && column <= Constants.Columns - 3)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row + 1, column + 2].GetComponent<Shape>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row + 1, column + 2]
                                };

                /* example *\
                 * * * * *
                 * * * * *
                 * * * & *
                 * & & * *
                 * * * * *
                \* example  */
            }
        }
        return null;
    }

    public static (List<GameObject>, string) Horizontal2Matches(int row, int column, ShapesArray shapes)
    {
        if (column <= Constants.Columns - 3)
        {
            if (shapes[row, column].GetComponent<Shape>().
                IsSameType(shapes[row, column + 1].GetComponent<Shape>()))
            {

                if (row >= 1 && column <= Constants.Columns - 3)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row - 1, column + 2].GetComponent<Shape>()))
                        return (new List<GameObject>()
                                {
                                    shapes[row, column + 2],
                                    shapes[row - 1, column + 2]
                                }, shapes[row, column].GetComponent<Shape>().Type);

                /* example *\
                 * * * * *
                 * * * * *
                 * * * * *
                 * & & * *
                 * * * & *
                \* example  */

                if (row <= Constants.Rows - 2 && column <= Constants.Columns - 3)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row + 1, column + 2].GetComponent<Shape>()))
                        return (new List<GameObject>()
                                {
                                    shapes[row, column + 2],
                                    shapes[row + 1, column + 2]
                                }, shapes[row, column].GetComponent<Shape>().Type);

                /* example *\
                 * * * * *
                 * * * * *
                 * * * & *
                 * & & * *
                 * * * * *
                \* example  */
            }
        }
        return (null, null);
    }

    public static List<GameObject> CheckHorizontal3(int row, int column, ShapesArray shapes)
    {
        if (column <= Constants.Columns - 4)
        {
            if (shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row, column + 1].GetComponent<Shape>()) &&
               shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row, column + 3].GetComponent<Shape>()))
            {
                return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row, column + 3]
                                };
            }

            /* example *\
              * * * * *  
              * * * * *
              * * * * *
              * & & * &
              * * * * *
            \* example  */
        }
        if (column >= 2 && column <= Constants.Columns - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row, column + 1].GetComponent<Shape>()) &&
               shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row, column - 2].GetComponent<Shape>()))
            {
                return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row, column -2]
                                };
            }

            /* example *\
              * * * * * 
              * * * * *
              * * * * *
              * & * & &
              * * * * *
            \* example  */
        }
        return null;
    }

    public static (List<GameObject>, string) Horizontal3Matches(int row, int column, ShapesArray shapes)
    {
        if (column <= Constants.Columns - 4)
        {
            if (shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row, column + 1].GetComponent<Shape>()) &&
               shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row, column + 3].GetComponent<Shape>()))
            {
                return (new List<GameObject>()
                                {
                                    shapes[row, column + 2],
                                    shapes[row, column + 3]
                                }, shapes[row, column].GetComponent<Shape>().Type);
            }

            /* example *\
              * * * * *  
              * * * * *
              * * * * *
              * & & * &
              * * * * *
            \* example  */
        }
        if (column >= 2 && column <= Constants.Columns - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row, column + 1].GetComponent<Shape>()) &&
               shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row, column - 2].GetComponent<Shape>()))
            {
                return (new List<GameObject>()
                                {
                                    shapes[row, column - 1],
                                    shapes[row, column -2]
                                }, shapes[row, column].GetComponent<Shape>().Type);
            }

            /* example *\
              * * * * * 
              * * * * *
              * * * * *
              * & * & &
              * * * * *
            \* example  */
        }
        return (null, null);
    }

    public static List<GameObject> CheckHorizontal4(int row, int column, ShapesArray shapes)
    {
        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa phía trên
        if (column >= 1 && column <= Constants.Columns - 2 && row >= 1)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()))
            {
                return new List<GameObject>()
            {
                shapes[row, column],
                shapes[row - 1, column - 1],
                shapes[row - 1, column + 1]
            };
            }
        }

        /* Example:
          * & * * *
          & * & * *
          * * * * *
          * * * * *
       */


        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa phía dưới
        if (column >= 1 && column <= Constants.Columns - 2 && row <= Constants.Rows - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column + 1].GetComponent<Shape>()))
            {
                return new List<GameObject>()
            {
                shapes[row, column],
                shapes[row + 1, column - 1],
                shapes[row + 1, column + 1]
            };
            }
        }

        /* Example:
         * * * * *
         * * * * *
         & * & * *
         * & * * *
      */

        return null;
    }
    public static (List<GameObject>, string) Horizontal4Matches(int row, int column, ShapesArray shapes)
    {
        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa phía trên
        if (column >= 1 && column <= Constants.Columns - 2 && row >= 1)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()))
            {
                return (new List<GameObject>()
            {
                shapes[row, column],
                shapes[row-1, column],
            }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * & * * *
           & * & * *
           * * * * *
           * * * * *
        */

        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa phía dưới
        if (column >= 1 && column <= Constants.Columns - 2 && row <= Constants.Rows - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column + 1].GetComponent<Shape>()))
            {
                return (new List<GameObject>()
            {
                shapes[row, column],
                shapes[row+1, column]
            }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * * * * *
           * * * * *
           & * & * *
           * & * * *
        */

        return (null, null);
    }

    public static (List<GameObject>, string) Match4Horizontal(int row, int column, ShapesArray shapes)
    {
        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa phía trên
        if (column >= 1 && column <= Constants.Columns - 3 && row >= 1)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column + 2].GetComponent<Shape>()))
            {
                return (new List<GameObject>()
            {
                shapes[row, column],
                shapes[row-1, column],
            }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * & * * *
           & * & & *
           * * * * *
           * * * * *
        */

        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa phía dưới
        if (column >= 1 && column <= Constants.Columns - 3 && row <= Constants.Rows - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column + 1].GetComponent<Shape>())&&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column + 2].GetComponent<Shape>()))
            {
                return (new List<GameObject>()
            {
                shapes[row, column],
                shapes[row+1, column]
            }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * * * * *
           * * * * *
           & * & & *
           * & * * *
        */

        if (column >= 2 && column <= Constants.Columns - 2 && row >= 1)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column - 2].GetComponent<Shape>()))
            {
                return (new List<GameObject>()
            {
                shapes[row, column],
                shapes[row-1, column],
            }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * * & * *
           & & * & *
           * * * * *
           * * * * *
        */

        if (column >= 2 && column <= Constants.Columns - 2 && row <= Constants.Rows - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column + 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column - 2].GetComponent<Shape>()))
            {
                return (new List<GameObject>()
            {
                shapes[row, column],
                shapes[row+1, column]
            }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }


        /* Example:
           * * * * *
           & & * & *
           * * & * *
           * * * * *
        */

        return (null, null);
    }

    public static List<GameObject> CheckVertical1(int row, int column, ShapesArray shapes)
    {
        if (row <= Constants.Rows - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().
                IsSameType(shapes[row + 1, column].GetComponent<Shape>()))
            {
                if (column >= 1 && row >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row - 1, column -1]
                                };

                /* example *\
                  * * * * *
                  * * * * *
                  * & * * *
                  * & * * *
                  & * * * *
                \* example  */

                if (column <= Constants.Columns - 2 && row >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row - 1, column + 1]
                                };

                /* example *\
                  * * * * *
                  * * * * *
                  * & * * *
                  * & * * *
                  * * & * *
                \* example  */
            }
        }
        return null;
    }

    public static (List<GameObject>, string) Vertical1Matches(int row, int column, ShapesArray shapes)
    {
        if (row <= Constants.Rows - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().
                IsSameType(shapes[row + 1, column].GetComponent<Shape>()))
            {
                if (column >= 1 && row >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()))
                        return (new List<GameObject>()
                                {
                                    shapes[row - 1, column],
                                    shapes[row - 1, column -1]
                                }, shapes[row, column].GetComponent<Shape>().Type);

                /* example *\
                  * * * * *
                  * * * * *
                  * & * * *
                  * & * * *
                  & * * * *
                \* example  */

                if (column <= Constants.Columns - 2 && row >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()))
                        return (new List<GameObject>()
                                {
                                    shapes[row-1, column],
                                    shapes[row - 1, column + 1]
                                }, shapes[row, column].GetComponent<Shape>().Type);

                /* example *\
                  * * * * *
                  * * * * *
                  * & * * *
                  * & * * *
                  * * & * *
                \* example  */
            }
        }
        return (null, null);
    }


    public static List<GameObject> CheckVertical2(int row, int column, ShapesArray shapes)
    {
        if (row <= Constants.Rows - 3)
        {
            if (shapes[row, column].GetComponent<Shape>().
                IsSameType(shapes[row + 1, column].GetComponent<Shape>()))
            {
                if (column >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row + 2, column - 1].GetComponent<Shape>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row + 2, column -1]
                                };

                /* example *\
                  * * * * *
                  & * * * *
                  * & * * *
                  * & * * *
                  * * * * *
                \* example  */

                if (column <= Constants.Columns - 2)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row + 2, column + 1].GetComponent<Shape>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row+1, column],
                                    shapes[row + 2, column + 1]
                                };

                /* example *\
                  * * * * *
                  * * & * *
                  * & * * *
                  * & * * *
                  * * * * *
                \* example  */

            }
        }
        return null;
    }

    public static (List<GameObject>, string) Vertical2Matches(int row, int column, ShapesArray shapes)
    {
        if (row <= Constants.Rows - 3)
        {
            if (shapes[row, column].GetComponent<Shape>().
                IsSameType(shapes[row + 1, column].GetComponent<Shape>()))
            {
                if (column >= 1)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row + 2, column - 1].GetComponent<Shape>()))
                        return (new List<GameObject>()
                                {
                                    shapes[row + 2, column],
                                    shapes[row + 2, column -1]
                                }, shapes[row, column].GetComponent<Shape>().Type);

                /* example *\
                  * * * * *
                  & * * * *
                  * & * * *
                  * & * * *
                  * * * * *
                \* example  */

                if (column <= Constants.Columns - 2)
                    if (shapes[row, column].GetComponent<Shape>().
                    IsSameType(shapes[row + 2, column + 1].GetComponent<Shape>()))
                        return (new List<GameObject>() { shapes[row + 2, column], shapes[row + 2, column + 1] }, shapes[row, column].GetComponent<Shape>().Type);

                /* example *\
                  * * * * *
                  * * & * *
                  * & * * *
                  * & * * *
                  * * * * *
                \* example  */

            }
        }
        return (null, null);
    }

    public static List<GameObject> CheckVertical3(int row, int column, ShapesArray shapes)
    {
        if (row <= Constants.Rows - 4)
        {
            if (shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row + 1, column].GetComponent<Shape>()) &&
               shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row + 3, column].GetComponent<Shape>()))
            {
                return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row + 3, column]
                                };
            }
        }

        /* example *\
          * & * * *
          * * * * *
          * & * * *
          * & * * *
          * * * * *
        \* example  */

        if (row >= 2 && row <= Constants.Rows - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row + 1, column].GetComponent<Shape>()) &&
               shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row - 2, column].GetComponent<Shape>()))
            {
                return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row - 2, column]
                                };
            }
        }

        /* example *\
          * * * * *
          * & * * *
          * & * * *
          * * * * *
          * & * * *
        \* example  */
        return null;
    }

    public static (List<GameObject>, string) Vertical3Matches(int row, int column, ShapesArray shapes)
    {
        if (row <= Constants.Rows - 4)
        {
            if (shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row + 1, column].GetComponent<Shape>()) &&
               shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row + 3, column].GetComponent<Shape>()))
            {
                return (new List<GameObject>() { shapes[row + 2, column], shapes[row + 3, column] }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* example *\
          * & * * *
          * * * * *
          * & * * *
          * & * * *
          * * * * *
        \* example  */

        if (row >= 2 && row <= Constants.Rows - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row + 1, column].GetComponent<Shape>()) &&
               shapes[row, column].GetComponent<Shape>().
               IsSameType(shapes[row - 2, column].GetComponent<Shape>()))
            {
                return (new List<GameObject>() { shapes[row - 1, column], shapes[row - 2, column] }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* example *\
          * * * * *
          * & * * *
          * & * * *
          * * * * *
          * & * * *
        \* example  */
        return (null, null);
    }

    public static List<GameObject> CheckVertical4(int row, int column, ShapesArray shapes)
    {
        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa bên trái
        if (row >= 1 && row <= Constants.Rows - 2 && column >= 1)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()))
            {
                return new List<GameObject>()
            {
                shapes[row, column],
                shapes[row - 1, column - 1],
                shapes[row + 1, column - 1]
            };
            }
        }

        /* Example:
           * & * *
           * * & *
           * & * *
           * * * *
        */

        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa bên phải
        if (row >= 1 && row <= Constants.Rows - 2 && column <= Constants.Columns - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column + 1].GetComponent<Shape>()))
            {
                return new List<GameObject>()
            {
                shapes[row, column],
                shapes[row - 1, column + 1],
                shapes[row + 1, column + 1]
            };
            }
        }

        /* Example:
           * & * *
           & * * *
           * & * *
           * * * *
        */

        return null;
    }

    public static (List<GameObject>, string) Vertical4Matches(int row, int column, ShapesArray shapes)
    {
        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa bên trái
        if (row >= 1 && row <= Constants.Rows - 2 && column >= 1)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()))
            {
                return (new List<GameObject>() { shapes[row, column], shapes[row, column - 1] }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * & * *
           * * & *
           * & * *
           * * * *
        */

        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa bên phải
        if (row >= 1 && row <= Constants.Rows - 2 && column <= Constants.Columns - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column + 1].GetComponent<Shape>()))
            {
                return (new List<GameObject>() { shapes[row, column], shapes[row, column + 1], }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * & * *
           & * * *
           * & * *
           * * * *
        */

        return (null, null);
    }

    public static (List<GameObject>, string) Match4Vertical(int row, int column, ShapesArray shapes)
    {
        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa bên trái
        if (row >= 2 && row <= Constants.Rows - 2 && column >= 1)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 2, column - 1].GetComponent<Shape>()))
            {
                return (new List<GameObject>() { shapes[row, column], shapes[row, column - 1] }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * & * *
           * * & *
           * & * *
           * & * *
        */

        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa bên phải
        if (row >= 2 && row <= Constants.Rows - 2 && column <= Constants.Columns - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column + 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row -2, column + 1].GetComponent<Shape>()))
            {
                return (new List<GameObject>() { shapes[row, column], shapes[row, column + 1], }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * & * *
           & * * *
           * & * *
           * & * *
        */

        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa bên trái
        if (row >= 1 && row <= Constants.Rows - 3 && column >= 1)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column - 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 2, column - 1].GetComponent<Shape>()))
            {
                return (new List<GameObject>() { shapes[row, column], shapes[row, column - 1] }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * & * *
           * & * *
           * * & *
           * & * *
        */

        // Trường hợp kiểm tra cách nhau một ô, biểu tượng cần tìm nằm ở giữa bên phải
        if (row >= 1 && row <= Constants.Rows - 3 && column <= Constants.Columns - 2)
        {
            if (shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row - 1, column + 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 1, column + 1].GetComponent<Shape>()) &&
                shapes[row, column].GetComponent<Shape>().IsSameType(shapes[row + 2, column + 1].GetComponent<Shape>()))
            {
                return (new List<GameObject>() { shapes[row, column], shapes[row, column + 1], }, shapes[row, column].GetComponent<Shape>().Type);
            }
        }

        /* Example:
           * & * *
           * & * *
           & * * *
           * & * *
        */

        return (null, null);
    }

    public static List<(GameObject, GameObject, int)> CalculateMovesAndScores(ShapesArray shapes, Dictionary<string, int> symbolPriority)
    {
        List<(GameObject, GameObject, int)> movesWithScores = new List<(GameObject, GameObject, int)>();

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                // Get potential moves
                var potentialMoves = new List<(List<GameObject>, string)>
            {
                Horizontal1Matches(row, column, shapes),
                Horizontal2Matches(row, column, shapes),
                Horizontal3Matches(row, column, shapes),
                Horizontal4Matches(row, column, shapes),
                Vertical1Matches(row, column, shapes),
                Vertical2Matches(row, column, shapes),
                Vertical3Matches(row, column, shapes),
                Vertical4Matches(row, column, shapes),
            };

                // Calculate scores for potential moves
                foreach (var move in potentialMoves)
                {
                    if (move.Item1 != null && move.Item1.Count == 2)
                    {
                        var item1 = move.Item1[0];
                        var item2 = move.Item1[1];

                        // Calculate the score for this move
                        int score = 0;
                        if (move.Item2 != null && move.Item2.StartsWith("Symbol_"))
                        {
                            string cleanType = move.Item2.Substring(7); // Remove "Symbol_" prefix
                            if (symbolPriority.ContainsKey(cleanType))
                            {
                                score += symbolPriority[cleanType];
                            }
                        }
                        // Add move to the list if it has a score
                        if (score > 0)
                        {
                            movesWithScores.Add((item1, item2, score));
                        }
                    }
                }
            }
        }

        // Sort moves by score in descending order
        movesWithScores.Sort((x, y) => y.Item3.CompareTo(x.Item3));
        return movesWithScores;
    }

    public static List<(GameObject, GameObject, int)> CalculateMovesAndScoresForMatch4(ShapesArray shapes, Dictionary<string, int> symbolPriority)
    {
        List<(GameObject, GameObject, int)> movesWithScores = new List<(GameObject, GameObject, int)>();

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                // Get potential moves
                var potentialMoves = new List<(List<GameObject>, string)>
            {
                Match4Horizontal(row,column,shapes),
                Match4Vertical(row, column, shapes)
            };

                // Calculate scores for potential moves
                foreach (var move in potentialMoves)
                {
                    if (move.Item1 != null && move.Item1.Count == 2)
                    {
                        var item1 = move.Item1[0];
                        var item2 = move.Item1[1];

                        // Calculate the score for this move
                        int score = 0;
                        if (move.Item2 != null && move.Item2.StartsWith("Symbol_"))
                        {
                            string cleanType = move.Item2.Substring(7); // Remove "Symbol_" prefix
                            if (symbolPriority.ContainsKey(cleanType))
                            {
                                score += symbolPriority[cleanType];
                            }
                        }

                        // Add move to the list if it has a score
                        if (score > 0)
                        {
                            movesWithScores.Add((item1, item2, score));
                        }
                    }
                }
            }
        }

        // Sort moves by score in descending order
        movesWithScores.Sort((x, y) => y.Item3.CompareTo(x.Item3));
        return movesWithScores;
    }





}
