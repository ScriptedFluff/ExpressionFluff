namespace ExpressionFluff.Exceptions.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Throw error indicating that the two supplied expression / query have a different parameter count
    /// </summary>
    /// <param name="leftCount"></param>
    /// <param name="rightCount"></param>
    public class ParameterCountMismatchException(int leftCount, int rightCount) : Exception($"Parameter list of count '{leftCount}' doesn't match count of '{rightCount}'");
    
    /// <summary>
    /// Throw error indicating that the two supplied expression / query have different types
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public class ParameterTypeMismatchException(Type left, Type right) : Exception($"Type: '{left.GetType()}' does not match type '{right.GetType()}'");
}
