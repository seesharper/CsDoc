#load "common.csx"

/// <summary>
/// This is the summary.
/// </summary>
public void SimpleMethod()
{
    
}


public class Sample
{
    /// <summary>
    /// This is the summary.
    /// </summary>
    public void Summary()
    {

    }

    /// <remarks>
    /// This is the remarks.
    /// </remarks>
    public void Remarks()
    {
    }

    /// <example>This is the example.</example>
    public void Example()
    {

    }

    /// <example>This is the example.
    /// <code>
    /// public class Foo
    /// {
    /// }
    /// </code>
    /// </example>
    public void ExampleWithCode()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <example>This is the example
    /// <code>
    /// <![CDATA[
    /// public class Foo<T>
    /// {
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void ExampleWithCData()
    {

    }



    /// <exception cref="InvalidOperationException">This is the exception.</exception>
    public void Exception()
    {

    }

    /// <param name="value">This is the param.</param>
    public void Param(int value)
    {

    }

    /// <param name="value">This is the param.</param>
    public void ParamWithDefaultValue(int value = 42)
    {

    }

    /// <typeparam name="T">This is the generic type parameter.</typeparam>
    public void TypeParameter<T>()
    {

    }

    /// <returns>This is the return value.</returns>
    public int Returns()
    {

    }

    /// <summary>   
    /// <list type="bullet">
    /// <listheader>
    /// <term>This is the header term.</term>
    /// <description>This is the header description.</description>
    /// </listheader>
    /// <item>
    /// <term>This is the first term.</term>
    /// <description>This is the first item.</description>
    /// </item>
    /// <item>
    /// <term>This is the second term.</term>
    /// <description>This is the second item.</description>
    /// </item>
    /// </list>
    /// </summary>
    public void BulletList()
    {

    }    
}

